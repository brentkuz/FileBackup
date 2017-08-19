using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.FileObserver.Factories
{
    public interface IPathSubjectFactory
    {
        IPathSubject GetPathSubject(Guid id, string path);
    }
    public class PathSubjectFactory : IPathSubjectFactory
    {
        public IPathSubject GetPathSubject(Guid id, string path)
        {
            return new PathSubject(id, path);
        }
    }
}
