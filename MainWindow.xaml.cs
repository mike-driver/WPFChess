using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

using WPFChess.ValidationsAndProcesses;

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
                    if (MoveValidations.ProcessCastle(gs.move))
                    {
                        Valid.Content = "Valid castle on king side: " + gs.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid move : " + gs.move;
                    }
                    break;

                case 3:
                    if (MoveValidations.ProcessCastle(gs.move))
                    {
                        Valid.Content = "Valid castle on queen side: " + gs.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid move : " + gs.move;
                    }
                    break;

                case 4:
                    if (MoveValidations.GeneralMoveFormatOK(gs.move))
                    {
                        gs.array = MoveProcess.TranslateMove(gs.move);
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

        //4 chars
        public bool ValidateMove(GameState gs)
        {
            if (!IsItYourMove())
                return false;

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
            var whatPiece = Utility.WhatPieceIsHere(this, gs.array[0], gs.array[1]);
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

        public bool IsItYourMove()
        {
            var sourcePiece = Utility.WhatPieceIsHere(this, gs.array[0], gs.array[1]);
            if ((sourcePiece.Player == Player.White && gs.MM == "WHITE") || (sourcePiece.Player == Player.Black && gs.MM == "BLACK"))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            var sourcePiece = Utility.WhatPieceIsHere(this, gs.array[0], gs.array[1]);
            var destinationPiece = Utility.WhatPieceIsHere(this, gs.array[2], gs.array[3]);

            if (sourcePiece.Player != destinationPiece.Player)
            {
                return true;
            }

            return false;
        }

        public bool IsPieceAtThisLocation(int x, int y)
        {
            var whatPiece = Utility.WhatPieceIsHere(this, x, y);
            if (whatPiece != null)
            {
                return true;
            }
            return false;
        }

        public void MovePiece(int xsrc, int ysrc, int xdst, int ydst)
        {
            //get the piece to move
            var pieceToMove = Utility.WhatPieceIsHere(this, xsrc, ysrc);

            //take the piece if any in destination and add it to collection of pieces taken
            var pieceToTake = Utility.WhatPieceIsHere(this, xdst, ydst);  //get the piece to take if any
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

        //test method
        //test method
        public void MakeSomeMoves()
        {
            _ = this.Pieces.ElementAt(6); //test

            MovePiece(1, 7, 2, 5);  //white knight
            MovePiece(1, 0, 2, 2);  //black knight
            MovePiece(4, 4, 2, 2);  //nothing
            _ = Utility.WhatPieceIsHere(this, 1, 1);
        }
    }
}