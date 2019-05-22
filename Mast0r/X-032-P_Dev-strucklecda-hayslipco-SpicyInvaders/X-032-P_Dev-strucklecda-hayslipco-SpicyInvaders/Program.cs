/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 16.01.19
 * Description: Classe d'entrée du programme duquel va être lancé le menu puis le jeu
 */
using System;
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    /// <summary>
    /// Classe d'entrée du programme
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Ouverture des fichiers sons
            Sound.OpenSounds();

            int WINDOW_WIDTH = 140;
            int WINDOW_HEIGHT = 52;
            Console.BufferHeight = 52;
            
            // Cache le curseur par défaut
            Console.CursorVisible = false;

            // Taille de la fenêtre
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            // Instanciation de common et game
            Common common = new Common();
            Game game = new Game();

            //création menu paramètre
            List<string> parametersOption = new List<string> { "Son", "Affichage", "Difficulté", "Revenir" };
            List<string> onOffOptionVisual = new List<string> { "Activé / Désactivé", "Activé / Désactivé", "Chill / Chill Norris" };

            Menu parameters = new Menu(">>", ConsoleColor.Yellow, ConsoleColor.DarkMagenta, parametersOption, onOffOptionVisual);

            //création du menu principal
            List<string> mainMenuOptions = new List<string> { "Jouer", "Paramètres", "Highscores", "Mode d'emploi", "À propos", "Quitter" };
            Menu mainMenu = new Menu(">>", ConsoleColor.Cyan, ConsoleColor.Red, mainMenuOptions, onOffOptionVisual);
          
            // Joue le theme du menu
            Sound.MenuTheme(true);

            // Début de la boucle do... while
            do
            {
                Console.Clear();
                parameters.DrawTitle();
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
                            // changement de la difficulté
                            if (parameters.SelectedOption == 2)
                            {
                                parameters.DifficultySetting();
                            }
                            // efface les options des paramètres                       
                            if (parameters.SelectedOption == 3)
                            {
                                Console.Clear();
                            }
                        } while (parameters.SelectedOption != 3);

                        break;
                        
                    case 2:
                        // Affiche les highscores
                        Console.ForegroundColor = ConsoleColor.Red;
                        common.ShowTopScores();
                        common.StayInMenu();
                        Console.ForegroundColor = ConsoleColor.White;
                        break;

                    case 3:
                        // Affiche le mode d'emploi
                        mainMenu.ModeDemploi();
                        common.StayInMenu();
                        break;

                    case 4:
                        // Affiche l'à propos du jeu
                        mainMenu.Apropos();
                        common.StayInMenu();
                        break;

                    case 5:
                        // Quitte l'application
                        Environment.Exit(0);
                        break;

                }                
            } while (mainMenu.SelectedOption != 5);
            // Fin de la boucle do... while
        }
    }
}
