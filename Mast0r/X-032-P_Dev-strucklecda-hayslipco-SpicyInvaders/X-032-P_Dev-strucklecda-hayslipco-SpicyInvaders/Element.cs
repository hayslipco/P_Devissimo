/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 06.03.19
 * Description: Classe mère de chaque acteur du jeu
 */
using System.Collections.Generic;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    /// <summary>
    /// Classe mère de chaque acteur du jeu
    /// </summary>
    public class Element : Common
    {
        protected int _x;
        protected int _y;
        protected int _speed;
        protected int _maxLength;
        protected int _flickeringInt;
        protected int _animationInt;
        protected int _deathAnimationInt;
        protected List<List<string>> _deathAnimationStrings;
        protected List<string> _initialAppearence;
        protected List<string> _appearence;

        /// <summary>
        /// Constructeur de base
        /// </summary>
        /// <param name="x">position sur l'axe x</param>
        /// <param name="y">position sur l'axe y</param>
        /// <param name="speed">vitesse de déplacement</param>
        /// <param name="appearence">apparence</param>
        public Element(int x, int y, int speed, List<string> appearence)
        {
            _x = x;
            _y = y;
            _speed = speed;
            _appearence = appearence;
            _initialAppearence = _appearence;
            _maxLength = GetMaxLength(appearence);
            _flickeringInt = 0;
            _animationInt = 0;
            _deathAnimationInt = 0;
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

        //get set de l'apparence de l'element
        public List<string> Appearence
        {
            get
            {
                return _appearence;
            }
            set
            {
                _appearence = value;
                _maxLength = GetMaxLength(_appearence);
            }
        }

        /// <summary>
        /// get set de l'apparence de base de l'element
        /// </summary>
        public List<string> InitialAppearence
        {
            get
            {
                return _initialAppearence;
            }
            set
            {
                _initialAppearence = value;
            }
        }

        //get set de la vitesse de l'element
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

        /// <summary>
        /// get set de la longueur max de l'apparence de l'element
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }

        /// <summary>
        /// Inscrit les éléments dans un buffer pour qu'ils soient réécrits par la suite
        /// </summary>
        /// <param name="buffer">tableau de char qui accueillera les éléments chargés
        /// </param>
        public void Load(char[][] buffer)
        {
            //parcours des lignes du 'sprite'
            for(int l = 0; l < _appearence.Count; l++)
            {
                //parcours des caractères de chaque ligne
                for(int c = 0; c < _appearence[l].Length; c++)
                {
                    if(_y + l >= 0 && _y + l < buffer.Length && _x + c >= 0 && _x + c < buffer[0].Length )
                    buffer[_y + l][_x + c] = _appearence[l][c];
                }
            }
        }

        /// <summary>
        /// Fait clignoter l'élément
        /// </summary>
        /// <param name="flickerRate">indique la vitesse à laquelle l'élément clignote</param>
        public void Flicker(int flickerRate)
        {
            //int qui va gérer les intervalles de clignotement
            _flickeringInt++;

            if (_flickeringInt % (2*flickerRate) == 0)
            {
                int usualMaxLength = _maxLength;

                _appearence = new List<string> { "" };
                for (int i = 0; i < GetMaxLength(_initialAppearence); i++)
                {
                    _appearence[0] += " ";
                }
            }
            else if (_flickeringInt % flickerRate == 0)
            {
                _appearence = _initialAppearence;
            }

        }

        /// <summary>
        /// Fait varier l'apparence d'une ligne d'un sprite
        /// </summary>
        /// <param name="frames">Différentes apparences de la ligne</param>
        /// <param name="frameRate">Spécifie la vitesse à laquelle change le sprite</param>
        /// <param name="line">Indique à quelle ligne du sprite prend place le changement</param>
        public void AnimateLine(List<string> frames, int frameRate, int line)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                if (_animationInt < i * frameRate)
                {
                    _appearence[line] = frames[i];
                    break;
                }
            }

            if (_animationInt >= frameRate * frames.Count)
            {
                _animationInt = 0;
            }
            else
            {
                _animationInt++;
            }

        }

        /// <summary>
        /// Méthode pour animer la mort d'un élément
        /// </summary>
        public void Die()
        {
            if (_deathAnimationInt < _deathAnimationStrings.Count)
            {
                _appearence = _deathAnimationStrings[_deathAnimationInt];
                _deathAnimationInt++;
            }
        }


        /// <summary>
        /// Méthode pour animer la mort d'un élément
        /// </summary>
        public void Die(int tick, int modulo)
        {
            if (tick % modulo == 0)
            {

                if (_deathAnimationInt < _deathAnimationStrings.Count)
                {
                    _appearence = _deathAnimationStrings[_deathAnimationInt];
                    _deathAnimationInt++;
                }
            }
        }

    }

}
