using System;
using System.Threading;

namespace ThreadPool
{
    class Mutex
    {
        private int state; 

        public Mutex()
        {
            this.state = 0;
        }

        public void Lock()
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref state, 1, 0) == 0)
                    return;
            }
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref state, 0);
        }
    }
}
