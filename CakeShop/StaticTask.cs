using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop
{
    class StaticTask
    {
        private static StaticTask _instance = null;

        public void cleanFileInDirectory(string Path)
        {
            //DirectoryInfo directoryInfo = new DirectoryInfo(Path);
            //foreach (FileInfo file in directoryInfo.GetFiles())
            //{
            //    file.Delete();

            //}

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "CleanFolder.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $"{Path}";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }

        }

        public static StaticTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StaticTask();
                }
                return _instance;
            }
        }

        private StaticTask()
        {

        }
    }
}
