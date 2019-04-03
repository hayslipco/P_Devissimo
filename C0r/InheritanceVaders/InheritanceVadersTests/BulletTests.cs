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
    public class BulletTests
    {
        [TestMethod()]
        public void BulletCtorTest()
        {
            //Arrange
            Bullet bullet;

            //Act
            bullet = new Bullet(new List<string> { "█" }, 0, 0, 2, true, false);

            //Assert
            Assert.IsNotNull(bullet);
            Assert.IsInstanceOfType(bullet, typeof(Bullet));
        }

        [TestMethod()]
        public void MoveTest()
        {
            //Arrange
            //bullet montante
            Bullet bullet = new Bullet(new List<string> { "█" }, 5, 5, 2, true, false);

            //Act
            bullet.Move();
            bullet.Move();

            //Assert
            Assert.AreEqual(5 - 1 - 1, bullet.Y);
        }

        [TestMethod()]
        public void CollidesWithTest()
        {
            //Arrange
            Bullet bullet = new Bullet(new List<string> { "█" }, 5, 5, 2, true, false);
            Enemy enemy = new Enemy(5, 5, 2, new List<string> { "-_-" }, false);
            Enemy enemy2 = new Enemy(9, 5, 2, new List<string> { "-_-" }, false);

            bool collide;
            bool miss;

            //Act
            collide = bullet.CollidesWith(enemy);
            miss = bullet.CollidesWith(enemy2);

            //Assert
            Assert.IsTrue(collide);
            Assert.IsFalse(miss);
        }
    }
}