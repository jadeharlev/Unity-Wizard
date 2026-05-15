using System.IO;
using FolderStructure.Core;
using UnityEditor;
using UnityEngine;

namespace FolderStructure.Editor {
    public static class FolderPresetMenu {
        // Should we generate a sandbox folder?
        private static readonly bool doSandbox = true;
        
        // Should we generate .keep files for version control?
        private static readonly bool makeKeepFiles = true;
        
        [MenuItem("Assets/Unity Wizard/Run Folder Preset (temp)", true)]
        private static bool ValidateRunnable() => Selection.activeObject is FolderPresetSO || Selection.activeObject is FolderPresetGroupSO;

        [MenuItem("Assets/Unity Wizard/Run Folder Preset (temp)", false)]
        private static void Run() {
            FolderService folderService;
            if (doSandbox) {
                folderService = new FolderService(Path.Join(Application.dataPath, "SandboxedFolder"), Debug.LogWarning,
                    Debug.Log, makeKeepFiles);
            } else {
                folderService = new FolderService(Application.dataPath, Debug.LogWarning, Debug.Log, makeKeepFiles);
            }

            if (Selection.activeObject is FolderPresetSO presetSO) {
                CreateFoldersFromPreset(presetSO, folderService);
            } else if (Selection.activeObject is FolderPresetGroupSO presetGroupSO) {
                foreach (FolderPresetSO preset in presetGroupSO.IncludedPresets) {
                    CreateFoldersFromPreset(preset, folderService);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void CreateFoldersFromPreset(FolderPresetSO presetSO, FolderService folderService) {
            folderService.BatchCreateFolders(presetSO.folderPaths);
        }
    }
}