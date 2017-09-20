//Author: Brent Kuzmanich
//Comment: Subject to monitor events for the given path

using FileBackup.Utility;
using System;
using System.IO;

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

        
        public PathSubject(Guid id, string path, IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
            this.id = id;
            this.path = path;
            WireUp();
        }
                
        public void SetObserver(IPathObserver observer)
        {
            this.observer = observer;
        }

        //Wireup FileSystemWatcher events
        private void WireUp()
        {
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
              | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            
            // Add event handlers
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.Error += new ErrorEventHandler(OnError);
            
            // Begin watching
            watcher.EnableRaisingEvents = true;
        }

        //Notify observer on event
        private void Notify(string path, ChangeType.Type eventType, params object[] list)
        {
            observer.Update(id: id, path: path, changeType: eventType, list: list);
        }

        //Handle OnChanged, OnCreated, and OnDeleted
        private void OnChanged(object source, FileSystemEventArgs e)
        {            
            var type = e.ChangeType.ToString().ToChangeType();
            if (type == ChangeType.Type.Deleted || IsValid(e.FullPath))
            {                
                Notify(fileHelper.GetFilename(e.FullPath), type);
            }
        }

        //Handle OnRenamed
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            if (IsValid(e.FullPath))
            {
                var type = e.ChangeType.ToString().ToChangeType();
                Notify(path: fileHelper.GetFilename(e.FullPath), eventType: type, list: fileHelper.GetFilename(e.OldFullPath));
            }
        }

        //Handle Error
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

        //Validate event target - Exists and not hidden
        private bool IsValid(string path)
        {
            if (!File.Exists(path))
                return false;
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
