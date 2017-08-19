using FileBackup.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.FileObserver
{
    public interface IPathSubject : IDisposable
    {
        void SetObserver(IPathObserver observer);
    }


    public class PathSubject : IPathSubject
    {      
        private bool disposed = false;
        private FileSystemWatcher watcher = new FileSystemWatcher();
        private Guid id;
        private string path;
        private int retries = 0;
        private IPathObserver observer;
        private IFileHelper fileHelper;

        
        public PathSubject(Guid id, string path)
        {
            //inject
            fileHelper = new FileHelper();
            this.id = id;
            this.path = path;
            WireUp();
        }


        public void SetObserver(IPathObserver observer)
        {
            this.observer = observer;
        }

        private void WireUp()
        {
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
              | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            
            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.Error += new ErrorEventHandler(OnError);
            
            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        private void Notify(string path, ChangeType.Type eventType, params object[] list)
        {
            observer.Update(id: id, path: path, changeType: eventType, list: list);
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var type = e.ChangeType.ToString().ToChangeType();
            if (IsValid(e.FullPath) || type == ChangeType.Type.Deleted)
            {                
                Notify(fileHelper.GetFilename(e.FullPath), type);
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            if (IsValid(e.FullPath))
            {
                var type = e.ChangeType.ToString().ToChangeType();
                Notify(path: fileHelper.GetFilename(e.FullPath), eventType: type, list: fileHelper.GetFilename(e.OldFullPath));
            }
        }

        private void OnError(object source, ErrorEventArgs e)
        {            
            if (retries < 10)
            {
                watcher.Dispose();
                watcher = new FileSystemWatcher();
                WireUp();
            }
            else
            {
                //Bubble up exception
                var ex = e.GetException();                
                Notify(path, ChangeType.Type.Error, new object[] { ex });
            }            
        }
        //private bool IsDuplicateEvent(string path)
        //{
            
        //}
        private bool IsValid(string path)
        {
            //file validation goes here
            return !fileHelper.IsHidden(path);
        }
        

        #region IDisposable
        protected void Dispose(bool disposing)
        {
            if(!disposed && disposing)
            {
                watcher.Dispose();
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
