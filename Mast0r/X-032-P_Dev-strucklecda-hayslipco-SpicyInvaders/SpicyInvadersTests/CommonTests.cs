/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 03.04.19
 * Description: Classe de tests pour Common
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders.Tests
{
    /// <summary>
    /// Classe de tests pour Common
    /// </summary>
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

        [TestMethod()]
        public void GetFrontLineEnemiesTest()
        {
            //Arrange
            List<string> appearence = new List<string> { "-_-" };

            Enemy[,] enemies = new Enemy[,]
            {
                {new Enemy(0,0,2, appearence, false) , new Enemy(2,0,2,appearence,false), new Enemy(4,0,2,appearence, false) },
                {new Enemy(0,1,2,appearence,false), new Enemy(2,1,0,appearence, false), new Enemy(4,1,0,appearence,false) },
                {new Enemy(0,2,2,appearence,false), new Enemy(2,2,0,appearence, false), new Enemy(4,2,0,appearence,false) },
                {new Enemy(0,3,2,appearence,false), new Enemy(2,3,0,appearence, false), new Enemy(4,3,0,appearence,false) },
                {new Enemy(0,4,2,appearence,false), new Enemy(2,4,0,appearence, false), new Enemy(4,4,0,appearence,false) }

            };

            //Act
            List<Enemy> frontLineEnemies = GetFrontLineEnemies(enemies);

            //Assert
            Assert.AreEqual(enemies[4, 0], frontLineEnemies[0]);
            Assert.AreEqual(enemies[4, 1], frontLineEnemies[1]);
            Assert.AreEqual(enemies[4, 2], frontLineEnemies[2]);

        }
    }
}