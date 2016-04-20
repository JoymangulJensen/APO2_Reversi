using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    class Piece
    {

        #region Properties
        private int x; // Abscisse

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        private int y; // Ordonnée

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        private Player player; // Joueur

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        #endregion 

        #region Constructor

        public Piece(int x , int y)
        {
            this.x = x;
            this.y = y;
            this.player = null; // Empty by default
        }

        public Piece(int x, int y, Player p)
        {
            this.x = x;
            this.y = y;
            this.player = p;
        }

        #endregion

        public void switchPiece()
        {
            if (this.player.Owner == Player.HUMAN)
            {
                this.player.Owner = Player.COMPUTER;
            }
            else
            {
                this.player.Owner = Player.HUMAN;
            }
        }

        public String ToString()
        {
            String res = " ";
            res += "X : " + this.X + "  y : " + this.Y + "player : " + this.Player;
            return res;
        }
    }
}
