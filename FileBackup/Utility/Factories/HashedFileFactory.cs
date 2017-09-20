//Author: Brent Kuzmanich
//Comment: Factory for DI

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
        private IHashingStrategy hash;
        private IFileHelper fileHelper;
        public HashedFileFactory(IHashingStrategy hash, IFileHelper fileHelper)
        {
            this.hash = hash;
            this.fileHelper = fileHelper;
        }
        public IHashedFile Create(string path)
        {
            //TODO: Inject
            return new HashedFile(path, hash, fileHelper);
        }
    }
}
