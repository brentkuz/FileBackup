//Author: Brent Kuzmanich
//Comment: Factory for DI

using FileBackup.Utility;
using System;

namespace FileBackup.FileObserver.Factories
{
    public interface IPathSubjectFactory
    {
        IPathSubject GetPathSubject(Guid id, string path);
    }
    public class PathSubjectFactory : IPathSubjectFactory
    {
        private IFileHelper fileHelper;
        public PathSubjectFactory(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }
        public IPathSubject GetPathSubject(Guid id, string path)
        {
            return new PathSubject(id, path, fileHelper);
        }
    }
}
