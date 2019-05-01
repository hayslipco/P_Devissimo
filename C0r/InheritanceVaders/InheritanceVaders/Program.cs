/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 23.01.19
 * Description: Classe d'entrée du programme duquel va être lancé le menu puis le jeu
 */
using System;

namespace InheritanceVaders
{
    /// <summary>
    /// Classe d'entrée du programme
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            int WINDOW_WIDTH = 120;
            int WINDOW_HEIGHT = 70;

            Console.CursorVisible = false;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            Game game = new Game();

            game.Launch();
        }
    }
}
