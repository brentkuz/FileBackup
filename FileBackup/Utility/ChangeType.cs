using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup.Utility
{
    public static class ChangeType
    {
        public enum Type
        {
            Changed,
            Created,
            Deleted,
            Disposed,
            Error,
            Renamed
        }

        public static Type ToChangeType(this string type)
        {            
            Type res;
            Enum.TryParse<Type>(type, out res);
            return res;            
        }
    }

}
