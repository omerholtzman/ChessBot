using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ChessAI.Scripts.Pieces;
using ChessBot.Board;

namespace ChessAI.Board
{
    public class BoardConsoleAPI : BoardAPI
    {
        private const string defultStartingBoard = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/";
        private Board board;
        public BoardConsoleAPI(Board board)
        {
            this.board = board;
        }

        public BoardConsoleAPI()
        {
            this.board = new Board();
        }
        public void renderBoard()
        {
            Piece[, ] printableBoard = this.board.getBoard();

            for (int i = printableBoard.GetLength(0) - 1; i >= 0; i--)
            {
                Console.Write((i + 1).ToString() + " |");
                for (int j = 0; j < printableBoard.GetLength(1); j++)
                {
                    if (printableBoard[i, j] != null)
                    {
                        Console.Write(" " + printableBoard[i, j].getRepresentation() + " ");
                    }
                    else
                    {
                        if ((i + j) % 2 == 0)
                        {
                            Console.Write(" * ");
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    Console.Write(" ");
                }
                Console.WriteLine("");
                Console.WriteLine("--                               ");
            }
            // Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  | a | b | c | d | e | f | g | h |", Console.ForegroundColor);
        }

        public void initializeBoard(String boardString)
        {
            this.board.resetBoard();
            this.board.InitializeBoard(boardString);
        }

        public void initializeBoard()
        {
            initializeBoard(defultStartingBoard);
        }
        public Piece makeMove(String move)
        {
            int[] location = stringToLocation(move);
            return board.MovePiece(location[0], location[1], location[2], location[3]);
        }

        public bool canMove(String move)
        {
            int[] location = stringToLocation(move);
            return board.canMove(location[0], location[1], location[2], location[3]);
        }

        public int[] stringToLocation(string move)
        {
            // a2 b4 format
            int[] newMove = new int[4] {
                (int)Char.GetNumericValue(move[1]) - 1,
                (move[0]) - 'a',
                (int)Char.GetNumericValue(move[4]) - 1,
                (move[3]) - 'a'};
            
            return newMove;
        }

        

        public void resetBoard()
        {
            initializeBoard();
        }

        public List<string> GetValidMoves(bool color)
        {
            return this.board.GetValidMoves(color);
        }

    }
}

