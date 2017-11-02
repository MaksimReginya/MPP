using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{    
    [ExportXML]
    class TestClassB: I
    {
        private string _privateString;
        public string _publicString;
        protected TestClassA obj;
        private void PrivateMethod()
        { }

        public static string PublicMethod(string param)
        {
            return param;
        }
    }

    interface I
    {
        
    }
}
