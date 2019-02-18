using System;
using System.Collections.Generic;
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
            int WINDOW_HEIGHT = 40;

            //Mise en place de la musique
            WindowsMediaPlayer player = new WindowsMediaPlayer();

            player.URL = "GesaffelSemiEP.mp3";
            player.controls.stop();

            Console.CursorVisible = false;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            


            //création d'un game
            Game game = new Game();

            //création du menu principal
            List<string> mainMenuOptions = new List<string> { "Jouer", "Paramètres", "Highscores", "Mode d'emploi", "A propos", "Exit" };
            Menu mainMenu = new Menu(">>", ConsoleColor.Cyan, ConsoleColor.Red, mainMenuOptions);
            mainMenu.EnterMenu();
            //Switch case du menu principal
            switch (mainMenu.SelectedOption)
            {
                case 0:
                    //player.controls.play();
                    game.Launch();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;

            }


            Console.ReadLine();


        }
    }
}
