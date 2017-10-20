using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;

namespace XMLSerializer
{
    class AssemblyClassesInfoCreator
    {        
        private readonly Dictionary<Type, bool> _isMentioned = new Dictionary<Type, bool>();
        private Type[] _types;                

        public AssemblyClassesInfoCreator(Assembly assembly)
        {            
            InitMentionMap(assembly);
        }

        private void InitMentionMap(Assembly assembly)
        {            
            _types = assembly.GetTypes();
            foreach (var type in _types)
            {
                _isMentioned.Add(type, false);
            }
        }

        public IEnumerable<ClassMembers> Create()
        {           
            var classes = new List<ClassMembers>();            
            foreach (var type in _types)
            {
                if (!_isMentioned[type])
                {
                    ClassMembers classMembers = CreateClassInfo(type);
                    if (classMembers != null) 
                    {
                        classes.Add(classMembers);
                    }
                }
            }
            return classes;
        }

        private ClassMembers CreateClassInfo(Type type)
        {                    
            if (!type.IsClass || type.GetCustomAttribute(typeof(ExportXML)) == null || _isMentioned[type])
            {
                return null;
            }

            _isMentioned[type] = true;

            var classMembers = new ClassMembers
            {
                Namespace = type.Namespace,
                ClassName = type.Name,
                Inheritors = GetInheritors(type),
                Methods = GetMethods(type)
            };            
            GetClassFields(classMembers, type);

            return classMembers;
        }

        private void GetClassFields(ClassMembers classMembers, Type type)
        {
            var usualFields = new List<ClassMembers.UsualField>();
            var childClassFields = new List<ClassMembers.ChildClassField>();

            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                if (field.FieldType.IsClass && _types.Contains(field.FieldType))
                {
                    ClassMembers cd = CreateClassInfo(field.FieldType);
                    if (cd != null)
                    {
                        var rf = new ClassMembers.ChildClassField
                        {
                            Name = field.Name,
                            AccessModifier = field.Attributes.ToString(),
                            Type = cd
                        };

                        childClassFields.Add(rf);
                        continue;
                    }                    
                }

                var vf = new ClassMembers.UsualField
                {
                    Name = field.Name,
                    Type = field.FieldType,
                    AccessModifier = field.Attributes.ToString()
                };

                usualFields.Add(vf);
            }
            classMembers.UsualFields = usualFields;
            classMembers.ChildClassFields = childClassFields;
        }

        private List<string> GetInheritors(Type type)
        {
            var list = new List<string>();
            foreach (var realizedInteface in type.GetInterfaces())
            {
                list.Add(realizedInteface.ToString());
            }
            var subClasses = _types.Where(t => t.IsSubclassOf(type));
            foreach (var subClass in subClasses)
            {
                list.Add(subClass.Name);
            }
            return list;
        }

        private List<ClassMembers.Method> GetMethods(Type type)
        {
            var list = new List<ClassMembers.Method>();
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var classMethod = new ClassMembers.Method
                {
                    Name = method.Name,
                    ReturnType = method.ReturnType,
                    Params = GetMethodParams(method),
                    AccessModifier = GetMethodAccessModifier(method)
                };                

                list.Add(classMethod);
            }
            return list;
        }

        private List<ClassMembers.Method.Param> GetMethodParams(MethodInfo method)
        {
            var list = new List<ClassMembers.Method.Param>();
            foreach (var paramInfo in method.GetParameters())
            {
                var param = new ClassMembers.Method.Param
                {
                    Name = paramInfo.Name,
                    Type = paramInfo.ParameterType
                };
                
                list.Add(param);
            }
            return list;
        }

        private string GetMethodAccessModifier(MethodInfo method)
        {           
            if (method.IsPrivate) return "Private";
            if (method.IsPublic) return "Public";            
            return "Protected";
        }
    }
}
