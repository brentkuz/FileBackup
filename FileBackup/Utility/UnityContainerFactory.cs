//Author: Brent Kuzmanich
//Comment:

using FileBackup.FileBackup;
using FileBackup.FileBackup.Factories;
using FileBackup.FileObserver;
using FileBackup.FileObserver.Factories;
using FileBackup.Utility.Factories;
using FileBackup.Utility.Hashing;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.Utility
{
    public interface IUnityContainerFactory
    {
        IUnityContainer GetContainer();
    }
    public class UnityContainerFactory : IUnityContainerFactory
    {
        public IUnityContainer GetContainer()
        {
            var container = new UnityContainer();
            Register(container);
            return container;
        }
        private void Register(IUnityContainer container)
        {
            //factories
            container.RegisterType<IBackupManagerFactory, BackupManagerFactory>();
            container.RegisterType<IIndexFactory, IndexFactory>();
            container.RegisterType<IRepositoryFactory, RepositoryFactory>();
            container.RegisterType<IPathSubjectFactory, PathSubjectFactory>();
            container.RegisterType<IPathObserverFactory, PathObserverFactory>();
            container.RegisterType<IHashedFileFactory, HashedFileFactory>();
            container.RegisterType<IUnityContainerFactory, UnityContainerFactory>();
            //classes
            container.RegisterType<IBackupManager, BackupManager>();
            container.RegisterType<IIndex, Index>();
            container.RegisterType<IPathObserver, PathObserver>();
            container.RegisterType<IPathSubject, PathSubject>();
            container.RegisterType<IHashedFile, HashedFile>();
            container.RegisterType<IHashingStrategy, MD5Hash>();
            container.RegisterType<IFileHelper, FileHelper>();
            container.RegisterType<ILogger, Logger>();

            
        }
    }
}
