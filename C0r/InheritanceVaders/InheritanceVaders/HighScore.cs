/*
 * ETML
 * Auteurs: Davor S. et Corwin H.
 * Date de création: 13.03.19
 * Description: Classe utilisée pour gérer et stoquer les highscores
 */
using System;

namespace InheritanceVaders
{
    /// <summary>
    /// Classe utilisée pour gérer et stoquer les highscores
    /// </summary>
    [Serializable()]
    public class HighScore : Common
    {
        private int _score;
        private string _name;
        private DateTime _date;

        /// <summary>
        /// Constructeur sans paramètres pour permettre la serialization
        /// </summary>
        public HighScore()
        {

        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="score">score du joueur</param>
        /// <param name="name">nom du joueur</param>
        public HighScore(int score, string name)
        {
            _score = score;
            _name = name;
            _date = DateTime.Now;
        }

        /// <summary>
        /// get set de la date à laquelle le record est établi
        /// </summary>
        public DateTime date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        /// <summary>
        /// get set du score
        /// </summary>
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
        /// get set du nom
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

    }
}
