using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ThreadPool
{
    public class ThreadPool
    {
        public delegate void TaskDelegate();
        private int threadCount;
        private Thread[] threads;
        private Thread sheduleThread;
        private ConcurrentQueue<TaskDelegate> taskQueue;
        private Dictionary<int, ManualResetEvent> threadsEvents;                                

        public ThreadPool()
        {}

        public ThreadPool(int threadCount)
        {
            if (threadCount <= 0)
                throw new ArgumentException("threadCount must be greater than zero", "threadCount");            

            taskQueue = new ConcurrentQueue<TaskDelegate>();

            sheduleThread = new Thread(ShedulingThreads);
            sheduleThread.Name = "Shedule thread";
            sheduleThread.Start();    

            threadsEvents = new Dictionary<int, ManualResetEvent>();            
            this.threadCount = threadCount;
            threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(ThreadWork);
                threadsEvents.Add(threads[i].ManagedThreadId, new ManualResetEvent(false));                
                threads[i].Start();
            }
        }

        private void ThreadWork()
        {
            while (true)
            {
                threadsEvents[Thread.CurrentThread.ManagedThreadId].WaitOne();
                TaskDelegate work = SelectTask();
                try
                {
                    work();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + ex.Message);
                }
                finally
                {                                  
                    threadsEvents[Thread.CurrentThread.ManagedThreadId].Reset();
                }                
            }
        }      

        private TaskDelegate SelectTask()
        {
            bool exit = false;
            TaskDelegate task = null;
            while (!exit)
            {
                exit = taskQueue.TryDequeue(out task);
            }
            return task;
        }

        private void SelectFreeThread()
        {           
            foreach (var thread in threads)
            {
                if (!threadsEvents[thread.ManagedThreadId].WaitOne(0))
                {
                    threadsEvents[thread.ManagedThreadId].Set();
                    break;
                }
            }           
        }

        private void ShedulingThreads()
        {
            while (true)
            {
                if (!taskQueue.IsEmpty)
                {
                    SelectFreeThread();
                }
            }
        }

        public void EnqueueTask(TaskDelegate task)
        {          
            taskQueue.Enqueue(task);
        }        

        public void Abort()
        {
            sheduleThread.Abort();
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Abort();
            }
        }
    }
}