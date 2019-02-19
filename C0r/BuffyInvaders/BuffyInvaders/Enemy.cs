using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuffyInvaders
{
    public class Enemy
    {
        private const int SPEED = 3;
        private const int FIRE_FREQ = 200;

        string _appearence;

        List<String> _deathAnimationStrings;

        public ConsoleColor _color;
        int _x;
        int _y;
        int _speed;
        int _hp;
        int _fireFrequency;
        int _score;
        int _deathAnimationInt;
        private bool _goingLeft = false;
        private bool _isChanging = false;
        private bool _isAlive = true;
        private bool _isSpecial;


        /// <summary>
        /// constructeur avec coordonnées
        /// </summary>
        public Enemy(string appearence, ConsoleColor color, int x, int y, int speed, bool special)
        {
            _appearence = appearence;
            _color = color;
            _x = x;
            _y = y;
            _speed = speed;
            _fireFrequency = FIRE_FREQ;
            _isSpecial = special;

            _hp = 1;
            _score = 1000;

            IsGoingLeft = _goingLeft;

            _deathAnimationInt = 0;
            _deathAnimationStrings = new List<string>()
            {
               "|/*\\|",
               "/>o<-\\",
               "-->o<--",
               "\\ - /",
               "_ * _",
               "  *  ",
               " "
            };
        }

        //get set pour savoir la direction des ennemis
        public bool IsGoingLeft
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
        //get set pour savoir si les ennemis sont en cours de transition
        public bool IsChanging
        {
            get
            {
                return _isChanging;
            }
            set
            {
                _isChanging = value;
            }
        }
        //get set pour savoir si un ennemi est mort
        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
            }
        }
        //get set de la position x
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
        //get set de la position y
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
        //get set de l'apparence de l'ennemi
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

        //get set de la vitesse de l'ennemi
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        //get set de la fréquence de tir
        public int FireFrequency
        {
            get
            {
                return _fireFrequency;
            }
            set
            {
                _fireFrequency = value;
            }
        }

        /// <summary>
        /// Méthode de déplacement des ennemis
        /// </summary>
        public void Move()
        {
            if (_goingLeft)
            {
                if (!_isChanging)
                {
                    _x--;
                }
                else
                {
                    _y++;
                    _goingLeft = false;
                    _isChanging = false;
                }
            }
            else
            {
                if (!_isChanging)
                {
                    _x++;
                }
                else
                {
                    _y++;
                    _goingLeft = true;
                    _isChanging = false;
                }
            }
        }

        public void SpecialMove()
        {
            if (_goingLeft)
            {
                _x--;
            }
            else
            {
                _x++;
            }
        }

        public void Load(char[][] buffer)
        {
            if (_x < buffer[0].Length && _y >= 0)
            for (int i = 0; i < _appearence.Length; i++)
            {
                    if (_x + i < buffer[0].Length && _x + i >= 0)
                    {
                        buffer[_y][_x + i] = _appearence[i];
                    }
            }
        }

        public void Shoot(List<Bullet> projectiles)
        {
            projectiles.Add(new Bullet("█", _x + _appearence.Length/2, _y, 1, false, false));
        }

        public void Die()
        {
            if(_deathAnimationInt < _deathAnimationStrings.Count)
            {
                _appearence = _deathAnimationStrings[_deathAnimationInt];
                _deathAnimationInt++;
            }
        }

    }
}
