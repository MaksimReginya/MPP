using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{    
    [ExportXML]
    class TestClassB
    {
        private string _privateString;
        public string _publicString;
        public TestClassA obj;
        private void PrivateMethod()
        { }

        public string PublicMethod(string param)
        {
            return param;
        }
    }
}
