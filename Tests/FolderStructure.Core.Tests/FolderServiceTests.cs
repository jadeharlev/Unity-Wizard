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
            folderService = new FolderService(basePath);
        }
        
        
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

        public void Dispose() {
            if (Directory.Exists(basePath)) {
                Directory.Delete(basePath, true);
            }
        }
    }
}