/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 03.04.19
 * Description: Classe de tests pour Enemy
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders.Tests
{
    /// <summary>
    /// Classe de tests pour Enemy
    /// </summary>
    [TestClass()]
    public class EnemyTests
    {
        [TestMethod()]
        public void EnemyCtorTest()
        {
            //Arrange
            Enemy enemy;

            //Act
            enemy = new Enemy(0, 0, 2, new List<string> { "-_-" }, false);

            //Assert
            Assert.IsNotNull(enemy);
            Assert.IsInstanceOfType(enemy, typeof(Enemy));
        }

        [TestMethod()]
        public void ShootTest()
        {
            //Arrange
            List<Bullet> projectiles = new List<Bullet>();
            Enemy enemy = new Enemy(0, 0, 0, new List<string> { "-_-" }, false);

            //Act
            enemy.Shoot(projectiles);
            enemy.Shoot(projectiles);
            enemy.Shoot(projectiles);
            enemy.Shoot(projectiles);


            //Assert
            Assert.AreEqual(4, projectiles.Count);
        }

        [TestMethod()]
        public void MoveTest()
        {
            //Arrange
            Enemy enemy = new Enemy(10, 10, 1, new List<string> { "-_-" }, false);

            //Act
            enemy.IsGoingLeft = false;
            enemy.Move();
            enemy.Move();
            enemy.Move();
            enemy.IsChanging = true;
            enemy.Move();
            enemy.Move();

            //Assert
            Assert.AreEqual(10 + 1 + 1 + 1 + 0 - 1, enemy.X);
            Assert.AreEqual(10 + 0 + 0 + 0 + 1 + 0, enemy.Y);
            Assert.IsFalse(enemy.IsChanging);
        }
    }
}