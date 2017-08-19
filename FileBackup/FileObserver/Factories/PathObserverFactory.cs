using FileBackup.FileBackup.Factories;
using FileBackup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.FileObserver.Factories
{
    public interface IPathObserverFactory
    {
        IPathObserver GetPathObserver(string indexPath, int delay, RepositoryType reposType);
    }
    public class PathObserverFactory : IPathObserverFactory
    {
        private IIndexFactory indexFactory;
        private IBackupManagerFactory backupFactory;
        private ILogger logger;
        private IFileHelper fileHelper;
        private IPathSubjectFactory pathSubjectFactory;
        public PathObserverFactory(IIndexFactory indexFactory, IBackupManagerFactory backupFactory, ILogger logger, IFileHelper fileHelper, IPathSubjectFactory pathSubjectFactory)
        {
            this.indexFactory = indexFactory;
            this.backupFactory = backupFactory;
            this.logger = logger;
            this.fileHelper = fileHelper;
            this.pathSubjectFactory = pathSubjectFactory;
        }
        public IPathObserver GetPathObserver(string indexPath, int delay, RepositoryType reposType)
        {
            return new PathObserver(indexPath, delay, reposType, indexFactory, backupFactory, logger, fileHelper, pathSubjectFactory);
        }
    }
}
