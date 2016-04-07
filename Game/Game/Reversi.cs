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
        /// <summary>
        /// Grille
        /// </summary>
        private Board board;

        public Reversi()
        {
            InitializeComponent();
            init();
        }

        /// <summary>
        /// Initialise la grille
        /// </summary>
        private void init()
        {
            this.board = new Board();

            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {
                    PictureBox p = initPictureBox();
                    boardGUI.Controls.Add(p, col, row);
                }
            } 
            this.refreshBoard();
        }

        /// <summary>
        /// Renvoie une PictureBox correctement initialisée
        /// </summary>
        /// <returns></returns>
        private PictureBox initPictureBox()
        {
            PictureBox p = new PictureBox();
            p.Visible = true;
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            p.Click += new EventHandler(this.pictureBox_Click);
            p.MouseHover += new EventHandler(this.pictureBox_Hover);
            p.BackColor = Color.Transparent;
            p.Image = null;
            return p;
        }

        /// <summary>
        /// Actualise l'affichage de la grille selon board
        /// </summary>
        private void refreshBoard()
        {
            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {

                    PictureBox p = (PictureBox)boardGUI.GetControlFromPosition(col, row);

                    if (this.board.Grid[col, row].Player == null)
                    {
                        // this.boardGUI.
                    }      
                    else
                    {
                        if (this.board.Grid[col, row].Player.Owner == Player.HUMAN)
                        {
                            // AI : Afficher Noir
                            p.Image = Image.FromFile("../../Resources/black.png");
                        }
                        else
                        {
                            p.Image = Image.FromFile("../../Resources/white.png");
                        }
                    }
                }
            }
            this.refreshScore();

        }

        private void refreshScore()
        {
            this.labelScore1.Text = "Score : " + board.numberOfPieceByPlayer(Player.COMPUTER);
        }

        /// <summary>
        /// Clic sur PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            TableLayoutPanelCellPosition position = boardGUI.GetPositionFromControl(p);
            int x = position.Row;
            int y = position.Column;
            MessageBox.Show(board.testNeighbour(board.Grid[y, x]).ToString());
            this.refreshBoard();
        }

        /// <summary>
        /// Surf sur PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Hover(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            TableLayoutPanelCellPosition position = boardGUI.GetPositionFromControl(p);
            int x = position.Row;
            int y = position.Column;
            
        }

    }
}
