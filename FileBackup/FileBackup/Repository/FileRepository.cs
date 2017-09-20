//Author: Brent Kuzmanich
//Comment: File system repository for persisting changes to source files

using System;
using System.IO;

namespace FileBackup.FileBackup.Repository
{
    public interface IRepository : IDisposable
    {
        void Insert(string sourcePath, string destPath);
        void Update(string sourcePath, string destPath);
        void Delete(string path);
        void Rename(string oldPath, string newPath);
    }

    public class FileSystemRepository : IRepository
    {
        //Add file to dest path
        public void Insert(string sourcePath, string destPath)
        {
            if (!File.Exists(sourcePath))
                throw new ArgumentException("FileRepository.Insert - Invalid source path");
            File.Copy(sourcePath, destPath, false);
        }
        //Copy over changes to dest 
        public void Update(string sourcePath, string destPath)
        {
            if (!File.Exists(sourcePath))
                throw new ArgumentException("FileRepository.Update - Invalid source path");
            if (!File.Exists(destPath))
                throw new ArgumentException("FileRepository.Update - Invalid destination path");
            File.Copy(sourcePath, destPath, true);
        }
        //Delete file at path
        public void Delete(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("FileRepository.Delete - Invalid path");
            File.Delete(path);
        }
        //Update name
        public void Rename(string oldPath, string newPath)
        {
            if (!File.Exists(oldPath))
                throw new ArgumentException("FileRepository.Delete - Invalid path");
            File.Move(oldPath, newPath);
        }



        public void Dispose()
        {
            return;
        }
    }
}
