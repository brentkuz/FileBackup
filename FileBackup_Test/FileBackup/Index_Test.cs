using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileBackup.Utility;
using FileBackup.FileBackup;

namespace FileBackup_Test.FileBackup
{
    [TestClass]
    public class Index_Test
    {
        [TestMethod]
        public void Index_GetPath_ReturnsPaths()
        {
            var id = new Guid("d67fb962-856b-4e84-93fa-0d9237091eed");
            var lines = new string[] { @"d67fb962-856b-4e84-93fa-0d9237091eed	C:\Users\brentkuz\Documents\FileBackup\Files\Test1	C:\Users\brentkuz\Documents\FileBackup\Backup\Test1" };
            var fh = new Mock<IFileHelper>();
            fh.Setup(m => m.ReadAllLines(It.IsAny<string>())).Returns(lines);
            fh.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            var index = new Index("~", fh.Object);

            var source = index.GetPath(id, PathType.Source);
            var back = index.GetPath(id, PathType.Backup);

            Assert.AreEqual(@"C:\Users\brentkuz\Documents\FileBackup\Files\Test1", source);
            Assert.AreEqual(@"C:\Users\brentkuz\Documents\FileBackup\Backup\Test1", back);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Index_GetPath_ThrowsExForInvalidPath()
        {            
            var fh = new Mock<IFileHelper>();
            fh.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
            var index = new Index("~", fh.Object);            
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Index_GetPath_InvalidGuidThrowsEx()
        {
            var id = new Guid("d67fb962-856b-4e84-93fa-0d9237091aaa");
            var lines = new string[] { @"d67fb962-856b-4e84-93fa-0d9237091eed	C:\Users\brentkuz\Documents\FileBackup\Files\Test1	C:\Users\brentkuz\Documents\FileBackup\Backup\Test1" };
            var fh = new Mock<IFileHelper>();
            fh.Setup(m => m.ReadAllLines(It.IsAny<string>())).Returns(lines);
            fh.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            var index = new Index("~", fh.Object);

            var source = index.GetPath(id, PathType.Source);
        }
    }
}
