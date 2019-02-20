using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace BuffyInvaders
{

    class Program
    {

        static void Main(string[] args)
        {
            int WINDOW_WIDTH = 120;
            int WINDOW_HEIGHT = 60;

            //Mise en place de la musique
            WindowsMediaPlayer player = new WindowsMediaPlayer();

            player.URL = "GesaffelSemiEP.mp3";
            player.controls.stop();

            Console.CursorVisible = false;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            


            //création d'un game
            Game game = new Game();

            //création menu paramètre
            List<string> parametersOption = new List<string> { "Son", "Affichage","Difficulté", "Quitter"};
            Menu parameters = new Menu(">>", ConsoleColor.Yellow, ConsoleColor.DarkMagenta, parametersOption );

            //création du menu principal
            List<string> mainMenuOptions = new List<string> { "Jouer", "Paramètres", "Highscores", "Mode d'emploi", "A propos", "Exit" };
            Menu mainMenu = new Menu(">>", ConsoleColor.Cyan, ConsoleColor.Red, mainMenuOptions);
            
            do
            {
                Debug.Write("Début de boucle");
                mainMenu.EnterMenu();
                
                //Switch case du menu principal
                switch (mainMenu.SelectedOption)
                {
           
                    case 0:
                        player.controls.play();
                        game.Launch();
                        break;

                    case 1:
                        // entre dans les paramètres son, difficulté et affichage                        
                        do
                        {
                            parameters.Parameters();
                            // désactive le son du jeu
                            if (parameters.SelectedOption == 0)
                            {
                                
                                Console.WriteLine("Désactivé");
                                Debug.Write("Désactivation du son");
                                player.controls.stop();
                            }
                            // changement de l'affichage in-game
                            if (parameters.SelectedOption == 1)
                            {
                                game.Flickering();
                                Debug.Write("Changement de l'affichage in-game");
                            }
                            if (parameters.SelectedOption == 2)
                            {
                                Debug.Write("Changement de la difficulté");
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
                Debug.Write("fin de boucle");
            } while (mainMenu.SelectedOption != 4);
            Debug.Write("Sorti du switch");
            Console.ReadLine();


        }
    }
}
