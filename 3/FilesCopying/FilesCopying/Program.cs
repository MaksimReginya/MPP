using System;
using System.IO;
using System.Threading;

namespace FilesCopying
{
    static class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: [source_folder] [dest_folder]");
                return 1;
            }
            string sourceFolder = args[0];
            string destFolder = args[1];

            FilesCopier copier = new FilesCopier();
            Console.WriteLine("Copying...");
            int copiedFilesCount = copier.StartCopying(sourceFolder, destFolder);
            Console.WriteLine("Number of copied files: {0}", copiedFilesCount);            
            return 0;
        }
    }

    class FilesCopier
    {
        private enum CopyType
        {
            Directory,
            File
        }
        private int count = 0;
        private int result = 0;      
        private ManualResetEvent resetEvent;        
        
        public FilesCopier()
        {
            resetEvent = new ManualResetEvent(false);
        }

        public int StartCopying(string targetFolder, string destinationFolder)
        {    
            Copy(targetFolder, destinationFolder, CopyType.Directory);
            resetEvent.WaitOne();
            resetEvent.Reset();

            Copy(targetFolder, destinationFolder, CopyType.File);
            resetEvent.WaitOne();
            resetEvent.Dispose();
            return result;
        }

        private void Copy(string source, string dest, CopyType copyType)
        {
            string[] paths = null;
            WaitCallback work;
            if (copyType == CopyType.Directory)
            {
                paths = Directory.GetDirectories(source, "*", SearchOption.AllDirectories);
                work = CreateDir;
            }             
            else
            {
                paths = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
                work = CopyFile;
            }                
                        
            count = paths.Length;
            Tuple<string, string> couple;
            foreach (var path in paths)
            {
                couple = new Tuple<string, string>(path, path.Replace(source, dest));
                ThreadPool.QueueUserWorkItem(work, couple);
            }            
        }

        private void CreateDir(object directoryCouple)
        {
            var directoryTuple = directoryCouple as Tuple<string, string>;
            if (!Directory.Exists(directoryTuple.Item2))
            {
                try
                {
                    Directory.CreateDirectory(directoryTuple.Item2);
                }
                catch
                {
                    Console.WriteLine("Error: can not create directory: " + directoryTuple.Item2);
                }
            }

            if (Interlocked.Decrement(ref count) == 0)
            {
                resetEvent.Set();
            }
        }

        private void CopyFile(object fileCouple)
        {
            var fileTuple = fileCouple as Tuple<string, string>;
            try
            {
                File.Copy(fileTuple.Item1, fileTuple.Item2, true);
                Interlocked.Increment(ref result);
            }
            catch
            {
                Console.WriteLine("Error: can not copy file: " + fileTuple.Item2);
            }

            if (Interlocked.Decrement(ref count) == 0)
            {
                resetEvent.Set();
            }
        }        
    }
}
