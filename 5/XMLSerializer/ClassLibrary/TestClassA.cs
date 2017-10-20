using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [ExportXML]
    class TestClassA
    {
        private int _privateInt;
        public int _publicInt;
        public TestClassB obj;
        private void PrivateMethod()
        { }

        public int PublicMethod(int param)
        {
            return param;
        }
    }
}
