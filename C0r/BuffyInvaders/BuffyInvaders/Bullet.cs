using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuffyInvaders
{
    public class Bullet
    {
        private string _appearence;
        private int _x;
        private int _y;
        private int _length;
        private bool _goingUp;

        public Bullet(string appearence, int x, int y, int length, bool goingUp)
        {
            _appearence = appearence;
            _x = x;
            _y = y;
            _length = length;
            _goingUp = goingUp;
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

        public int Length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
            }
        }

        public bool GoingUp
        {
            get
            {
                return _goingUp;
            }

            set
            {
                _goingUp = value;
            }
        }

        public void Load(char[][] buffer)
        {
            if(_y < buffer.GetLength(0) && _y >= 0 && _x >= 0 && _x < buffer[0].Length)
            for(int i = 0; i < _appearence.Length; i++)
            {
                buffer[_y][_x + i] = _appearence[i];
            }
        
        }

        public void Move()
        {
            if (_goingUp)
            {
                _y--;
            }
            else
            {
                _y++;
            }
        }

        public bool CollidesWith(Enemy enemy)
        {
            if(_x > enemy.X && _x < enemy.X + enemy.Appearence.Length && _y == enemy.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
