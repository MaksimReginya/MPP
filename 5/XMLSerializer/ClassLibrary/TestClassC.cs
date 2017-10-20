using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [ExportXML]
    class TestClassC: TestClassB
    {
        private object _privateObject;
        public object _publicObject;

        private void PrivateMethod()
        { }

        public object PublicMethod(object param)
        {
            return param;
        }
    }
}
