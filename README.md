# Reactive Media Object System

## Introduction

Reactive Media is a design concept being developed by Luke Skarth-Hayley for his PhD thesis.

The concept seeks to reconcile tensions between interactivity and narrative experiences by using tacit and implicit inputs from the player, in the form of their attention on specific objects within set locations, to drive changes in the narrative tendency over time.

This plugin implements some of these design concepts in Unity and is available for others to explore the possibilities enabled by the system and said concepts.

## Key Concepts

### Focus

### Attention

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