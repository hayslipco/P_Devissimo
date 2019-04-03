using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{


    public class Bullet : Element
    {
        private bool _goingUp;
        private bool _isShort;

        public Bullet(List<string> appearence, int x, int y, int speed, bool goingUp, bool isShort) : base (x, y, speed, appearence)
        {
            _appearence = appearence;
            _x = x;
            _y = y;
            _speed = speed;
            _goingUp = goingUp;
            _isShort = isShort;
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

        public bool IsShort
        {
            get
            {
                return _isShort;
            }

            set
            {
                _isShort = value;
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

        public bool CollidesWith(Element element)
        {
            if (_x >= element.X && _x <= element.X + element.MaxLength && _y == element.Y)
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
