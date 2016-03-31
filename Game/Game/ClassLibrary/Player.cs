using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    class Player
    {

        private int owner; // Soit 1 soit 2

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// White
        /// </summary>
        public const int HUMAN = 1; // White

        /// <summary>
        /// Black
        /// </summary>
        public const int COMPUTER = 2; // Black

        public Player(int player)
        {
            this.owner = player;
        }
    }
}
