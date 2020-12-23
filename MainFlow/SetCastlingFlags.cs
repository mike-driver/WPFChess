using System;
using System.Collections.Generic;
using System.Text;

using WPFChess.Models;

namespace WPFChess.MainFlow
{
    public partial class Utility
    {
        public static void SetCastlingFlags(WPFChess.MainWindow wmw, int xFile, int yRank, GameState gs)
        {
            var piece = Utility.WhatPieceIsHere(wmw, xFile, yRank);
            if (piece != null)
            {
                //white king has moved
                if (piece.Player == Player.White && piece.Type == PieceType.King)
                    gs.WKM = true;
                //black king has moved
                if (piece.Player == Player.Black && piece.Type == PieceType.King)
                    gs.BKM = true;

                //white rook king side moved
                if (xFile == 7 && yRank == 7)
                    gs.WRK = true;
                //white rook queen side moved
                if (xFile == 0 && yRank == 7)
                    gs.WRQ = true;
                //black rook king side moved
                if (xFile == 7 && yRank == 0)
                    gs.BRK = true;
                //black rook queen side moved
                if (xFile == 0 && yRank == 0)
                    gs.BRQ = true;
            }
        }
    }
}