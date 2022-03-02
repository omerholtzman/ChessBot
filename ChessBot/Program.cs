using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAI.Board;
using ChessAI.Scripts.Pieces;
using ChessBot.ChessGame;

namespace ChessAI
{
    class Program
    {
        static void Main(string[] args)
        {
            BoardConsoleAPI boardAPI = new BoardConsoleAPI(new ChessAI.Board.Board());
            boardAPI.initializeBoard();

            ChessGame newGame = new ChessGame(boardAPI);
            newGame.playGame();
        }
    }
}
