using System;
using System.IO;

namespace FolderStructure.Core {
    public class Files {
        public string BasePath { get; private set; }

        /// <summary>
        /// Represents operations for folder creation using a specified base path.
        /// <param name="basePath">The base path from which to create other folders</param>
        /// </summary>
        public Files(string basePath) {
            if (!Directory.Exists(basePath)) {
                Directory.CreateDirectory(basePath);
            }
            else {
                Console.WriteLine("Warning: folder already existed: " + basePath);
            }
            BasePath = basePath;
        }
        
        /// <summary>
        /// Creates a folder at the specified path, using the base path from the object's creation.
        /// </summary>
        /// <param name="path">The path where the folder is to be created.</param>
        public void CreateFolder(string path) {
            var combinedPath = Path.Combine(BasePath, path);
            if (!Directory.Exists(combinedPath)) {
                Directory.CreateDirectory(combinedPath);
            }
            else {
                Console.WriteLine("Warning: folder already existed: " + combinedPath);
            }
        }

        /// <summary>
        /// Creates a folder at the specified path, assuming the base path is included in the parameter.
        /// </summary>
        /// <param name="path">The relative or absolute path where the folder is to be created.</param>
        public static void CreateFolderAssumingBasePath(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            } else {
                Console.WriteLine("Warning: folder already existed: " + path);
            }
        }
    }
}