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