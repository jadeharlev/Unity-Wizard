using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Folder Preset", menuName = "Unity Wizard/Folder Structure Preset")]
[Serializable]
public class FolderPresetSO : ScriptableObject {
    public string PresetName;
    [TextArea]
    public string PresetDescription;

    public List<string> folderPaths = new List<string>();
    
    [SerializeField]
    public List<RenamePair> foldersToRename = new(); 
}

[Serializable]
public struct RenamePair {
    public string OldName;
    public string NewName;
}