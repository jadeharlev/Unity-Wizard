using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace FolderStructure.Core {
    public sealed class FolderServiceTests : IDisposable {

        private readonly string basePath;
        private readonly FolderService folderService;

        public FolderServiceTests() {
            basePath = Path.Combine(Path.GetTempPath(), "UnityWizardTests", Guid.NewGuid().ToString());
            if (Directory.Exists(basePath)) {
                Directory.Delete(basePath, true);
            }
            folderService = new FolderService(basePath, createKeepFiles: true);
        }
        
        #region Folder Creation
        [Fact]
        public void BasePathIsUsedForNewFolders() {
            folderService.CreateFolder("CustomFolder");
            var expectedPath = Path.Combine(basePath, "CustomFolder");
            Assert.True(Directory.Exists(expectedPath));
        }

        [Fact]
        public void CreatingAlreadyExistentFolderIsIdempotent() {
            var path = Path.Combine(basePath, "Repeat");
            folderService.CreateFolder("Repeat");
            File.WriteAllText(Path.Combine(path, "test.txt"), "hello");
            
            folderService.CreateFolder("Repeat");
            
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, "test.txt")));
        }
        
        [Fact]
        public void StaticallyCreatingAlreadyExistentFolderIsIdempotent() {
            var path = Path.Combine(basePath, "Repeat");
            FolderService.CreateFolderAtPath(path);
            File.WriteAllText(Path.Combine(path, "test.txt"), "hello");
            
            FolderService.CreateFolderAtPath(path);
            
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, "test.txt")));
        }
        
        [Fact]
        public void ConstructorCreatingAlreadyExistentFolderIsIdempotent() {
            var path = Path.Combine(basePath, "Repeat");
            FolderService.CreateFolderAtPath(path);
            File.WriteAllText(Path.Combine(path, "test.txt"), "hello");
            
            // invoke constructor again
            var _files2 = new FolderService(path);
            
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, "test.txt")));
        }

        [Fact]
        public void StaticFolderCreationWorksUsingPathAsGiven() {
            var path = Path.Combine(basePath, "StaticFolder");
            FolderService.CreateFolderAtPath(path);
            Assert.True(Directory.Exists(path));
        }

        [Fact]
        public void BasePathIsUsedInBatchCreation() {
            IEnumerable<string> folderList = new List<string>
            {
                "Gustavo",
                "Jade",
                "Kamron"
            };
            folderService.BatchCreateFolders(folderList);
            Assert.True(Directory.Exists(Path.Combine(basePath, "Gustavo")));
            Assert.True(Directory.Exists(Path.Combine(basePath, "Jade")));
            Assert.True(Directory.Exists(Path.Combine(basePath, "Kamron")));
        }
        
        [Fact]
        public void StaticBatchFolderCreationWorksUsingPathAsGiven() {
            var customBasePath = Path.Combine(basePath, "Batch");
            var gustavoPath = Path.Combine(customBasePath, "Gustavo");
            var jadePath = Path.Combine(customBasePath, "Jade");
            var kamronPath = Path.Combine(customBasePath, "Kamron");
            IEnumerable<string> folderList = new List<string>
            {
                gustavoPath,
                jadePath,
                kamronPath
            };
            FolderService.BatchCreateFoldersAtPaths(folderList);
            Assert.True(Directory.Exists(gustavoPath));
            Assert.True(Directory.Exists(jadePath));
            Assert.True(Directory.Exists(kamronPath));
        }

        [Fact]
        public void CreateFolderRespectsKeepPreferences() {
            folderService.CreateFolder("KeepFolder");
            var path = Path.Combine(basePath, "KeepFolder");
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, ".keep")));
            
            var folderService2 = new FolderService(basePath, createKeepFiles: false);
            folderService2.CreateFolder("DontKeepFolder");
            path = Path.Combine(basePath, "DontKeepFolder");
            Assert.True(Directory.Exists(path));
            Assert.False(File.Exists(Path.Combine(path, ".keep")));
        }
        
        [Fact]
        public void StaticCreateFolderRespectsKeepPreferences() {
            var path = Path.Combine(basePath, "StaticKeepFolder");
            FolderService.CreateFolderAtPath(path, createKeepFiles: true);
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, ".keep")));
            
            path = Path.Combine(basePath, "StaticDontKeepFolder");
            FolderService.CreateFolderAtPath(path, createKeepFiles: false);
            Assert.True(Directory.Exists(path));
            Assert.False(File.Exists(Path.Combine(path, ".keep")));
        }

        [Fact]
        public void CreateFolderBlocksExtensions() {
            var folderName = "ExtensionFolder.sh";
            var path = Path.Combine(basePath, folderName);
            folderService.CreateFolder(folderName);
            Assert.False(Directory.Exists(path));
        }
        
        [Fact]
        public void StaticCreateFolderBlocksExtensions() {
            var folderName = "ExtensionFolder.sh";
            var path = Path.Combine(basePath, folderName);
            FolderService.CreateFolderAtPath(path);
            Assert.False(Directory.Exists(path));
        }
        #endregion
        
        #region Renaming

        [Fact]
        public void FolderRenamingRemovesOriginalFolder() {
            folderService.CreateFolder("FolderToRename");
            Assert.True(Directory.Exists(Path.Combine(basePath, "FolderToRename")));
            folderService.RenameFolder("FolderToRename", "RenamedFolder");
            Assert.False(Directory.Exists(Path.Combine(basePath, "FolderToRename")));
            Assert.True(Directory.Exists(Path.Combine(basePath, "RenamedFolder")));
            Assert.True(File.Exists(Path.Combine(basePath, "RenamedFolder", ".keep")));
        }

        [Fact]
        public void CannotRenameNonexistentFolder() {
            var newFolderName = Guid.NewGuid().ToString();
            var newPath = Path.Combine(basePath, newFolderName);
            Assert.False(Directory.Exists(newPath));
            folderService.RenameFolder(Guid.NewGuid().ToString(), newFolderName);
            Assert.False(Directory.Exists(newPath));
        }

        [Fact]
        public void CannotRenameFolderWithExtension() {
            var oldFolderName = Guid.NewGuid() + ".sh";
            var newFolderName = Guid.NewGuid().ToString();
            var oldPath = Path.Combine(basePath, oldFolderName);
            var newPath = Path.Combine(basePath, newFolderName);
            
            folderService.CreateFolder(oldFolderName);
            Assert.False(Directory.Exists(oldPath));
            
            folderService.RenameFolder(oldFolderName, newFolderName);
            Assert.False(Directory.Exists(newPath));
        }
        
        [Fact]
        public void CannotRenameFolderToHaveExtension() {
            var oldFolderName = Guid.NewGuid().ToString();
            var newFolderName = Guid.NewGuid().ToString();
            var oldPath = Path.Combine(basePath, oldFolderName);
            var newPath = Path.Combine(basePath, newFolderName, ".png");
            
            folderService.CreateFolder(oldFolderName);
            Assert.True(Directory.Exists(oldPath));
            
            folderService.RenameFolder(oldFolderName, newFolderName);
            Assert.False(Directory.Exists(newPath));
        }
        
        [Fact]
        public void StaticFolderRenamingRemovesOriginalFolder() {
            var renameFolderPath = Path.Combine(basePath, "FolderToRenameStatic");
            var newFolderPath = Path.Combine(basePath, "RenamedFolderStatic");
            
            FolderService.CreateFolderAtPath(renameFolderPath);
            Assert.True(Directory.Exists(renameFolderPath));
            
            FolderService.RenameFolderAtGivenPaths(renameFolderPath, newFolderPath);
            Assert.False(Directory.Exists(renameFolderPath));
            Assert.True(Directory.Exists(newFolderPath));
            Assert.True(File.Exists(Path.Combine(newFolderPath, ".keep")));
        }

        [Fact]
        public void CannotStaticRenameNonexistentFolder() {
            var newFolderName = Guid.NewGuid().ToString();
            var newPath = Path.Combine(basePath, newFolderName);
            Assert.False(Directory.Exists(newPath));
            FolderService.RenameFolderAtGivenPaths(Guid.NewGuid().ToString(), newPath);
            Assert.False(Directory.Exists(newPath));
        }

        [Fact]
        public void CannotStaticRenameFolderWithExtension() {
            var oldFolderName = Guid.NewGuid() + ".sh";
            var newFolderName = Guid.NewGuid().ToString();
            var oldPath = Path.Combine(basePath, oldFolderName);
            var newPath = Path.Combine(basePath, newFolderName);
            
            FolderService.CreateFolderAtPath(oldPath);
            Assert.False(Directory.Exists(oldPath));
            
            folderService.RenameFolder(oldPath, newPath);
            Assert.False(Directory.Exists(newPath));
        }
        
        [Fact]
        public void CannotStaticRenameFolderToHaveExtension() {
            var oldFolderName = Guid.NewGuid().ToString();
            var newFolderName = Guid.NewGuid().ToString();
            var oldPath = Path.Combine(basePath, oldFolderName);
            var newPath = Path.Combine(basePath, newFolderName, ".png");
            
            FolderService.CreateFolderAtPath(oldPath);
            Assert.True(Directory.Exists(oldPath));
            
            FolderService.RenameFolderAtGivenPaths(oldPath, newPath);
            Assert.False(Directory.Exists(newPath));
        }

        [Fact]
        public void CanBatchRenameFolders() {
            Dictionary<string, string> folderRenames = new Dictionary<string, string>
            {
                { "JadeFolderRename", "JadeFolderRenamed" },
                { "GustavoFolderRename", "GustavoFolderRenamed" },
                { "KamronFolderRename", "KamronFolderRenamed" },
                { "LindsayFolderRename", "LindsayFolderRenamed" },
            };

            foreach (string key in folderRenames.Keys) {
                folderService.CreateFolder(key);
            }

            foreach (var pair in folderRenames) {
                Assert.True(Directory.Exists(Path.Join(basePath, pair.Key)));
                Assert.False(Directory.Exists(Path.Join(basePath, pair.Value)));
                folderService.RenameFolder(pair.Key, pair.Value);
                Assert.True(Directory.Exists(Path.Join(basePath, pair.Value)));
                Assert.False(Directory.Exists(Path.Join(basePath, pair.Key)));
            }
        }
        
        [Fact]
        public void CanStaticBatchRenameFolders() {
            Dictionary<string, string> folderRenames = new Dictionary<string, string>
            {
                { Path.Join(basePath, "JadeFolderRename"), Path.Join(basePath, "JadeFolderRenamed") },
                { Path.Join(basePath, "GustavoFolderRename"), Path.Join(basePath, "GustavoFolderRenamed") },
                { Path.Join(basePath, "KamronFolderRename"), Path.Join(basePath, "KamronFolderRenamed") },
                { Path.Join(basePath, "LindsayFolderRename"), Path.Join(basePath, "LindsayFolderRenamed") },
            };

            foreach (string key in folderRenames.Keys) {
                FolderService.CreateFolderAtPath(key);
            }

            foreach (var pair in folderRenames) {
                Assert.True(Directory.Exists(pair.Key));
                Assert.False(Directory.Exists(pair.Value));
                FolderService.RenameFolderAtGivenPaths(pair.Key, pair.Value);
                Assert.True(Directory.Exists(pair.Value));
                Assert.False(Directory.Exists(pair.Key));
            }
        }
        #endregion
        
        public void Dispose() {
            if (Directory.Exists(basePath)) {
                Directory.Delete(basePath, true);
            }
        }
    }
}