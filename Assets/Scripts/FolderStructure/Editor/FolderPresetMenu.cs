using System.IO;
using FolderStructure.Core;
using UnityEditor;
using UnityEngine;

namespace FolderStructure.Editor {
    public static class FolderPresetMenu {
        [MenuItem("Assets/Unity Wizard/Run Folder Preset (temp)", true)]
        private static bool ValidateRunnable() => Selection.activeObject is FolderPresetSO || Selection.activeObject is FolderPresetGroupSO;

        [MenuItem("Assets/Unity Wizard/Run Folder Preset (temp)", false)]
        private static void Run() {
            if (Selection.activeObject is FolderPresetSO presetSO) {
                CreateFoldersFromPreset(presetSO);
            } else if (Selection.activeObject is FolderPresetGroupSO presetGroupSO) {
                foreach (FolderPresetSO preset in presetGroupSO.IncludedPresets) {
                    CreateFoldersFromPreset(preset);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void CreateFoldersFromPreset(FolderPresetSO presetSO) {
            var service = new FolderService(Path.Join(Application.dataPath, "SandboxedFolder"), Debug.LogWarning);
            service.BatchCreateFolders(presetSO.folderPaths);
        }
    }
}