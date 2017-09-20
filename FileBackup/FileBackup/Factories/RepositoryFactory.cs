//Author: Brent Kuzmanich
//Comment: Factory for DI

using FileBackup.FileBackup.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.FileBackup.Factories
{
    public enum RepositoryType
    {
        File
    }
    public interface IRepositoryFactory
    {
        IRepository GetRepository(RepositoryType type);
    }
    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository GetRepository(RepositoryType type)
        {
            //TODO: inject
            switch (type)
            {
                case RepositoryType.File:
                    return new FileSystemRepository();
                default:
                    return null;
            }
        }
    }
}
