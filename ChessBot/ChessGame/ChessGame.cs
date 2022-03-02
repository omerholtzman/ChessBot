using ChessAI.Scripts.Pieces;
using ChessBot.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBot.ChessGame
{
    class ChessGame
    {
        private const bool WHITE = false;
        private const bool BLACK = true;

        private Stack<KeyValuePair<String, Piece>> Moves = new Stack<KeyValuePair<string, Piece>>();

        private bool currentTurn = WHITE;
        private BoardAPI boardAPI;

        public ChessGame(BoardAPI newBoardAPI)
        {
            this.boardAPI = newBoardAPI;
            this.boardAPI.initializeBoard();
            this.boardAPI.renderBoard();
        }
        public bool getCurrentTurn()
        {
            return this.currentTurn;
        }

        public bool playGame()
        {
            playTurn();
            return !this.currentTurn;
        }

        public void playTurn()
        {
            string currentTurnString = "";
            Piece capturedPiece;

            do
            {
                Console.Write("Play next turn: ");
                currentTurnString = Console.ReadLine();
            } while (!boardAPI.canMove(currentTurnString));

            capturedPiece = boardAPI.makeMove(currentTurnString);
            this.Moves.Push(new KeyValuePair<string, Piece>(currentTurnString, capturedPiece));
            boardAPI.renderBoard();

            if (!gameHasEnded())
            {
                this.currentTurn = !this.currentTurn;
                playTurn();
            }

        }

        public bool gameHasEnded()
        {
            return false;
        }

        public void resetGame()
        {
            currentTurn = WHITE;
            boardAPI.resetBoard();
            this.Moves = new Stack<KeyValuePair<string, Piece>>();
        }
    }
}
