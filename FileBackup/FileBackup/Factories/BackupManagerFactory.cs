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
        public IBackupManager GetBackupManager(IIndex index, RepositoryType reposType)
        {
            //TODO: inject
            return new BackupManager(new RepositoryFactory(), new FileHelper(),index, new HashedFileFactory(), reposType);
        }
    }
}
