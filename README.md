<p align="center">
    <a href="https://www.hkviz.org">
        <img width="128" height="128" src="images/logo_glow@0.25x.png">
    </a>
</p>

# HKViz mod

A Hollow Knight mod which records analytics while playing, like player movement and collected items, health, charms, deaths over time.

The visualizations + website for viewing recordings is contained in the [hkviz-web repository](https://github.com/hkviz/hkviz-web).

Visit [hkviz.org](https://www.hkviz.org/) to see the site in action:

- 🎥 to record analytics for your own gameplays visit the [install guide](https://www.hkviz.org/guide/install).
- 🌐 to analyze public gameplays of other players explore [public gameplays](https://www.hkviz.org/run).
- 📜 and lastly there is the [visualization guide](https://www.hkviz.org/guide/analytics) that explains the
  different visualizations and how to interpret them.

## I have Ideas/Feedback (e.g. for some additional data to record.)

Feel free to [open a issue](https://github.com/hkviz/hkviz-mod/issues) on this repository or write @olivergrack on discord.

## Setup for development

Clone this repo, and create copy the `LocalOverrides.targets.example` in the `HKVizMod` folder, and rename it to `LocalOverrides.targets`.
Change the directory reference in the copied file to point to your HollowKnight installation.

Install the `HK Modding Extensions` for VisualStudio, by searching `Manage Extensions` inside the VisualStudio search.
Inside the extensions panel search for HollowKnight and install the found extension.

### The following programs might also be helpful when modding

- [AssetRipper](https://assetripper.github.io/AssetRipper/articles/Downloads.html) to decompile a complete Unity project from HollowKnight. Mostly useful for c# scripts.
- [AssetStudio](https://github.com/Perfare/AssetStudio) to extract assets from HollowKnight, like sprites and textures.
- [FSMViewAvalonia](https://github.com/nesrak1/FSMViewAvalonia) to view FSMs.
