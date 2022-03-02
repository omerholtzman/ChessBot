using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.Pieces
{
    class Pawn : Piece
    {
        private bool Moved;
        private int diffFactor;
        private bool movedTwiceNow = false;
        public Pawn(bool Color, int Row, int Col)
        {
            InitializePiece(Color, Row, Col);
            this.Moved = false;
            this.Representation = 'p';
            this.diffFactor = Color ? -1 : 1;
            this.directions = new int[4, 2] { { diffFactor, 0 }, { 2 * diffFactor, 0 }, { diffFactor, 1 }, { diffFactor, -1 } };
        }

        public override bool CanMove(int Row, int Col)
        {
            return (this.Col == Col && 
                (Row == this.Row + 1 * diffFactor || // Basic Move
                (Row == this.Row + 2 * diffFactor && (!this.Moved))) || // first Move
                (Math.Abs(this.Col - Col) == 1 && Row == this.Row + diffFactor));  // pawn attack
        }
        new public void Move(int Row, int Col)
        {
            base.Move(Row, Col);
            this.Moved = true;
            if (Row - this.Row == 2) this.movedTwiceNow = true;
        }
        public bool hasMoved()
        {
            return Moved;
        }

        public bool GetMovedTwiceNow()
        {
            return this.movedTwiceNow;
        }

        public void SetMovedTwiceNow(bool newMovedTwice)
        {
            this.movedTwiceNow = newMovedTwice;
        }
    }
}
