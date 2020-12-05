using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WPFChess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ChessPiece> Pieces;
        public StringBuilder moveList = new StringBuilder();
        public Collection<ChessPiece> piecesTaken;

        private int moveCnt = 0;
        private bool WhitesMove = true;
        public GameState gs = new GameState();

        public MainWindow()
        {
            InitializeComponent();

            this.piecesTaken = new Collection<ChessPiece>();

            this.Pieces = new ObservableCollection<ChessPiece>() {
                new ChessPiece{Pos=new Point(0, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(1, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(2, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(3, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(4, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(5, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(6, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(7, 6), Type=PieceType.Pawn, Player=Player.White},
                new ChessPiece{Pos=new Point(0, 7), Type=PieceType.Rook, Player=Player.White},
                new ChessPiece{Pos=new Point(1, 7), Type=PieceType.Knight, Player=Player.White},
                new ChessPiece{Pos=new Point(2, 7), Type=PieceType.Bishop, Player=Player.White},
                new ChessPiece{Pos=new Point(3, 7), Type=PieceType.Queen, Player=Player.White},
                new ChessPiece{Pos=new Point(4, 7), Type=PieceType.King, Player=Player.White},
                new ChessPiece{Pos=new Point(5, 7), Type=PieceType.Bishop, Player=Player.White},
                new ChessPiece{Pos=new Point(6, 7), Type=PieceType.Knight, Player=Player.White},
                new ChessPiece{Pos=new Point(7, 7), Type=PieceType.Rook, Player=Player.White},
                new ChessPiece{Pos=new Point(0, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(1, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(2, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(3, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(4, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(5, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(6, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(7, 1), Type=PieceType.Pawn, Player=Player.Black},
                new ChessPiece{Pos=new Point(0, 0), Type=PieceType.Rook, Player=Player.Black},
                new ChessPiece{Pos=new Point(1, 0), Type=PieceType.Knight, Player=Player.Black},
                new ChessPiece{Pos=new Point(2, 0), Type=PieceType.Bishop, Player=Player.Black},
                new ChessPiece{Pos=new Point(3, 0), Type=PieceType.Queen, Player=Player.Black},
                new ChessPiece{Pos=new Point(4, 0), Type=PieceType.King, Player=Player.Black},
                new ChessPiece{Pos=new Point(5, 0), Type=PieceType.Bishop, Player=Player.Black},
                new ChessPiece{Pos=new Point(6, 0), Type=PieceType.Knight, Player=Player.Black},
                new ChessPiece{Pos=new Point(7, 0), Type=PieceType.Rook, Player=Player.Black}
            };

            ChessBoard.ItemsSource = this.Pieces;

            // [ initial state
            gs = new GameState
            {
                MM = "WHITE"
            };
            WhoseMove.Content = "WHITE to move ...";
            // ]
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoveList.Content = InputBox.Text;
            gs.move = InputBox.Text;

            MoveList.Content = moveList;
            Valid.Content = "";

            switch (gs.move.Length)
            {
                case 1:
                    if (Validations.ProcessCommand(gs.move))
                    {
                        Valid.Content = "Valid command : " + gs.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid command : " + gs.move;
                    }
                    break;

                case 2:
                    if (ProcessCastle(gs.move))
                    {
                        Valid.Content = "Valid castle on king side: " + gs.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid move : " + gs.move;
                    }
                    break;

                case 3:
                    if (ProcessCastle(gs.move))
                    {
                        Valid.Content = "Valid castle on queen side: " + gs.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid move : " + gs.move;
                    }
                    break;

                case 4:
                    if (GeneralMoveFormatOK(gs.move))
                    {
                        gs.array = TranslateMove(gs.move);
                        if (ValidateMove(gs))
                        {
                            MovePiece(gs.array[0], gs.array[1], gs.array[2], gs.array[3]);
                            moveCnt++;
                            WhitesMove = (moveCnt % 2 == 0);
                            moveList.Append(gs.MM + "-" + gs.move + " ");
                            gs.MM = WhitesMove ? "WHITE" : "BLACK";
                            WhoseMove.Content = gs.MM + " to move ...";
                            Valid.Content = "Valid move : " + gs.move;
                        }
                        else
                        {
                            Valid.Content = "Invalid move : " + gs.move;
                        }
                    }
                    else
                    {
                        Valid.Content = "Invalid move : " + gs.move;
                    }
                    break;
            }
        }

        //2 or 3 char
        public bool ProcessCastle(string move)
        {
            if (move == "00" || move == "000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //4 chars
        public bool ValidateMove(GameState gs)
        {
            // it is invalid if the source location of the move does not have a piece to move
            if (!IsPieceAtSourceLocation(gs.array))
                return false;

            // it is invalid if the destination of the move has a piece that is not of the opposite colour
            if ((IsPieceAtDestinationLocation(gs.array)) && (!IsPieceAtSourceDestinationLocationOppositeColour(gs)))
                return false;

            if (!IsPieceMovingCorrectly(gs))
                return false;

            return true;
        }

        //////////////////
        public bool IsPieceMovingCorrectly(GameState gs)
        {
            bool result = false;
            var whatPiece = WhatPieceIsHere(gs.array[0], gs.array[1]);
            bool pieceAtDst = IsPieceAtDestinationLocation(gs.array);
            PieceMoveValidations pmv = new PieceMoveValidations();

            switch (whatPiece.Type)
            {
                case PieceType.Pawn:
                    if (whatPiece.Player == Player.White)
                    {
                        if (pmv.PieceMovingLikeAWhitePawn(pieceAtDst, gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (pmv.PieceMovingLikeABlackPawn(pieceAtDst, gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
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
                    if (pmv.PieceMovingLikeAKing(gs.array[0], gs.array[1], gs.array[2], gs.array[3]))
                    {
                        result = true;
                    }
                    break;

                default:
                    break;
            }
            return result;
        }

        public bool IsPieceAtSourceLocation(int[] array)
        {
            if (IsPieceAtThisLocation(array[0], array[1]))
            {
                return true;
            }
            return false;
        }

        public bool IsPieceAtDestinationLocation(int[] array)
        {
            if (IsPieceAtThisLocation(array[2], array[3]))
            {
                return true;
            }
            return false;
        }

        public bool IsPieceAtSourceDestinationLocationOppositeColour(GameState gs)
        {
            var sourcePiece = WhatPieceIsHere(gs.array[0], gs.array[1]);
            var destinationPiece = WhatPieceIsHere(gs.array[2], gs.array[3]);

            if (sourcePiece.Player != destinationPiece.Player)
            {
                return true;
            }

            return false;
        }

        public bool IsPieceAtThisLocation(int x, int y)
        {
            var whatPiece = WhatPieceIsHere(x, y);
            if (whatPiece != null)
            {
                return true;
            }
            return false;
        }

        public bool GeneralMoveFormatOK(string move)
        {
            const string MoveRegExExpression = "([a-h]{1}[1-8]{1}[a-h]{1}[1-8]{1})";

            if (!Regex.Match(move, MoveRegExExpression).Success)
            {
                return false;
            }

            return true;
        }

        public int[] TranslateMove(string movestr)
        {
            int[] array = new int[4];

            var src = movestr.Substring(0, 2);
            var dst = movestr.Substring(2, 2);

            array[0] = GetFile(src);
            array[1] = GetRank(src);
            array[2] = GetFile(dst);
            array[3] = GetRank(dst);

            return array;
        }

        private static int GetFile(string pos)
        {
            //'a' ... 'h' ----->  1 ..... 8
            // subtract 1 for 0 index
            char letter = Convert.ToChar(pos.Substring(0, 1));
            int F = (int)letter - 96 - 1;
            return F;
        }

        private static int GetRank(string pos)
        {
            // 1 ..... 8  -----> 0 .... 7  .... because the board is the other way around in the array
            // and we subtract 1 for the 0 index start
            int swap = 8 + 1 - (int.Parse(pos.Substring(1, 1)));
            int R = swap - 1;
            return R;
        }

        public void MovePiece(int xsrc, int ysrc, int xdst, int ydst)
        {
            //get the piece to move
            var pieceToMove = WhatPieceIsHere(xsrc, ysrc);

            //take the piece if any in destination and add it to collection of pieces taken
            var pieceToTake = WhatPieceIsHere(xdst, ydst);  //get the piece to take if any
            if (pieceToTake != null)                        //if there is a piece here take it
            {
                this.Pieces.Remove(pieceToTake);            //remove the piece from board
                this.piecesTaken.Add(pieceToTake);          //add the piece to the list of taken pieces
            }

            //move the piece - add and remove from the observable location
            ChessPiece moveTo = new ChessPiece
            {
                Player = pieceToMove.Player,
                Type = pieceToMove.Type,
                Pos = new Point(xdst, ydst)     //put the piece here
            };
            this.Pieces.Add(moveTo);
            this.Pieces.Remove(pieceToMove); //remove the piece
        }

        public ChessPiece WhatPieceIsHere(int xRank, int yFile)
        {
            var pieceToMove = this.Pieces.Where(x => x.Pos.X == xRank && x.Pos.Y == yFile).ToList();  //return the piece

            return pieceToMove.FirstOrDefault();
        }

        //test method
        //test method
        public void MakeSomeMoves()
        {
            _ = this.Pieces.ElementAt(6); //test

            MovePiece(1, 7, 2, 5);  //white knight
            MovePiece(1, 0, 2, 2);  //black knight
            MovePiece(4, 4, 2, 2);  //nothing
            _ = WhatPieceIsHere(1, 1);
        }
    }
}