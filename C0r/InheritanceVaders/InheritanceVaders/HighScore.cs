using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceVaders
{
    [Serializable()]
    public class HighScore : Common
    {
        private int _score;
        private string _name;
        private DateTime _date;

        public HighScore()
        {

        }

        public HighScore(int score, string name)
        {
            _score = score;
            _name = name;
            _date = DateTime.Now;
        }

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
