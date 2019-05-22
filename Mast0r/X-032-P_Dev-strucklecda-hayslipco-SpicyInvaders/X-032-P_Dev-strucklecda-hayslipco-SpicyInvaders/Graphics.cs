/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 06.03.19
 * Description: Classe utilisée pour générer de la décoration du monde sur les côtés
 */
using System;
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    /// <summary>
    /// Classe utilisée pour générer de la décoration du monde sur les côtés de la fenêtre
    /// </summary>
    public class Graphics : Common
    {       
        private List<string> _lightning = new List<string>();
        private List<string> _TwoDLightning = new List<string>();
        private List<string> _TwoDLightning2 = new List<string>();
        List<List<string>> _TwoDLightnings = new List<List<string>>();

        private Random random = new Random();

        public Graphics(bool visualDisplay)
        {
            _lightning.Add("╔╩╦╩╩╩═╬¤");
            _lightning.Add("╚╦╩╦╦╦═╬");

            _TwoDLightning.Add("═╗ ╔══╦╗  ╔═");
            _TwoDLightning.Add(" ╚═╩══╬╩══╝");
            _TwoDLightning.Add("      ╚═══╦═╝");
            _TwoDLightning.Add("          ╚════");

            _TwoDLightning2.Add("╔═╦══╗");
            _TwoDLightning2.Add("╠╦╝  ╚══");
            _TwoDLightning2.Add("╚╬═══╦╗");
            _TwoDLightning2.Add("═╩══╦╩╝ ╔═══");
            _TwoDLightning2.Add("    ╚═══╝");

            _TwoDLightnings.Add(_TwoDLightning);
            _TwoDLightnings.Add(_TwoDLightning2);

            windowHeight = Console.WindowHeight;
            windowWidth = Console.WindowWidth;
        }

        /// <summary>
        /// Fait apparaître les strings de _lightning dans les bords de la console
        /// </summary>
        /// <param name="buffer">Buffer dans lequel charger ces strings</param>
        public void SideLigthning(char[][] buffer)
        {
            if (visualDisplay)
            {
                for (int i = 0; i < windowHeight - 1; i++)
                    for (int s = 0; s < 2; s++)
                        if (random.Next(4) == 0)
                        {
                            for (int l = 0; l < _lightning[0].Length; l++)
                            {
                                if (random.Next(4) == 0)
                                {
                                    buffer[i][l + (s * (windowWidth - _lightning[0].Length - 1))] = _lightning[0][l];
                                }
                            }
                        }
            }
                             
        }

        /// <summary>
        /// Fait apparaître les éclairs dans _TwoDLightnings dans les bords de la console
        /// </summary>
        /// <param name="buffer">Buffer dans lequel charger ces strings</param>
        /// <param name="line">Indique à quelle ligne il faut afficher les éclairs</param>
        public void SLightningTwoD(char[][] buffer, int line)
        {
            int ranIndex;

            if (visualDisplay)
            {
                ranIndex = random.Next(_TwoDLightnings.Count);

                if (line < buffer.Length - _TwoDLightnings[ranIndex].Count)
                {
                    for (int s = 0; s < 2; s++)
                    {
                        for (int i = 0; i < _TwoDLightnings[ranIndex].Count; i++)
                        {
                            for (int l = 0; l < _TwoDLightnings[ranIndex][i].Length; l++)
                            {
                                buffer[line + i][l + (s * (windowWidth - _TwoDLightnings[ranIndex][i].Length - 1))] = _TwoDLightnings[ranIndex][i][l];
                            }

                        }
                    }
                }
            }
                          
        }

        
    }
}
