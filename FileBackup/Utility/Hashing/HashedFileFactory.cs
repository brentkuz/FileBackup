using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.Utility.Hashing
{
    public interface IHashedFileFactory
    {
        IHashedFile Create(string path);
    }

    public class HashedFileFactory : IHashedFileFactory
    {
        public IHashedFile Create(string path)
        {
            return new HashedFile(path);
        }
    }
}
