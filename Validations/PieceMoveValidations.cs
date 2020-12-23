using System;

using WPFChess.MainFlow;

namespace WPFChess.Validations
{
    internal class PieceMoveValidations
    {
        public bool PieceMovingLikeABlackPawn(WPFChess.MainWindow wmw, bool pieceAtDst, int xsrc, int ysrc, int xdst, int ydst)
        {
            //on any move can move forward 1 square if destination is free
            if ((ysrc - ydst == -1) && (!pieceAtDst) && (xsrc - xdst == 0))
                return true;
            //on first move can move forward 2 squares if destination is free and square is empty in between
            if ((ysrc == 1) && (ysrc - ydst == -2) && (xsrc - xdst == 0))
            {
                var pieceHere = Utility.WhatPieceIsHere(wmw, xsrc, ysrc + 1);
                if (!pieceAtDst && pieceHere == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //can move forward 1 square diagonally if taking a piece
            if ((((ysrc - ydst) == -1)) && (pieceAtDst) && (Math.Abs(xsrc - xdst) == 1))
                return true;
            //TODO
            //can move forward 1 square diagonally into empty square if executing en passant
            //if destintion square is the last rank (1 or 8) then change to another piece
            return false;
        }

        public bool PieceMovingLikeAWhitePawn(WPFChess.MainWindow wmw, bool pieceAtDst, int xsrc, int ysrc, int xdst, int ydst)
        {
            //on any move can move forward 1 square if destination is free
            if ((ysrc - ydst == 1) && (!pieceAtDst) && (xsrc - xdst == 0))
                return true;
            //on first move can move forward 2 squares if destination is free and square is empty in between
            if ((ysrc == 6) && (ysrc - ydst == 2) && (xsrc - xdst == 0))
            {
                var pieceHere = Utility.WhatPieceIsHere(wmw, xsrc, ysrc - 1);
                if (!pieceAtDst && pieceHere == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //can move forward 1 square diagonally if taking a piece
            if ((((ysrc - ydst) == 1)) && (pieceAtDst) && (Math.Abs(xsrc - xdst) == 1))
                return true;
            //TODO
            //can move forward 1 square diagonally into empty square if executing en passant
            //if destintion square is the last rank (1 or 8) then change to another piece
            return false;
        }

        public bool PieceMovingLikeARook(int xsrc, int ysrc, int xdst, int ydst)
        {
            //rooks (castles) move along a rank or along a file in any direction, not diagonally
            //so, either the x (rank) or y (file) must have the same starting and ending value
            //so, its moving horizontal or vertically
            if ((xsrc == xdst) || (ysrc == ydst))
            {
                return true;
            }
            return false;
        }

        public bool PieceMovingLikeAKnight(int xsrc, int ysrc, int xdst, int ydst)
        {
            //knights move 2x1 OR 1x2
            if (((Math.Abs(xsrc - xdst) == 2) && (Math.Abs(ysrc - ydst) == 1)) || ((Math.Abs(xsrc - xdst) == 1) && (Math.Abs(ysrc - ydst) == 2)))
                return true;
            return false;
        }

        public bool PieceMovingLikeABishop(int xsrc, int ysrc, int xdst, int ydst)
        {
            //bishops move by equal n squares in x and y directions
            if (Math.Abs(xsrc - xdst) == Math.Abs(ysrc - ydst))
                return true;
            return false;
        }

        public bool PieceMovingLikeAQueen(int xsrc, int ysrc, int xdst, int ydst)
        {
            //queens move like a rook or bishop
            if ((PieceMovingLikeARook(xsrc, ysrc, xdst, ydst)) || (PieceMovingLikeABishop(xsrc, ysrc, xdst, ydst)))
            {
                return true;
            }

            return false;
        }

        public bool PieceMovingLikeAKing(GameState gs, int xsrc, int ysrc, int xdst, int ydst)
        {
            //kings move one square in any direction, so r and f can both change by 1 or one of them can change by 1
            //must allow for castling - 0 or 00 ... TO DO
            if ((Math.Abs(xsrc - xdst) <= 1) && (Math.Abs(ysrc - ydst) <= 1))
                return true;
            //is it castling
            if (Math.Abs(xsrc - xdst) == 2)
            {
                if (xdst == 6 && ydst == 7)
                    gs.WKCRS = true;
                if (xdst == 6 && ydst == 0)
                    gs.BKCRS = true;

                if (xdst == 2 && ydst == 7)
                    gs.WKCQS = true;
                if (xdst == 2 && ydst == 0)
                    gs.BKCQS = true;

                return true;
            }

            return false;
        }
    }
}