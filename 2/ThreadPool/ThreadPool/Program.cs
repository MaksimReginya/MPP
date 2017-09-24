using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {        
        static void Main(string[] args)
        {
            var mutex = new Mutex();
            for (var i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    mutex.Lock();
                    TaskForThread();
                    mutex.Unlock();
                }).Start();
            }            
            Console.ReadLine();

            Console.WriteLine("Thread pool starts working");            
            ThreadPool pool = new ThreadPool(4);            
            for (int i = 0; i < 10; i++)
            {
                pool.EnqueueTask(TaskForThread);
            }            
            Console.ReadLine();           
            pool.Abort();
            Console.WriteLine("Main Thread: Done");
            Console.ReadLine();     
        }

        static void TaskForThread()
        {
            Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " started to work");
            Thread.Sleep(1000);
            Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " finished to work");
        }
    }
}
