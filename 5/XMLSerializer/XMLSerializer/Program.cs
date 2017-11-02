using System;
using System.Xml;

namespace XMLSerializer
{
    class Program
    {        
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: Program.exe [path to assembly] [path to output file]");
                return;
            }
            var assembly = args[0];
            var destFile = args[1];

            var serializer = new XMLSerializer(assembly);            
            XmlDocument xmlDocument = serializer.Serialize();

            var writer = new Writer();
            writer.Write(xmlDocument, destFile);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }   
}
