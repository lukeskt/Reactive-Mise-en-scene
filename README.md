# Reactive Mise-en-scène Unity Plugin

Reactive Mise-en-scène is a plugin that uses the position of objects on screen as a proxy for viewer/player attention, and this attention is then measured over time and associated with different narrative tendencies. 

Each specified object has attention tracked over time and is tagged with a locale and a tendency. Locales are the locations/scenes/areas objects reside within, and tendencies are the narrative tendencies/branches/moods with which objects are associated.

Each locale has a dominant narrative tendency, which is determined over time based on the total attention rating for objects within it, by their associated tendencies.

On entering a new locale, we can use the loader system to determine how elements of the locale are changed either by the global tendency of the narrative, or the tendency of another locale (e.g., the last locale visited). The loader system can provide positive, negative, or more balanced feedback into the overall system, enabling experiences that narrow to a single tendency outcome or present different tendencies/moods/perspectives over time and/or per locale.

The purpose of the plugin is to enable reactive narrative experiences that are shaped via the implicit and tacit interactions of the user through attention, rather than by their explicit actions in game or their selection of branching choices seen in other narrative systems. However, the system is flexible enough that it can be adapted to other use cases.

Please note that this plugin is a work-in-progress and should be considered experimental/beta software. It is part of an on-going PhD research project by the author, Luke Skarth-Hayley. I am keen to discuss use cases with creators, see what people use the system for, and perhaps conduct studies with users. I am also planning game jams and other means to promote and explore the plugin and the design concepts underpinning it. Please follow me on Twitter [@lukeskarth](https://www.twitter.com/lukeskarth) or get in touch via rmes@oneirica.systems.

## Contents

* [Installation](#installation)
* [Usage](#usage)
* [Further Docuemntation](#further-documentation)
* [Contributing](#contributing)
* [Credits](#credits)
* [License](#license)

## Installation

*Tutorial Video pending*

### .unitypackage

Download the package from [releases](releases).

Follow the Unity documentation here: https://docs.unity3d.com/Manual/AssetPackagesImport.html 

### Package Manager

Please note the package manager installation method is pending setup, it won’t work at present.

Download the tar.gz file from [releases](releases).

Follow the Unity documentation here: https://docs.unity3d.com/Manual/upm-ui-tarball.html 

## Usage

*Tutorial Video pending*

The Reactive Mise-en-scène system’s core functionality is as follows:

1.	Customise the locales and tendencies to be relevant to your project by editing…?!?
2.	Create the actual locales/areas you want in your experience or game, along with the objects, and divide these up per locale based on which ones you want to load depending on the tendency. This might be complex, so refer to the tutorial video when available, or check out the demo scene in the plugin.
3.	Add the Focus and Focus Measures components to all the objects you want to track player attention on. Set the Locale and Tendency on each.
4.	Add the Attention Data Manager prefab (containing the Attention Data Manager component) to the scene.
5.	Add loaders and configure which objects and elements they will load dependent upon which locale/global tendency you want to track, and the algorithm that will be used to determine what to load. You might want to use the Locale prefab, which is a nested set of game objects with attached loader scripts, as a guide to how to set up a locale.

This is a simplification of the process, so please do watch the tutorial video above and/or refer to the further documentation in the wiki as linked below.

## Further Documentation

See the [wiki](https://github.com/lukeskt/Reactive-Mise-en-scene/wiki) for more detailed information about the plugin, along with the design and academic underpinnings of the system.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md)

## Credits

The Focus implementation in this plugin is inspired by the Focus interaction work of Benford and Fahlén in [*A Spatial Model of Interaction in Large Virtual Environments*](https://link.springer.com/chapter/10.1007/978-94-011-2094-4_8) and Greenhalgh and Benford in [*MASSIVE: a distributed virtual reality system incorporating spatial trading*](https://ieeexplore.ieee.org/abstract/document/499999).

Thank you to my supervisors Martin Flintham, Sarah Martindale, and Steve Benford for all guidance and advice received so far and in the future as my PhD progresses.

Thank you also to Paul Tennent, Christine Li, Chris Greenhalgh, Stuart Reeves, Jocelyn Spence and so many others in the Mixed Reality Lab at University of Nottingham for advice, feedback, and conversations on my research and everything else.

Finally thank you to my industry partner BBC R&D, with special thanks to Si Lumb, Phil Stenton, Tim Pearce and Rajiv Ramdhany for help and guidance, and encouraging me to release this plugin as a means to widening its reach.

## License

Released under [MIT License](./LICENSE).
