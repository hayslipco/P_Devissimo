using Microsoft.VisualStudio.TestTools.UnitTesting;
using InheritanceVaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders.Tests
{
    [TestClass()]
    public class ElementTests
    {
        [TestMethod()]
        public void ElementCtorTest()
        {
            //Arrange
            Element element;

            //Act
            element = new Element(2, 2, 2, new List<string> { "o  o", "\\_/" });

            //Assert
            Assert.IsNotNull(element);
            Assert.IsInstanceOfType(element, typeof(Element));
        }

        [TestMethod()]
        public void LoadTest()
        {
            //Arrange
            int xPos = 5;
            int yPos = 5;
            Element element = new Element(xPos, yPos, 4, new List<string> { "o" });
            char[][] buffer = new char[10][];

            //Act
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new char[10];
            }

            element.Load(buffer);

            //Assert
            Assert.AreEqual('o', buffer[xPos][yPos]);
        }
    }
}