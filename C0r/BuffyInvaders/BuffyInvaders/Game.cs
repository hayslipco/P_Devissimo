using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace BuffyInvaders
{
    public class Game
    {
        private const int ENEMY_ROW = 10;
        private const int FPS_TEMPO = 15;
        private const int BULLET_SPEED = 2;
        private const int SHOT_DELAY = 10;
        private const int BACKGROUND_THRESHOLD = 400;
        private const int FLICKER_RATE = 55;


        public int windowWidth = Console.WindowWidth;
        public int windowHeight = Console.WindowHeight;

        private int shotDelayTimer = 0;
        private int delta;
        private int ticks;
        private int playerSpeed = 2;
        private int enemySpeed = 2;
        private int backgroundTicker = 2;
        private int graphicsInt;
        private int graphicsLoop;

        private string livesIndicator;

        private bool hasLost = false;
        private bool backgroundUp = true;
        private bool waveAlive = true;
        private bool spawning = false;

        private Enemy XtremeEnemy;
        private Graphics graphics = new Graphics();
        private Random random = new Random();

        private char[][] buffer;
        private Enemy[,] enemies;
        private Enemy specialEnemy;
        private List<Bullet> projectiles = new List<Bullet>();
        private Stopwatch timer = new Stopwatch();

        public void Launch()
        {

            //nettoyage de la console
            Console.Clear();

            //création du vaisseau joueur
            SpaceShip player = new SpaceShip("<I>-T-<I>", 5, windowHeight - 3);

            //création du string indiquant le nombre de vies
            livesIndicator = player.Appearence + " × " + player.Lives;

            //initialisation du buffer dans lequel chaque caractère sera chargé avant de tout Write en un coup
            buffer = new char[windowHeight - 1][];
            for(int i = 0; i < buffer.GetLength(0); i++)
            {
                buffer[i] = new char[windowWidth - 1];
            }
            enemies = new Enemy[ENEMY_ROW, ENEMY_ROW];

            //création de la vague d'ennemis et de l'ennemi spécial
            for(int l = 0; l < ENEMY_ROW; l++)
                for(int c = 0; c < ENEMY_ROW; c++)
                {
                    enemies[l, c] = new Enemy("|-O-|", ConsoleColor.White, c * ("|-O-|".Length + 2), l * 2 + 1, enemySpeed, false);
                }

            specialEnemy = new Enemy("", ConsoleColor.Black, -11, 3, 4, true);
            specialEnemy.IsAlive = false;

            //boucle qui met à jour le jeu
            do
            {

                //démarrage du timer qui va calculer le temps de temporisation nécessaire
                timer.Restart();

                //augmentation des timer de délai
                shotDelayTimer++;

                //gestion du timer en charge de l'arrière plan galactique
                if (backgroundUp)
                {
                    backgroundTicker++;
                    if(backgroundTicker > BACKGROUND_THRESHOLD)
                    {
                        backgroundUp = false;
                    }
                }
                else
                {
                    backgroundTicker--;
                    if (backgroundTicker < 3)
                    {
                        backgroundUp = true;
                    }
                }

                //mise à vide du buffer
                for (int i = 0; i < buffer.GetLength(0); i++)
                    for (int j = 0; j < buffer[0].Length; j++)
                    {
                        ////space background
                        //if (i * (buffer[0].Length - j) % backgroundTicker == 0)
                        //{
                        //    buffer[i][j] = '*';
                        //}
                        //else //fin space background
                        {
                            buffer[i][j] = ' ';
                        }
                    }

                //Gros graphismes jolis
                if (ticks % FLICKER_RATE == 0)
                {
                    graphicsInt++;
                    graphicsLoop++;
                }
                if (graphicsInt >= windowHeight)
                {
                    graphicsInt = 0;
                }

                if (graphicsLoop > windowHeight / 2)
                {
                    graphicsLoop = 0;
                }

                if (graphicsLoop < windowHeight / 4)
                {
                    graphics.SideLigthning(buffer);
                }
                else if (ticks < windowHeight / 3)
                {
                    graphics.SideLightningSlide(buffer, graphicsInt);
                }
                else
                {
                    graphics.SLightningTwoD(buffer, Math.Abs(graphicsInt - 20));
                    graphics.SLightningTwoD(buffer, Math.Abs(graphicsInt - 10));
                    graphics.SLightningTwoD(buffer, graphicsInt);
                    graphics.SLightningTwoD(buffer, graphicsInt + 10);
                }//fin des jolis graphismes

                //contôle du joueur
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo entry = Console.ReadKey(true);

                    switch (entry.Key)
                    {
                        case ConsoleKey.RightArrow:
                            if (!player.SpaceFlight)
                            {
                                player.MoveShip(2);
                            }
                            else
                            {
                                player.GoingLeft = false;
                                player.Stopped = false;
                                player.Appearence = ">I<-T->I<";
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (!player.SpaceFlight)
                            {
                                player.MoveShip(-2);
                            }
                            else
                            {
                                player.GoingLeft = true;
                                player.Stopped = false;
                                player.Appearence = ">I<-T->I<";
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            player.Stopped = true;
                            break;
                        case ConsoleKey.Spacebar:
                            if (shotDelayTimer > SHOT_DELAY)
                            {
                                projectiles.Add(new Bullet("█", player.X + player.Appearence.Length / 2, player.Y, 1, true));
                                shotDelayTimer = 0;
                            }
                            break;
                        case ConsoleKey.Q:
                            if (!player.SpaceFlight)
                            {
                                player.SpaceFlight = true;
                                player.Appearence = ">I<-T->I<";
                            }
                            else
                            {
                                player.SpaceFlight = false;
                                player.Appearence = "<I>-T-<I>";
                            }
                            break;
                        case ConsoleKey.K:
                            foreach(Enemy e in enemies)
                            {
                                e.IsAlive = false;
                            }
                            break;
                        case ConsoleKey.O:
                            ticks = 45;
                            break;
                            
                    }
                } //fin du contrôle du joueur

                //gestion du déplacement en mode "SpaceFlight"
                if (player.SpaceFlight && !player.Stopped)
                {
                    if (ticks % playerSpeed == 0)
                    {
                        if (player.GoingLeft)
                        {
                            player.MoveShip(-2);
                        }
                        else
                        {
                            player.MoveShip(2);
                        }
                    }
                }

                //gestion du déplacement des tirs
                for(int i = 0; i < projectiles.Count; i++)
                {
                    if (projectiles[i].Y > 0 && projectiles[i].Y <= windowHeight && ticks % BULLET_SPEED == 0)
                    {
                        projectiles[i].Move();
                    }
                    else if (projectiles[i].Y <= 1 && projectiles[i].GoingUp || projectiles[i].Y >= windowHeight && !projectiles[i].GoingUp)
                    {
                        projectiles.Remove(projectiles[i]);
                    }

                }

                //gestion des collisions des tirs
                for (int i = 0; i < projectiles.Count; i++)
                {
                    //si un ennemi est touché
                    foreach (Enemy e in enemies)
                    {
                        if(projectiles.Count > i)
                        if (projectiles[i].CollidesWith(e) && projectiles[i].GoingUp)
                        {
                            e.IsAlive = false;
                            e.Appearence = " ";
                            projectiles.Remove(projectiles[i]);
                        }
                    }
                    //si l'ennemi special est touché
                    if (projectiles.Count > i)
                        if (projectiles[i].CollidesWith(specialEnemy) && projectiles[i].GoingUp)
                        {
                            specialEnemy.IsAlive = false;
                            specialEnemy.Appearence = " ";
                            projectiles.Remove(projectiles[i]);
                        }


                    //si joueur est touché
                    if (i < projectiles.Count)
                    if(!projectiles[i].GoingUp && projectiles[i].X >= player.X && projectiles[i].X <= player.X + player.Appearence.Length && projectiles[i].Y == player.Y)
                    {
                        projectiles.Remove(projectiles[i]);
                        player.Lives--;
                        livesIndicator = player.Appearence + " × " + player.Lives;

                    }
                }

                //apparition d'un ennemi special
                if(ticks % 200 == 0 && !specialEnemy.IsAlive)
                {
                    specialEnemy.Appearence = "███████████";
                    specialEnemy.IsAlive = true;
                    specialEnemy.X = -11;
                }

                //gestion de l'apparition de nouvelles vagues
                if (!waveAlive)
                {
                    player.Lives++;
                    livesIndicator = player.Appearence + " × " + player.Lives;
                    spawning = true;
                    //création de la nouvelle vague d'ennemis
                    for (int l = 0; l < ENEMY_ROW; l++)
                        for (int c = 0; c < ENEMY_ROW; c++)
                        {
                            enemies[l, c] = new Enemy("|=_=|", ConsoleColor.White, c * ("|=_=|".Length + 2), l * 2 - ENEMY_ROW * 2, 3, false);
                        }
                    waveAlive = true;
                }

                
                //gestion du déplacement des ennemis
                //on récupère l'ennemi qui se trouve le plus à gauche ou à droite dépendant du sens du groupe
                if (enemies[0, 0].IsGoingLeft)
                {
                    XtremeEnemy = GetEnemyExtremity(enemies, "left");
                    //si l'ennemi le plus à gauche se trouve au bord de l'écran, on fait changer de direction tous les ennemis
                    if(XtremeEnemy.X == 0)
                    {
                        for (int i = 0; i < ENEMY_ROW; i++)
                            for (int j = 0; j < ENEMY_ROW; j++)
                            {
                                enemies[i, j].IsChanging = true;
                            }
                    }
                }
                else
                {
                    XtremeEnemy = GetEnemyExtremity(enemies, "right");
                    //on fait de même mais dans le cas où le groupe se déplace vers la droite
                    if (XtremeEnemy.X == windowWidth - XtremeEnemy.Appearence.Length - 2)
                    {
                        for (int i = 0; i < ENEMY_ROW; i++)
                            for (int j = 0; j < ENEMY_ROW; j++)
                            {
                                enemies[i, j].IsChanging = true;
                            }
                    }
                } //fin déplacement ennemi

                //gestion des tirs ennemis
                foreach(Enemy e in GetFrontLineEnemies(enemies))
                {
                    if(e.IsAlive)
                    {
                        if(random.Next(e.FireFrequency) == e.FireFrequency - 1)
                        {
                            e.Shoot(projectiles);
                        }
                    }
                }

                //déplacement, vérification et chargement des ennemis
                waveAlive = false;
                spawning = false;
                foreach(Enemy e in enemies)
                {
                    if (ticks % enemies[0, 0].Speed == 0)
                    {
                        //on déplace les ennemis
                        e.Move();
                    }
                    if (e.IsAlive)
                    {
                        //on les charge dans le buffer
                        e.Load(ref buffer);
                    }

                    if (e.IsAlive)
                    {
                        //on vérifie qu'il y ait au moins un ennemi encore en vie
                        waveAlive = true;
                    }

                    if(e.Y < 0)
                    {
                        spawning = true;
                    }
                }

                if (spawning)
                {
                    foreach (Enemy e in enemies)
                    {
                        e.IsChanging = true;
                    }
                }

                //déplacement/chargement de l'enemi spécial
                if (specialEnemy.IsAlive && ticks % specialEnemy.Speed == 0)
                {
                    specialEnemy.SpecialMove();
                }

                specialEnemy.Load(ref buffer);

                //si le special quitte la fenêtre
                if (specialEnemy.X > windowWidth)
                {
                    specialEnemy.IsAlive = false;
                    specialEnemy.Appearence = "";
                }


                //chargement des éléments dans le buffer
                //le vaisseau du joueur
                player.LoadShip(ref buffer);

                //les projectiles
                foreach(Bullet b in projectiles)
                { 
                    b.Load(ref buffer);
                }

                //le HUD
                for (int i = 0; i < livesIndicator.Length; i++)
                {
                    buffer[0][windowWidth - livesIndicator.Length + i - 1] = livesIndicator[i];
                }

                timer.Stop();

                if(timer.ElapsedMilliseconds > 1)
                Debug.Write("BeforeWrite: " + timer.ElapsedMilliseconds + " | ");
                timer.Start();

                //écriture de l'entierté de la fenêtre
                string fullWindow = "";

                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < buffer.GetLength(0); i++)
                {

                    fullWindow += new string(buffer[i]) + " "; 
                }

                if (ticks % FLICKER_RATE == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                if (ticks % (FLICKER_RATE * 2) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                if (ticks % (FLICKER_RATE * 3) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(fullWindow);
                //fin de l'écriture des éléments

                //on vérifie qu'aucun ennemi ait atteint le vaisseau ou que le joueur n'aie plus de vies
                if (GetEnemyExtremity(enemies, "bottom").Y == player.Y || player.Lives < 0)
                {
                    hasLost = true;
                }

                //calcul du temps de tempo en fonction du temps pris à effectuer la boucle
                timer.Stop();

                delta = (FPS_TEMPO - (int)timer.ElapsedMilliseconds);
                //on empêche un thread.sleep avec un temps négatif
                if(delta < 0)
                {
                    delta = 0;
                }

                if(timer.ElapsedMilliseconds > 9)
                Debug.Write(" lowTime: " + timer.ElapsedMilliseconds + " | ");

                //on temporise le thread un moment
                Thread.Sleep(delta);
                ticks++;

            } while (!hasLost);

            Console.Clear();
            Console.SetCursorPosition(windowWidth / 2, windowHeight / 2);
            Console.WriteLine("Vous avez été envahi");
            Console.WriteLine("                                                          big seum");

        }//fin de Launch()


        public static Enemy GetEnemyExtremity(Enemy[,] enemies, string Extremity)
        {
            Enemy theMostXtreme;
            switch (Extremity)
            {
                case "left":
                    theMostXtreme = enemies[0, ENEMY_ROW - 1];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].X < theMostXtreme.X && enemies[i, j].IsAlive)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                case "right":
                    theMostXtreme = enemies[0, 0];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].X > theMostXtreme.X && enemies[i, j].IsAlive)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                case "top":
                    theMostXtreme = enemies[ENEMY_ROW - 1, 0];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].Y < theMostXtreme.Y && enemies[i, j].IsAlive)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                case "bottom":
                    theMostXtreme = enemies[0, 0];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].Y > theMostXtreme.Y && enemies[i, j].IsAlive)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                default:
                    theMostXtreme = enemies[0, 0];
                    break;

            }

            return theMostXtreme;
        }//fin de GetExtremityEnemy

        public List<Enemy> GetFrontLineEnemies(Enemy[,] enemies)
        {
            Enemy lowest;
            List<Enemy> frontLine = new List<Enemy>();
            for(int c = 0; c < enemies.GetLength(1); c++)
            {
                lowest = enemies[0, 0];
                for (int l = 0; l < enemies.GetLength(0); l++)
                {
                    if(enemies[l,c].Y > lowest.Y && enemies[l,c].IsAlive)
                    {
                        lowest = enemies[l, c];
                    }
                }
                frontLine.Add(lowest);

            }

            return frontLine;
        }
    }
}
