using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(string sourcePath, string destPath)
        {
            if (!File.Exists(sourcePath))
                throw new ArgumentException("FileRepository.Insert - Invalid source path");
            File.Copy(sourcePath, destPath, false);
        }
        public void Update(string sourcePath, string destPath)
        {
            if (!File.Exists(sourcePath))
                throw new ArgumentException("FileRepository.Update - Invalid source path");
            if (!File.Exists(destPath))
                throw new ArgumentException("FileRepository.Update - Invalid destination path");
            File.Copy(sourcePath, destPath, true);
        }
        public void Delete(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("FileRepository.Delete - Invalid path");
            File.Delete(path);
        }
        public void Rename(string oldPath, string newPath)
        {
            if (!File.Exists(oldPath))
                throw new ArgumentException("FileRepository.Delete - Invalid path");
            File.Move(oldPath, newPath);
        }
    }
}
