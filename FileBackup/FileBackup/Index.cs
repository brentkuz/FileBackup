//Author: Brent Kuzmanich
//Comment: Abstraction of the source.idx file used for resolving
//source to destination and vice versa.

using FileBackup.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileBackup.FileBackup
{
    public enum PathType
    {
        Source = 0,
        Backup = 1
    }
    public interface IIndex 
    {
        Dictionary<Guid, string[]> Values();
        string GetPath(Guid id, PathType type);
    }
    public class Index : IIndex
    {
        //Key: Guid; Value: string[] {source, backup}
        private Dictionary<Guid, string[]> index = new Dictionary<Guid, string[]>();
        
        public Index(string indexPath, IFileHelper fileHelper)
        {
            if (!fileHelper.Exists(indexPath))
                throw new ArgumentException("Index - Index path is invalid");
            //create lookup
            var file = fileHelper.ReadAllLines(indexPath);
            foreach (var f in file)
            {
                var parts = f.Split('\t');
                index.Add(new Guid(parts[0]), new string[] { parts[1], parts[2] });
            }
        }

        public Dictionary<Guid, string[]> Values()
        {
            return index;
        }

        //Resolve path
        public string GetPath(Guid id, PathType type)
        {
            if (!index.Keys.Contains(id))
                throw new ArgumentException("Index - Invalid Id");
            return index[id][(int)type];
        }
    }
}
