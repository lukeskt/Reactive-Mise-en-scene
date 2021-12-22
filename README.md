# Reactive Mise-en-scène Unity Plugin

[![Introduction Video](http://img.youtube.com/vi/CpbM8zDdTQM/0.jpg)](http://www.youtube.com/watch?v=CpbM8zDdTQM "Reactive Mise-en-scène Introduction Video")

*Introduction Video*

Reactive Mise-en-scène is a Unity (2019+) plugin that uses the position of objects on screen as a proxy for player attention. This attention is then measured over time and associated with different "tendencies" and "locales", which can respectively represent, for example, different narrative tendencies and different areas or scenes.

Each locale has a dominant narrative tendency, which is determined over time based on the total attention rating for objects within it, by their associated tendencies.

On entering a new locale, we can use a prefab loading system to determine how elements of the locale are changed either by the global tendency of the narrative, or the tendency of another locale (e.g., the last locale visited). The prefab loading system can provide positive, negative, or balanced feedback into the overall system, enabling experiences that narrow to a single tendency outcome or present different tendencies/moods/perspectives over time and/or per locale.

The purpose of the plugin is to enable reactive narrative experiences that are shaped via the implicit and tacit interactions of the user through their attention (inferred by camera position), rather than by their explicit actions in game or their selection of branching choices seen in other narrative systems. The system is also flexible enough to be adapted for other use cases.

Please note that this plugin is a work-in-progress and should be considered experimental/beta software. It is part of an on-going PhD research project by the author, Luke Skarth-Hayley. You can read a summary of my research here: https://highlights.cdt.horizon.ac.uk/students/psxls4.

I am keen to discuss use cases with creators, see what people use the system for, and perhaps conduct studies with users. I am also planning game jams and other means to promote and explore the plugin and the design concepts underpinning it. Please follow and DM me on Twitter [@lukeskarth](https://www.twitter.com/lukeskarth) or get in touch via rmes@oneirica.systems.

## Contents

* [Installation](#installation)
* [Usage](#usage)
* [Further Docuemntation](#further-documentation)
* [Contributing](#contributing)
* [Credits](#credits)
* [Contact](#contact)
* [License](#license)

## Installation

Download the .unitypackage from [releases](releases).

Follow the Unity documentation on how to import the package here: https://docs.unity3d.com/Manual/AssetPackagesImport.html 

## Usage

[![IMAGE ALT TEXT](http://img.youtube.com/vi/ygI-2F8ApUM/0.jpg)](http://www.youtube.com/watch?v=ygI-2F8ApUM "Reactive Mise-en-scène Demo Scene Video")

*Tutorial Video pending*

The Reactive Mise-en-scène system’s core functionality is as follows:

1.	Customise the locales and tendencies to be relevant to your project by editing the RMS.asset file in \_Reactive Mise-en-scène\Scripts\DataStore
2.	Add the Attention Data Manager prefab (containing the Attention Data Manager component) to the scene.
3.	Add the Focus and Focus Measures components to all the objects you want to track player attention on. Set the Locale and Tendency on each.
4.	(optional) Write your own behaviour components inheriting from FocusReactiveBehaviour that implement "tacit" interactions based on an object's current Focus state.
5.	Create Prefabs with either Focus and FocusMeasures components (to serve as further Reactive Objects that track attention), or with the ReactiveObjectTags component, setting the locale and tendency per prefab on either the FocusMeasures or ReactiveObjectTags respectively. This enables the prefab to be used with the LoadPrefab.
6. 	Make use of the LoadPrefab component (e.g. via the PlacementPoint prefab) to load different prefabs based on the order of tendencies. Do this by adding a prefab per-tendency that you want to load to the LoadPrefab's list and configuring which tendency you want loading based on tendency order (e.g. StrongestTendency loads the tendency with the highest attention value) either Globally or for a specific Locale.

This is a simplification of the process, so please do watch the tutorial video above and/or refer to the further documentation in the wiki as linked below. Alternatively, look at how the Demo Scene, provided with the plugin, is set up, to see one example setup of the system in use.

[![Demo Scene Tutorial Video](http://img.youtube.com/vi/HcV73VpRVlc/0.jpg)](http://www.youtube.com/watch?v=HcV73VpRVlc "Reactive Mise-en-scène Demo Scene Video")

*Demo Scene Tutorial Video*

## Further Documentation

See the [wiki](https://github.com/lukeskt/Reactive-Mise-en-scene/wiki) for more detailed information about the plugin.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md)

## Credits

The Focus implementation in this plugin is inspired by the Focus interaction work of Benford and Fahlén in [*A Spatial Model of Interaction in Large Virtual Environments*](https://link.springer.com/chapter/10.1007/978-94-011-2094-4_8) and Greenhalgh and Benford in [*MASSIVE: a distributed virtual reality system incorporating spatial trading*](https://ieeexplore.ieee.org/abstract/document/499999).

Thank you to my supervisors Martin Flintham, Sarah Martindale, and Steve Benford for all guidance and advice received so far and in the future as my PhD progresses.

Thank you also to Paul Tennent, Christine Li, Jocelyn Spence, Chris Greenhalgh, Stuart Reeves and so many others in the Mixed Reality Lab at University of Nottingham for advice, feedback, and conversations on my research and everything else.

Finally thank you to my industry partner BBC R&D, with special thanks to Si Lumb, Phil Stenton, Tim Pearce and Rajiv Ramdhany for help and guidance, and encouraging me to release this plugin as a means to widening its reach.

## Contact

If you would like to speak to me about this plugin or my research please contact me via:

Twitter: [@lukeskarth](https://www.twitter.com/lukeskarth)

E-mail: rmes@oneirica.systems

I'm particularly keen to hear from you if you decide to use this plugin, and would be very interested to see what you make with it. I am planning on organising workshops and a game jam using this plugin, which I will advertise on Twitter and via other channels in due course.

## License

Released under [MIT License](./LICENSE).
