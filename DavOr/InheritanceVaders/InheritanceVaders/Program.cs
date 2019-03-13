using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    public class Program
    {
        static void Main(string[] args)
        {

            const int WINDOW_WIDTH = 120;
            const int WINDOW_HEIGHT = 40;
            

            Console.CursorVisible = false;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            Game game = new Game();

            //création menu paramètre
            List<string> parametersOption = new List<string> { "Son", "Affichage", "Difficulté", "Quitter" };
            List<string> onOffOption = new List<string> { "Activé / Désactivé", "Activé / Désactivé" };
            Menu parameters = new Menu(">>", ConsoleColor.Yellow, ConsoleColor.DarkMagenta, parametersOption, onOffOption);

            
           

            //création du menu principal
            List<string> mainMenuOptions = new List<string> { "Jouer", "Paramètres", "Highscores", "Mode d'emploi", "A propos", "Exit" };
            Menu mainMenu = new Menu(">>", ConsoleColor.Cyan, ConsoleColor.Red, mainMenuOptions, onOffOption);

            do
            {
                Debug.WriteLine("Début de boucle");
                mainMenu.EnterMenu();

                //Switch case du menu principal
                switch (mainMenu.SelectedOption)
                {

                    case 0:
                        //player.controls.play();
                        game.Launch();
                        break;

                    case 1:
                        // efface l'affichage du menu principal
                        Console.Clear();

                        // entre dans les paramètres son, difficulté et affichage                        
                        do
                        {
                            parameters.OnOffOptions();                      
                            parameters.Parameters();
                            
                            
                            // désactive le son du jeu
                            if (parameters.SelectedOption == 0)
                            {
                                parameters.SoundOption();
                            }
                            // changement de l'affichage in-game
                            if (parameters.SelectedOption == 1)
                            {
                                parameters.GameDisplay();
                            }
                            if (parameters.SelectedOption == 2)
                            {
                                Debug.WriteLine("Changement de la difficulté");
                            }
                            // efface les options des paramètres                       
                            if (parameters.SelectedOption == 3)
                            {
                                Console.Clear();
                            }
                        } while (parameters.SelectedOption != 3);
                        break;

                    case 2:
                        Console.WriteLine("Bienvenu dans le highscore");
                        break;

                    case 5:
                        Environment.Exit(0);
                        break;

                }
                Debug.WriteLine("fin de boucle");
            } while (mainMenu.SelectedOption != 4);
            Debug.WriteLine("Sorti du switch");
            Console.ReadLine();

            

        }
    }
}
