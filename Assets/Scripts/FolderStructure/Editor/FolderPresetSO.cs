using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Folder Preset", menuName = "Unity Wizard/Folder Structure Preset")]
[Serializable]
public class FolderPresetSO : ScriptableObject {
    public string PresetName;
    [TextArea]
    public string PresetDescription;

    [Header("Folder Renames (first)")]
    [SerializeField]
    public List<RenamePair> foldersToRename = new();
    
    [Header("Folders to create (second)")]
    public List<string> folderPaths = new List<string>();
    
}

[Serializable]
public struct RenamePair {
    public string OldName;
    public string NewName;
}