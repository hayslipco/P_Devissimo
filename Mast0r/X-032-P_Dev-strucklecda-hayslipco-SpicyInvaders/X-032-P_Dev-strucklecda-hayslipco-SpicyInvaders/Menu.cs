using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    public class Menu : Common
    {
        private string _selector;
        private int _optionSelected;
        private int _leftPadding;
        private int _topPadding;
        private bool _stay;              
        private ConsoleColor _cursorColor;
        private ConsoleColor _optionsColor;
        private List<string> _options = new List<string>();
        private List<string> _onOffOptionVisual = new List<string>();
        private Graphics grapOnOff = new Graphics(visualDisplay);
        private int _difficulty = 1;

        private int menuDrawStart = Console.WindowHeight / 3;

        public int SelectedOption
        {
            get
            {
                return _optionSelected;
            }
            set
            {
                _optionSelected = value;
            }
        }

        public Menu(string selector, ConsoleColor cursorColor, ConsoleColor optionsColor, List<string> options, List<string> onOffOptionVisual)
        {
            _selector = selector;
            _cursorColor = cursorColor;
            _options = options;
            _optionsColor = optionsColor;
            _onOffOptionVisual = onOffOptionVisual;

            _topPadding = Console.WindowHeight / options.Count / 2;
            _leftPadding = Console.WindowWidth / 3;
            _stay = true;
            
            _optionSelected = 0;
        }

        /// <summary>
        /// Dessine le titre à partir de la liste
        /// </summary>
        public void DrawTitle()
        {
            List<string> title = new List<string>
            {
                "   _____       _            ____                     __         ",
                "  / ___/____  (_)______  __/  _/___ _   ______ _____/ /__  _____",
                @"  \__ \/ __ \/ / ___/ / / // // __ \ | / / __ `/ __  / _ \/ ___/",
                @" ___/ / /_/ / / /__/ /_/ // // / / / |/ / /_/ / /_/ /  __/ /    ",
                @"/____/ .___/_/\___/\__, /___/_/ /_/|___/\__,_/\\__,_/\___/_/     ",
                "    /_/           /____/                                        ",
            };

            // Affiche le titre
            WriteTitle(title);
        }

        public void DrawParameter()
        {
            List<string> param = new List<string>
            {
                "   ___                         __            ",
               @"  / _ \___ ________ ___ _ ___ / /________ ___",
                " / ___/ _ `/ __/ _ `/  ' / -_/ __/ __/ -_(_-'",
               @"/_/   \_,_/_/  \_,_/_/_/_\__/\__/_/ \__/___/",
            };

            WriteParam(param);
        }

        public void DrawApropos()
        {
            List<string> aPropos = new List<string>
            {
                "    ___                                     ",
                "   /   |      ____  _________  ____  ____  _____",
               @"  / /| |     / __ \/ ___/ __ \/ __ \/ __ \/ ___/",
                " / ___ |    / /_/ / /  / /_/ / /_/ / /_/ (__  ) ",
               @"/_/  |_|   / .___/_/   \____/ .___/\____/____/  ",
                "          /_/              /_/                  "
            };

            WriteParam(aPropos);
        }

        public void DrawModeDEmploi()
        {
            List<string> modeDemploi = new List<string>
            {
                "    __  ___          __            __   __                        __      _ ",
                "   /  |/  /___  ____/ /__     ____/ /  /_/   __  ____ ___   ___  / /___  (_)",
               @"  / /|_/ / __ \/ __  / _ \   / __  /       / _ \/ __ `__ \/ __ \/ / __ \/ / ",
                " / /  / / /_/ / /_/ /  __/  / /_/ /       /  __/ / / / / / /_/ / / /_/ / /  ",
               @"/_/  /_/\____/\__,_/\___/   \__,_/        \___/_/ /_/ /_/ .___/_/\____/_/   ",
                "                                                       /_/                  "
            };

            WriteParam(modeDemploi);
        }



        private void DrawMenu()
        {
            Console.ForegroundColor = _optionsColor;
            for (int i = 0; i < _options.Count; i++)
            {
                if (i > 4)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.SetCursorPosition(_leftPadding, menuDrawStart + (i * _topPadding));
                Console.WriteLine(_options[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Affiche dès le lancement du menu paramètres si les options sont activé ou désactivé
        /// </summary>
        public void OnOffOptionsVisual()
        {
            for (int i = 0; i < _onOffOptionVisual.Count; i++)
            {
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + (i * _topPadding));
                Console.WriteLine(_onOffOptionVisual[i]);
            }

            CheckOptionOn();
        }



        private void DrawCursor()
        {
            Console.CursorLeft = _leftPadding - _selector.Length - 1;
            Console.Write("   ");

            Console.SetCursorPosition(_leftPadding - _selector.Length - 1, menuDrawStart + (_optionSelected * _topPadding));
            Console.ForegroundColor = _cursorColor;
            Console.Write(_selector);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void EnterMenu()
        {
            DrawMenu();
            DrawCursor();
            _stay = true;
            while (_stay)
            {
                if (Console.KeyAvailable)
                {

                    ConsoleKey keyPressed = Console.ReadKey().Key;

                    switch (keyPressed)
                    {

                        case ConsoleKey.DownArrow:
                            Sound.OofSound();
                            if (_optionSelected < _options.Count - 1)
                            {
                                _optionSelected++;
                            }
                            else
                            {
                                _optionSelected = 0;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            Sound.OofSound();
                            if (_optionSelected < _options.Count - 1)
                            {
                                _optionSelected++;
                            }
                            else
                            {
                                _optionSelected = 0;
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            Sound.OofSound();
                            if (_optionSelected > 0)
                            {
                                _optionSelected--;
                            }
                            else
                            {
                                _optionSelected = _options.Count - 1;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            Sound.OofSound();
                            if (_optionSelected > 0)
                            {
                                _optionSelected--;
                            }
                            else
                            {
                                _optionSelected = _options.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            Sound.ClickSound();
                            _stay = false;
                            break;

                    } //fin switch
                    if (_stay)
                    {
                        DrawCursor();
                    }

                }//fin if KeyAvailable

            } //fin select Mode
        }

        /// <summary>
        /// Affichage du menu paramètre
        /// </summary>
        public void Parameters()
        {
            EnterMenu();
            DrawCursor();
        }

        /// <summary>
        /// Active ou désactive le son
        /// </summary>
        public void SoundOption()
        {
            if (Sound.onOffSound)
            {               
                Sound.onOffSound = false;
                Sound.MenuTheme(false);                
            }
            else
            {                
                Sound.onOffSound = true;
                Sound.MenuTheme(true);                               
            }
        }


        /// <summary>
        /// Active ou désactive l'affichage in-game
        /// </summary>
        public void GameDisplay()
        {
            if (visualDisplay)
            {
                grapOnOff.OnOffVisualDisplay();
            }
            else
            {
                grapOnOff.OnOffVisualDisplay();
            }
        }
                          
        /// <summary>
        /// Vérifie quelles options sont activées ou désactivées pour permettre d'identifier l'état de l'option
        /// </summary>
        public void CheckOptionOn()
        {
            // Affichage de quelle option de son est choisie
            if (Sound.onOffSound)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1);
                Console.WriteLine("------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1);
                Console.WriteLine("---------");
            }

            // Affichage de quelle option d'afficahge est choisie
            if (visualDisplay)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1 + _topPadding);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1 + _topPadding);
                Console.WriteLine("------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1 + _topPadding);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1 + _topPadding);
                Console.WriteLine("---------");
            }

            // Affichage de quelle difficulté est choisie
            if (difficulty)
            {              
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(_leftPadding + 28, menuDrawStart + 1 + _topPadding * 2);
                Console.Write("            ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1 + _topPadding * 2);
                Console.WriteLine("-----");
            }
            else
            {       
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1 + _topPadding * 2);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 28, menuDrawStart + 1 + _topPadding * 2);
                Console.WriteLine("------------");
            }                      
        }

        /// <summary>
        /// Affiche le mode d'emploi du jeu
        /// </summary>
        public void ModeDemploi()
        {
            Console.Clear();
            DrawTitle();
            DrawModeDEmploi();

            WriteHowToPlay("Spacebar", "Tir");
            WriteHowToPlay("1", "Mode sniper");
            WriteHowToPlay("2", "Mode tir normal (par défaut)");
            WriteHowToPlay("3", "Mode fusil à pompe");
            WriteHowToPlay("Escape", "Pause");
            WriteHowToPlay("←", "Bouger à gauche");
            WriteHowToPlay("→", "Bouger à droite");
            WriteHowToPlay("↓", "Stopper le vaisseau (mode déplacement lisse)");
            WriteHowToPlay("Q", "Activer/désactiver le déplacement lisse");
            WriteHowToPlay("E", "Bouclier");
        }

        /// <summary>
        /// Affiche des infos concernant le jeu
        /// </summary>
        public void Apropos()
        {
            Console.Clear();
            DrawTitle();
            DrawApropos();
            Console.CursorTop = 17;
            CenteredWriteLine("SpicyInvader est un jeu de tir, codé en c#, crée par Hayslip Corwin et Struklec Davor Le but du jeu est de détruire les vaisseaux" +
                " ennemis avant qu'ils n'envahissent la terre.", 10);

        }
    }
}
