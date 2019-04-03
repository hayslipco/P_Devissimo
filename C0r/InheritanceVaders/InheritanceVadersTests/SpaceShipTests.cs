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
    public class SpaceShipTests
    {
        [TestMethod()]
        public void SpaceShipCtorTest()
        {
            //Arrange
            SpaceShip ship;

            //Act
            ship = new SpaceShip(0, 0, 2, new List<string> { "<-o->" });

            //Assert
            Assert.IsNotNull(ship);
            Assert.IsInstanceOfType(ship, typeof(SpaceShip));
        }

        [TestMethod()]
        public void MoveShipTest()
        {
            //Arrange
            SpaceShip ship = new SpaceShip(10, 10, 2, new List<string> { "<-o->" });

            //Act
            ship.MoveShip(5);
            ship.MoveShip(-2);
            ship.MoveShip(3);

            //Assert
            Assert.AreEqual(10 + 5 - 2 + 3, ship.X);
        }
    }
}