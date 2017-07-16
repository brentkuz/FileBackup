using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileBackup.Utility.Hashing;
using FileBackup.FileBackup.Repository;
using FileBackup.FileBackup;
using FileBackup.Utility;

namespace FileBackup_Test.FileBackup
{
    [TestClass]
    public class BackupManager_Test
    {
        [TestMethod]
        public void BackupManager_ProcessChanged_UpdatesFileIfHashChanged()
        {
            var fname = "file.txt";
            var index = new Mock<IIndex>();
            index.Setup(m => m.GetPath(It.IsAny<Guid>(), PathType.Source)).Returns("path1");
            index.Setup(m => m.GetPath(It.IsAny<Guid>(), PathType.Backup)).Returns("path2");
            var fileHelp = new Mock<IFileHelper>();
            fileHelp.Setup(m => m.BuildPath("path1", fname)).Returns("path1\\file.txt");
            fileHelp.Setup(m => m.BuildPath("path2", fname)).Returns("path2\\file.txt");
            //files
            var source = new Mock<IHashedFile>();
            source.Setup(m => m.Hash).Returns("123123");
            var back = new Mock<IHashedFile>();
            back.Setup(m => m.Hash).Returns("546456");
            source.Setup(m => m.CompareTo(back.Object)).Returns(1);

            var hsf = new Mock<IHashedFileFactory>();
            hsf.Setup(m => m.Create("path1\\file.txt")).Returns(source.Object);
            hsf.Setup(m => m.Create("path2\\file.txt")).Returns(back.Object);
            
            var repos = new Mock<IRepository>();         
            
            var mngr = new BackupManager("", repos.Object, fileHelp.Object, index.Object, hsf.Object);

            mngr.ProcessChanged(new Guid(), fname);

            repos.Verify(m => m.Update(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BackupManager_ProcessChanged_SkipsUpdateIfUnchangedHash()
        {
            var fname = "file.txt";
            var index = new Mock<IIndex>();
            index.Setup(m => m.GetPath(It.IsAny<Guid>(), PathType.Source)).Returns("path1");
            index.Setup(m => m.GetPath(It.IsAny<Guid>(), PathType.Backup)).Returns("path2");
            var fileHelp = new Mock<IFileHelper>();
            fileHelp.Setup(m => m.BuildPath("path1", fname)).Returns("path1\\file.txt");
            fileHelp.Setup(m => m.BuildPath("path2", fname)).Returns("path2\\file.txt");
            //files
            var source = new Mock<IHashedFile>();
            source.Setup(m => m.Hash).Returns("123123");
            var back = new Mock<IHashedFile>();
            back.Setup(m => m.Hash).Returns("123123");
            source.Setup(m => m.CompareTo(back.Object)).Returns(0);

            var hsf = new Mock<IHashedFileFactory>();
            hsf.Setup(m => m.Create("path1\\file.txt")).Returns(source.Object);
            hsf.Setup(m => m.Create("path2\\file.txt")).Returns(back.Object);

            var repos = new Mock<IRepository>();

            var mngr = new BackupManager("", repos.Object, fileHelp.Object, index.Object, hsf.Object);

            mngr.ProcessChanged(new Guid(), fname);

            repos.Verify(m => m.Update(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

    }
}
