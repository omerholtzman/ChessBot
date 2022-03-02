using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Scripts.Pieces
{
    class Rook : Piece
    {
        private bool Moved;
        public Rook(bool Color, int Row, int Col)
        {
            InitializePiece(Color, Row, Col);
            this.Moved = false;
            this.Representation = 'r';
            this.directions = new int[4, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };
        }

        public override bool CanMove(int Row, int Col)
        {
            return (this.Row == Row || this.Col == Col) && base.CanMove(Row, Col);
        }

        new public void Move(int Row, int Col)
        {
            base.Move(Row, Col);
            this.Moved = true;
        }

        public bool HasMoved()
        {
            return this.Moved;
        }
    }
}
