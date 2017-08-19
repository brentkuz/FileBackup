using FileBackup.FileBackup;
using FileBackup.FileBackup.Factories;
using FileBackup.FileObserver.Factories;
using FileBackup.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileBackup.FileObserver
{

    public interface IPathObserver : IDisposable
    {
        void Update(Guid id, string path, ChangeType.Type changeType, params object[] list);
        void Attach(IPathSubject sub);
    }

    public class PathObserver : IPathObserver
    {
        private bool disposed = false;
        private int delay;
        
        private List<IPathSubject> subs = new List<IPathSubject>();
        private IBackupManager manager;
        private ILogger logger;
       

        //public PathObserver(string indexPath, int delay, RepositoryType reposType) : this(indexPath, delay, reposType, new IndexFactory(), new BackupManagerFactory(), new Logger(), new FileHelper(), new PathSubjectFactory()) { }
        public PathObserver(string indexPath, int delay, RepositoryType reposType, IIndexFactory indexFactory, IBackupManagerFactory backupFactory,
            ILogger logger, IFileHelper fileHelper, IPathSubjectFactory pathSubjectFactory)
        {
            this.delay = delay;
            var index = indexFactory.GetIndex(indexPath);
            this.manager = backupFactory.GetBackupManager(index, reposType);
            this.logger = logger;
           
            foreach (var i in index.Values())
            {
                var parts = i.Value;
                if(!Directory.Exists(parts[0]) || !Directory.Exists(parts[1]))
                {
                    logger.Log(System.Diagnostics.EventLogEntryType.Warning, null, new ArgumentException("Could not create binding to path: " + parts[1]));
                    continue;
                }
                var sub = pathSubjectFactory.GetPathSubject(i.Key, parts[0]);
                Attach(sub);
            }
            
        }

        public bool Disposed { get { return disposed; } }

        public void Update(Guid id, string path, ChangeType.Type changeType, params object[] list)
        {
            try
            {
                //delay execution to prevent "File in use..." error
                Thread.Sleep(delay);
                switch (changeType)
                {
                    case ChangeType.Type.Changed:
                        {
                            manager.ProcessChanged(id, path);
                            break;
                        }
                    case ChangeType.Type.Created:
                        {
                            manager.ProcessCreated(id, path);
                            break;
                        }
                    case ChangeType.Type.Deleted:
                        {
                            manager.ProcessDeleted(id, path);
                            break;
                        }
                    case ChangeType.Type.Renamed:
                        {
                            manager.ProcessRenamed(id, path, list[0].ToString());
                            break;
                        }
                    case ChangeType.Type.Error:
                        {
                            var msg = "The file backup has failed for path: " + path;
                            logger.Log(System.Diagnostics.EventLogEntryType.Error, msg, (Exception)list[0]);
                            break;
                        }
                    case ChangeType.Type.Disposed:
                        {
                            var msg = "The file backup binding was disposed for path: " + path;
                            logger.Log(System.Diagnostics.EventLogEntryType.Warning, msg);

                            //TODO: Attempt restart for path

                            break;
                        }
                }
            }
            catch(Exception ex)
            {
                var msg = "The file backup has failed for path: " + path;
                logger.Log(System.Diagnostics.EventLogEntryType.Error, msg, ex);
            }
        }

        public void Attach(IPathSubject sub)
        {
            sub.SetObserver(this);
            subs.Add(sub);
        }


        #region IDisposable      
        protected void Dispose(bool disposing)
        {
            if(!disposed && disposing)
            {
                foreach(var sub in subs)
                {
                    sub.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
