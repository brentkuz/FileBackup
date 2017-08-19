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
        public IPathObserver GetPathObserver(string indexPath, int delay, RepositoryType reposType)
        {
            return new PathObserver(indexPath, delay, reposType, new IndexFactory(), new BackupManagerFactory(), new Logger(), new FileHelper(), new PathSubjectFactory());
        }
    }
}
