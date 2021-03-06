﻿/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 06.03.19
 * Description: Classe commune servant à stocker des valeurs et méthodes accessibles par tous les autres classes du programme
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    /// <summary>
    /// Classe commune servant à stocker des valeurs et méthodes accessibles par tous les autres classes du programme
    /// </summary>
    public class Common
    {
        protected const int FPS_TEMPO = 15;
        protected const int BACKGROUND_THRESHOLD = 400;
        protected const int FLICKER_RATE = 20;
        protected const int PLAYER_RESPAWN_TIME = 100;
        protected const int PLAYER_INIT_LIVES = 3;
        protected const int SHOT_COST = 400;
        protected const int SHORTSHOT_DELAY = 70;
        protected const int MIDSHOT_DELAY = 35;
        protected const int LONGSHOT_DELAY = 70;
        protected const int SHORTSHOT_SPEED = 6;
        protected const int MIDSHOT_SPEED = 3;
        protected const int LONGSHOT_SPEED = 1;

        protected static bool visualDisplay = true;
        protected static bool difficulty = true;
        protected bool hardMode = true;

        public List<HighScore> highScores = new List<HighScore>();

        protected int graphicsMargin = 0;

        //attributs changeant avec la dfficulté
        protected int enemyRow = 8;
        protected int enemyBulletSpeed = 4;
        protected int enemyFireFreq = 100;
        protected int specialEnemySpawn = 3000;
        protected int shieldDelay = 600;
        protected int shieldDuration = 200;
        protected int specialEnemyPoints = 10000;
        protected int enemyPoints = 1000;

        protected int windowWidth = Console.WindowWidth;
        protected int windowHeight = Console.WindowHeight;

        /// <summary>
        /// Change les valeurs pour les difficultés
        /// </summary>
        /// <param name="difficulty"></param>
        public void SetDifficulty(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    enemyRow = 8;
                    enemyBulletSpeed = 4;
                    enemyFireFreq = 100;
                    shieldDelay = 600;
                    shieldDuration = 200;
                    specialEnemySpawn = 3000;
                    enemyPoints = 800;
                    specialEnemyPoints = 10 * enemyPoints;
                    break;

                case "Hard":
                    enemyRow = 10;
                    enemyBulletSpeed = 3;
                    enemyFireFreq = 50;
                    shieldDelay = 600;
                    shieldDuration = 100;
                    specialEnemySpawn = 4000;
                    enemyPoints = 1000;
                    specialEnemyPoints = 10 * enemyPoints;
                    break;
            }
        }

        /// <summary>
        /// Set la difficulté du jeu
        /// </summary>
        public void DifficultySetting()
        {
            if (difficulty)   
            {
                difficulty = false;
                SetDifficulty("Hard");               
            }
            else
            {
                SetDifficulty("Easy");
                difficulty = true;               
            }
        }

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
            //instanciation d'un ennemi se trouvant au milieu de la console, pour pouvoir ensuite comparer avec les ennemis dans l'essaim
            Enemy theMostXtreme;
            theMostXtreme = new Enemy(windowWidth / 2, windowHeight / 2, 1, new List<string> { "" }, false);
            for (int i = 0; i < enemies.GetLength(0); i++)
                for (int j = 0; j < enemies.GetLength(1); j++)
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

        /// <summary>
        /// Récupère les ennemis vivants les plus bas de chaque colonne de l'essaim
        /// </summary>
        /// <param name="enemySwarm">essaim à vérifier</param>
        /// <returns>Liste des ennemis en première ligne</returns>
        public List<Enemy> GetFrontLineEnemies(Enemy[,] enemySwarm)
        {
            Enemy lowest;
            List<Enemy> frontLine = new List<Enemy>();
            //Parcours des colonnes
            for (int c = 0; c < enemySwarm.GetLength(1); c++)
            {
                lowest = enemySwarm[0, c];
                //Parcours des lignes
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

        /// <summary>
        /// Trouve^le string le plus long dans une liste et renvoie sa longueur
        /// </summary>
        /// <param name="strings">liste de strings à comparer</param>
        /// <returns>longueur maximale des strings de la liste</returns>
        public int GetMaxLength(List<string> strings)
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

        /// <summary>
        /// Récupère les Highscores stockés dans le fichier highscores.xml
        /// </summary>
        protected void LoadHighScores()
        {
            var serializer = new XmlSerializer(highScores.GetType(), "X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders");
            object obj;
            if (File.Exists("highscores.xml"))
            {
                using (var reader = new StreamReader("highscores.xml"))
                {
                    obj = serializer.Deserialize(reader.BaseStream);
                }
                highScores = (List<HighScore>)obj;
            }
            else
            {
                SaveHighScores();
            }

        }

        /// <summary>
        /// Enregistre les Highscores dans le fichier highscores.xml
        /// </summary>
        protected void SaveHighScores()
        {
            var serializer = new XmlSerializer(highScores.GetType(), "X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders");
            using (var writer = new StreamWriter("highscores.xml", false))
            {
                serializer.Serialize(writer.BaseStream, highScores);
            }
        }

        /// <summary>
        /// Nettoie la console et affiche les Highscores l'un après l'autre avec un titre
        /// </summary>
        public void ShowTopScores()
        {
            Console.Clear();
            LoadHighScores();

            List<string> title = new List<string>
            {
                "    __  ___       __                                     ",
                "   / / / (_)___ _/ /_     ______________  ________  _____",
                "  / /_/ / / __ `/ __ \\   / ___/ ___/ __ \\/ ___/ _ \\/ ___/",
                " / __  / / /_/ / / / /  (__  ) /__/ /_/ / /  /  __(__  ) ",
                "/_/ /_/_/\\__, /_/ /_/  /____/\\___/\\____/_/   \\___/____/  ",
                "        /____/                                           "

            };

            Console.CursorTop = 12;
            WriteTitle(title);
            Console.WriteLine("\n\n");

            foreach (HighScore h in highScores)
            {
                Console.CursorLeft = (windowWidth / 2) - ("[22/22/22] : moyenne environ --- 100000 points\n".Length / 2);
                Console.WriteLine("[{0}/{1}/{2}] : {3} --- {4} points\n", h.date.Day, h.date.Month, h.date.Year, h.Name, h.Score);
                Thread.Sleep(150);
            }
        }

        /// <summary>
        /// Affiche un titre constitué de plusieurs lignes stockés dans une liste
        /// </summary>
        /// <param name="strings">lignes du titre</param>
        public void WriteTitle(List<string> strings)
        {
            int leftPadding = (Console.WindowWidth / 2) - (strings[0].Length / 2);

            foreach (string s in strings)
            {
                Console.CursorLeft = leftPadding;
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Affiche du texte au centre de la console en le coupant et en l'affichant sur plusieurs lignes si nécessaire
        /// </summary>
        /// <param name="s">texte à écrire</param>
        /// <param name="minLeftPadding">padding minimum que le texte ne doit pas dépasser, si c'est le cas, il est split en plusieurs lignes</param>
        public void CenteredWriteLine(string s, int minLeftPadding)
        {
            int charsPerLine = Console.WindowWidth - (2 * minLeftPadding);
            List<string> lines = new List<string>();

            //découpage du string sur plusieurs lignes
            if (s.Length > charsPerLine)
            {
                int numLines = (int)Math.Round((double)(s.Length / charsPerLine));

                for (int i = 0; i <= numLines; i++)
                {
                    string line;
                    if (i == numLines)
                    {
                        line = s.Substring(i * charsPerLine);
                    }
                    else
                    {
                        line = s.Substring((i * charsPerLine), (i + 1) * charsPerLine);
                        if (line.Last() != ' ')
                        {
                            line += '-';
                        }
                    }

                    lines.Add(new String(line.ToCharArray()));
                }
            }
            else
            {
                lines.Add(s);
            }

            //affichage
            foreach (string line in lines)
            {
                if (line == lines.Last())
                {
                    int leftPadding = (Console.WindowWidth / 2) - (line.Length / 2);
                    Console.CursorLeft = leftPadding;
                }
                else
                {
                    Console.CursorLeft = minLeftPadding;
                }

                if (Console.CursorTop < Console.WindowHeight / 2 + 2)
                {
                    Console.WriteLine(line);
                }
                else
                {
                    Console.Write(line);
                }
            }
        }

        public void OnOffVisualDisplay()
        {
            if (visualDisplay)
            {
                visualDisplay = false;
                Debug.WriteLine("Désactivé");
            }
            else
            {
                visualDisplay = true;
                Debug.WriteLine("Activé");
            }
        }

        /// <summary>
        /// Dispose le texte du mode d'emploi
        /// </summary>
        /// <param name="s"></param>
        public void WriteHowToPlay(string btn, string desc)
        {
            Console.WriteLine();
            Console.CursorLeft = (Console.WindowWidth / 2 - 25);
            Console.Write(btn);
            Console.CursorLeft = (Console.WindowWidth / 2 + 5);
            Console.WriteLine(desc);
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strings"></param>
        public void WriteParam(List<string> strings)
        {
            int leftPadding = (Console.WindowWidth / 2) - (strings[0].Length / 2);
            int topPadding = 8;
            Console.SetCursorPosition(leftPadding, topPadding);
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string s in strings)
            {
                Console.SetCursorPosition(leftPadding, topPadding);
                topPadding++;
                Console.WriteLine(s);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Méthode pour rester dans une des options du menu
        /// </summary>
        public void StayInMenu()
        {
            ConsoleKeyInfo pressedKey;
            do
            {
                pressedKey = Console.ReadKey(true);

            } while (pressedKey.Key != ConsoleKey.Escape && pressedKey.Key != ConsoleKey.Enter && pressedKey.Key != ConsoleKey.Backspace);
            Console.Clear();
        }
    }
}
