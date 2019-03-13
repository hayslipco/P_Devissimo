using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    public class Menu
    {
        private string _title;
        private string _selector;
        private int _optionSelected;
        private int _leftPadding;
        private int _topPadding;
        private bool _stay;
        private bool _soundOn;
        private bool _gameDisplay;
        private ConsoleColor _cursorColor;
        private ConsoleColor _optionsColor;
        private List<string> _options = new List<string>();
        private List<string> _onOffOption = new List<string>(); //

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

        public Menu(string selector, ConsoleColor cursorColor, ConsoleColor optionsColor, List<string> options, List<string> onOffOption)
        {
            _selector = selector;
            _cursorColor = cursorColor;
            _options = options;
            _optionsColor = optionsColor;
            _onOffOption = onOffOption; //

            _topPadding = Console.WindowHeight / options.Count / 2;
            _leftPadding = Console.WindowWidth / 3;
            _stay = true;
            _soundOn = true;
            _gameDisplay = true;
            _optionSelected = 0;
        }

        private void DrawMenu()
        {          
            Console.ForegroundColor = _optionsColor;
            for (int i = 0; i < _options.Count; i++)
            {
                Console.SetCursorPosition(_leftPadding, menuDrawStart + (i * _topPadding));
                Console.WriteLine(_options[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Affiche dès le lancement du menu paramètres si les options sont activé ou désactivé
        /// </summary>
        public void OnOffOptions()
        {
            for (int i = 0; i < _onOffOption.Count; i++)
            {
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + (i * _topPadding));
                Console.WriteLine(_onOffOption[i]);
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
                        case ConsoleKey.RightArrow:
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
                        case ConsoleKey.LeftArrow:
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
            Console.SetCursorPosition(_leftPadding, 10);
            Console.WriteLine("Paramètres");
            EnterMenu();
            DrawCursor();              
        }

        /// <summary>
        /// Active ou désactive le son
        /// </summary>
        public void SoundOption()
        {
            if (_soundOn)
            {
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1);
                Console.WriteLine("------");
                Debug.WriteLine("Désactivation du son");
                //player.controls.stop();
                _soundOn = false;
            }
            else
            {
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart);
                Console.WriteLine("----------");
                Debug.WriteLine("Activation du son");
                //player.controls.start();
                _soundOn = true;
            }
        }

        
        /// <summary>
        /// Active ou désactive l'affichage in-game
        /// </summary>
        public void GameDisplay()
        {
            //game.Flickering();
            if (_gameDisplay)
            {   
                Debug.WriteLine("Désactivation de l'affichage");
                //game.Flickering();
                _gameDisplay = false;
            }
            else
            {  
                Debug.WriteLine("Activation de l'affichage");
                //game.Flickering();
                _gameDisplay = true;
            }
        }

        /// <summary>
        /// Vérifie quelles options sont activées ou désactivées pour permettre d'identifier l'état de l'option
        /// </summary>
        public void CheckOptionOn()
        {
            if (_soundOn)
            {
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1);
                Console.WriteLine("------");
            }
            else
            {
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1);
                Console.WriteLine("---------");
            }

            if (_gameDisplay)
            {
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1 + _topPadding);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1 + _topPadding);
                Console.WriteLine("------");
            }
            else
            {
                Console.SetCursorPosition(_leftPadding + 20, menuDrawStart + 1 + _topPadding);
                Console.Write("         ");
                Console.SetCursorPosition(_leftPadding + 29, menuDrawStart + 1 + _topPadding);
                Console.WriteLine("---------");
            }
        }

        /// <summary>
        /// Affiche le mode d'emploi du jeu
        /// </summary>
        public void ModeDemploi()
        {

        }

        /// <summary>
        /// Affiche des infos concernant le jeu
        /// </summary>
        public void Apropos()
        {

        }
        
    }
}
