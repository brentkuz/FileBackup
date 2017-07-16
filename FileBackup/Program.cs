using FileBackup.FileObserver;
using FileBackup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FileBackup.Utility.Hashing;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace FileBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            string INDEX = ConfigurationManager.AppSettings["indexFileName"];
            int delay = Convert.ToInt16(ConfigurationManager.AppSettings["delayTime"]);
            var path = GetProjectDir() + INDEX;

            
           
            Run(path, delay);

            Thread.Sleep(Timeout.Infinite);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run(string path, int delay)
        {
            var obs = new PathObserver(path, delay);           
        }


        private static string GetProjectDir()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var bin = dir.IndexOf("bin");
            if(bin > 0)
            {
                dir = dir.Substring(0, bin);
            }
            return dir;
        }
    }
}
