using Game.ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Reversi : Form
    {
        private Board board;

        public Reversi()
        {
            InitializeComponent();

        }

        private void init()
        {
            this.board = new Board();
            this.refreshBoard();

            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {
                    // Init picture box
                }
            }

        }

        private void refreshBoard()
        {
            for (int y = 0; y < 8; y ++)
            {
                for (int x = 0; x < 8; x ++)
                {
                    PictureBox p = (PictureBox)boardGUI.GetControlFromPosition(x, y);
                    if (this.board.Grid[x, y].Player == null)
                    {
                        // this.boardGUI.
                    }      
                    else
                    {
                        if (this.board.Grid[x, y].Player.Owner == Player.COMPUTER)
                        {
                            // AI : Afficher Noir
                            p.Image = Image.FromFile("../Resources/black.png");

                        }
                        else
                        {
                            p.Image = Image.FromFile("../Resources/white.png");
                        }
                    }
                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            // play
        }

        private void pictureBox_Hover(object sender, EventArgs e)
        {
            // hover
        }

    }
}
