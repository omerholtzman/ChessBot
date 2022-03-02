using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessBot.Pieces
{
    class PieceFactory
    {
        public static Piece createPiece(String PieceName, bool Color, int Row, int Col)
        {
            switch (PieceName){
                case "Queen":
                    return new Queen(Color, Row, Col);
                case "King":
                    return new King(Color, Row, Col);
                case "Pawn":
                    return new Pawn(Color, Row, Col);
                case "Rook":
                    return new Rook(Color, Row, Col);
                case "Bishop":
                    return new Bishop(Color, Row, Col);
                case "Knight":
                    return new Knight(Color, Row, Col);
                default:
                    return null;
            }
        }
    }
}
