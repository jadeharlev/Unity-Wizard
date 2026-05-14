using System;
using System.Collections.Generic;
using System.IO;

namespace FolderStructure.Core {
    public class FolderService {
        public string BasePath { get; private set; }
        private readonly Action<string> logWarning;

        /// <summary>
        /// Represents operations for folder creation using a specified base path.
        /// <param name="basePath">The base path from which to create other folders</param>
        /// </summary>
        public FolderService(string basePath, Action<string> logWarning = null) {
            if (!Directory.Exists(basePath)) {
                Directory.CreateDirectory(basePath);
            }
            else {
                Console.WriteLine("Warning: folder already existed: " + basePath);
            }
            BasePath = basePath;
            this.logWarning = logWarning ?? Console.WriteLine;
        }
        
        /// <summary>
        /// Creates a folder at the specified path, using the base path from the object's creation.
        /// </summary>
        /// <param name="path">The path where the folder is to be created.</param>
        public void CreateFolder(string path) {
            var combinedPath = Path.Combine(BasePath, path);
            if (Path.IsPathRooted(path) || path.Contains("..")) {
                logWarning.Invoke("Warning: attempted path escape: " + path);
                return;
            }
            if (!Directory.Exists(combinedPath)) {
                Directory.CreateDirectory(combinedPath);
            }
            else {
                logWarning.Invoke("Warning: folder already existed: " + combinedPath);
            }
        }

        /// <summary>
        /// Creates a folder at the specified path, assuming the base path is included in the parameter.
        /// </summary>
        /// <param name="path">The relative or absolute path where the folder is to be created.</param>
        public static void CreateFolderAtPath(string path, Action<string> logWarningMethod = null) {
            if (logWarningMethod == null) logWarningMethod = Console.WriteLine;
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            } else {
                logWarningMethod.Invoke("Warning: folder already existed: " + path);
            }
        }

        /// <summary>
        /// Creates multiple folders using the base path from the object's creation.
        /// </summary>
        /// <param name="folderList">List of folders to create</param>
        public void BatchCreateFolders(IEnumerable<string> folderList) {
            foreach (string folder in folderList) {
                CreateFolder(folder);
            }
        }
        
        /// <summary>
        /// Creates multiple folders, assuming the base path is included in each provided string.
        /// </summary>
        /// <param name="folderList">List of folders to create, using the full path from the included string</param>
        public static void BatchCreateFoldersAtPaths(IEnumerable<string> folderList, Action<string> logWarningMethod = null) {
            foreach (string folder in folderList) {
                CreateFolderAtPath(folder, logWarningMethod);
            }
        }
    }
}