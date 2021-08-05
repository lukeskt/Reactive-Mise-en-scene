# Reactive Media Object System

## Introduction

Reactive Media is a design concept being developed by Luke Skarth-Hayley for his PhD thesis.

The concept seeks to reconcile tensions between interactivity and narrative experiences by using tacit and implicit inputs from the player, in the form of their attention on specific objects within set locations, to drive changes in the narrative tendency over time.

This plugin implements some of these design concepts in Unity and is available for others to explore the possibilities enabled by the system and said concepts.

## Key Concepts

### Focus

Focus is an interaction concept created by Greenhalgh *et al.* [1]. 

### Attention

The conception of Attention in this plugin is the amount of time an object is attended to by the player, i.e. how long the object is on screen, and at varying points from the centre of the screen. This is used to determine a simple Attention metric for each object. Each object is associated with a narrative tendency and a locale (see below), and the metrics for each object feed into locale-based and global levels of attention to specific narrative tendencies, which are then referenced elsewhere in the system or in code you write in order to drive adaptive and reactive changes in the experience.

### DataManager

The DataManager holds a list of structs containing the key information from each reactive object which has an attention metric associated with it. The DataManager calculates the overall attention rating per locale and globally. It is a singleton object that persists across scenes in an experience to track attention without needing all reactive objects to persist in memory.

### Locales

A locale is an area or location an object is associated with. It is akin to Unity's built-in tag system, so you are not tied to logically parenting an object to a specific locale, you simply select the locale from a dropdown list on the FocusMeasures script

### Tendencies

A tendency is a flavour or aspect of the narrative that is presented. It is not quite the same as a branch in the typical tree-based interactive narrative, rather at whatever point or interval you decide when using the reactive media system you can check the attention ratings for each tendency either globally or for a specific locale, and from there make decisions on the kind of content to present to the player.

For example, on approaching a new locale, the player's presence might trigger which objects to load in the locale based on the ratings for each tendency. You could also decide to check which tendency has the highest rating, and load a dramatic scene in the area based on this, or choose to load a dramatic scene for the lowest rated tendency to challenge the player's assumptions. Decision algorithms are discussed in more detail below.

### Decision Algorithms / Loaders

As mentioned above, there are a series of loaders for different parts of a locale. Each of these can load content based on different decision algorithms. The algorithms are as follows:

#### 1. Max Value
#### 2. Min Value
#### 3. Proportional
#### 4. Inverse Proportion
#### 5. Competitor Distribution (in development)
#### 6. Single Value Threshold (to be implemented)
#### 7. Multi-Value Threshold (to be implemented)
#### 6. Preset
#### 7. Random

## Core Features

- Focus-based [1] interactions where objects are aware of their position within the camera viewport.
- Attention metrics derived from the object's position in the camera viewport over time.
- A datamanager which tracks objects and their association with specific locales and narrative tendencies.
- A collection of loader scripts which request data from the datamanager to determine how to configure further locales and the narrative elements within them.
- A collection of algorithms in the loader scripts to offer different means of configuring scenes/locales.

## Download

See releases.

## Installation

### .unitypackage method

### Package Manager method

## Usage

Tutorial Video pending...

To cover: Focus.cs, FocusMeasures.cs, AttentionDataManager.cs, Loader scripts...

## Additional Documentation

Link to website?

## References