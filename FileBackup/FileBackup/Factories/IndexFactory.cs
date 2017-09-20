//Author: Brent Kuzmanich
//Comment: Factory for DI

using FileBackup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.FileBackup.Factories
{
    public interface IIndexFactory
    {
        IIndex GetIndex(string indexPath);
    }

    public class IndexFactory : IIndexFactory
    {
        private IFileHelper fileHelper;
        public IndexFactory(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }
        public IIndex GetIndex(string indexPath)
        {
            //TODO: Inject
            return new Index(indexPath, fileHelper);
        }
    }
}
