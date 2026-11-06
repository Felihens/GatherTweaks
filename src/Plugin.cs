using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace GatherTweaks
{
    public enum SlowdownMode { Off, Picking, AllActions }

    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "felih.GatherTweaks";
        public const string NAME = "Gather Tweaks";
        public const string VERSION = "1.0.0";

        internal static ManualLogSource Log;
        internal static ConfigEntry<SlowdownMode> NoSlowdown;
        internal static ConfigEntry<bool> DirectToInventory;
        internal static ConfigEntry<bool> TreeFallsWhereYouLook;
        internal static ConfigEntry<float> TreeFallPush;
        internal static ConfigEntry<bool> DetailedLog;

        private void Awake()
        {
            Log = Logger;
            NoSlowdown = Config.Bind("1 - Movement", "NoSlowdown", SlowdownMode.Picking,
                "Removes the walk-speed slowdown of short actions. Picking = only when picking berries, mushrooms, stones and the like. AllActions = also when equipping and other short actions. Off = vanilla.");
            DirectToInventory = Config.Bind("2 - Picking", "DirectToInventory", true,
                "What you pick goes straight into your inventory. If it doesn't fit, it drops on the ground as usual.");
            TreeFallsWhereYouLook = Config.Bind("3 - Trees", "FallWhereYouLook", true,
                "A tree you cut down falls in the direction your camera is facing.");
            TreeFallPush = Config.Bind("3 - Trees", "FallPush", 3f,
                new ConfigDescription("How hard the falling tree is pushed (vanilla is 1). Higher makes the direction more reliable.",
                    new AcceptableValueRange<float>(0.5f, 6f)));
            DetailedLog = Config.Bind("9 - Debug", "DetailedLog", false, "Writes details to the BepInEx log.");
            new Harmony(GUID).PatchAll(typeof(Plugin).Assembly);
            Log.LogInfo(NAME + " " + VERSION + " loaded");
        }

        private void Update()
        {
            Picking.Update();
        }

        internal static void Debug(string msg)
        {
            if (DetailedLog.Value) Log.LogInfo(msg);
        }
    }

    // Picking: remembers what you just picked, then pulls the items it drops straight into your inventory.
    [HarmonyPatch]
    internal static class Picking
    {
        private const float Window = 2.5f;   // seconds after picking during which drops are collected
        private const float Radius = 3f;     // metres around the picked object
        internal static float LastPickTime = -100f;
        private static Vector3 _lastPickPos;
        private static readonly List<ItemDrop> Pending = new List<ItemDrop>();
        private static readonly MethodInfo RequestOwn = AccessTools.Method(typeof(ItemDrop), "RequestOwn");

        [HarmonyPatch(typeof(Pickable), "Interact")]
        [HarmonyPostfix]
        private static void Interacted(Pickable __instance, Humanoid character, bool __result)
        {
            if (!__result || character == null || character != Player.m_localPlayer) return;
            LastPickTime = Time.time;
            _lastPickPos = __instance.transform.position;
            Plugin.Debug("[pick] " + __instance.name);
        }

        // When your own game handles the pick (you own that bush), put the items straight into your
        // inventory instead of dropping them.
        private static bool _myPick;

        [HarmonyPatch(typeof(Pickable), "RPC_Pick")]
        [HarmonyPrefix]
        private static void PickStart(long sender)
        {
            _myPick = Plugin.DirectToInventory.Value && Player.m_localPlayer != null && sender == ZNet.GetUID();
        }

        [HarmonyPatch(typeof(Pickable), "RPC_Pick")]
        [HarmonyFinalizer]
        private static void PickEnd()
        {
            _myPick = false;
        }

        [HarmonyPatch(typeof(Pickable), "Drop")]
        [HarmonyPrefix]
        private static bool Drop(GameObject prefab, int stack)
        {
            if (!_myPick || prefab == null || stack <= 0) return true;
            Player p = Player.m_localPlayer;
            Inventory inv = p != null ? p.GetInventory() : null;
            if (inv == null || !inv.CanAddItem(prefab, stack))
            {
                Plugin.Debug("[pick] inventory full, dropping " + prefab.name + " x" + stack);
                return true;
            }
            inv.AddItem(prefab, stack);
            ItemDrop id = prefab.GetComponent<ItemDrop>();
            if (id != null)
                p.Message(MessageHud.MessageType.TopLeft, "$msg_added " + id.m_itemData.m_shared.m_name, stack, id.m_itemData.GetIcon());
            Plugin.Debug("[pick] to inventory (direct): " + prefab.name + " x" + stack);
            return false;
        }

        // Fallback: another player's game handled the pick and dropped the items; grab them.
        [HarmonyPatch(typeof(ItemDrop), "Awake")]
        [HarmonyPostfix]
        private static void Spawned(ItemDrop __instance)
        {
            if (!Plugin.DirectToInventory.Value || Time.time - LastPickTime > Window) return;
            Pending.Add(__instance);
            Plugin.Debug("[pick] drop seen: " + __instance.name);
        }

        internal static void Update()
        {
            if (Pending.Count == 0) return;
            Player p = Player.m_localPlayer;
            bool expired = Time.time - LastPickTime > Window;
            for (int i = Pending.Count - 1; i >= 0; i--)
            {
                ItemDrop d = Pending[i];
                if (d == null || p == null) { Pending.RemoveAt(i); continue; }
                if (expired)
                {
                    ZNetView zv = d.GetComponent<ZNetView>();
                    Plugin.Debug("[pick] gave up on " + d.name + " (owner " + (zv != null && zv.IsValid() && zv.IsOwner()) + ")");
                    Pending.RemoveAt(i); continue;
                }
                if (Vector3.Distance(d.transform.position, _lastPickPos) > Radius)
                {
                    Plugin.Debug("[pick] ignored drop too far: " + d.name);
                    Pending.RemoveAt(i); continue;
                }
                ZNetView nv = d.GetComponent<ZNetView>();
                if (nv != null && nv.IsValid() && !nv.IsOwner())
                {
                    RequestOwn.Invoke(d, null);   // owned by another player: ask for it, try again next frame
                    continue;
                }
                string name = d.m_itemData != null ? d.m_itemData.m_shared.m_name : d.name;
                if (p.Pickup(d.gameObject, false, false))
                {
                    Plugin.Debug("[pick] to inventory: " + name);
                    Pending.RemoveAt(i);
                }
                // If it didn't fit, keep trying until the window ends; then it simply stays on the ground.
            }
        }
    }

    // No slowdown: short actions (picking, equipping) normally force walking speed.
    [HarmonyPatch]
    internal static class Slowdown
    {
        [HarmonyPatch(typeof(Player), "InMinorActionSlowdown")]
        [HarmonyPrefix]
        private static bool NoSlow(Player __instance, ref bool __result)
        {
            SlowdownMode mode = Plugin.NoSlowdown.Value;
            if (mode == SlowdownMode.Off || __instance != Player.m_localPlayer) return true;
            if (mode == SlowdownMode.Picking && Time.time - Picking.LastPickTime > 1.5f) return true;
            __result = false;
            return false;
        }
    }

    // Trees: the game makes a cut tree fall along the hit's direction. Your client builds that hit,
    // so pointing it along the camera works on servers too.
    [HarmonyPatch]
    internal static class Trees
    {
        [HarmonyPatch(typeof(TreeBase), "Damage")]
        [HarmonyPrefix]
        private static void Aim(HitData hit)
        {
            if (!Plugin.TreeFallsWhereYouLook.Value || hit == null || Player.m_localPlayer == null || GameCamera.instance == null) return;
            if (hit.GetAttacker() != Player.m_localPlayer) return;
            Vector3 f = GameCamera.instance.transform.forward;
            f.y = 0f;
            if (f.sqrMagnitude < 0.01f) return;
            hit.m_dir = f.normalized * Plugin.TreeFallPush.Value;
        }
    }
}
