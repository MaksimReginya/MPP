using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    Console.WriteLine("Thread_" + Thread.CurrentThread.ManagedThreadId + " locked mutex");
                    Thread.Sleep(400);
                    Console.WriteLine("Thread_" + Thread.CurrentThread.ManagedThreadId + " unlocked mutex");
                    mutex.Unlock();
                }).Start();
            }
            Console.WriteLine();
            Console.ReadLine();

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
            Console.WriteLine("Thread " + Thread.CurrentThread.Name + " started to work");
            Thread.Sleep(1000);
            Console.WriteLine("Thread " + Thread.CurrentThread.Name + " finished to work");
        }
    }
}
