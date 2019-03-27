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
        private bool _dead;
        private List<string> _shieldAnimation;

        public SpaceShip(int x, int y, int speed, List<string> appearence) : base(x, y, speed, appearence)
        {
            _lives = PLAYER_INIT_LIVES;
            _spaceFlight = false;
            _goingLeft = true;
            _stopped = true;
            _shieldUp = false;
            _dead = false;

            _shieldAnimation = new List<string>{ "   ▀█▀   ", "  ▀▀█▀▀  ", " ▀▀▀▀▀▀▀ ", "▀▀▀▀▀▀▀▀▀",
                            "▀▀▀▀▀▀▀▀▀", "█▀▀▀▀▀▀▀█", "█▀▀▀ ▀▀▀█", "█▀▀   ▀▀█", "█▀     ▀█", "█       █",
                        "█▄     ▄█", "█▄▄   ▄▄█", "▄▄▄▄ ▄▄▄▄", "▄▄▄▄▄▄▄▄▄", " ▄▄▄▄▄▄▄ ", "  ▄▄█▄▄  ", "   ▄█▄   ", "    █    "};

            _deathAnimationStrings = new List<List<string>>
            {
                new List<string>{"*\\     /*", "<I>-T-<I>" },
                new List<string>{"**|   |***", "<T>-T-<T>"},
                new List<string>{"*        *", "<x>-|-<x>"},
                new List<string>{"*            *", "<x> - | - <x>"},
                new List<string>{"*               *", "<x >  -  |  -  <x >"},
                new List<string>{"*                  *", "<  >  -   |   -    <x"},
                new List<string>{"*                     *", "<     >                 x"},
                new List<string>{"*", "<                                  x"}
            };
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
                if(_lives < 0)
                {
                    _dead = true;
                }
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

        public bool Dead
        {
            get
            {
                return _dead;
            }
            set
            {
                _dead = value;
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
            if (_x + pixels >= 0 + graphicsMargin && _x + pixels + _maxLength + graphicsMargin < Console.WindowWidth)
            {
                _x += pixels;
            }
        }

        public void AnimateShield()
        {
            AnimateLine(_shieldAnimation, 3, 0);
        }

        public void RespawnBlink(int playerRespawnTimer)
        {
            if (playerRespawnTimer < PLAYER_RESPAWN_TIME)
            {
                Flicker(5);
            }
            //si le vaisseau ne fait qu'une ligne, c'est qu'il a terminé son clignotement en étant invisible, cette condition remet player.appearence au bon 'sprite'
            else if (Appearence.Count <= 1)
            {
                Flicker(1);
            }
        }
        
    }
}
