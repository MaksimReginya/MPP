using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicList;

namespace DynamicList.Tests
{
    [TestClass]
    public class DynamicListTests
    {
        DynamicList<int> list;

        [TestMethod]
        public void TestConstructor()
        {                        
            list = new DynamicList<int>() { 1, 2, 3, 4 };           
            Assert.IsNotNull(list);          
        }

        [TestMethod]
        public void TestConstructorWithParams()
        {
            list = new DynamicList<int>(5);
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestAdd()
        {            
            DynamicList<int> expected = new DynamicList<int>() { 1, 2, 3, 4 };
            DynamicList<int> actual = new DynamicList<int>() { 1, 2, 3 };            
            actual.Add(4);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void TestCount()
        {            
            list = new DynamicList<int>() { 1, 2, 3, 4 };
            int expected = 4;
            int actual = list.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGet()
        {            
            list = new DynamicList<int>() { 1, 2, 3, 4 };
            int expected = 3;
            int actual = list[2];            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRemoveByIndex()
        {            
            DynamicList<int> expected = new DynamicList<int>() { 1, 2, 4 };
            DynamicList<int> actual = new DynamicList<int>() { 1, 2, 3, 4 };                      
            actual.RemoveAt(2);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void TestRemoveByValue()
        {            
            DynamicList<int> expected = new DynamicList<int>() { 1, 2, 4 };
            DynamicList<int> actual = new DynamicList<int>() { 1, 2, 3, 4 };            
            actual.Remove(3);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void TestClear()
        {            
            DynamicList<int> expected = new DynamicList<int>() { };
            DynamicList<int> actual = new DynamicList<int>() { 1, 2, 3, 4 };            
            actual.Clear();
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void TestForeach()
        {            
            DynamicList<int> expected = new DynamicList<int>() { 1, 4, 9, 16 };
            DynamicList<int> temp = new DynamicList<int>() { 1, 2, 3, 4 };
            DynamicList<int> actual = new DynamicList<int>() { };            
            foreach (int el in temp)
            {
                actual.Add(el * el);
            }            
            Assert.IsTrue(expected.Equals(actual));
        }
           
        [TestMethod]
        public void TestForeachMyClass()
        {
            DynamicList<MyClass> expected = new DynamicList<MyClass>() { new MyClass() { variable = 0 }, new MyClass() { variable = 1 } };
            int i;
            for (i=0; i<expected.Count; i++)
            {
                Assert.AreEqual(i, expected[i].variable);
            }            
        }

        class MyClass
        {
            public int variable;
        }    
    }    
}
