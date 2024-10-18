using System;
using System.Drawing;
using System.Windows.Forms;

namespace _4opeenrij
{
    public partial class Form1 : Form
    {
        private const int Rows = 6;
        private const int Cols = 7;
        private const int CellSize = 50;
        private char[,] board = new char[Rows, Cols];
        private bool isPlayerOneTurn = true;
        private bool gameOver = false;

        public Form1()
        {
            InitializeComponent();
            this.Text = "4 op een rij";
            gamePanel.Paint += GamePanel_Paint;
            gamePanel.MouseClick += GamePanel_MouseClick;
            resetButton.Click += ResetButton_Click;

            // Set initial turn label
            turnLabel.Text = "Speler 1's beurt"; // Player 1's turn
            ResetGame();
        }

        private void ResetGame()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    board[row, col] = ' ';
                }
            }
            isPlayerOneTurn = true;
            gameOver = false;
            turnLabel.Text = "Speler 1's beurt"; // Reset to Player 1's turn
            gamePanel.Invalidate();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    DrawCell(g, row, col);
                }
            }
        }

        private void DrawCell(Graphics g, int row, int col)
        {
            int x = col * CellSize;
            int y = row * CellSize;
            g.DrawRectangle(Pens.Black, x, y, CellSize, CellSize);

            if (board[row, col] == 'O')
            {
                g.DrawEllipse(Pens.Blue, x + 5, y + 5, CellSize - 10, CellSize - 10);
            }
            else if (board[row, col] == 'X')
            {
                g.DrawLine(Pens.Red, x + 5, y + 5, x + CellSize - 5, y + CellSize - 5);
                g.DrawLine(Pens.Red, x + CellSize - 5, y + 5, x + 5, y + CellSize - 5);
            }
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameOver)
                return;

            int col = e.X / CellSize;

            if (col >= 0 && col < Cols)
            {
                for (int row = Rows - 1; row >= 0; row--)
                {
                    if (board[row, col] == ' ')
                    {
                        board[row, col] = isPlayerOneTurn ? 'O' : 'X';
                        gamePanel.Invalidate();

                        if (CheckForWinner(row, col))
                        {
                            gameOver = true;
                            MessageBox.Show($"Speler {(isPlayerOneTurn ? 1 : 2)} wint!");
                            return;
                        }
                        else if (IsBoardFull())
                        {
                            gameOver = true;
                            MessageBox.Show("Gelijkspel");
                            return;
                        }

                        isPlayerOneTurn = !isPlayerOneTurn;

                        // Update the turn label
                        turnLabel.Text = isPlayerOneTurn ? "Speler 1's beurt" : "Speler 2's beurt";
                        break;
                    }
                }
            }
        }

        private bool CheckForWinner(int row, int col)
        {
            char player = board[row, col];
            return CheckDirection(row, col, 1, 0, player) ||
                   CheckDirection(row, col, 0, 1, player) ||
                   CheckDirection(row, col, 1, 1, player) ||
                   CheckDirection(row, col, 1, -1, player);
        }

        private bool CheckDirection(int row, int col, int dRow, int dCol, char player)
        {
            int count = 1;
            count += CountInDirection(row, col, dRow, dCol, player);
            count += CountInDirection(row, col, -dRow, -dCol, player);
            return count >= 4;
        }

        private int CountInDirection(int row, int col, int dRow, int dCol, char player)
        {
            int r = row + dRow;
            int c = col + dCol;
            int count = 0;
            while (r >= 0 && r < Rows && c >= 0 && c < Cols && board[r, c] == player)
            {
                count++;
                r += dRow;
                c += dCol;
            }
            return count;
        }

        private bool IsBoardFull()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    if (board[row, col] == ' ')
                        return false;
                }
            }
            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void turnLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
