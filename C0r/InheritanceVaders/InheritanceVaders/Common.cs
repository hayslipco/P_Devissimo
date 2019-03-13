﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InheritanceVaders
{
    public class Common
    {
        protected const int ENEMY_ROW = 7;
        protected const int FPS_TEMPO = 20;
        protected const int ENEMY_BULLET_SPEED = 3;
        protected const int ENEMY_FIRE_FREQ = 50;
        protected const int SHOT_DELAY = 10;
        protected const int BACKGROUND_THRESHOLD = 400;
        protected const int FLICKER_RATE = 20;
        protected const int PLAYER_RESPAWN_TIME = 100;
        protected const int PLAYER_INIT_LIVES = 0;
        protected const int SHOT_COST = 400;
        protected const int SHIELD_DELAY = 350;

        protected bool hardMode = true;

        public List<HighScore> highScores = new List<HighScore>();

        protected int graphicsMargin = 0;

        protected int windowWidth = Console.WindowWidth;
        protected int windowHeight = Console.WindowHeight;

        /// <summary>
        /// Méthode pour trouver un des ennemis vivants se trouvant aux extrémités du groupe
        /// </summary>
        /// <param name="enemies"></param>
        /// <param name="Extremity">extrémité à vérifier (haut, bas, gauche, droite)</param>
        /// <param name="lineSpecificMultiplier">le multiple des lignes à vérifier</param>
        /// <param name="lineSpecificIncrementer">un décalage pour seléctionner exactement quels lignes sont à vérifier</param>
        /// <returns></returns>
        public Enemy GetEnemyExtremity(Enemy[,] enemies, string Extremity, int lineSpecificMultiplier, int lineSpecificIncrementer)
        {
            Enemy theMostXtreme;
            theMostXtreme = new Enemy(Console.WindowWidth / 2, Console.WindowHeight / 2, 0, new List<string> { "" }, false);

            for (int i = 0; i < ENEMY_ROW; i++)
                for (int j = 0; j < ENEMY_ROW; j++)
                {
                    switch (Extremity)
                    {
                        case "left":
                            if (enemies[i, j].X < theMostXtreme.X && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                            break;
                        case "right":
                            if (enemies[i, j].X > theMostXtreme.X && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                            break;
                        case "top":
                            if (enemies[i, j].Y < theMostXtreme.Y && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                            break;
                        case "bottom":
                            if (enemies[i, j].Y > theMostXtreme.Y && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                            break;
                        default:
                            return enemies[0, 0];
                    }
                }

            return theMostXtreme;
        }//fin de GetExtremityEnemy

        public List<Enemy> GetFrontLineEnemies(Enemy[,] enemySwarm)
        {
            Enemy lowest;
            List<Enemy> frontLine = new List<Enemy>();
            for (int c = 0; c < enemySwarm.GetLength(1); c++)
            {
                lowest = enemySwarm[0, c];
                for (int l = 0; l < enemySwarm.GetLength(0); l++)
                {
                    if (enemySwarm[l, c].Y > lowest.Y && enemySwarm[l, c].IsAlive)
                    {
                        lowest = enemySwarm[l, c];
                    }
                }

                frontLine.Add(lowest);
            }

            return frontLine;
        }

        protected int GetMaxLength(List<string> strings)
        {
            int maxLength = 0;

            foreach (string s in strings)
            {
                if (s.Length > maxLength)
                {
                    maxLength = s.Length;
                }
            }

            return maxLength;
        }

        protected int GetMostLength()
        {
            int mostLength;
            Graphics graphics = new Graphics();

            mostLength = graphics.GetMostLength();
            return mostLength;
        }

        protected void LoadHighScores()
        {
            var serializer = new XmlSerializer(highScores.GetType(), "InheritanceVaders");
            object obj;
            using (var reader = new StreamReader("highscores.xml"))
            {
                obj = serializer.Deserialize(reader.BaseStream);
            }

            highScores = (List<HighScore>)obj;
        }

        protected void SaveHighScores()
        {
            var serializer = new XmlSerializer(highScores.GetType(), "InheritanceVaders");
            using (var writer = new StreamWriter("highscores.xml", false))
            {
                serializer.Serialize(writer.BaseStream, highScores);
            }
        }

        public void ShowTopScores()
        {
            LoadHighScores();

            foreach (HighScore h in highScores)
            {
                Console.CursorLeft = windowWidth / 3;
                Console.WriteLine("[{0}/{1}/{2}] : {3} --- {4} points\n", h.date.Day, h.date.Month, h.date.Year, h.Name, h.Score);
                Thread.Sleep(100);
            }
        }

    }
}
