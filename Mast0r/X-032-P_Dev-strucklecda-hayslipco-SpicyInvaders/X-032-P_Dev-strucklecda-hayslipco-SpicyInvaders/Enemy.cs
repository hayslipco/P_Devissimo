﻿/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 30.01.19
 * Description: Classe des ennemis du jeu
 */
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    /// <summary>
    /// Classe des ennemis
    /// </summary>
    public class Enemy : Element
    {
        int _fireFrequency;
        int _score;
        private bool _goingLeft = false;
        private bool _isChanging = false;
        private bool _isAlive = true;
        private bool _isSpecial;

        /// <summary>
        /// Constructeur d'un ennemi
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speed"></param>
        /// <param name="appearence"></param>
        /// <param name="special">indique si l'ennemi est un ennemi special</param>
        public Enemy(int x, int y, int speed, List<string> appearence, bool special) : base (x, y, speed, appearence)
        {
            _isSpecial = special;
            _score = enemyPoints;
            _fireFrequency = enemyFireFreq;
            IsGoingLeft = _goingLeft;

            if (_isSpecial)
            {
                _score = specialEnemyPoints;

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

        /// <summary>
        /// Méthode faisant bouger les ennemis
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
                    _y ++;
                    _goingLeft = true;
                    _isChanging = false;
                }
            }
        }

        /// <summary>
        /// Méthode faisant bouger l'ennemi spécial
        /// </summary>
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

        /// <summary>
        /// Tir des ennemis
        /// </summary>
        /// <param name="projectiles"></param>
        public void Shoot(List<Bullet> projectiles)
        {
            projectiles.Add(new Bullet(new List<string> { "█" }, _x + _maxLength / 2, _y, enemyBulletSpeed, false, false));
        }
    }
}
