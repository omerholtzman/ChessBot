using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(bool Color, int Row, int Col)
        {
            InitializePiece(Color, Row, Col);
            this.Representation = 'b';
            this.directions = new int[4, 2] { { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } };
        }

        public override bool CanMove(int Row, int Col)
        {
            return (Math.Abs(this.Row - Row) == Math.Abs(this.Col - Col)) && base.CanMove(Row, Col);
        }
    }
}
