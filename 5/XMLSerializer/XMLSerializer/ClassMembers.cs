using System;
using System.Collections.Generic;

namespace XMLSerializer
{
    class ClassMembers
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public List<string> Inheritors { get; set; }
        public List<UsualField> UsualFields { get; set; }
        public List<ChildClassField> ChildClassFields { get; set; }
        public List<Method> Methods { get; set; }

        public class UsualField
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public string AccessModifier { get; set; }
        }

        public class ChildClassField
        {
            public string Name { get; set; }
            public ClassMembers Type { get; set; }
            public string AccessModifier { get; set; }
        }
        
        public class Method
        {
            public string Name { get; set; }
            public string AccessModifier { get; set; }
            public Type ReturnType { get; set; }
            public List<Param> Params { get; set; }

            public class Param
            {
                public string Name { get; set; }
                public Type Type { get; set; }
            }
        }        
    }
}
