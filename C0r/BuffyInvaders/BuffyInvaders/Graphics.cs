using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuffyInvaders
{
    public class Graphics
    {
        private List<string> _lightning = new List<string>();
        private List<string> _TwoDLightning = new List<string>();
        private List<string> _TwoDLightning2 = new List<string>();
        List<List<string>> _TwoDLightnings = new List<List<string>>();

        private Random random = new Random();

        public Graphics()
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

        }

        public void SideLigthning(char[][] buffer)
        {

            for (int i = 0; i < Console.WindowHeight - 1; i++)
                for(int s = 0; s < 2; s++)
                if (random.Next(4) == 0)
                {
                    for (int l = 0; l < _lightning[0].Length; l++)
                    {
                        if (random.Next(4) == 0)
                        {
                            buffer[i][l + (s * (Console.WindowWidth - _lightning[0].Length - 1))] = _lightning[0][l];
                        }
                    }
                }
        }

        public void SideLightningSlide(char[][] buffer, int line)
        {
            int ranIndex;

            if (line < buffer.Length)
            {
                ranIndex = random.Next(_lightning.Count);

                for (int s = 0; s < 2; s++)
                {
                    if (random.Next(4) == 0)
                    {
                        for (int l = 0; l < _lightning[ranIndex].Length; l++)
                        {
                            buffer[line][l + (s * (Console.WindowWidth - _lightning[ranIndex].Length - 1))] = _lightning[ranIndex][l];
                        }
                    }
                }
            }
        }

        public void SLightningTwoD(char[][] buffer, int line)
        {
            int ranIndex;

            ranIndex = random.Next(_TwoDLightnings.Count);

            if (line < buffer.Length - _TwoDLightnings[ranIndex].Count)
            {
                for (int s = 0; s < 2; s++)
                {

                    //if (random.Next(4) == 0)
                    //{
                        for (int i = 0; i < _TwoDLightnings[ranIndex].Count; i++)
                        {
                            for (int l = 0; l < _TwoDLightnings[ranIndex][i].Length; l++)
                            {
                                buffer[line + i][l + (s * (Console.WindowWidth - _TwoDLightnings[ranIndex][i].Length - 1))] = _TwoDLightnings[ranIndex][i][l];
                            }

                        }
                    //}
                }
            }
        }

    } 
}
