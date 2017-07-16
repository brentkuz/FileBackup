using System;
using System.IO;

namespace FileBackup.Utility.Hashing
{
    public interface IHashedFile : IComparable
    {
        string Hash { get; }
        string Path { get; }
    }
    public class HashedFile : IHashedFile
    {
        private string hash;
        private string path;

        public HashedFile(string path) : this(path, new MD5Hash(), new FileHelper()) { }
        public HashedFile(string path, IHashingStrategy alg, IFileHelper fileHelper)
        {
            if (!fileHelper.Exists(path))
                throw new ArgumentException("HashedFile: Invalid path");
            byte[] bytes = fileHelper.ReadAllBytes(path);
            hash = alg.GetHash(bytes);
            this.path = path; 
        }

        public string Hash
        {
            get { return hash; }
        }

        public string Path
        {
            get { return path; }
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(HashedFile))
                throw new ArgumentException("Can only compare to HashedFile type.");
            HashedFile cmpr = (HashedFile)obj;
            return hash.CompareTo(cmpr.Hash);
        }
    }
}
