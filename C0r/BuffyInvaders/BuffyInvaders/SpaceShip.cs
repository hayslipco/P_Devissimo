using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuffyInvaders
{
    public class SpaceShip
    {
        private const int LIVES = 10;

        private string _appearence;
        private int _x;
        private int _y;
        private int _lives;
        private bool _spaceFlight;
        private bool _goingLeft;
        private bool _stopped;

        public SpaceShip(string appearence, int x, int y)
        {
            _appearence = appearence;
            _x = x;
            _y = y;
            _lives = LIVES;
            _spaceFlight = false;
            _goingLeft = true;
            _stopped = true;
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
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

        public string Appearence
        {
            get
            {
                return _appearence;
            }
            set
            {
                _appearence = value;
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

        public void MoveShip(int pixels)
        {
            if (_x + pixels >= 0 && _x + pixels + _appearence.Length < Console.WindowWidth)
            {
                _x += pixels;
            }
        }

        public void LoadShip(ref char[][] buffer)
        {
            for(int i = 0; i < _appearence.Length; i++)
            {
                buffer[_y][_x + i] = _appearence[i];
            }
        }

    }
}
