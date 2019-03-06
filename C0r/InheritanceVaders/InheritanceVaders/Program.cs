using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    public class Program
    {
        static void Main(string[] args)
        {

            int WINDOW_WIDTH = 140;
            int WINDOW_HEIGHT = 60;

            Console.CursorVisible = false;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            Game game = new Game();

            game.Launch();

        }
    }
}
