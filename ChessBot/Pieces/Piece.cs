using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Scripts.Pieces
{
    public abstract class Piece
    {
        protected bool Color;
        protected int Col;
        protected int Row;
        protected char Representation;
        protected int[,] directions;

        public void InitializePiece(bool Color, int Row, int Col)
        {
            this.Color = Color;
            this.Row = Row;
            this.Col = Col;
        }

        public virtual bool CanMove(int Row, int Col)
        {
            return (this.Row != Row) || (this.Col != Col);
        }
        public void Move(int Row, int Col)
        {
            this.Row = Row;
            this.Col = Col;
        }

        public bool getColor()
        {
            return this.Color;
        }

        public int[] getLocation()
        {
            return new int[2] {this.Row, this.Col};
        }

        public virtual char getRepresentation()
        {
            return !this.Color ? Char.ToUpper(this.Representation) : this.Representation;
        }

        public virtual List<string> getValidMoves(ChessAI.Board.Board board)
        {
            List<string> possibleMoves = new List<string>();
            int currentCol, currentRow;
            for (int i = 0; i < this.directions.GetLongLength(0); i++)
            {
                currentCol = this.getLocation()[1];
                currentRow = this.getLocation()[0];
                while (board.canMove(currentRow, currentCol, currentRow + this.directions[i, 0],
                    currentCol + this.directions[i, 1]))
                {
                    possibleMoves.Add(board.TranslateMoveToString(getLocation()[0], getLocation()[1],
                        currentRow + this.directions[i, 0], currentCol + this.directions[i, 1]));
                    currentRow += this.directions[i, 0];
                    currentCol += this.directions[i, 1];
                }
            }
            return possibleMoves;
        }
    }
}
