# Gather Tweaks

A client-side BepInEx mod for Valheim: picked items go straight into your inventory, picking no longer slows you down, and trees fall the way you are looking.

Mod para Valheim (BepInEx, client-side): itens colhidos vão direto para o inventário, colher não te deixa mais lento, e as árvores caem para onde você olha.

For players, see [package/README.md](package/README.md). That file is also the Thunderstore page.
Para jogadores, veja [package/README.md](package/README.md).

Download: [Thunderstore](https://thunderstore.io/c/valheim/p/DJAventuras/GatherTweaks/) · [Nexus Mods](https://www.nexusmods.com/valheim/mods/4897) · [GitHub Releases](https://github.com/Felihens/GatherTweaks/releases)

Sister mods / mods irmãos: [Gather Tweaks](https://github.com/Felihens/GatherTweaks), [Build Tweaks](https://github.com/Felihens/BuildTweaks), [DigForHoe](https://github.com/Felihens/DigForHoe), [Resource Scanner](https://github.com/Felihens/ResourceScanner), [Ore Weak Spots](https://github.com/Felihens/OreWeakSpots), [Tree Weak Spots](https://github.com/Felihens/TreeWeakSpots).

## Building

`sh build.sh` compiles `src/*.cs` against the game's assemblies and installs the dll into `BepInEx/plugins/GatherTweaks/` (`sh build.sh nocopy` only compiles). Then `powershell -File pack.ps1` builds `dist/GatherTweaks-<version>.zip` from `package/`.

## License

MIT
