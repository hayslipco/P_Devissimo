using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    public class SpaceShip : Element
    {

        private int _lives;
        private bool _spaceFlight;
        private bool _goingLeft;
        private bool _stopped;
        private bool _shieldUp;
        
        public SpaceShip(int x, int y, int speed, List<string> appearence) : base(x, y, speed, appearence)
        {
            _lives = PLAYER_INIT_LIVES;
            _spaceFlight = false;
            _goingLeft = true;
            _stopped = true;
            _shieldUp = false;
        }

        public int Lives
        {
            get
            {
                return _lives;
            }
            set
            {
                _lives = value;
            }
        }

        public bool SpaceFlight
        {
            get
            {
                return _spaceFlight;
            }
            set
            {
                _spaceFlight = value;
            }
        }

        public bool GoingLeft
        {
            get
            {
                return _goingLeft;
            }
            set
            {
                _goingLeft = value;
            }
        }

        public bool Stopped
        {
            get
            {
                return _stopped;
            }
            set
            {
                _stopped = value;
            }
        }

        public bool ShieldUp
        {
            get
            {
                return _shieldUp;
            }
            set
            {
                _shieldUp = value;
            }
        }

        public void MoveShip(int pixels)
        {
            if (_x + pixels >= 0 && _x + pixels + _maxLength < Console.WindowWidth)
            {
                _x += pixels;
            }
        }

        //public void flicker(int flickerRate)
        //{
        //    if (flickerRate % 12 == 0)
        //    {
        //        _appearence = _blinkAppearence;
        //    }
        //    else if (flickerRate % 6 == 0)
        //    {
        //        int usualMaxLength = _maxLength;

        //        //on simule la longueur maximum du vaisseau pour empêcher qu'il ne se coince dans les bords de la console lors du clignotement
        //        _appearence = new List<string>();
        //        _appearence.Add("");
        //        for (int i = 0; i < usualMaxLength; i++)
        //        {
        //            _appearence[0] += " ";
        //        }

        //    }
        //}
        
    }
}
