using Microsoft.VisualStudio.TestTools.UnitTesting;
using InheritanceVaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace InheritanceVaders.Tests
{
    [TestClass()]
    public class CommonTests : Common
    {
        [TestMethod()]
        public void GetEnemyExtremityTest()
        {
            //Arrange
            List<string> enemyAppearence = new List<string> { "-_-" };

            Enemy topEnemy = new Enemy(2, 1, 2, enemyAppearence, false);
            Enemy bottomEnemy = new Enemy(2, windowHeight, 2, enemyAppearence, false);
            Enemy leftEnemy = new Enemy(1, 5, 2, enemyAppearence, false);
            Enemy rightEnemy = new Enemy(windowWidth, 5, 2, enemyAppearence, false);

            //Act
            Enemy[,] enemies = new Enemy[,] { { topEnemy, leftEnemy }, { rightEnemy, bottomEnemy } };

            List<Enemy> foundEnemies = new List<Enemy>
            {
                GetEnemyExtremity(enemies, "top", 1, 0),
                GetEnemyExtremity(enemies, "bottom", 1, 0),
                GetEnemyExtremity(enemies, "left", 1, 0),
                GetEnemyExtremity(enemies, "right", 1, 0)
            };

            Debug.Write("********" + foundEnemies.Count + "***********");

            //Assert
            Assert.AreEqual(topEnemy, foundEnemies[0]);
            Assert.AreEqual(bottomEnemy, foundEnemies[1]);
            Assert.AreEqual(leftEnemy, foundEnemies[2]);
            Assert.AreEqual(rightEnemy, foundEnemies[3]);

        }

        [TestMethod()]
        public void GetMaxLengthTest()
        {
            //Arrange
            int maxLength;
            string s1 = "***********";
            string s2 = "***";
            string s3 = "*******";
            string s4 = "***********************";

            List<string> testStrings = new List<string>() { s1, s2, s3, s4 };

            //Act
            maxLength = GetMaxLength(testStrings);


            //Assert
            Assert.AreEqual(s4.Length, maxLength);
        }
    }
}