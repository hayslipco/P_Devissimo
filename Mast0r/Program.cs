/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 23.01.19
 * Description: Classe d'entrée du programme duquel va être lancé le menu puis le jeu
 */
using SpicyInvader;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InheritanceVaders
{
    /// <summary>
    /// Classe d'entrée du programme
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Sound.OpenSounds();

            int WINDOW_WIDTH = 140;
            int WINDOW_HEIGHT = 52;

            Console.CursorVisible = false;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            //création menu paramètre
            List<string> parametersOption = new List<string> { "Son", "Affichage", "Difficulté", "Quitter" };
            List<string> onOffOptionVisual = new List<string> { "Activé / Désactivé", "Activé / Désactivé", "Very Chill / Chill / Not Chill / Chill Norris" };

            Menu parameters = new Menu(">>", ConsoleColor.Yellow, ConsoleColor.DarkMagenta, parametersOption, onOffOptionVisual);

            //création du menu principal
            List<string> mainMenuOptions = new List<string> { "Jouer", "Paramètres", "Highscores", "Mode d'emploi", "A propos", "Exit" };
            Menu mainMenu = new Menu(">>", ConsoleColor.Cyan, ConsoleColor.Red, mainMenuOptions, onOffOptionVisual);

            Game game = new Game();

            Sound.MenuTheme(true);

            do
            {
                
                //menuTheme.Play();

                Console.Clear();
                parameters.DrawTitle();
                Debug.WriteLine("Début de boucle");
                mainMenu.EnterMenu();

                //Switch case du menu principal
                switch (mainMenu.SelectedOption)
                {

                    case 0:
                        //menuTheme.Stop();
                        game.Launch();
                        Sound.MenuTheme(true);
                        break;

                    case 1:
                        // efface l'affichage du menu principal
                        Console.Clear();
                        parameters.DrawTitle();
                        parameters.DrawParameter();
                        // entre dans les paramètres son, difficulté et affichage                        
                        do
                        {
                            // Affiche les paramètres
                            parameters.OnOffOptionsVisual();
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

                    case 3:
                        ConsoleKeyInfo pressedKey;
                        mainMenu.ModeDemploi();
                        do
                        {

                            pressedKey = Console.ReadKey(true);

                        } while (pressedKey.Key != ConsoleKey.Escape);
                        Console.Clear();

                        break;

                    case 4:
                        ConsoleKeyInfo pressed;
                        mainMenu.Apropos();

                        do
                        {

                            pressed = Console.ReadKey(true);

                        } while (pressed.Key != ConsoleKey.Escape);

                        Console.Clear();


                        break;

                    case 5:
                        Environment.Exit(0);
                        break;

                }
                Debug.WriteLine("fin de boucle");
            } while (mainMenu.SelectedOption != 5);
            Debug.WriteLine("Sorti du switch");
        }
    }
}
