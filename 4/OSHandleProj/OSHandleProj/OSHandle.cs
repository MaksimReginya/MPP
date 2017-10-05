using System;
using System.Runtime.InteropServices;

namespace OSHandleProj
{
    public class OSHandle: IDisposable
    {
        public IntPtr Handle { get; set; }        
        private bool isDisposed = false; // To detect redundant calls

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        public OSHandle()
        {
            Handle = IntPtr.Zero;
        }
        
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if (Handle != IntPtr.Zero)
                {
                    if (!CloseHandle(Handle))
                    {
                        throw new Exception("Handle cannot be closed.");
                    }
                }

                isDisposed = true;
            }
        }
        
        ~OSHandle() {
            Dispose(false);
        }
        
        //This code added to correctly implement the disposable pattern.
        public void Dispose()
        {        
            Dispose(true);         
            GC.SuppressFinalize(this);
        }
    }
}
