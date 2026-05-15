using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Folder Preset Group", menuName = "Unity Wizard/Folder Preset Group")]
public class FolderPresetGroupSO : ScriptableObject {
    public string PresetGroupName;
    [TextArea]
    public string PresetGroupDescription;
    public List<FolderPresetSO> IncludedPresets;
}
