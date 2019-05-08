/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 03.04.19
 * Description: Classe de tests pour Element
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders.Tests
{
    /// <summary>
    /// Classe de tests pour Element
    /// </summary>
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