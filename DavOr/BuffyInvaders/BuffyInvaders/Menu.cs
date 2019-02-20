using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuffyInvaders
{
    public class Menu
    {
        private string _title;
        private string _selector;
        private int _optionSelected;
        private int _leftPadding;
        private int _topPadding;
        private bool _stay;
        private ConsoleColor _cursorColor;
        private ConsoleColor _optionsColor;
        private List<string> _options = new List<string>();

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

        public Menu(string selector, ConsoleColor cursorColor, ConsoleColor optionsColor, List<string> options)
        {
            _selector = selector;
            _cursorColor = cursorColor;
            _options = options;
            _optionsColor = optionsColor;

            _topPadding = Console.WindowHeight / options.Count / 2;
            _leftPadding = Console.WindowWidth / 3;
            _stay = true;
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
            Console.Clear();
            Console.SetCursorPosition(_leftPadding, 10);
            Console.WriteLine("Paramètres");
            EnterMenu();
            DrawCursor();      
            
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
