/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 06.02.19
 * Description: Classe des projectiles du jeu
 */
using System.Collections.Generic;

namespace InheritanceVaders
{

    /// <summary>
    /// Classe des projectiles du jeu
    /// </summary>
    public class Bullet : Element
    {
        private bool _goingUp;
        private bool _isShort;

        /// <summary>
        /// Constructeur de Bullet
        /// </summary>
        /// <param name="appearence"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speed"></param>
        /// <param name="goingUp">indique dans quel sens va le projectile</param>
        /// <param name="isShort">indique si le projectile fait partie du shortshot</param>
        public Bullet(List<string> appearence, int x, int y, int speed, bool goingUp, bool isShort) : base (x, y, speed, appearence)
        {
            _appearence = appearence;
            _x = x;
            _y = y;
            _speed = speed;
            _goingUp = goingUp;
            _isShort = isShort;
        }

        /// <summary>
        /// get set de la direction du projectile
        /// </summary>
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

        /// <summary>
        /// get set de la portée du projectile
        /// </summary>
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

        /// <summary>
        /// Déplace le projectile en fonction de son sens
        /// </summary>
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

        /// <summary>
        /// Vérifie si le projectile est en contact avec un element
        /// </summary>
        /// <param name="element">element à vérifier</param>
        /// <returns>retourne true si le projectile est en contact avec element</returns>
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
