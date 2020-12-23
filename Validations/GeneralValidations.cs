using System;

using WPFChess.MainFlow;
using WPFChess.Models;

namespace WPFChess.Validations
{
    public static class GeneralValidations
    {
        public static bool ValidateMove(WPFChess.MainWindow wmw, GameState gs)
        {
            //get the pieces at src and dst
            var pieceAtSrc = Utility.WhatPieceIsHere(wmw, gs.array[0], gs.array[1]);
            var pieceAtDst = Utility.WhatPieceIsHere(wmw, gs.array[2], gs.array[3]);

            //check there is a piece to move from here
            if (pieceAtSrc == null)
                return false;

            //if there is a destination piece it cannot be of the same colour as source piece colour, ie. you can't take your own piece!
            if (pieceAtDst != null && (pieceAtSrc.Player == pieceAtDst.Player))
                return false;

            //check that it is the correct turn of move
            if ((pieceAtSrc.Player == Player.White && gs.WM == "BLACK") || (pieceAtSrc.Player == Player.Black && gs.WM == "WHITE"))
                return false;

            // it is invalid if the destination of the move has a piece that is not of the opposite colour
            //if ((IsPieceAtDestinationLocation(gs.array)) && (!IsPieceAtSourceDestinationLocationOppositeColour(gs)))
            //  return false;

            //if (!IsClearPath(gs))
            //    return false;

            if (!IsPieceMovingCorrectly(wmw, gs))
                return false;

            //all validated ok
            return true;
        }

        public static bool IsClearPath(WPFChess.MainWindow wmw, GameState gs)
        {
            bool result = false;
            var whatPiece = Utility.WhatPieceIsHere(wmw, gs.array[0], gs.array[1]);

            //any piece moving 1 square doesn't need this check
            if ((Math.Abs(gs.array[0] - gs.array[2]) <= 1) && (Math.Abs(gs.array[1] - gs.array[3]) <= 1))
            {
                result = true;
            }
            //knights dont need this check as they jump over
            if (whatPiece.Type == PieceType.Knight)
            {
                result = true;
            }

            return result;
        }

        public static bool IsPieceMovingCorrectly(WPFChess.MainWindow wmw, GameState gs)
        {
            bool result = false;
            var whatPiece = Utility.WhatPieceIsHere(wmw, gs.array[0], gs.array[1]);
            bool pieceAtDst = IsPieceAtDestinationLocation(wmw, gs.array);
            PieceMoveValidations pmv = new PieceMoveValidations();

            switch (whatPiece.Type)
            {
                case PieceType.Pawn:
                    if (whatPiece.Player == Player.White)
                    {
                        if (pmv.PieceMovingLikeAWhitePawn(wmw, pieceAtDst, gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (pmv.PieceMovingLikeABlackPawn(wmw, pieceAtDst, gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                        {
                            result = true;
                        }
                    }
                    break;

                case PieceType.Rook:
                    if (pmv.PieceMovingLikeARook(gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                    {
                        result = true;
                    }
                    break;

                case PieceType.Knight:
                    if (pmv.PieceMovingLikeAKnight(gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                    {
                        result = true;
                    }
                    break;

                case PieceType.Bishop:
                    if (pmv.PieceMovingLikeABishop(gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                    {
                        result = true;
                    }
                    break;

                case PieceType.Queen:
                    if (pmv.PieceMovingLikeAQueen(gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                    {
                        result = true;
                    }
                    break;

                case PieceType.King:
                    if (pmv.PieceMovingLikeAKing(gs, gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                    {
                        result = true;
                    }
                    break;

                default:
                    break;
            }
            return result;
        }

        public static bool IsItYourMove(WPFChess.MainWindow wmw, GameState gs)
        {
            var sourcePiece = Utility.WhatPieceIsHere(wmw, gs.array[0], gs.array[1]);
            if ((sourcePiece.Player == Player.White && gs.WM == "WHITE") || (sourcePiece.Player == Player.Black && gs.WM == "BLACK"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPieceAtDestinationLocation(WPFChess.MainWindow wmw, int[] array)
        {
            if (IsPieceAtThisLocation(wmw, array[2], array[3]))
            {
                return true;
            }
            return false;
        }

        public static bool IsPieceAtThisLocation(WPFChess.MainWindow wmw, int x, int y)
        {
            var whatPiece = Utility.WhatPieceIsHere(wmw, x, y);
            if (whatPiece != null)
            {
                return true;
            }
            return false;
        }
    }
}