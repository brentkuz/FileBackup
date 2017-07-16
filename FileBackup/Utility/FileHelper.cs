using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.Utility
{
    public interface IFileHelper
    {
        bool IsHidden(string path);
        string GetFilename(string path);
        string BuildPath(string root, string filename);
    }
    public class FileHelper : IFileHelper
    {
        public bool IsHidden(string path)
        {
            FileInfo fi = new FileInfo(path);
            return fi.Attributes.HasFlag(FileAttributes.Hidden);
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
    }
}
