using System;
using System.IO;
using Xunit;

namespace FolderStructure.Core {
    public sealed class FilesTests : IDisposable {

        private readonly string basePath;
        private readonly Files files;

        public FilesTests() {
            basePath = Path.Combine(Path.GetTempPath(), "UnityWizardTests", Guid.NewGuid().ToString());
            files = new Files(basePath);
        }
        
        
        [Fact]
        public void BasePathIsUsedForNewFolders() {
            files.CreateFolder("CustomFolder");
            var expectedPath = Path.Combine(basePath, "CustomFolder");
            Assert.True(Directory.Exists(expectedPath));
        }

        [Fact]
        public void CreatingAlreadyExistentFolderIsIdempotent() {
            var path = Path.Combine(basePath, "Repeat");
            files.CreateFolder("Repeat");
            File.WriteAllText(Path.Combine(path, "test.txt"), "hello");
            
            files.CreateFolder("Repeat");
            
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, "test.txt")));
        }
        
        [Fact]
        public void StaticallyCreatingAlreadyExistentFolderIsIdempotent() {
            var path = Path.Combine(basePath, "Repeat");
            Files.CreateFolderAssumingBasePath(path);
            File.WriteAllText(Path.Combine(path, "test.txt"), "hello");
            
            Files.CreateFolderAssumingBasePath(path);
            
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, "test.txt")));
        }
        
        [Fact]
        public void ConstructorCreatingAlreadyExistentFolderIsIdempotent() {
            var path = Path.Combine(basePath, "Repeat");
            Files.CreateFolderAssumingBasePath(path);
            File.WriteAllText(Path.Combine(path, "test.txt"), "hello");
            
            var files2 = new Files(path);
            
            Assert.True(Directory.Exists(path));
            Assert.True(File.Exists(Path.Combine(path, "test.txt")));
        }

        [Fact]
        public void StaticFolderCreationWorksUsingPathAsGiven() {
            var path = Path.Combine(basePath, "StaticFolder");
            Files.CreateFolderAssumingBasePath(path);
            Assert.True(Directory.Exists(path));
        }

        public void Dispose() {
            if (Directory.Exists(basePath)) {
                Directory.Delete(basePath, true);
            }
        }
    }
}