/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 03.04.19
 * Description: Classe de tests pour SpaceShip
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders.Tests
{
    /// <summary>
    /// Classe de tests pour SpaceShip
    /// </summary>
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