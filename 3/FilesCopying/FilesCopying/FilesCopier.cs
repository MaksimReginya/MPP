using System;
using System.IO;
using System.Threading;

namespace FilesCopying
{
    class FilesCopier
    {
        private int threadCount = 0;
        private ManualResetEvent resetEvent = new ManualResetEvent(false);

        public FilesCopier()
        {}

        public int Copy(string targetFolder, string destinationFolder)
        {
            string[] filesPaths = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);
            threadCount = filesPaths.Length;
            Tuple<string, string> fileCouple;
            foreach (var filePath in filesPaths)
            {
                fileCouple = new Tuple<string, string>(filePath, filePath.Replace(targetFolder, destinationFolder));               
                ThreadPool.QueueUserWorkItem(CopyFile, fileCouple);                
            }

            resetEvent.WaitOne();
            resetEvent.Dispose();
            return threadCount;
        }

        private void CopyFile(object fileCouple)
        {
            var fileTuple = fileCouple as Tuple<string, string>;
            File.Copy(fileTuple.Item1, fileTuple.Item2, true);
            if (Interlocked.Decrement(ref threadCount) == 0)
            {
                resetEvent.Set();
            }
        }
    }
}
