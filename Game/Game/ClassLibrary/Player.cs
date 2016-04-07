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
        private static int currentPlayer = HUMAN;

        /// <summary>
        /// Number of pieces
        /// </summary>
        private int score = 0;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public static int CurrentPlayer
        {
            get { return Player.currentPlayer; }
            set { Player.currentPlayer = value; }
        }

        /// <summary>
        /// Num of the player (1 or 2)
        /// </summary>
        private int owner;

        /// <summary>
        /// Name of the player
        /// </summary>
        private string name = "Player";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// Black
        /// </summary>
        public const int HUMAN = 1; // Black

        /// <summary>
        /// White
        /// </summary>
        public const int COMPUTER = 2; // White

        public Player(int player)
        {
            this.owner = player;        
        }

        public Player(int player, String name)
        {
            this.owner = player;
            this.name = name;
        }

        
    }
}
