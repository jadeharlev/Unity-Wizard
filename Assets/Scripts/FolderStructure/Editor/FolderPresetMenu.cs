using System.IO;
using System.Linq;
using FolderStructure.Core;
using UnityEditor;
using UnityEngine;

namespace FolderStructure.Editor {
    public static class FolderPresetMenu {
        // Should we generate a sandbox folder?
        private static bool doSandbox = true;
        
        // Should we generate .keep files for version control?
        private static readonly bool makeKeepFiles = true;
        
        [MenuItem("Assets/Unity Wizard/Run Folder Preset (temp)", true)]
        private static bool ValidateRunnable() => Selection.activeObject is FolderPresetSO || Selection.activeObject is FolderPresetGroupSO;

        [MenuItem("Assets/Unity Wizard/Run Folder Preset (temp)", false)]
        private static void Run() {
            FolderService folderService;
            if (doSandbox) {
                folderService = new FolderService(Path.Join(Application.dataPath, "SandboxedFolder"), Debug.LogWarning,
                    Debug.Log, makeKeepFiles, AssetDatabaseMover.MoveSandboxed);
            } else {
                folderService = new FolderService(Application.dataPath, Debug.LogWarning, Debug.Log, makeKeepFiles,
                    AssetDatabaseMover.Move);
            }

            if (Selection.activeObject is FolderPresetSO presetSO) {
                CreateFoldersFromPreset(presetSO, folderService);
                if (presetSO.foldersToRename.Count > 0) {
                    RenameFoldersFromPreset(presetSO, folderService);
                }
            } else if (Selection.activeObject is FolderPresetGroupSO presetGroupSO) {
                foreach (FolderPresetSO preset in presetGroupSO.IncludedPresets) {
                    CreateFoldersFromPreset(preset, folderService);
                }
            }
            
            AssetDatabase.Refresh();
        }

        private static void RenameFoldersFromPreset(FolderPresetSO presetSO, FolderService folderService) {
            folderService.BatchRenameFolders(presetSO.foldersToRename
                .Select(pair => (pair.OldName, pair.NewName))
                .Where(pair => !string.IsNullOrEmpty(pair.OldName) && !string.IsNullOrEmpty(pair.NewName)));
        }

        private static void CreateFoldersFromPreset(FolderPresetSO presetSO, FolderService folderService) {
            folderService.BatchCreateFolders(presetSO.folderPaths);
        }
    }
}