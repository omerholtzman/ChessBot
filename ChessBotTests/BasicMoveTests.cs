using NUnit.Framework;
using ChessAI.Scripts.Pieces;
using ChessAI.Board;


namespace ChessBotTests
{
    class BasicMoveTests : ChessTest
    {
        [Test]
        public void BasicMove()
        {
            boardAPI.initializeBoard();
            ExpectMove("a2 a4");
            ExpectMove("b1 c3");
            boardAPI.makeMove("b1 c3");
            UnexpectedMove("b1 c3");
        }

        [Test]
        public void BasicCastle()
        {
            boardAPI.initializeBoard("8/8/8/8/8/8/8/4K2R/");
            ExpectMove("e8 g8");
        }

        [Test]
        public void BasicPawnMove()
        {
            boardAPI.initializeBoard("8/8/8/8/4k2K/8/8/7P");
            ExpectMove("h8 h6");
            boardAPI.makeMove("h8 h6");
            UnexpectedMove("h6 h4");
            UnexpectedMove("h6 h5");  // king occuppied that
            UnexpectedMove("h6 g5");
        }

        [Test]
        public void BasicKnightMove()
        {
            boardAPI.initializeBoard("8/8/8/8/k7/8/1B6/8/");
            boardAPI.renderBoard();
            ExpectMove("a5 b3");
            ExpectMove("a5 c6");
            ExpectMove("a5 b7");
        }

        [Test]
        public void BasicQueenMove()
        {
            boardAPI.initializeBoard("8/8/8/2Qk4/3b4/5k1K/3B4/3r4/");
            boardAPI.renderBoard();
            ExpectMove("c4 d4");
            boardAPI.makeMove("c4 d4");
            ExpectMove("d4 d5");
            UnexpectedMove("d4 d6");
            boardAPI.makeMove("d4 d5");
            UnexpectedMove("d5 d8");
            boardAPI.renderBoard();
        }
    }

    // todo: go over the code and make sure its readable and right
    // todo: make a list of all possible moves
    // todo: choose one at random
    // todo: eval function??
}