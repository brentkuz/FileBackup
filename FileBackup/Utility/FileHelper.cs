using System;
using System.IO;
using System.Linq;

namespace FileBackup.Utility
{
    public interface IFileHelper
    {
        bool IsHidden(string path);
        string GetFilename(string path);
        string BuildPath(string root, string filename);
        string[] ReadAllLines(string filePath);
        byte[] ReadAllBytes(string filePath);
        bool Exists(string filePath);
    }
    public class FileHelper : IFileHelper
    {
        public bool IsHidden(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden;
        }
        public string GetFilename(string path)
        {
            var parts = path.Split('\\');
            var name = parts[parts.Length - 1];
            if (!name.Contains('.'))
                throw new ArgumentException("Path does not specify a file.");
            return name;
        }
        public string BuildPath(string root, string filename)
        {
            return root + "\\" + filename;
        }
        public string[] ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
        public byte[] ReadAllBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
