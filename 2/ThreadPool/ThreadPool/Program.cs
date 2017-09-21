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
                    Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId + " locked mutex.");
                    Thread.Sleep(400);
                    Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId + " unlocked mutex.");
                    mutex.Unlock();
                }).Start();
            }
            Console.ReadLine();
            /*ThreadPool pool = new ThreadPool(4);
            Task task1 = new Task(() =>
            {
                Console.WriteLine(Thread.CurrentThread.Name + ": task1 starts working");                
                Console.WriteLine(Thread.CurrentThread.Name + ": task1 ends working");
            });
            Task task2 = new Task(() =>
            {
                Console.WriteLine(Thread.CurrentThread.Name + ": task2 starts working");                
                Console.WriteLine(Thread.CurrentThread.Name + ": task2 ends working");
            });
            Task task3 = new Task(() =>
            {
                Console.WriteLine(Thread.CurrentThread.Name + ": task3 starts working");                
                Console.WriteLine(Thread.CurrentThread.Name + ": task3 ends working");
            });
            Task task4 = new Task(() =>
            {
                Console.WriteLine(Thread.CurrentThread.Name + ": task4 starts working");                
                Console.WriteLine(Thread.CurrentThread.Name + ": task4 ends working");
            });
            pool.EnqueueTask(task1);
            pool.EnqueueTask(task2);
            pool.EnqueueTask(task3);
            pool.EnqueueTask(task4);
            pool.Stop();
            Console.WriteLine("Main Thread: Done");
            Console.ReadLine();     */
        }
    }
}
