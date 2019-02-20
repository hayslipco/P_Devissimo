using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace BuffyInvaders
{
    public class Game
    {
        private const int ENEMY_ROW = 10;
        private const int FPS_TEMPO = 20;
        private const int ENEMY_BULLET_SPEED = 3;
        private const int SHOT_DELAY = 10;
        private const int BACKGROUND_THRESHOLD = 400;
        private const int FLICKER_RATE = 10;
        private const int PLAYER_RESPAWN_TIME = 100;

        public int windowWidth = Console.WindowWidth;
        public int windowHeight = Console.WindowHeight;

        private int shotDelayTimer = 0;
        private int shotDelay = 10;
        private int playerRespawnTimer = PLAYER_RESPAWN_TIME;
        private int delta;
        private int ticks;
        private int playerSpeed = 2;
        private int enemySpeed = 2;
        private int backgroundTicker = 2;
        private int graphicsInt;
        private int graphicsLoop;
        private int waveCount;
        private int waveSpawnX;

        private string livesIndicator;
        private string newWaveAppearence;
        private string fullWindow;
        private List<string> shipAnimation;

        private bool hasLost = false;
        private bool backgroundUp = true;
        private bool waveAlive = true;
        private bool spawning = false;
        private bool shortShot = false;
        private bool midShot = true;
        private bool longShot = false;
        private bool flickering; // affichage

        private Enemy XtremeEnemy;
        private Enemy oddXtremeEnemy;
        //ennemi stationaire invisible utilisé pour garder une condition à false (lors de vagues non-inversées)
        private Enemy invisibleEnemy;
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
            shipAnimation = new List<string>()
            {
                "<I>-T-<I>",
                "<I>-T-<I>",
                ">=<-T->=<",
                ">=<-T->=<"
            };

            //création du string indiquant le nombre de vies
            livesIndicator = player.Appearence + " × " + player.Lives;

            //initialisation du buffer dans lequel chaque caractère sera chargé avant de tout Write en un coup
            buffer = new char[windowHeight - 1][];
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                buffer[i] = new char[windowWidth];
            }
            enemies = new Enemy[ENEMY_ROW, ENEMY_ROW];

            //création de la vague d'ennemis, de l'ennemi spécial et de l'ennemi invisible
            for (int l = 0; l < ENEMY_ROW; l++)
                for (int c = 0; c < ENEMY_ROW; c++)
                {
                    enemies[l, c] = new Enemy("|-O-|", ConsoleColor.White, c * ("|-O-|".Length + 2), l * 2 + 1, enemySpeed, false);
                }

            specialEnemy = new Enemy("", ConsoleColor.Black, -11, 3, 4, true);
            specialEnemy.IsAlive = false;

            invisibleEnemy = new Enemy(" ", ConsoleColor.White, 1, 1, enemySpeed, false);
            invisibleEnemy.IsAlive = false;

            waveCount = 1;

            //boucle qui met à jour le jeu
            do
            {

                //démarrage du timer qui va calculer le temps de temporisation nécessaire
                timer.Restart();

                //augmentation des timer de délai
                shotDelayTimer++;
                playerRespawnTimer++;

                //gestion du timer en charge de l'arrière plan galactique
                if (backgroundUp)
                {
                    backgroundTicker++;
                    if (backgroundTicker > BACKGROUND_THRESHOLD)
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
                        //space background
                        if (flickering && i * (buffer[0].Length - j) % backgroundTicker == 0) // changement de l'affichage
                        {
                            buffer[i][j] = '*';
                        }
                        else //fin space background
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
                    for (int i = 0; i < 8; i++)
                    {
                        graphics.SLightningTwoD(buffer, Math.Abs(graphicsInt - (i * 15)));
                    }
                }//fin des jolis graphismes

                    //contrôle du joueur
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
                                    //player.Appearence = ">I<-T->I<";
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
                                    //player.Appearence = ">I<-T->I<";
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                player.Stopped = true;
                                break;
                            case ConsoleKey.Spacebar:
                                if (shotDelayTimer > shotDelay)
                                {
                                    if (shortShot)
                                    {
                                        projectiles.Add(new Bullet("■", player.X + (player.Appearence.Length / 3), player.Y, 4, true, true));
                                        projectiles.Add(new Bullet("■", player.X + (2 * player.Appearence.Length / 3), player.Y, 4, true, true));
                                    }
                                    else if (longShot)
                                    {
                                        projectiles.Add(new Bullet("▓", player.X + player.Appearence.Length / 2, player.Y, 1, true, false));
                                    }
                                    else if (midShot)
                                    {
                                        projectiles.Add(new Bullet("█", player.X + player.Appearence.Length / 2, player.Y, 2, true, false));
                                    }

                                    shotDelayTimer = 0;
                                }
                                break;
                            case ConsoleKey.Q:
                                if (!player.SpaceFlight)
                                {
                                    player.SpaceFlight = true;
                                    //player.Appearence = ">I<-T->I<";
                                }
                                else
                                {
                                    player.SpaceFlight = false;
                                    //player.Appearence = "<I>-T-<I>";
                                }
                                break;
                            case ConsoleKey.K:
                                foreach (Enemy e in enemies)
                                {
                                    e.IsAlive = false;
                                }
                                break;
                            case ConsoleKey.D1:
                                shotDelay = 15;
                                shortShot = false;
                                longShot = true;
                                midShot = false;
                                break;
                            case ConsoleKey.D2:
                                shotDelay = 8;
                                shortShot = false;
                                midShot = true;
                                longShot = false;
                                break;
                            case ConsoleKey.D3:
                                shotDelay = 4;
                                shortShot = true;
                                longShot = false;
                                midShot = false;
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
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        if (projectiles[i].Y >= 0 && projectiles[i].Y <= windowHeight)
                        {
                            if (projectiles[i].GoingUp && ticks % projectiles[i].speed == 0)
                            {
                                projectiles[i].Move();

                                //on retire les balles courtes
                                if (projectiles[i].IsShort && projectiles[i].Y < 2 * windowHeight / 3)
                                {
                                    projectiles.Remove(projectiles[i]);
                                }
                            }
                            else if (!projectiles[i].GoingUp && ticks % ENEMY_BULLET_SPEED == 0)
                            {
                                projectiles[i].Move();
                            }
                        }
                        else if (projectiles[i].Y < 1 && projectiles[i].GoingUp || projectiles[i].Y >= windowHeight && !projectiles[i].GoingUp)
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
                            if (projectiles.Count > i)
                                if (projectiles[i].GoingUp && projectiles[i].CollidesWith(e))
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
                            if (!projectiles[i].GoingUp && projectiles[i].X >= player.X && projectiles[i].X <= player.X + player.Appearence.Length && projectiles[i].Y == player.Y && playerRespawnTimer > PLAYER_RESPAWN_TIME)
                            {
                                projectiles.Remove(projectiles[i]);
                                player.Lives--;
                                livesIndicator = player.Appearence + " × " + player.Lives;
                                playerRespawnTimer = 0;

                            }
                    }

                    //apparition d'un ennemi special
                    if (ticks % 200 == 0 && !specialEnemy.IsAlive)
                    {
                        specialEnemy.Appearence = "██████";
                        specialEnemy.IsAlive = true;
                        specialEnemy.X = -specialEnemy.Appearence.Length;
                    }

                    //gestion de l'apparition de nouvelles vagues
                    if (!waveAlive && random.Next(20) == 10)
                    {
                        waveCount++;
                        player.Lives++;
                        livesIndicator = player.Appearence + " × " + player.Lives;
                        spawning = true;
                        newWaveAppearence = "|-O-|";

                        if (waveCount % 2 == 0)
                        {
                            waveSpawnX = windowWidth - ((newWaveAppearence.Length + 2) * ENEMY_ROW);
                        } else
                        {
                            waveSpawnX = 1;
                        }
                        //création de la nouvelle vague d'ennemis
                        for (int l = 0; l < ENEMY_ROW; l++)
                            if (waveCount % 4 == 0 && l % 2 == 0)
                            {
                                for (int c = 0; c < ENEMY_ROW; c++)
                                {
                                    enemies[l, c] = new Enemy(newWaveAppearence, ConsoleColor.White, c * (newWaveAppearence.Length + 2) + ((windowWidth / 2) - (ENEMY_ROW * (newWaveAppearence.Length + 2)) / 2), l * 2 - ENEMY_ROW * 2, 3, false);
                                    enemies[l, c].IsGoingLeft = true;
                                }
                            }
                            else if (waveCount % 4 == 0)
                            {
                                for (int c = 0; c < ENEMY_ROW; c++)
                                {
                                    enemies[l, c] = new Enemy(newWaveAppearence, ConsoleColor.White, c * (newWaveAppearence.Length + 2) + ((windowWidth / 2) - (ENEMY_ROW * (newWaveAppearence.Length + 2)) / 2), l * 2 - ENEMY_ROW * 2, 3, false);
                                }
                            }
                            else
                            {
                                for (int c = 0; c < ENEMY_ROW; c++)
                                {
                                    enemies[l, c] = new Enemy(newWaveAppearence, ConsoleColor.White, c * (newWaveAppearence.Length + 2) + waveSpawnX, l * 2 - ENEMY_ROW * 2, 3, false);
                                }
                            }
                        waveAlive = true;
                    }


                    //gestion du déplacement des ennemis
                    //on récupère l'ennemi qui se trouve le plus à gauche ou à droite dépendant du sens du groupe
                    if (enemies[0, 0].IsGoingLeft)
                    {
                        //ajout de l'ennemi à observer pour vérifier que les deux groupes restent dans la fenêtre (que lors de vague inversée)
                        if (waveCount % 4 == 0)
                        {
                            XtremeEnemy = GetEnemyExtremity(enemies, "left", 2, 0);
                            oddXtremeEnemy = GetEnemyExtremity(enemies, "right", 2, 1);
                        }
                        else
                        {
                            XtremeEnemy = GetEnemyExtremity(enemies, "left", 1, 0);
                            oddXtremeEnemy = invisibleEnemy;
                        }

                        //condition pour empêcher que oddXtremeEnemy et XtremeEnemy soient le même
                        if (XtremeEnemy == oddXtremeEnemy)
                        {
                            oddXtremeEnemy = invisibleEnemy;
                        }

                        //si l'ennemi le plus à gauche se trouve au bord de l'écran (ou aussi l'ennemi le plus à droite en cas de vague inversée),
                        //on fait changer de direction tous les ennemis
                        if (XtremeEnemy.X == 0 || oddXtremeEnemy.X == windowWidth - oddXtremeEnemy.Appearence.Length - 1)
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


                        if (waveCount % 4 == 0)
                        {
                            XtremeEnemy = GetEnemyExtremity(enemies, "right", 2, 0);
                            oddXtremeEnemy = GetEnemyExtremity(enemies, "left", 2, 1);
                        }
                        else
                        {
                            XtremeEnemy = GetEnemyExtremity(enemies, "right", 1, 0);
                            oddXtremeEnemy = invisibleEnemy;
                        }

                        //condition pour empêcher que oddXtremeEnemy et XtremeEnemy soient le même
                        if (XtremeEnemy == oddXtremeEnemy)
                        {
                            oddXtremeEnemy = invisibleEnemy;
                        }

                        //on fait de même mais dans le cas où le groupe se déplace vers la droite
                        if (XtremeEnemy.X == windowWidth - XtremeEnemy.Appearence.Length - 1 || oddXtremeEnemy.X == 0)
                        {
                            for (int i = 0; i < ENEMY_ROW; i++)
                                for (int j = 0; j < ENEMY_ROW; j++)
                                {
                                    enemies[i, j].IsChanging = true;
                                }
                        }
                    } //fin déplacement ennemi

                    //gestion des tirs ennemis
                    foreach (Enemy e in GetFrontLineEnemies(enemies))
                    {
                        if (e.IsAlive)
                        {
                            if (random.Next(e.FireFrequency) == e.FireFrequency - 1)
                            {
                                e.Shoot(projectiles);
                            }
                        }
                    }

                    //déplacement, vérification et chargement des ennemis
                    waveAlive = false;
                    spawning = false;
                    foreach (Enemy e in enemies)
                    {
                        if (ticks % enemies[0, 0].Speed == 0)
                        {
                            //on déplace les ennemis
                            e.Move();
                        }
                        if (e.Appearence != " ")
                        {
                            //on les charge dans le buffer
                            e.Load(buffer);
                        }

                        if (e.IsAlive)
                        {
                            //on vérifie qu'il y ait au moins un ennemi encore en vie
                            waveAlive = true;
                        }

                        if (e.Y < 0)
                        {
                            spawning = true;
                        }

                        if (!e.IsAlive)
                        {
                            e.Die();
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

                    specialEnemy.Load(buffer);

                    //si le special quitte la fenêtre
                    if (specialEnemy.X > windowWidth)
                    {
                        specialEnemy.IsAlive = false;
                        specialEnemy.Appearence = "";
                    }


                    //chargement des éléments dans le buffer
                    //le vaisseau du joueur

                    //condition pour faire clignoter le vaisseau
                    if (playerRespawnTimer < PLAYER_RESPAWN_TIME)
                    {
                        player.flicker(playerRespawnTimer);
                    }
                    else if (player.SpaceFlight)
                    {
                        player.Appearence = shipAnimation[ticks % shipAnimation.Count];
                    }
                    else
                    {
                        player.Appearence = "<I>-T-<I>";
                    }
                    player.LoadShip(buffer);

                    //les projectiles
                    foreach (Bullet b in projectiles)
                    {
                        b.Load(buffer);
                    }

                    //le HUD
                    for (int i = 0; i < livesIndicator.Length; i++)
                    {
                        buffer[0][windowWidth - livesIndicator.Length + i - 1] = livesIndicator[i];
                    }

                    timer.Stop();

                    if (timer.ElapsedMilliseconds > 1)
                        Debug.Write("BeforeWrite: " + timer.ElapsedMilliseconds + " | ");
                    timer.Start();

                    //écriture de l'entierté de la fenêtre
                    fullWindow = "";

                    Console.SetCursorPosition(0, 0);
                    for (int i = 0; i < buffer.GetLength(0); i++)
                    {
                        fullWindow += new string(buffer[i]);
                    }

                    if (ticks % FLICKER_RATE == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }

                    if (ticks % (FLICKER_RATE * 2) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }

                    if (ticks % (FLICKER_RATE * 3) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write(fullWindow);

                    fullWindow = "";
                    //fin de l'écriture des éléments

                    //on vérifie qu'aucun ennemi ait atteint le vaisseau ou que le joueur n'aie plus de vies
                    if (GetEnemyExtremity(enemies, "bottom", 1, 0).Y == player.Y || player.Lives < 0)
                    {
                        hasLost = true;
                    }

                    //calcul du temps de tempo en fonction du temps pris à effectuer la boucle
                    timer.Stop();

                    delta = (FPS_TEMPO - (int)timer.ElapsedMilliseconds);
                    //on empêche un thread.sleep avec un temps négatif
                    if (delta < 0)
                    {
                        delta = 0;
                    }

                    if (timer.ElapsedMilliseconds > 9)
                        Debug.Write(" lowTime: " + timer.ElapsedMilliseconds + " | ");

                    //on temporise le thread un moment
                    Thread.Sleep(delta);
                    ticks++;

                } while (!hasLost) ;

                Console.Clear();
                Console.SetCursorPosition(windowWidth / 2, windowHeight / 2);
                Console.WriteLine("Vous avez été envahi");
                Console.WriteLine("                                                          big seum");

            }//fin de Launch()


        public static Enemy GetEnemyExtremity(Enemy[,] enemies, string Extremity, int lineSpecificMultiplier, int lineSpecificIncrementer)
        {
            Enemy theMostXtreme;
            switch (Extremity)
            {
                case "left":
                    theMostXtreme = enemies[0, ENEMY_ROW - 1];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].X < theMostXtreme.X && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                case "right":
                    theMostXtreme = enemies[0, 0];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].X > theMostXtreme.X && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                case "top":
                    theMostXtreme = enemies[ENEMY_ROW - 1, 0];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].Y < theMostXtreme.Y && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
                            {
                                theMostXtreme = enemies[i, j];
                            }
                    break;
                case "bottom":
                    theMostXtreme = enemies[0, 0];
                    for (int i = 0; i < ENEMY_ROW; i++)
                        for (int j = 0; j < ENEMY_ROW; j++)
                            if (enemies[i, j].Y > theMostXtreme.Y && enemies[i, j].IsAlive && (i + lineSpecificIncrementer) % lineSpecificMultiplier == 0)
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
        /// <summary>
        /// Désactive/active l'affichage in-game
        /// </summary>
        /// <returns></returns>
        public void Flickering()
        {
            if (flickering == false)
            {
                flickering = true;
            }
            else
            {
                flickering = false;
            }
        }
    }
}
