using ChessAI.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.Board
{
    interface BoardAPI
    {
        void resetBoard();

        void initializeBoard();

        void renderBoard();

        Piece makeMove(string moveString);

        bool canMove(string moveString);

        List<string> GetValidMoves(bool color);
    }
}
