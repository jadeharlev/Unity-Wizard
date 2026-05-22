using System.IO;
using UnityEditor;
using UnityEngine;

namespace FolderStructure.Editor {
    public static class AssetDatabaseMover {
        public static void Move(string absoluteSource, string absoluteDestination) {
            string relativeSource = MakeProjectRelative(absoluteSource);
            string relativeDestination = MakeProjectRelative(absoluteDestination);

            if (relativeSource == null || relativeDestination == null) {
                Directory.Move(absoluteSource, absoluteDestination);
                return;
            }
            
            string error = AssetDatabase.MoveAsset(relativeSource, relativeDestination);
            if (!string.IsNullOrEmpty(error)) {
                Debug.LogWarning($"Move from {relativeSource} to {relativeDestination} failed: " + error);
            }
        }
        
        public static void MoveSandboxed(string absoluteSource, string absoluteDestination) {
            string relativeSource = MakeProjectSandboxedRelative(absoluteSource);
            string relativeDestination = MakeProjectSandboxedRelative(absoluteDestination);

            if (relativeSource == null || relativeDestination == null) {
                Directory.Move(absoluteSource, absoluteDestination);
                return;
            }
            
            string error = AssetDatabase.MoveAsset(relativeSource, relativeDestination);
            if (!string.IsNullOrEmpty(error)) {
                Debug.LogWarning($"Move from {relativeSource} to {relativeDestination} failed: " + error);
            }
        }

        private static string MakeProjectRelative(string absolutePath) {
            string projectRoot = Directory.GetParent(Application.dataPath)!.FullName;
            string normalizedAbsolutePath = Path.GetFullPath(absolutePath).Replace("\\", "/");
            string normalizedRoot = projectRoot.Replace("\\", "/").TrimEnd('/') + "/";

            if (!normalizedAbsolutePath.StartsWith(normalizedRoot)) return null;
            return normalizedAbsolutePath.Substring(normalizedRoot.Length);
        }
        
        private static string MakeProjectSandboxedRelative(string absolutePath) {
            string projectRoot = Path.Join(Directory.GetParent(Application.dataPath)!.FullName, "SandboxedFolder");
            string normalizedAbsolutePath = Path.GetFullPath(absolutePath).Replace("\\", "/");
            string normalizedRoot = projectRoot.Replace("\\", "/").TrimEnd('/') + "/";

            if (!normalizedAbsolutePath.StartsWith(normalizedRoot)) return null;
            return normalizedAbsolutePath.Substring(normalizedRoot.Length);
        }
    }
}