# Reactive Mise-en-scène Unity Plugin

## Introduction

Reactive Mise-en-scène is a design concept being developed by Luke Skarth-Hayley for his PhD thesis.

The concept seeks to reconcile tensions between interactivity and narrative experiences by using tacit and implicit inputs from the player, in the form of their attention on specific objects within set locations, to drive changes in the environment/set dressing of scenes, and the narrative tendency over time.

This plugin implements some of these design concepts in Unity and is available for others to explore the possibilities enabled by the system and said concepts.

## Contents

* [Installation](#installation)
* [Usage](#usage)
* [Contributing](#contributing)
* [Credits](#credits)
* [License](#license)

## Installation

See the [releases](./releases) page for downloads.

### .unitypackage method

Open the Unity project you want to import the plugin into.

In the menu bar at the top of the editor window click Assets -> Import Package -> Custom Package...

Find the .unitypackage you downloaded, select it, click Open.

If this is the first time you are using the plugin with the project, just click Import. You should now have a folder called ReactiveMise-en-scene under the Assets folder in the Project file explorer.

### Package Manager method

The modern Unity package manager install is pending creation...

## Usage

Reactive Mise-en-scène

Tutorial Video pending...

To cover: Focus.cs, FocusMeasures.cs, AttentionDataManager.cs, Loader scripts...

### Core Features

- Focus-based [1] interactions where objects are aware of their position within the camera viewport.
- Attention metrics derived from the object's position in the camera viewport over time.
- A datamanager which tracks objects and their association with specific locales and narrative tendencies.
- A collection of loader scripts which request data from the datamanager to determine how to configure further locales and the narrative elements within them.
- A collection of algorithms in the loader scripts to offer different means of configuring scenes/locales.

### Focus 

Focus is an interaction concept created by Greenhalgh *et al.* [1]. 

### Attention

The conception of Attention in this plugin is the amount of time an object is attended to by the player, i.e. how long the object is on screen, and at varying points from the centre of the screen. This is used to determine a simple Attention metric for each object. Each object is associated with a narrative tendency and a locale (see below), and the metrics for each object feed into locale-based and global levels of attention to specific narrative tendencies, which are then referenced elsewhere in the system or in code you write in order to drive adaptive and reactive changes in the experience.

### DataManager

The DataManager holds a list of structs containing the key information from each reactive object which has an attention metric associated with it. The DataManager calculates the overall attention rating per locale and globally. It is a singleton object that persists across scenes in an experience to track attention without needing all reactive objects to persist in memory.

### Loader System / Decision Algorithms

As mentioned above, there are a series of loaders for different parts of a locale. Each of these can load content based on different decision algorithms.

### Locales

A locale is an area or location an object is associated with. It is akin to Unity's built-in tag system, so you are not tied to logically parenting an object to a specific locale, you simply select the locale from a dropdown list on the FocusMeasures script

### Tendencies

A tendency is a flavour or aspect of the narrative that is presented. It is not quite the same as a branch in the typical tree-based interactive narrative, rather at whatever point or interval you decide when using the reactive mise-en-scène system you can check the attention ratings for each tendency either globally or for a specific locale, and from there make decisions on the kind of content to present to the player.

For example, on approaching a new locale, the player's presence might trigger which objects to load in the locale based on the ratings for each tendency. You could also decide to check which tendency has the highest rating, and load a dramatic scene in the area based on this, or choose to load a dramatic scene for the lowest rated tendency to challenge the player's assumptions. Decision algorithms are discussed in more detail below.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md)

## Credits

The Focus implementation in this plugin is inspired by the Focus interaction work of Benford and Fahlén in [*A Spatial Model of Interaction in Large Virtual Environments*](https://link.springer.com/chapter/10.1007/978-94-011-2094-4_8) and Greenhalgh and Benford in [*MASSIVE: a distributed virtual reality system incorporating spatial trading*](https://ieeexplore.ieee.org/abstract/document/499999).

Thank you to my supervisors Martin Flintham, Sarah Martindale, and Steve Benford for all guidance and advice received so far and in the future as my PhD progresses.

Thank you also to Paul Tennent, Christine Li, Chris Greenhalgh, Stuart Reeves, Jocelyn Spence and so many others in the Mixed Reality Lab at University of Nottingham for advice, feedback, and conversations on my research and everything else.

Finally thank you to my industry partner BBC R&D, with special thanks to Si Lumb, Phil Stenton, Tim Pearce and Rajiv Ramdhany for help and guidance, and encouraging me to release this plugin as a means to widening its reach.

## License

Released under MIT license. See [LICENSE.](./LICENSE)
