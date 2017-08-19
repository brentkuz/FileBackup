using FileBackup.Utility;
using FileBackup.Utility.Factories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.FileBackup.Factories
{
    public interface IBackupManagerFactory
    {
        IBackupManager GetBackupManager(IIndex index, RepositoryType reposType);
    }

    public class BackupManagerFactory : IBackupManagerFactory
    {
        private IRepositoryFactory reposFactory;
        private IFileHelper fileHelper;
        private IHashedFileFactory hashedFileFactory;
        public BackupManagerFactory(IRepositoryFactory reposFactory, IFileHelper fileHelper, IHashedFileFactory hashedFileFactory)
        {
            this.reposFactory = reposFactory;
            this.fileHelper = fileHelper;
            this.hashedFileFactory = hashedFileFactory;
        }
        public IBackupManager GetBackupManager(IIndex index, RepositoryType reposType)
        {
            //TODO: inject
            return new BackupManager(reposFactory, fileHelper,index, hashedFileFactory, reposType);
        }
    }
}
