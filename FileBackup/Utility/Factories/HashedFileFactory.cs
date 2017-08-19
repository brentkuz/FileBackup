using FileBackup.Utility.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.Utility.Factories
{
    public interface IHashedFileFactory
    {
        IHashedFile Create(string path);
    }

    public class HashedFileFactory : IHashedFileFactory
    {
        public IHashedFile Create(string path)
        {
            //TODO: Inject
            return new HashedFile(path, new MD5Hash(), new FileHelper());
        }
    }
}
