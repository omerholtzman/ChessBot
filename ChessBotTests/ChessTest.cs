using ChessAI.Board;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotTests
{
    class ChessTest
    {
        protected BoardConsoleAPI boardAPI;
        [SetUp]
        public void Setup()
        {
            this.boardAPI = new BoardConsoleAPI();
        }


        protected void ExpectMove(string move)
        {
            Assert.AreEqual(true, boardAPI.canMove(move));
        }

        protected void UnexpectedMove(string move)
        {
            Assert.AreEqual(false, boardAPI.canMove(move));

        }
    }
}
