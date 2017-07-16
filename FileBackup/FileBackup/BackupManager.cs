using FileBackup.FileBackup.Repository;
using FileBackup.Utility;
using FileBackup.Utility.Hashing;
using System;

namespace FileBackup.FileBackup
{
    public interface IBackupManager : IDisposable
    {
        void ProcessChanged(Guid id, string filename);
        void ProcessCreated(Guid id, string filename);
        void ProcessRenamed(Guid id, string newFilename, string oldFilename);
        void ProcessDeleted(Guid id, string filename);

    }
    public class BackupManager : IBackupManager
    {
        private bool disposed = false;
        private IRepository repos;
        private IIndex index;
        private IFileHelper fileHelper;
        private IHashedFileFactory hashedFileFactory;

        public BackupManager(string indexPath) 
            : this(indexPath, new FileSystemRepository(), new FileHelper(), new Index(indexPath), new HashedFileFactory()) { }
        public BackupManager(string indexPath, IRepository repos, IFileHelper fileHelper, IIndex index, IHashedFileFactory hashedFileFactory)
        {
            this.repos = repos;
            this.fileHelper = fileHelper;
            this.index = index;
            this.hashedFileFactory = hashedFileFactory;
        }


        public void ProcessChanged(Guid id, string filename)
        {
            //get paths
            var fFile = hashedFileFactory.Create(fileHelper.BuildPath(index.GetPath(id, PathType.Source), filename));
            var bFile = hashedFileFactory.Create(fileHelper.BuildPath(index.GetPath(id, PathType.Backup), filename));
            //compare files and copy over if changed
            var cmpr = fFile.CompareTo(bFile);
            if (fFile.CompareTo(bFile) != 0)
            {
                Console.WriteLine("Changed: " + filename);
                repos.Update(fFile.Path, bFile.Path);
            }
        }

        public void ProcessCreated(Guid id, string filename)
        {
            Console.WriteLine("Created: " + filename);
            var dest = fileHelper.BuildPath(index.GetPath(id, PathType.Backup), filename);
            var source = fileHelper.BuildPath(index.GetPath(id, PathType.Source), filename);
            if (!fileHelper.Exists(source))
                throw new Exception("BackupManager.ProcessCreated - Invalid source path");
            repos.Insert(source, dest);
        }

        public void ProcessRenamed(Guid id, string newFilename, string oldFilename)
        {
            Console.WriteLine("Renamed: " + oldFilename + " -> " + newFilename);
            var oldPath = fileHelper.BuildPath(index.GetPath(id, PathType.Backup), oldFilename);
            var newPath = fileHelper.BuildPath(index.GetPath(id, PathType.Backup), newFilename);
            if (!fileHelper.Exists(oldPath))
                throw new Exception("BackupManager.ProcessRenamed - Invalid old path");
            repos.Rename(oldPath, newPath);
        }

        public void ProcessDeleted(Guid id, string filename)
        {
            Console.WriteLine("Deleted: " + filename);
            var path = fileHelper.BuildPath(index.GetPath(id, PathType.Backup), filename);
            if (!fileHelper.Exists(path))
                throw new Exception("BackupManager.ProcessDeleted - Invalid delete target path");
            repos.Delete(path);
        }


        #region IDisposable
        protected void Dispose(bool disposing)
        {
            if(!disposed && disposing)
            {
                repos.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
