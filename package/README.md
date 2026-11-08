# Gather Tweaks

**[English](#english) · [Português](#português)**

---

## English

Small quality-of-life changes for gathering. What you pick goes straight into your inventory, picking no longer slows you down, and a tree you cut down falls the way you are looking.

### Features

- **Client-side only.** The server and other players don't need the mod.
- **Straight to inventory:** berries, mushrooms, flint, stones, branches and anything else you pick go straight into your inventory instead of popping out onto the ground. If something doesn't fit, it drops as usual.
- **No slowdown:** picking normally forces you to walk for a moment. With this mod you keep your speed.
- **Trees fall where you look:** a tree you cut down falls in the direction your camera is facing, so it doesn't land on you, your house or in the water.
- **Works on servers.** If another player's game handles the bush or tree, the mod still grabs the items and still aims the tree.

### Requirements

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)

### Installation

**r2modman / Thunderstore Mod Manager:** click *Install*. To install a downloaded zip, go to Settings → *Import local mod*.

**Nexus Mods / Vortex:** also on [Nexus Mods](https://www.nexusmods.com/valheim/mods/4897).

**Manual:**
1. Extract the zip.
2. Copy the `plugins/GatherTweaks` folder into `Valheim/BepInEx/plugins/`.
   Result: `Valheim/BepInEx/plugins/GatherTweaks/GatherTweaks.dll`.
3. Launch the game.

To update, delete the old `GatherTweaks` folder before copying the new one.

### Configuration

Press **F1** in game (requires Configuration Manager) or edit `BepInEx/config/felih.GatherTweaks.cfg`.

| Option | Default | What it does |
|---|---|---|
| NoSlowdown | Picking | `Picking` = no slowdown when picking. `AllActions` = also when equipping and other short actions. `Off` = as in the base game. |
| DirectToInventory | on | Picked items go straight into your inventory. |
| FallWhereYouLook | on | Trees fall in the direction you are looking. |
| FallPush | 3 | How hard the falling tree is pushed (the base game uses 1). Higher makes the direction more reliable. 0.5 to 6. |
| DetailedLog | off | Writes details to the BepInEx log (for bug reports). |

### Notes

- Works well with **Build Tweaks**, **Tree Weak Spots** and **Resource Scanner** (same author).

---

## Português

Pequenas melhorias para a coleta. O que você colhe vai direto para o inventário, colher não te deixa mais lento, e a árvore que você corta cai para o lado em que você está olhando.

### Recursos

- **Só no seu jogo.** O servidor e os outros jogadores não precisam ter o mod.
- **Direto para o inventário:** berries, cogumelos, sílex, pedras, galhos e tudo o mais que você colhe vai direto para o inventário, em vez de pular no chão. Se não couber, cai no chão como no jogo normal.
- **Sem lentidão:** no jogo normal, colher te obriga a andar devagar por um instante. Com o mod você mantém a velocidade.
- **Árvores caem para onde você olha:** a árvore que você derruba cai na direção da câmera, então ela não cai em cima de você, da sua casa ou na água.
- **Funciona em servidores.** Se é o jogo de outro jogador que controla a moita ou a árvore, o mod ainda pega os itens e ainda direciona a árvore.

### Requisitos

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)

### Instalação

**r2modman / Thunderstore Mod Manager:** clique em *Install*. Para instalar um zip baixado, use Configurações → *Import local mod*.

**Nexus Mods / Vortex:** também está no [Nexus Mods](https://www.nexusmods.com/valheim/mods/4897).

**À mão:**
1. Extraia o zip.
2. Copie a pasta `plugins/GatherTweaks` para dentro de `Valheim/BepInEx/plugins/`.
   Resultado: `Valheim/BepInEx/plugins/GatherTweaks/GatherTweaks.dll`.
3. Abra o jogo.

Para atualizar, apague a pasta `GatherTweaks` antiga antes de copiar a nova.

### Configuração

Aperte **F1** no jogo (precisa do Configuration Manager) ou edite `BepInEx/config/felih.GatherTweaks.cfg`.

| Opção | Padrão | O que faz |
|---|---|---|
| NoSlowdown | Picking | `Picking` = sem lentidão ao colher. `AllActions` = também ao equipar e em outras ações curtas. `Off` = como no jogo normal. |
| DirectToInventory | ligado | Itens colhidos vão direto para o inventário. |
| FallWhereYouLook | ligado | Árvores caem na direção para onde você olha. |
| FallPush | 3 | Força do empurrão na árvore que cai (o jogo usa 1). Mais alto deixa a direção mais certeira. De 0.5 a 6. |
| DetailedLog | desligado | Escreve detalhes no log do BepInEx (para relatar bugs). |

### Observações

- Combina com o **Build Tweaks**, o **Tree Weak Spots** e o **Resource Scanner** (mesmo autor).
