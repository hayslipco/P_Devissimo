using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    public class Enemy : Element
    {

        

        int _hp;
        int _fireFrequency;
        int _score;
        private bool _goingLeft = false;
        private bool _isChanging = false;
        private bool _isAlive = true;
        private bool _isSpecial;

        public Enemy(int x, int y, int speed, List<string> appearence, bool special) : base (x, y, speed, appearence)
        {
            _isSpecial = special;

            _hp = 1;
            _score = 1000;
            _fireFrequency = ENEMY_FIRE_FREQ;


            IsGoingLeft = _goingLeft;

            if (_isSpecial)
            {
                _score = 10000;

                _deathAnimationStrings = new List<List<string>>()
                {
                   new List<string>{ "  ███  ", "███ ███" },
                   new List<string>{ "  ███  ", "██ █ ██" },
                   new List<string>{ "  █ █  ", "██   ██" },
                   new List<string>{ " █   █ ", "█     █" },
                   new List<string>{ "█     █", "█     █" },
                   new List<string>{"  *  " },
                   new List<string>{ " " }
                };
            }
            else
            {
                _deathAnimationStrings = new List<List<string>>()
                {

                   new List<string>{ "|/*\\|" },
                   new List<string>{ "/>o<-\\" },
                   new List<string>{"-->o<--" },
                   new List<string>{"\\ - /" },
                   new List<string>{"_ * _" },
                   new List<string>{"  *  " },
                   new List<string>{ " " }
                };
            }
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
        //get set pour savoir si un ennemi est special
        public bool IsSpecial
        {
            get
            {
                return _isSpecial;
            }
            set
            {
                _isSpecial = value;
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

        //get set du score de l'ennemi
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

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

            if(_x < 0)
            {
                _deathAnimationInt = 0;
            }

        }

        public void Shoot(List<Bullet> projectiles)
        {
            projectiles.Add(new Bullet(new List<string> { "█" }, _x + _maxLength / 2, _y, ENEMY_BULLET_SPEED, false, false));
        }
    }
}
