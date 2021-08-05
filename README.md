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

### Locales

### Tendencies

### Decision Algorithms

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