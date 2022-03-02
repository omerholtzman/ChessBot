using ChessAI.Scripts.Pieces;
using ChessBot.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Board
{
    public class Board
    {
        private const int BOARD_SIZE = 8;
        private const bool WHITE = false;
        private const bool BLACK = true;

        private static Dictionary<int, char> MovesDictionary = new Dictionary<int, char> {[0] = 'a',
        [1] = 'b', [2] = 'c', [3] = 'd', [4] = 'e', [5] = 'f', [6] = 'g', [7] = 'h'};


        private int[] WhiteKingLocation = new int[2];
        private int[] BlackKingLocation = new int[2];

        private List<Piece> WhiteCapturedPieces = new List<Piece>();
        private List<Piece> BlackCapturedPieces = new List<Piece>();

        private List<Piece> BlackPieces = new List<Piece>();
        private List<Piece> WhitePieces = new List<Piece>();

        private static Dictionary<char, String> pieceDictionary = new Dictionary<char, String> {
            ['k'] = "King", ['n'] = "Knight", ['q'] = "Queen",
            ['b'] = "Bishop", ['p'] = "Pawn", ['r'] = "Rook"
        };

        private Piece[, ] board;

        private Piece takenTemporaryPiece;

        public Board()
        {
            this.board = new Piece[BOARD_SIZE, BOARD_SIZE];
        }

        public void resetBoard()
        {
            BlackCapturedPieces = new List<Piece>();
            WhiteCapturedPieces = new List<Piece>();
            BlackPieces = new List<Piece>();
            WhitePieces = new List<Piece>();

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    this.board[i, j] = null;
                }
            }
        }

        public void InitializeBoard(string boardString)
        {
            int currentCol = 0;
            int currentRow = 0;
            int stringIndex = 0;
            char currentChar;
            Piece currentPiece;
            
            while (stringIndex < boardString.Length)
            {
                currentChar = boardString.ElementAt(stringIndex);
                if (Char.IsLetter(currentChar)){
                    currentPiece = this.PieceHandler(currentChar, currentRow, currentCol);
                    if (typeof(King).IsInstanceOfType(currentPiece)) updateKingLocation(currentPiece);
                    addToPieceCollection(currentPiece);
                    this.board[currentRow, currentCol] = currentPiece;
                    currentCol++;
                }
                else if (Char.IsDigit(currentChar)){
                    currentCol += (int) currentChar - '0';
                }
                else if (currentChar == '/')
                {
                    currentCol = 0;
                    currentRow++;
                }
                stringIndex++;
            }
        }

        private void addToPieceCollection(Piece currentPiece)
        {
            if (currentPiece.getColor())
            {
                BlackPieces.Add(currentPiece);
            }
            else
            {
                WhitePieces.Add(currentPiece);
            }
        }

        private Piece PieceHandler(Char pieceChar, int Row, int Col)
        {
            bool Color = Char.IsUpper(pieceChar) ? BLACK : WHITE;
            
            return PieceFactory.createPiece(Board.pieceDictionary[Char.ToLower(pieceChar)],
                Color, Row, Col);
        }

        public List<string> GetValidMoves(bool playerColor)
        {
            List<string> validMoves = new List<string>();
            List<Piece> playerCollection = playerColor ? BlackPieces : WhitePieces;
            foreach (Piece piece in playerCollection)
            {
                validMoves.AddRange(piece.getValidMoves(this));
            }
            
            return validMoves;
        }

        public string TranslateMoveToString(int lastRow, int lastCol, int newRow, int newCol)
        {
            StringBuilder newStringBuilder = new StringBuilder();
            newStringBuilder.Append(MovesDictionary[lastCol]);
            newStringBuilder.Append(lastRow.ToString());
            newStringBuilder.Append(" ");
            newStringBuilder.Append(MovesDictionary[newCol]);
            newStringBuilder.Append(newRow.ToString());

            return newStringBuilder.ToString();
        }

        public Piece[, ] getBoard()
        {
            return this.board;
        }

        public Piece MovePiece(int lastRow, int lastCol, int newRow, int newCol)
        {
            Piece capturedPiece = board[newRow, newCol];
            Piece movingPiece = board[lastRow, lastCol];

            // adds to the captured pieces
            if (capturedPiece != null) addToCapturedPieces(capturedPiece);

            movingPiece.Move(newRow, newCol);
            board[newRow, newCol] = movingPiece;
            board[lastRow, lastCol] = null;

            // updates king location
            if (typeof(King).IsInstanceOfType(movingPiece)) updateKingLocation(movingPiece);
            
            return capturedPiece;
        }

        private void updateKingLocation(Piece movingPiece)
        {
            if ((movingPiece).getColor())
            {
                this.BlackKingLocation = movingPiece.getLocation();
            }
            else
            {
                this.WhiteKingLocation = movingPiece.getLocation();
            }
        }
        private void addToCapturedPieces(Piece capturedPiece)
        {
            if (capturedPiece.getColor())
            {
                BlackCapturedPieces.Add(capturedPiece);
            }
            else
            {
                WhiteCapturedPieces.Add(capturedPiece);
            }
        }

        public bool canMove(int lastRow, int lastCol, int newRow, int newCol)
        {
            Piece CurrentPiece = board[lastRow, lastCol];

            if (!moveInBoard(new int[4] { lastCol, lastRow, newRow, newCol })) return false;  // move not in board
            if (CurrentPiece == null) return false;  // there is no a piece in old

            bool currentColor = CurrentPiece.getColor();

            if (board[newRow, newCol] == null || board[newRow, newCol].getColor() != currentColor)
            {
                if (!typeof(King).IsInstanceOfType(CurrentPiece)
                    && !typeof(Pawn).IsInstanceOfType(CurrentPiece))
                {
                    // Rook, Queen, Bishop, Knight legal moves:
                    if (!CurrentPiece.CanMove(newRow, newCol)) return false;   // piece cannot perform this move
                    if (!typeof(Knight).IsInstanceOfType(CurrentPiece))
                    {
                        if (SomeoneInterferes(lastRow, lastCol, newRow, newCol)) return false;  // someone interferes
                    }
                }
            }
            else { return false; }     // contains same-color

            // king moves
            if (typeof(King).IsInstanceOfType(CurrentPiece)) 
            {
                // castles
                if(Math.Abs(lastCol - newCol) == 2 && lastRow == newRow && canCastle((King) CurrentPiece, newCol))
                    return checkKingSafety(currentColor, lastRow, lastCol, newRow, newCol);

                // regular king move
                if (CurrentPiece.CanMove(newRow, newCol) && 
                    (board[newRow, newCol] == null || board[newRow, newCol].getColor() != currentColor));
                return checkKingSafety(currentColor, lastRow, lastCol, newRow, newCol);
            }

            if (typeof(Pawn).IsInstanceOfType(CurrentPiece))
            {
                if (pawnAdvance(CurrentPiece, lastRow, lastCol, newRow, newCol))
                    return checkKingSafety(currentColor, lastRow, lastCol, newRow, newCol);

                // enpasante and capture
                if (pawnAttack(currentColor, lastRow, lastCol, newRow, newCol))
                    return checkKingSafety(currentColor, lastRow, lastCol, newRow, newCol);

                return false;
            }

            return checkKingSafety(currentColor, lastRow, lastCol, newRow, newCol);
        }

        private bool checkKingSafety(bool currentColor, int lastRow, int lastCol, int newRow, int newCol)
        {
            // REACHED HERE MEANS PSEUODO-LEGAL MOVE
            DoMove(lastRow, lastCol, newRow, newCol);

            // my king isnt under check after the move. 
            if (!MyKingUnderCheck(currentColor))
            {
                UndoMove(lastRow, lastCol, newRow, newCol);
                return true;
            }
            else
            {
                UndoMove(lastRow, lastCol, newRow, newCol);
                return false;
            }
        }
        
        private void DoMove(int lastRow, int lastCol, int newRow, int newCol)
        {
            Piece movingPiece = board[lastRow, lastCol];
            this.takenTemporaryPiece = board[newRow, newCol];
            board[newRow, newCol] = movingPiece;
            movingPiece.Move(newRow, newCol);
            if (typeof(King).IsInstanceOfType(movingPiece)) updateKingLocation(movingPiece);
            board[lastRow, lastCol] = null;
        }

        private void UndoMove(int lastRow, int lastCol, int newRow, int newCol)
        {
            Piece returnedPiece = board[newRow, newCol];
            board[lastRow, lastCol] = returnedPiece;
            returnedPiece.Move(lastRow, lastCol);
            if (typeof(King).IsInstanceOfType(returnedPiece)) updateKingLocation(returnedPiece);
            board[newRow, newCol] = this.takenTemporaryPiece;
        }

        private bool pawnAttack(bool Color, int lastRow, int lastCol, int newRow, int newCol)
        {
            if (pawnDiagonalMove(Color, lastRow, lastCol, newRow, newCol))
            {
                // regular capture
                if (board[newRow, newCol] != null) return board[newRow, newCol].getColor() != Color;

                // enpasante!
                else {return (typeof(Pawn).IsInstanceOfType(board[lastRow, newCol]) &&
                        ((Pawn)board[lastRow, newCol]).GetMovedTwiceNow());}
            }
            return false;
        }

        private bool pawnDiagonalMove(bool Color, int lastRow, int lastCol, int newRow, int newCol)
        {
            int diffFactor = getDiffFactor(Color);
            return (lastRow + diffFactor == newRow && Math.Abs(newCol - lastCol) == 1);
        }

        private bool pawnAdvance(Piece CurrentPiece, int lastRow, int lastCol, int newRow, int newCol)
        {
            int diffFactor = getDiffFactor(CurrentPiece.getColor());


            if (lastCol == newCol)
            {
                // double move -> we don't have intereferes, right?!
                if (lastRow + (diffFactor * 2) == newRow && !((Pawn)CurrentPiece).hasMoved() &&
                    board[newRow - (1 * diffFactor), newCol] == null && board[newRow, lastCol] == null) return true;

                // single move
                if (lastRow + (1 * diffFactor) == newRow && board[newRow, lastCol] == null) return true;
            }
            return false;
        }

        private bool canCastle(King king, int newCol)
        {
            // todo: check that the king is not being threated in the middle
            if (king.hasMoved()) return false;
            if (king.getColor() == BLACK)
            {
                if (king.getLocation()[1] > newCol)
                {
                    // clear line between king and a valid rook
                    if (board[7, 1] == null && board[7, 2] == null && board[7, 3] == null) return validRook(7, 0);
                }
                else
                {
                    if (board[7, 5] == null && board[7, 6] == null) return validRook(7, 7);
                }
            }
            else
            {
                if (king.getLocation()[1] > newCol)
                {
                    if (board[0, 1] == null && board[0, 2] == null && board[0, 3] == null) return validRook(0, 0);
                }
                else
                {
                    if (board[0, 5] == null && board[0, 6] == null) return validRook(0, 7);  
                }
            }
            return false;
        }

        private bool validRook(int Row, int Col)
        {
            if (board[Row, Col] != null)
            {
                if (typeof(Rook).IsInstanceOfType(board[Row, Col]))
                {
                    return !((Rook) board[Row, Col]).HasMoved();
                }
            }
            return false;
        }

        private bool SomeoneInterferes(int lastRow, int lastCol, int newRow, int newCol)
        {
            int ColDiff = Math.Sign(newCol - lastCol);
            int RowDiff = Math.Sign(newRow - lastRow);

            while (lastRow != newRow - RowDiff || lastCol != newCol - ColDiff)
            {
                lastRow += RowDiff;
                lastCol += ColDiff;
                if (board[lastRow, lastCol] != null) return true;  
            }

            return false;
        }

        private bool MyKingUnderCheck(bool kingColor)
        {
            // consider the move was done
            int kingRow = kingColor ? BlackKingLocation[0] : WhiteKingLocation[0];
            int kingCol = kingColor ? BlackKingLocation[1] : WhiteKingLocation[1];

            // pawns attack the king:
            if (pawnsAttackKing(kingColor, kingRow, kingCol)) return true;

            // knights attack the king
            if (knightsAttackKing(kingColor, kingRow, kingCol)) return true;

            // Rook or Queen in Row
            if (LinesAttack(kingColor, kingRow, kingCol)) return true;

            // Bishop or Queen in Diagonal
            if (DiagonalsAttack(kingColor, kingRow, kingCol)) return true;

            // King attack the King
            if (KingAttackKing(kingColor, kingRow, kingCol)) return true;

            return false;
        }

        private bool KingAttackKing(bool kingColor, int kingRow, int kingCol)
        {
            int otherKingRow = kingColor ? WhiteKingLocation[0] : BlackKingLocation[0];
            int otherKingCol = kingColor ? WhiteKingLocation[1] : BlackKingLocation[1];

            return Math.Abs(kingRow - otherKingRow) <= 1 && Math.Abs(kingCol - otherKingCol) <= 1;
        }

        private bool DiagonalsAttack(bool Color, int Row, int Col)
        {
            int[,] directions = new int[4, 2] { { 1, 1 }, { 1, -1 }, { -1, 1 }, {-1 , -1}};
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                if (LineAttack(!Color, new int[] {Row, Col}, new int[] { directions[i, 0], directions[i, 1] })) return true;
            }
            return false;
        }

        private bool LinesAttack(bool Color, int Row, int Col)
        {
            int[,] directions = new int[4, 2] { { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, 0 } };
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                if (LineAttack(!Color, new int[] {Row, Col}, new int[] {directions[i, 0], directions[i, 1]})) return true;
            }
            return false;
        }

        private bool LineAttack(bool Color, int[] StartingLocation, int[] direction)
        {
            int currentRow = StartingLocation[0];
            int currentCol = StartingLocation[1];
            Piece CurrentPiece = board[currentRow, currentCol];
            while (moveInBoard(new int[2] {currentRow + direction[0], currentCol + direction[1] }))
            {
                currentCol += direction[1];
                currentRow += direction[0];
                CurrentPiece = board[currentRow, currentCol];
                if (CurrentPiece != null)
                {
                    if (CurrentPiece.getColor() == Color)
                    {
                        if (typeof(Queen).IsInstanceOfType(CurrentPiece)) return true;
                        if (typeof(Rook).IsInstanceOfType(CurrentPiece) && (Math.Abs(direction[0]) != Math.Abs(direction[1]))) return true;
                        if (typeof(Bishop).IsInstanceOfType(CurrentPiece) && (Math.Abs(direction[0]) == Math.Abs(direction[1]))) return true;
                        return false;
                    }
                    else {return false;}
                }
            }
            return false;
        }

        private bool knightsAttackKing(bool Color, int Row, int Col)
        {
            int[, ] directions = new int[8, 2] {{1, 2}, {-1, 2}, {1,  -2}, {-1, -2},
                                                {2, 1}, {2, -1}, {-2 , 1}, {-2, -1}};
            for (int i = 0; i < directions.GetLength(0); i++){
                if (knightAttackKing(!Color, Row + directions[i, 0], Col + directions[i, 1])) return true;
            }
            return false;
        }

        private bool knightAttackKing(bool Color, int Row, int Col)
        {
            if (moveInBoard(new int[] { Row, Col }))
            {
                if (board[Row, Col] != null)
                {
                    if (board[Row, Col].getColor() == Color && typeof(Knight).IsInstanceOfType(board[Row, Col])) return true;
                }
            }
            return false;
        }

        private bool pawnsAttackKing(bool Color, int Row, int Col)
        {
            int diff = Color ? -1 : 1;
            if ((Col != 0 && Row + diff < 8 && Row + diff > -1 && pawnAttackKing(!Color, Row + diff, Col - 1)) ||
                (Col != 7 && Row + diff < 8 && Row + diff > -1 && pawnAttackKing(!Color, Row + diff, Col + 1))) return true;
            return false;
        }

        private bool pawnAttackKing(bool Color, int Row, int Col)
        {
            if (board[Row, Col] == null) return false;
            if (typeof(Pawn).IsInstanceOfType(this.board[Row, Col]) 
                && this.board[Row, Col].getColor() == Color) return true;
            return false;
        }

        private bool moveInBoard(int[] rowsAndCols)
        {
            for (int i = 0; i < rowsAndCols.Length; i++)
            {if (0 > rowsAndCols[i] || 7 < rowsAndCols[i]) {return false;}}
            return true;
        }

        private int getDiffFactor(bool Color)
        {
            return Color ? -1 : 1;
        }
    }
}
