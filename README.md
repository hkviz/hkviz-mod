<p align="center">
    <a href="https://www.hkviz.org">
        <img width="128" height="128" src="images/logo_glow@0.25x.png">
    </a>
</p>

# HKViz mods

A mod that recordings detailed analytics of your [Hollow Knight](https://www.hollowknight.com) and [Silksong](https://hollowknightsilksong.com/) gameplays, and uploads them to the [HKViz website](https://www.hkviz.org) for visualization and analysis.

> [!NOTE]  
> The Silksong mod is currently in early development, and cannot be used yet.

The mods record detailed analytics while playing, like:
- player movement
- collected items
- defeated bosses
- damage taken, deaths, health over time
- ...and much more...

The visualizations + website for viewing recordings is contained in the [hkviz-web repository](https://github.com/hkviz/hkviz-web).

Visit [hkviz.org](https://www.hkviz.org/) to see the site in action.

__Getting Started Links__
- 🎥 [Recording Guide](https://www.hkviz.org/guide/install) to get started with recording your own gameplays.
- 🌐 [Public Gameplays](https://www.hkviz.org/run) to analyze public gameplays of others.
- 📜 [Analytics Guide](https://www.hkviz.org/guide/analytics) that explains the various visualizations, and how to use them.

## I have Ideas/Feedback (e.g. for some additional data to record.)

Feel free to [open an issue](https://github.com/hkviz/hkviz-mod/issues) on this repository or write @olivergrack on discord.

## Setup for development

Clone this repo, and create copy the `LocalOverrides.targets.example` in the `HKVizMod` folder, and rename it to `LocalOverrides.targets`.
Change the directory reference in the copied file to point to your HollowKnight installation.

Install the `HK Modding Extensions` for Visual Studio, by searching `Manage Extensions` inside the Visual Studio search.
Inside the extensions panel search for HollowKnight and install the found extension.

### The following programs might also be helpful when modding

- [AssetRipper](https://assetripper.github.io/AssetRipper/articles/Downloads.html) to decompile a complete Unity project from HollowKnight. Mostly useful for c# scripts.
- [AssetStudio](https://github.com/Perfare/AssetStudio) to extract assets from HollowKnight, like sprites and textures.
- [FSMViewAvalonia](https://github.com/nesrak1/FSMViewAvalonia) to view FSMs.
