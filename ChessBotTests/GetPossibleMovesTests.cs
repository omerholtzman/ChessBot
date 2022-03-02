using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotTests
{
    class GetPossibleMovesTests : ChessTest
    {
        [Test]
        public void PawnAndKingVSKing()
        {
            boardAPI.initializeBoard("8/8/8/8/p7/8/4K2k/8/");
            boardAPI.renderBoard();
            int whiteMoves = boardAPI.GetValidMoves(true).Count;
            int blackMoves = boardAPI.GetValidMoves(false).Count;
            Assert.AreEqual(boardAPI.GetValidMoves(true).Count, boardAPI.GetValidMoves(false).Count);

        }
    }
}
