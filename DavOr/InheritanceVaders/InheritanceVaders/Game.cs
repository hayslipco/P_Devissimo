﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    class Game : Common
    {

        private int shotDelayTimer = 0;
        private int shotDelay = 15;
        private int shieldDelayTimer = 250;
        private int shieldDelay = 250;
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
        private List<string> playerWithShield;
        private List<string> normalPlayer;

        private bool exitGame;
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
        //private Graphics graphics = new Graphics();
        private Random random = new Random();

        private char[][] buffer;
        private List<Element> elements;
        private Enemy[,] enemySwarm;
        private Enemy specialEnemy;
        private List<Bullet> projectiles = new List<Bullet>();
        private List<Enemy> enemies;
        private Stopwatch timer = new Stopwatch();


        public void Launch()
        {
            
            Console.Clear();
            exitGame = false;

            //intialisation de la liste contenant tous les éléments
            elements = new List<Element>();

            //initialisation de la liste contenant tous les ennemis
            enemies = new List<Enemy>();

            //création du vaisseau joueur
           SpaceShip player = new SpaceShip(5, windowHeight - 3, playerSpeed, new List<string> { "/\\     /\\", "<I>-T-<I>" });

            normalPlayer = new List<string> { "/\\     /\\", "<I>-T-<I>" };
            playerWithShield = new List<string> { "▀▀▀▀▀▀▀▀▀", "<I>-T-<I>" };

            //initialisation du buffer dans lequel chaque caractère sera chargé avant de tout Write en un coup
            buffer = new char[windowHeight - 1][];
            for (int i = 0; i < buffer.GetLength(0); i++)
            {
                buffer[i] = new char[windowWidth];
            }

            //création du string indiquant le nombre de vies
            livesIndicator = "♥ × " + player.Lives;

            //création de la vague d'ennemis, de l'ennemi spécial et de l'ennemi invisible
            enemySwarm = new Enemy[ENEMY_ROW, ENEMY_ROW];

            for (int l = 0; l < ENEMY_ROW; l++)
                for (int c = 0; c < ENEMY_ROW; c++)
                {
                    enemySwarm[l, c] = new Enemy(c * ("|-O-|".Length + 2), l * 2 + 1, enemySpeed, new List<string> { "|-O-|" }, false);
                    enemies.Add(enemySwarm[l, c]);
                }

            //ennemi spécial initialisé en étant vide, plus tard nous modifierons son apparence
            specialEnemy = new Enemy(-11, 3, 4, new List<string> { "" }, true);
            specialEnemy.IsAlive = false;
            enemies.Add(specialEnemy);

            //ennemi utilisé lors des vagues inversés, cela empêche la double vérification des extremités 
            invisibleEnemy = new Enemy(1, 1, enemySpeed, new List<string> { "" }, false);
            invisibleEnemy.IsAlive = false;
            enemies.Add(invisibleEnemy);

            waveCount = 1;

            //boucle qui met à jour le jeu
            do
            {
                //démarrage du timer qui va calculer le temps de temporisation nécessaire
                timer.Restart();

                //augmentation des timers de délai
                shotDelayTimer++;
                playerRespawnTimer++;
                shieldDelayTimer++;

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
                        case ConsoleKey.E:
                            if(shieldDelayTimer > shieldDelay)
                            player.ShieldUp = true;
                            shieldDelayTimer = 0;
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
                        case ConsoleKey.Spacebar:
                            //condition pour gérer le délai de tir
                            if (shotDelayTimer > shotDelay)
                            {
                                //conditions pour vérifier le mode de tir sélectionné, on fait varier le nombre, l'apparence et la vitesse en fonction du mode
                                if (shortShot)
                                {
                                    projectiles.Add(new Bullet(new List<string> { "■" }, player.X + (player.MaxLength / 3), player.Y, 4, true, true));
                                    projectiles.Add(new Bullet(new List<string> { "■" }, player.X + (2 * player.MaxLength / 3), player.Y, 4, true, true));
                                    projectiles.Add(new Bullet(new List<string> { "■" }, player.X + player.MaxLength, player.Y, 4, true, true));
                                }
                                else if (longShot)
                                {
                                    projectiles.Add(new Bullet(new List<string> { "█", "█" }, player.X + player.MaxLength / 2, player.Y, 1, true, false));
                                }
                                else if (midShot)
                                {
                                    projectiles.Add(new Bullet(new List<string> { "█" }, player.X + player.MaxLength / 2, player.Y, 2, true, false));
                                }

                                //on met à zéro pour qu'il prenne un certain temps à se remettre > shotDelay, pour que la condition soit de nouveau vraie
                                shotDelayTimer = 0;
                            }
                            break;
                        //Gestion du mode de tir
                        //chaque case va modifier les bools pour connaître el mode de tir, de plus, shotDelay est changé car chaque mode de tir a des délais différents
                        case ConsoleKey.D1: //tir long
                            shotDelay = 25;
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
                        case ConsoleKey.K: //tue tous les ennemis 
                            foreach (Enemy e in enemies)
                            {
                                e.IsAlive = false;
                            }
                            break;
                        case ConsoleKey.Escape:
                            exitGame = true;
                            projectiles.Clear();
                            break;
                    }
                }

                //gestion du déplacement en mode "SpaceFlight" lorsque le joueur n'est pas à l'arrêt
                if (player.SpaceFlight && !player.Stopped)
                {
                    //playerSpeed est utilisé pour gérer la vitesse du joueur
                    if (ticks % player.Speed == 0)
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

                //gestion des tirs ennemis
                foreach (Enemy e in GetFrontLineEnemies(enemySwarm))
                {
                    //si l'ennemi le plus à l'avant de chaque colonne est en vie
                    if (e.IsAlive)
                    {
                        //on fait tirer l'ennemi de manîère aléatoire en fonction de la propriété FireFrequency
                        if (random.Next(e.FireFrequency) == 1)
                        {
                            e.Shoot(projectiles);
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
                        if (ticks % projectiles[i].Speed == 0)
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
                    else if (projectiles[i].Y < 1 || projectiles[i].Y >= windowHeight)
                    {
                        projectiles.Remove(projectiles[i]);
                        
                    }
                } //fin des déplacements de tirs
                

                //gestion des collisions des tirs
                for (int i = 0; i < projectiles.Count; i++)
                {
                    //si un ennemi est touché
                    foreach (Enemy e in enemies)
                    {
                        //condition pour empêcher du pointeur null exception s'il arrivait qu'on enlève des projectiles
                        if (projectiles.Count > i)
                            //si un projectile montant, donc au joueur, entre en collision avec un ennemi
                            if (projectiles[i].GoingUp && projectiles[i].CollidesWith(e) && e.IsAlive)
                            {
                                e.IsAlive = false;
                                e.Appearence.Clear();
                                e.Appearence.Add(" ");
                                projectiles.Remove(projectiles[i]);
                            }
                    }
                    //si joueur est touché
                    //condition pour empêcher le pointer null
                    if (i < projectiles.Count)
                        //condition pour vérifier que le projectile descend, qu'il soit au même niveau y, qu'il soit dans la plage x du vaisseau et que le joueur ne soit plus invincible (après avoi reçu une balle récemment)
                        if (projectiles[i].CollidesWith(player) && !projectiles[i].GoingUp && playerRespawnTimer > PLAYER_RESPAWN_TIME)
                        {
                            if (!player.ShieldUp)
                            {
                                projectiles.Remove(projectiles[i]);
                                player.Lives--;
                                //on met à jour le string indiquant le nombre de vies du joueur
                                livesIndicator = "♥ × " + player.Lives;
                                //on met à zéro le timer de respawn pour donner un temps d'invincibilité au joueur
                                playerRespawnTimer = 0;
                            }
                            else
                            {
                                projectiles[i].GoingUp = true;
                                shieldDelayTimer -= 20;
                            }
                        }
                } //fin gestion des collisions

                //gestion de l'apparition de nouvelles vagues
                //lorsqu'il n y a plus d'ennemis vivants dans la vague et que random atteignent un certain nombre (cela permet un petit temps aléatoire de temporisation entre les vagues)
                if (!waveAlive && random.Next(20) == 10)
                {
                    waveCount++;
                    player.Lives++;
                    livesIndicator = "♥ × " + player.Lives;
                    //booléen pour gérer le déplacement des ennemis pendant leur arrivée dans la fenêtre de console
                    spawning = true;
                    //string utilisé pour définir l'apparence des ennemis de la prochaine vague
                    newWaveAppearence = "|-O-|";

                    //condition pour que les vagues alternent leur arrivée de la gauche et de la droite
                    if (waveCount % 2 == 0)
                    {
                        waveSpawnX = windowWidth - ((newWaveAppearence.Length + 2) * ENEMY_ROW);
                    }
                    else
                    {
                        waveSpawnX = 1;
                    }

                    //création de la nouvelle vague d'ennemis
                    //boucle itérant à travers chaque ligne de la vague
                    for (int l = 0; l < ENEMY_ROW; l++)
                    {

                        //lors des vagues inversées, la vague apparaît au milieu de la fenêtre de console et une ligne sur deux aura sa direction de départ changée
                        if (waveCount % 4 == 0)
                        {
                            for (int c = 0; c < ENEMY_ROW; c++)
                            {
                                enemySwarm[l, c] = new Enemy(c * (newWaveAppearence.Length + 2) + ((windowWidth / 2) - (ENEMY_ROW * (newWaveAppearence.Length + 2)) / 2), l * 2 - ENEMY_ROW * 2, 3, new List<string> { newWaveAppearence }, false);
                                if (l % 2 == 0)
                                {
                                    enemySwarm[l, c].IsGoingLeft = true;
                                }
                                enemies.Add(enemySwarm[l, c]);
                            }
                        }
                        else
                        {
                            for (int c = 0; c < ENEMY_ROW; c++)
                            {
                                enemySwarm[l, c] = new Enemy(c * (newWaveAppearence.Length + 2) + waveSpawnX, l * 2 - ENEMY_ROW * 2, 3, new List<string> { newWaveAppearence }, false);
                                enemies.Add(enemySwarm[l, c]);   
                            }
                        }
                    }

                    //après avoir rempli le tableau enemies de nouveaux ennemis, on peut considérer la vague comme vivante
                    waveAlive = true;
                } //fin apparition de nouvelles vagues

                //gestion du déplacement des ennemis
                //on récupère l'ennemi (ou les ennemis) qui se trouve(nt) le plus à gauche ou à droite dépendant du sens du groupe

                //si le groupe va à gauche
                if (enemySwarm[0, 0].IsGoingLeft)
                {
                    //ajout de l'ennemi à observer pour vérifier que les deux groupes restent dans la fenêtre (uniquement lors de vague inversée)
                    if (waveCount % 4 == 0)
                    {
                        //oddXtremeEnemy utilisé lors de vague inversée pour gérer le deuxième groupe d'ennemis

                        XtremeEnemy = GetEnemyExtremity(enemySwarm, "left", 2, 0);
                        oddXtremeEnemy = GetEnemyExtremity(enemySwarm, "right", 2, 1);
                    }
                    else
                    {
                        //lors de vagues non inversée, l'ennemi pour vérifier le deuxième groupe est envoyé à l'ennemi stationnaire invisible pour empêcher une fausse interprétation de la position de l'essein
                        XtremeEnemy = GetEnemyExtremity(enemySwarm, "left", 1, 0);
                        oddXtremeEnemy = invisibleEnemy;
                    }

                    //condition pour empêcher que oddXtremeEnemy et XtremeEnemy soient le même
                    if (XtremeEnemy == oddXtremeEnemy)
                    {
                        oddXtremeEnemy = invisibleEnemy;
                    }

                    //si l'ennemi le plus à gauche se trouve au bord de l'écran (ou aussi l'ennemi le plus à droite en cas de vague inversée),
                    //on fait changer de direction tous les ennemis
                    if (XtremeEnemy.X == 0 || oddXtremeEnemy.X == windowWidth - oddXtremeEnemy.MaxLength - 1)
                    {

                        foreach (Enemy e in enemySwarm)
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
                        XtremeEnemy = GetEnemyExtremity(enemySwarm, "right", 2, 0);
                        oddXtremeEnemy = GetEnemyExtremity(enemySwarm, "left", 2, 1);
                    }
                    else
                    {
                        XtremeEnemy = GetEnemyExtremity(enemySwarm, "right", 1, 0);
                        oddXtremeEnemy = invisibleEnemy;
                    }

                    //condition pour empêcher que oddXtremeEnemy et XtremeEnemy soient le même
                    if (XtremeEnemy == oddXtremeEnemy)
                    {
                        oddXtremeEnemy = invisibleEnemy;
                    }

                    //on fait de même mais dans le cas où le groupe se déplace vers la droite
                    if (XtremeEnemy.X == windowWidth - XtremeEnemy.MaxLength - 1 || oddXtremeEnemy.X == 0)
                    {
                        foreach (Enemy e in enemySwarm)
                        {
                            e.IsChanging = true;
                        }
                    }
                } //fin de déplacement des ennemis

                //déplacement et vérification des ennemis
                //on considère la vague comme morte jusqu'à ce qu'on trouve un ennemi vivant dans enemySwarm
                waveAlive = false;
                //on considère que la vague n'est pas entrain d'apparaître tant qu'on ne trouve aucun ennemi qui se trouve au dessus de la fenêtre de la console
                spawning = false;
                foreach (Enemy e in enemies)
                {
                    //condition pour gérer la vitesse des ennemis
                    if (ticks % e.Speed == 0)
                    {
                        //on déplace les ennemis
                        //s'il est spécial
                        if (e.IsSpecial)
                        {
                            e.SpecialMove();
                            //si l'ennemi spécial sort de la fenêtre, on le considère comme mort
                            if (e.X > windowWidth)
                            {
                                e.IsAlive = false;
                                e.Appearence.Clear();
                                e.Appearence.Add(" ");
                            }
                        }
                        else
                        {
                            e.Move();
                        }
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

                    //condition pour faire descendre l'essein s'il n'est pas encore complètement à l'écran
                    if (spawning)
                    {
                        e.IsChanging = true;
                    }
                }
                //fin déplacement ennemi

                //apparition d'un ennemi spécial
                if (ticks % 200 == 0 && !specialEnemy.IsAlive)
                {
                    specialEnemy.IsAlive = true;
                    specialEnemy.Appearence = new List<string> { "  ███  ", "███████" };
                    //on le fait apparaître juste à gauche de la fenêtre de console
                    specialEnemy.X = -specialEnemy.MaxLength;
                }

                //clignotement du joueur lorsqu'il est touché
                if(playerRespawnTimer < PLAYER_RESPAWN_TIME)
                {
                    player.Flicker(5);
                }
                else if (player.Appearence.Count <= 1)
                {
                    player.Flicker(1);
                }

                if (player.ShieldUp)
                {

                    if(shieldDelayTimer > shieldDelay/3)
                    {

                        player.Appearence = normalPlayer;
                        player.ShieldUp = false;
                    }
                    else
                    {
                        player.Appearence = playerWithShield;
                        player.AnimateLine(new List<string>{ "   ▀█▀   ", "  ▀▀█▀▀  ", " ▀▀▀▀▀▀▀ ", "▀▀▀▀▀▀▀▀▀",
                            "▀▀▀▀▀▀▀▀▀", "█▀▀▀▀▀▀▀█", "█▀▀▀ ▀▀▀█", "█▀▀   ▀▀█", "█▀     ▀█", "█       █",
                        "█▄     ▄█", "█▄▄   ▄▄█", "▄▄▄▄ ▄▄▄▄", "▄▄▄▄▄▄▄▄▄", " ▄▄▄▄▄▄▄ ", "  ▄▄█▄▄  ", "   ▄█▄   ", "    █    "}, 3, 0);
                    }

                }

                //mise à jour de la liste d'éléments
                elements.Clear();
                elements.Add(player);
                foreach (Element e in projectiles)
                {
                    elements.Add(e);
                }

                foreach (Element e in enemies)
                {
                    elements.Add(e);
                }

                //chargement des éléments dans le buffer
                foreach (Element e in elements)
                {
                    e.Load(buffer);
                }

                //le HUD (nombre de vies du joueur)
                for (int i = 0; i < livesIndicator.Length; i++)
                {
                    buffer[0][windowWidth - livesIndicator.Length + i - 1] = livesIndicator[i];
                }

                //écriture de l'entierté de la fenêtre

                //on remet le string de la fenêtre entière à vide
                fullWindow = "";

                //concatenation de chaque ligne de char de buffer dans fullWindow
                for (int i = 0; i < buffer.GetLength(0); i++)
                {
                    fullWindow += new string(buffer[i]);
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(fullWindow);


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
                
            } while (!(exitGame || hasLost));
            Console.Clear();
        }
    }
}
