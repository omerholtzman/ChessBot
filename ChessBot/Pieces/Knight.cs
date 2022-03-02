using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.Pieces
{
    class Knight : Piece
    {
        public Knight(bool Color, int Row, int Col)
        {
            InitializePiece(Color, Row, Col);
            this.Representation = 'n';
            this.directions = new int[8, 2] { { 2, 1 }, { 2, -1 }, { -1, 2 },
                { 1, 2 }, { -2, 1 }, { -2, -1 }, { 1, -2 }, { -1, -2 } };
        }

        public override bool CanMove(int Row, int Col)
        {
            return ((Math.Abs(this.Row - Row) == 2 && Math.Abs(this.Col - Col) == 1)
                    || (Math.Abs(this.Row - Row) == 1 && Math.Abs(this.Col - Col) == 2))
                    && base.CanMove(Row, Col);
        }
    }
}
