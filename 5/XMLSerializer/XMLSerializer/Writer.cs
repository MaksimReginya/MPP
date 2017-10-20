using System;
using System.Xml;

namespace XMLSerializer
{
    class Writer
    {        
        public void Write(XmlDocument xmlDocument, string dest)
        {
            xmlDocument.Save(dest);
        }
    }
}
