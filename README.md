﻿<p align="center">
    <a href="https://www.hkviz.org">
        <img width="128" height="128" src="images/logo_glow@0.25x.png">
    </a>
</p>

# HKViz mod


A Hollow Knight mod which records information about player movement and collected items, health, charms, deaths over time.
This allows visualizing that data, like seeing the path one has taken in playthrough, to which boss one has died the most, where one has spent the most time, and more.

The visualizations + website for viewing recordings is contained in the [hkviz-web repository](https://github.com/hkviz/hkviz-web).

Checkout [hkviz.org](https://www.hkviz.org/) for more infos. 

## I have Ideas/Feedback (e.g. for some additional data to record.)
Feel free to [open a issue](https://github.com/hkviz/hkviz-mod/issues) on this repository or write @olivergrack on discord.

## I have Ideas/Feedback for visualizations
Best [open a issue](https://github.com/hkviz/hkviz-web/issues) in the [hkviz-web repository](https://github.com/hkviz/hkviz-web) or write @olivergrack on discord.

## How to install

To get started recording your own gameplay analytics visit [hkviz.org](https://www.hkviz.org). If you already know how to install mods, you can also directly install the 'HKViz' mod with a mod installer of your choice.

You can also view gameplays from other players, if they send you a link and have set their gameplay to 'public' or 'unlisted'.

## Setup for development

Clone this repo, and create copy the `LocalOverrides.targets.example` in the `HKVizMod` folder, and rename it to `LocalOverrides.targets`. 
Change the directory reference in the copied file to point to your HollowKnight installation.

Install the `HK Modding Extensions` for VisualStudio, by searching `Manage Extensions` inside the VisualStudio search. 
Inside the extensions panel search for HollowKnight and install the found extension.

### The following programs might also be helpful when modding
- [AssetRipper](https://assetripper.github.io/AssetRipper/articles/Downloads.html) to decompile a complete Unity project from HollowKnight. Mostly useful for c# scripts.
- [AssetStudio](https://github.com/Perfare/AssetStudio) to extract assets from HollowKnight, like sprites and textures.
- [FSMViewAvalonia](https://github.com/nesrak1/FSMViewAvalonia) to view FSMs.
