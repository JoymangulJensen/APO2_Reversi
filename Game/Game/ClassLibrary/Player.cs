using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    class Player
    {

        /// <summary>
        /// Global
        /// Player who will put the next piece
        /// </summary>
        public static int currentPlayer;

        /// <summary>
        /// Num of the player (1 or 2)
        /// </summary>
        private int owner;

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
