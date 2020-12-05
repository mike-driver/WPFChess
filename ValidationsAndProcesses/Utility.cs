using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFChess.ValidationsAndProcesses
{
    public class Utility
    {
        public static ChessPiece WhatPieceIsHere(WPFChess.MainWindow wmw, int xRank, int yFile)
        {
            var pieceToMove = wmw.Pieces.Where(x => x.Pos.X == xRank && x.Pos.Y == yFile).ToList();  //return the piece

            return pieceToMove.FirstOrDefault();
        }
    }
}