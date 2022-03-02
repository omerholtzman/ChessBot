using ChessAI.Board;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotTests
{
    class BasicCheckTests : ChessTest
    {
        [Test]
        public void BasicRookCheck()
        {
            boardAPI.initializeBoard("8/8/8/7R/R2k1K2/8/8/8/");
            ExpectMove("d5 d6");
            UnexpectedMove("d5 c5");
            UnexpectedMove("d5 d4");
            UnexpectedMove("d5 e6");
        }

        [Test]
        public void BasicRowPin()
        {
            boardAPI.initializeBoard("8/8/8/7R/R1rk1K2/8/8/8/");
            ExpectMove("c5 a5");
            ExpectMove("c5 b5");
            ExpectMove("d5 d6");
            UnexpectedMove("c5 c4");
        }

        [Test]
        public void DoubleCheck()
        {
            boardAPI.initializeBoard("8/8/8/8/R1rk1K2/3R4/8/8/");
            ExpectMove("d5 d6");
            UnexpectedMove("c5 a5");
            UnexpectedMove("c5 c6");
        }

        [Test]
        public void EatProtectedPiece()
        {
            boardAPI.initializeBoard("8/8/8/8/R1Rk1K2/8/8/8/");
            ExpectMove("d5 d6");
            UnexpectedMove("d5 c5");
        }

        [Test]
        public void KingRuningIntoQueen()
        {
            boardAPI.initializeBoard("8/8/8/8/3k1K2/8/7q/8/");
            UnexpectedMove("f6 g7");
        }
    }
}
