//Author: Brent Kuzmanich
//Comment: Hash function implementation

using System;
using System.Security.Cryptography;
using System.Text;

namespace FileBackup.Utility.Hashing
{
    public interface IHashingStrategy 
    {
        string GetHash(byte[] buffer);
    }
    public class MD5Hash : IHashingStrategy
    {
        public string GetHash(byte[] buffer)
        {
            using (var md5 = MD5.Create())
            { 
                return Encoding.Default.GetString(md5.ComputeHash(buffer));
            }
        }
    }
}
