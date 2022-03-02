using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.Pieces
{
    class Queen : Piece
    {
        public Queen(bool Color, int Row, int Col)
        {
            InitializePiece(Color, Row, Col);
            this.Representation = 'q';
            this.directions = new int[8, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 },
                { 1, 0 }, { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } };
            
        }
    

        public override bool CanMove(int Row, int Col)
        {
            return ((this.Row == Row || this.Col == Col) || // Rook Move
                (Math.Abs(this.Row - Row) == Math.Abs(this.Col - Col)))  // Bishop Move
                && base.CanMove(Row, Col);
        }
    }
}
