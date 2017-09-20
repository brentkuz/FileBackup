using System;

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

        //Resolve string to ChangeType
        public static Type ToChangeType(this string type)
        {            
            Type res;
            Enum.TryParse<Type>(type, out res);
            return res;            
        }
    }

}
