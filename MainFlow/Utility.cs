using System.Windows;
using System.Linq;

namespace WPFChess.MainFlow

{
    public partial class Utility
    {
        public static GameState SetInitialGameState()
        {
            GameState gs = new GameState
            {
                WM = "WHITE",
                WKM = false,
                BKM = false,
                WRK = false,
                WRQ = false,
                BRK = false,
                BRQ = false
            };

            return gs;
        }

        public static ChessPiece WhatPieceIsHere(WPFChess.MainWindow wmw, int xRank, int yFile)
        {
            var pieceToMove = wmw.Pieces.Where(x => x.Pos.X == xRank && x.Pos.Y == yFile).ToList();  //return the piece

            return pieceToMove.FirstOrDefault();
        }

        public static void MovePiece(WPFChess.MainWindow wmw, int xsrc, int ysrc, int xdst, int ydst)
        {
            //get the piece to move
            var pieceToMove = Utility.WhatPieceIsHere(wmw, xsrc, ysrc);

            //take the piece if any in destination and add it to collection of pieces taken
            var pieceToTake = Utility.WhatPieceIsHere(wmw, xdst, ydst);  //get the piece to take if any
            if (pieceToTake != null)                        //if there is a piece here take it
            {
                wmw.Pieces.Remove(pieceToTake);            //remove the piece from board
                wmw.PiecesTaken.Add(pieceToTake);          //add the piece to the list of taken pieces
            }

            //move the piece - add and remove from the observable location
            ChessPiece moveTo = new ChessPiece
            {
                Player = pieceToMove.Player,
                Type = pieceToMove.Type,
                Pos = new Point(xdst, ydst)     //put the piece here
            };
            wmw.Pieces.Add(moveTo);
            wmw.Pieces.Remove(pieceToMove); //remove the piece
        }

        public static void MoveCastleWhenCastling(WPFChess.MainWindow wmw, GameState GS)
        {
            if (GS.WKCRS)
            {
                Utility.MovePiece(wmw, 7, 7, 5, 7); //move castle over to complete the castle move
                GS.WKCRS = false; //and reset the flag
            }
            if (GS.WKCQS)
            {
                Utility.MovePiece(wmw, 0, 7, 3, 7); //move castle over to complete the castle move
                GS.WKCQS = false; //and reset the flag
            }
            if (GS.BKCRS)
            {
                Utility.MovePiece(wmw, 7, 0, 5, 0); //move castle over to complete the castle move
                GS.BKCRS = false; //and reset the flag
            }
            if (GS.BKCQS)
            {
                Utility.MovePiece(wmw, 0, 0, 3, 0); //move castle over to complete the castle move
                GS.BKCQS = false; //and reset the flag
            }
        }

        //test method
        public void MakeSomeMoves(WPFChess.MainWindow wmw)
        {
            _ = wmw.Pieces.ElementAt(6); //test

            MovePiece(wmw, 1, 7, 2, 5);  //white knight
            MovePiece(wmw, 1, 0, 2, 2);  //black knight
            MovePiece(wmw, 4, 4, 2, 2);  //nothing
            _ = Utility.WhatPieceIsHere(wmw, 1, 1);
        }

        public static void Setup1(WPFChess.MainWindow wmw)
        {
            //pawns
            MovePiece(wmw, 4, 6, 4, 4);
            MovePiece(wmw, 4, 1, 4, 3);
            //knights
            MovePiece(wmw, 6, 7, 5, 5);
            MovePiece(wmw, 6, 0, 5, 2);
            //bishops
            MovePiece(wmw, 5, 7, 4, 6);
            MovePiece(wmw, 5, 0, 4, 1);
        }

        public static void Setup2(WPFChess.MainWindow wmw)
        {
            //pawns
            MovePiece(wmw, 3, 6, 3, 4);
            MovePiece(wmw, 3, 1, 3, 3);
            MovePiece(wmw, 2, 6, 2, 4);
            MovePiece(wmw, 2, 1, 2, 3);
            //knights
            MovePiece(wmw, 1, 0, 2, 2);
            MovePiece(wmw, 1, 7, 2, 5);
            //queens
            MovePiece(wmw, 3, 7, 3, 5);
            MovePiece(wmw, 3, 0, 3, 2);
            //bishops
            MovePiece(wmw, 2, 7, 3, 6);
            MovePiece(wmw, 2, 0, 3, 1);
        }
    }
}