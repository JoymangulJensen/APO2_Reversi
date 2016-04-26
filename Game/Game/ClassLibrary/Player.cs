using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    class Player
    {
        #region Properties

        #region Static
        /// <summary>
        /// Global
        /// Player who will put the next piece
        /// </summary>
        private static int currentPlayer = HUMAN;

        public static int CurrentPlayer
        {
            get { return Player.currentPlayer; }
            set { Player.currentPlayer = value; }
        }

        #endregion

        #region fields
        /// <summary>
        /// Num of the player (1 or 2)
        /// </summary>
        public int owner;

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// Number of pieces
        /// </summary>
        private int score = 0;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// Name of the player
        /// </summary>
        private string name = "Player";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #endregion

        #region Constants
        /// <summary>
        /// Black
        /// </summary>
        public const int HUMAN = 1; // Black

        /// <summary>
        /// White
        /// </summary>
        public const int COMPUTER = 2; // White

        #endregion

        #region Constructors

        public Player(int player)
        {
            this.owner = player;        
        }

        public Player(int player, String name)
        {
            this.owner = player;
            this.name = name;
        }
        #endregion
    }
}
