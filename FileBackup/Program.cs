//Author: Brent Kuzmanich
//Comment: Main app entry point

using FileBackup.FileObserver;
using FileBackup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FileBackup.Utility.Hashing;
using System.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.Practices.Unity;
using FileBackup.FileBackup.Factories;
using FileBackup.FileBackup.Repository;
using FileBackup.FileObserver.Factories;
using FileBackup.Utility.Factories;
using FileBackup.FileBackup;

namespace FileBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            //init
            var container = new UnityContainer();
            RegisterContainer(container);            

            string INDEX = ConfigurationManager.AppSettings["indexFileName"];
            int delay = Convert.ToInt16(ConfigurationManager.AppSettings["delayTime"]);
            var path = GetProjectDir() + INDEX;

            //start and run until closed
            var obsFact = container.Resolve<IPathObserverFactory>();
            Run(obsFact, path, delay, RepositoryType.File);
            Thread.Sleep(Timeout.Infinite);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run(IPathObserverFactory fact, string path, int delay, RepositoryType type)
        {            
            var obs = fact.GetPathObserver(path, delay, type);           
        }

        private static string GetProjectDir()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var bin = dir.IndexOf("bin");
            if(bin > 0)
            {
                dir = dir.Substring(0, bin);
            }
            return dir;
        }
        private static void RegisterContainer(UnityContainer container)
        {
            //factories
            container.RegisterType<IBackupManagerFactory, BackupManagerFactory>();
            container.RegisterType<IIndexFactory, IndexFactory>();
            container.RegisterType<IRepositoryFactory, RepositoryFactory>();
            container.RegisterType<IPathSubjectFactory, PathSubjectFactory>();
            container.RegisterType<IPathObserverFactory, PathObserverFactory>();
            container.RegisterType<IHashedFileFactory, HashedFileFactory>();
            //classes
            container.RegisterType<IBackupManager, BackupManager>();
            container.RegisterType<IIndex, Index>();
            container.RegisterType<IPathObserver, PathObserver>();
            container.RegisterType<IPathSubject, PathSubject>();
            container.RegisterType<IHashedFile, HashedFile>();
            container.RegisterType<IFileHelper, FileHelper>();
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IHashingStrategy, MD5Hash>();
        }
    }
}
