using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{      
    //Task that is executed in the ThreadPool
    public class Task
    {
        public delegate void TaskDelegate();
        public TaskDelegate Work { get; set; }       
        //True - task is executing, False - task is in execution queue
        public bool IsRunned { get;  set; }

        public Task()
        { }

        public Task(TaskDelegate work)
        {            
            this.Work = work;
        }
        
        public void Execute()
        {           
            Work();
        }              
    }
    
    public class ThreadPool
    {        
        private int threadCount;

        private ManualResetEvent stopEvent;
        private bool isStopping;
        private object stopLock;

        private Dictionary<int, ManualResetEvent> threadsEvent;
        private Thread[] threads;
        private LinkedList<int> workingThreads;
        private List<Task> tasks;              
                        
        public ThreadPool() { }
        
        public ThreadPool(int threadCount)
        {
            if (threadCount <= 0)
                throw new ArgumentException("threadCount must be greater than zero", "threadCount");

            this.threadCount = threadCount;

            this.stopLock = new object();
            this.stopEvent = new ManualResetEvent(false);            

            this.threads = new Thread[threadCount];
            this.threadsEvent = new Dictionary<int, ManualResetEvent>(threadCount);

            this.workingThreads = new LinkedList<int>();
            this.tasks = new List<Task>();            

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(ThreadWork) { Name = "Pool Thread_"+(i+1).ToString(), IsBackground = true };
                threadsEvent.Add(threads[i].ManagedThreadId, new ManualResetEvent(false));

                threads[i].Start();                
            }                        
        }
                
        private Task SelectTask()
        {
            lock (tasks)
            {                            
                Task task = null;
                try
                {
                    if (tasks.Count == 0)
                        throw new ArgumentException();
                    task = tasks.First((Task t) => !t.IsRunned);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + ": " + ex.Message);
                    return null;
                }                
                task.IsRunned = true;
                return task;                
            }
        }

        private void ThreadWork()
        {
            while (true)
            {
                threadsEvent[Thread.CurrentThread.ManagedThreadId].WaitOne();
                Task task = SelectTask();
                if (task != null)
                {                    
                    try
                    {
                        task.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + ": " + ex.Message);
                    }
                    finally
                    {                        
                        RemoveTask(task);
                        if (isStopping)
                            stopEvent.Set();
                        workingThreads.Remove(Thread.CurrentThread.ManagedThreadId);
                        threadsEvent[Thread.CurrentThread.ManagedThreadId].Reset();
                    }
                }
            }
        }        

        private void SelectAndRunThread()
        {                               
            lock (threads)
            {
                foreach (var thread in threads)
                {
                    if (!workingThreads.Contains(thread.ManagedThreadId))
                    {
                        workingThreads.AddLast(thread.ManagedThreadId);
                        threadsEvent[thread.ManagedThreadId].Set();
                        return;
                    }
                }
            }                                       
        }

        private void AddTask(Task task)
        {
            lock (tasks)
            {
                tasks.Add(task);
            }
            SelectAndRunThread();            
        }
                
        private void RemoveTask(Task task)
        {
            lock (tasks)
            {
                tasks.Remove(task);
            }
        }

        //Puts task in queue  
        public bool EnqueueTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task", "The Task can't be null.");
            lock (stopLock)
            {
                if (isStopping)
                {
                    return false;
                }                
            }
            AddTask(task);
            return true;            
        }                
       
        //Waits for all tasks to end 
        public void Stop()
        {           
            lock (stopLock)
            {
                isStopping = true;
            }
            while (tasks.Count > 0)
            {
                stopEvent.WaitOne();
                stopEvent.Reset();
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Abort();
                threadsEvent[threads[i].ManagedThreadId].Dispose();
            }
        }
    }
}
