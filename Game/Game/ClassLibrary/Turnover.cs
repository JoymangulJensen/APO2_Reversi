using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    class Turnover
    {
        /// <summary>
        /// The number od value in each direction
        /// Key : direction [1-8]
        /// Value : the number of value
        /// </summary>
        private Dictionary<int, int> value = new Dictionary<int, int>();
        public Dictionary<int, int> Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public Turnover()
        {
            //Initialise the turonver table
            for (int i = 1; i < 9; i++)
                this.value.Add(i, 0);
        }

        public void reinitialiase()
        {
            for (int i = 1; i < 9; i++)
                this.value[i] = 0;
        }

        public void clone(Turnover t)
        {
            for (int i = 1; i < 9; i++)
                this.value[i] = t.value[i];
        }
    }
}
