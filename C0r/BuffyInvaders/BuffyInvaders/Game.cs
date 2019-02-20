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
        private int playerSpeed = 1;
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

            //liste pour gérer l'animation du vaisseau
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

            //ennemi spécial initialisé en étant vide, plus tard nous modifierons son apparence
            specialEnemy = new Enemy("", ConsoleColor.Black, -11, 3, 4, true);
            specialEnemy.IsAlive = false;

            //ennemi utilisé lors des vagues inversés, cela empêche la double vérification des extremités 
            invisibleEnemy = new Enemy(" ", ConsoleColor.White, 1, 1, enemySpeed, false);
            invisibleEnemy.IsAlive = false;

            waveCount = 1;

            //boucle qui met à jour le jeu
            do
            {

                //démarrage du timer qui va calculer le temps de temporisation nécessaire
                timer.Restart();

                //augmentation des timers de délai
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

                //on augmente les int de 1 au même moment que change la couleur
                if (ticks % FLICKER_RATE == 0)
                {
                    graphicsInt++;
                    graphicsLoop++;
                }

                //graphicsInt permettant le placement des éclairs sur l'axe Y, il doit rester < WindowHeight
                if (graphicsInt >= windowHeight)
                {
                    graphicsInt = 0;
                }

                //graphicsLoop permet de faire le changement entre les éclairs et le débris, on utilise windowHeight ici juste pour ne pas avoir à recréer de variable
                if (graphicsLoop > windowHeight / 2)
                {
                    graphicsLoop = 0;
                }

                //fait que la moitié du temps soit dédié au débris et l'autre moitié au éclairs
                if (graphicsLoop < windowHeight / 4)
                {
                    graphics.SideLigthning(buffer);
                }
                else
                {
                    //boucle pour faire apparaître un certain nombre d'éclairs à la fois
                    for (int i = 0; i < 8; i++)
                    {
                        graphics.SLightningTwoD(buffer, Math.Abs(graphicsInt - (i * 15)));
                    }
                }//fin des jolis graphismes

                    //contôle du joueur
                    //on entre dans la condition que lorsqu'une touche est entrée
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo entry = Console.ReadKey(true);

                        switch (entry.Key)
                        {
                            case ConsoleKey.RightArrow:
                                if (!player.SpaceFlight) //en mode classique, on déplace le vaisseau de 2 pixels à chaque appui de boutton
                                {
                                    player.MoveShip(2);
                                }
                                else //en mode SpaceFlight, on change le bool qui régit la direction du vaisseau
                                {
                                    player.GoingLeft = false;
                                    player.Stopped = false;
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
                                }
                                break;
                            case ConsoleKey.DownArrow:
                            //on arrete le vaisseau
                                player.Stopped = true;
                                break;
                            case ConsoleKey.Spacebar:
                            //condition pour gérer le délai de tir
                                if (shotDelayTimer > shotDelay)
                                {
                                //conditions pour vérifier le mode de tir sélectionné, on fait varier le nombre, l'apparence et la vitesse en fonction du mode
                                    if (shortShot)
                                    {
                                        projectiles.Add(new Bullet("■", player.X + (player.Appearence.Length / 3), player.Y, 4, true, true));
                                        projectiles.Add(new Bullet("■", player.X + (2 * player.Appearence.Length / 3), player.Y, 4, true, true));
                                        projectiles.Add(new Bullet("■", player.X + player.Appearence.Length, player.Y, 4, true, true));
                                    }
                                    else if (longShot)
                                    {
                                        projectiles.Add(new Bullet("▓", player.X + player.Appearence.Length / 2, player.Y, 1, true, false));
                                    }
                                    else if (midShot)
                                    {
                                        projectiles.Add(new Bullet("█", player.X + player.Appearence.Length / 2, player.Y, 2, true, false));
                                    }

                                    //on met à zéro pour qu'il prenne un certain temps à se remettre > shotDelay, pour que la condition soit de nouveau vraie
                                    shotDelayTimer = 0;
                                }
                                break;
                            case ConsoleKey.Q:
                            //permet de basculer entre le mode de contrôle classique et le mode SpaceFlight
                                if (!player.SpaceFlight)
                                {
                                    player.SpaceFlight = true;
                                }
                                else
                                {
                                    player.SpaceFlight = false;
                                }
                                break;
                            case ConsoleKey.K: //tue tous les ennemis 
                                foreach (Enemy e in enemies)
                                {
                                    e.IsAlive = false;
                                }
                                break;
                            //Gestion du mode de tir
                            //chaque case va modifier les bools pour connaître el mode de tir, de plus, shotDelay est changé car chaque mode de tir a des délais différents
                            case ConsoleKey.D1: //tir long
                                shotDelay = 20;
                                shortShot = false;
                                longShot = true;
                                midShot = false;
                                break;
                            case ConsoleKey.D2: //tir moyen/classique
                                shotDelay = 8;
                                shortShot = false;
                                midShot = true;
                                longShot = false;
                                break;
                            case ConsoleKey.D3://tir court ==> trois projectiles lancés mais ne dépassent pas le tier du bas de la console
                                shotDelay = 15;
                                shortShot = true;
                                longShot = false;
                                midShot = false;
                                break;

                        }
                    } //fin du contrôle du joueur

                    //gestion du déplacement en mode "SpaceFlight" lorsque le joueur n'est pas à l'arrêt
                    if (player.SpaceFlight && !player.Stopped)
                    {
                    //playerSpeed est utilisé pour gérer la vitesse du joueur
                        if (ticks % playerSpeed == 0)
                        {
                            //s'il va à gauche, on le déplace vers la gauche...
                            if (player.GoingLeft)
                            {
                                player.MoveShip(-1);
                            }
                            else
                            {
                                player.MoveShip(1);
                            }
                        }
                    }

                    //gestion du déplacement des tirs
                    //boucle passant à travers chaque projectile présent dans le jeu (dans projectiles)
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        //tous les projectiles se trouvant dans la fenêtre de la console vont ête déplacés (sinon elles sont supprimées)
                        if (projectiles[i].Y >= 0 && projectiles[i].Y <= windowHeight)
                        {
                            //condition pour gérer la vitesse des projectiles qui ont chacun une variable speed attribué
                            if (ticks % projectiles[i].speed == 0)
                            {
                                projectiles[i].Move();

                                //on retire les balles courtes au tiers de la console
                                if (projectiles[i].IsShort && projectiles[i].Y < 2 * windowHeight / 3)
                                {
                                    projectiles.Remove(projectiles[i]);
                                }
                            }
                        }
                        //condition pour enlever les projectiles du joueur lorsqu'ils sortent de la fenêtre de la console
                        else if (projectiles[i].Y < 1 /*&& projectiles[i].GoingUp*/ || projectiles[i].Y >= windowHeight /*&& !projectiles[i].GoingUp*/)
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
                            //condition pour empêcher du pointeur null exception s'il arrivait qu'on enlève des projectiles
                            if (projectiles.Count > i)
                            //si un projectile montant, donc au joueur, entre en collision avec un ennemi
                                if (projectiles[i].GoingUp && projectiles[i].CollidesWith(e))
                                {
                                    e.IsAlive = false;
                                    e.Appearence = " ";
                                    projectiles.Remove(projectiles[i]);
                                }
                        }
                        //si l'ennemi special est touché, on fait de même
                        if (projectiles.Count > i)
                            if (projectiles[i].GoingUp && projectiles[i].CollidesWith(specialEnemy))
                            {
                                specialEnemy.IsAlive = false;
                                specialEnemy.Appearence = " ";
                                projectiles.Remove(projectiles[i]);
                            }


                        //si joueur est touché
                        //condition pour empêcher le pointer null
                        if (i < projectiles.Count)
                            //condition pour vérifier que le projectile descend, qu'il soit au même niveau y, qu'il soit dans la plage x du vaisseau et que le joueur ne soit plus invincible (après avoi reçu une balle récemment)
                            if (!projectiles[i].GoingUp && projectiles[i].X >= player.X && projectiles[i].X <= player.X + player.Appearence.Length && projectiles[i].Y == player.Y && playerRespawnTimer > PLAYER_RESPAWN_TIME)
                            {
                                projectiles.Remove(projectiles[i]);
                                player.Lives--;
                                //on met à jour le string indiquant le nombre de vies du joueur
                                livesIndicator = player.Appearence + " × " + player.Lives;
                                //on met à zéro le timer de respawn pour donner un temps d'invincibilité au joueur
                                playerRespawnTimer = 0;
                            }
                    }

                    //apparition d'un ennemi special
                    //tous les 200 ticks, on fait spawn un ennemi spécial, s'il n'est pas déjà en vie
                    if (ticks % 200 == 0 && !specialEnemy.IsAlive)
                    {
                        specialEnemy.Appearence = "██████";
                        specialEnemy.IsAlive = true;
                        //on le fait apparaître juste à gauche de la fenêtre de console
                        specialEnemy.X = -specialEnemy.Appearence.Length;
                    }

                    //gestion de l'apparition de nouvelles vagues
                    //lorsqu'il n y a plus d'ennemis vivants dans la vague et que random atteignent un certain nombre (cela permet un petit temps aléatoire de temporisation entre les vagues)
                    if (!waveAlive && random.Next(20) == 10)
                    {
                        waveCount++;
                        player.Lives++;
                        livesIndicator = player.Appearence + " × " + player.Lives;
                        //booléen pour gérer le déplacement des ennemis pendant leur arrivée dans la fenêtre de console
                        spawning = true;
                        //string utilisé pour définir l'apparence des ennemis de la prochaine vague
                        newWaveAppearence = "|-O-|";

                        //condition pour que les vagues alternent leur arrivée de la gauche et de la droite
                        if (waveCount % 2 == 0)
                        {
                            waveSpawnX = windowWidth - ((newWaveAppearence.Length + 2) * ENEMY_ROW);
                        } else
                        {
                            waveSpawnX = 1;
                        }
                        //création de la nouvelle vague d'ennemis
                        //boucle itérant à travers chaque ligne de la vague
                        for (int l = 0; l < ENEMY_ROW; l++)
                            //lors des vagues inversées, la vague apparaît au milieu de la fenêtre de console et une ligne sur deux aura sa direction de départ changée
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

                        //après avoir rempli le tableau enemies de nouveaux ennemis, on peut considérer la vague comme vivante
                        waveAlive = true;
                    }


                    //gestion du déplacement des ennemis
                    //on récupère l'ennemi (ou les ennemis) qui se trouve(nt) le plus à gauche ou à droite dépendant du sens du groupe

                    //si le groupe va à gauche
                    if (enemies[0, 0].IsGoingLeft)
                    {
                        //ajout de l'ennemi à observer pour vérifier que les deux groupes restent dans la fenêtre (uniquement lors de vague inversée)
                        if (waveCount % 4 == 0)
                        {
                            //oddXtremeEnemy utilisé lors de vague inversée pour gérer le deuxième groupe d'ennemis

                            XtremeEnemy = GetEnemyExtremity(enemies, "left", 2, 0);
                            oddXtremeEnemy = GetEnemyExtremity(enemies, "right", 2, 1);
                        }
                        else
                        {
                            //lors de vagues non inversée, l'ennemi pour vérifier le deuxième groupe est envoyé à l'ennemi stationnaire invisible pour empêcher une fausse interprétation de la position de l'essein
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
                            //for (int i = 0; i < ENEMY_ROW; i++)
                            //    for (int j = 0; j < ENEMY_ROW; j++)
                            //    {
                            //        enemies[i, j].IsChanging = true;
                            //    }

                            foreach(Enemy e in enemies)
                            {
                            e.IsChanging = true;
                            }
                        }
                    }
                    //si le groupe va à droite
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
                            //for (int i = 0; i < ENEMY_ROW; i++)
                            //    for (int j = 0; j < ENEMY_ROW; j++)
                            //    {
                            //        enemies[i, j].IsChanging = true;
                            //    }

                            foreach(Enemy e in enemies)
                            {
                                e.IsChanging = true;
                            }
                        }
                    } //fin déplacement ennemi

                    //gestion des tirs ennemis
                    foreach (Enemy e in GetFrontLineEnemies(enemies))
                    {
                        //si l'ennemi le plus à l'avant de chaque colonne est en vie
                        if (e.IsAlive)
                        {
                            //on fait tirer l'ennemi de manîère aléatoire en fonction de la propriété FireFrequency
                            if (random.Next(e.FireFrequency) == 1)
                            {
                                e.Shoot(projectiles, ENEMY_BULLET_SPEED);
                            }
                        }
                    }

                    //déplacement, vérification et chargement des ennemis
                    //on considère la vague comme morte jusqu'à ce qu'on trouve un ennemi vivant dans enemies
                    waveAlive = false;
                    //on considère que la vague n'est pas entrain d'apparaître tant qu'on ne trouve aucun ennemi qui se trouve au dessus de la fenêtre de la console
                    spawning = false;
                    foreach (Enemy e in enemies)
                    {
                        //condition pour gérer la vitesse des ennemis
                        if (ticks % enemies[0, 0].Speed == 0)
                        {
                            //on déplace les ennemis
                            e.Move();
                        }
                        //si l'ennemi n'est pas mort (ou n'a pas fini son animation de destruction)
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
                            //on vérifie que tous les ennemis soient dans la fenêtre de la console
                            spawning = true;
                        }

                        //condition pour lancer l'animation de destruction des ennemis
                        if (!e.IsAlive)
                        {
                            e.Die();
                        }
                    }

                //s'il manque des ennemis, on fait "descendre" toute la vague en les faisant constamment changer de sens (et donc descendre d'un cran)
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

                    //si le special quitte la fenêtre, on le considère comme mort
                    if (specialEnemy.X > windowWidth)
                    {
                        specialEnemy.IsAlive = false;
                        specialEnemy.Appearence = "";
                    }


                    //chargement des éléments dans le buffer

                    //le vaisseau du joueur
                    //condition pour faire clignoter le vaisseau lorsqu'il a été touché
                    if (playerRespawnTimer < PLAYER_RESPAWN_TIME)
                    {
                        player.flicker(playerRespawnTimer);
                    }
                    //animation du vaisseau en mode SpaceFLight
                    else if (player.SpaceFlight)
                    {
                        //on fait varier l'apparence du vaisseau selon le nombre du tick
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

                    //le HUD (nombre de vies du joueur)
                    for (int i = 0; i < livesIndicator.Length; i++)
                    {
                        buffer[0][windowWidth - livesIndicator.Length + i - 1] = livesIndicator[i];
                    }

                    timer.Stop();

                    if (timer.ElapsedMilliseconds > 1)
                        Debug.Write("BeforeWrite: " + timer.ElapsedMilliseconds + " | ");
                    timer.Start();

                    //écriture de l'entierté de la fenêtre

                    //on remet le string de la fenêtre entière à vide
                    fullWindow = "";

                    //concatenation de chaque ligne de char de buffer dans fullWindow
                    for (int i = 0; i < buffer.GetLength(0); i++)
                    {
                        fullWindow += new string(buffer[i]);
                    }

                    //conditions pour gérer les changements de couleurs
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

                //on écrit toute la fenêtre avec un string
                Console.SetCursorPosition(0, 0);
                Console.Write(fullWindow);

                    //fin de l'écriture des éléments

                    //on vérifie qu'aucun ennemi ait atteint le vaisseau ou que le joueur n'aie plus de vies
                    if (GetEnemyExtremity(enemies, "bottom", 1, 0).Y == player.Y || player.Lives < 0)
                    {
                        hasLost = true;
                    }

                    //calcul du temps de tempo en fonction du temps pris à effectuer la boucle update
                    timer.Stop();
                    
                    //variable du temps à attendre
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

        /// <summary>
        /// Méthode pour trouver un des ennemis vivants se trouvant aux extrémités du groupe
        /// </summary>
        /// <param name="enemies"></param>
        /// <param name="Extremity">extrémité à vérifier (haut, bas, gauche, droite)</param>
        /// <param name="lineSpecificMultiplier">le multiple des lignes à vérifier</param>
        /// <param name="lineSpecificIncrementer">un décalage pour seléctionner exactement quels lignes sont à vérifier</param>
        /// <returns></returns>
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
    }
}
