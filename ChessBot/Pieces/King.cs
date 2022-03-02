using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.Pieces
{
    class King : Piece
    {
        private bool Moved;
        public King(bool Color, int Row, int Col)
        {
            InitializePiece(Color, Row, Col);
            this.Moved = false;
            this.Representation = 'k'; 
            this.directions = new int[8, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 },
                { 1, 0 }, { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } };
        }

        public override bool CanMove(int Row, int Col)
        {
            return Math.Abs(this.Row - Row) <= 1 &&
                   Math.Abs(this.Col - Col) <= 1 && 
                   base.CanMove(Row, Col);
        }
        new public void Move(int Row, int Col)
        {
            base.Move(Row, Col);
            this.Moved = true;
        }
        public bool hasMoved()
        {
            return Moved;
        }
    }
}
