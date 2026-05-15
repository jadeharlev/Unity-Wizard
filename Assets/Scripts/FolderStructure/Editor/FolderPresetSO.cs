using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Folder Preset", menuName = "Unity Wizard/Folder Structure Preset")]
[System.Serializable]
public class FolderPresetSO : ScriptableObject {
    public string PresetName;
    [TextArea]
    public string PresetDescription;
    public List<string> folderPaths = new List<string>();
}
