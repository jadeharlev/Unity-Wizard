# Folder Structure System
This document contains an overview of the folder structure creation/scaffolding system. 
The goal of this system is to allow for batch creation of folders based on a predefined structure.

## Usage

### UI-Based
TODO: UI does not yet exist.

### Presets
Folder Preset ScriptableObjects can be used to quickly scaffold folder structures. Create them with `Right Click -> Create -> Unity Wizard -> Folder Structure Preset`. You can also create **groups** of presets, allowing you to combine existing presets.

### Core Code
The `FolderService` class provides shortcuts to quickly create folders. See the class's in-line documentation for more information. The class is intentionally thin to avoid coupling to any specific format, and is explicitly decoupled from Unity.