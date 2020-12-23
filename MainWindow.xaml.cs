using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

using WPFChess.ChessCore;
using WPFChess.MainFlow;
using WPFChess.Models;
using WPFChess.Validations;

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
        private bool whitesMove = true;
        public GameState gs = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            InputBox.Focus();

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
                WM = "WHITE",
                WKM = false,
                BKM = false,
                WRK = false,
                WRQ = false,
                BRK = false,
                BRQ = false
            };

            WhoseMove.Content = "WHITE to move ...";
            // ]
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ProcessMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProcessMove();
        }

        private void ProcessMove()
        {
            MoveList.Content = InputBox.Text;
            gs.move = InputBox.Text;

            if (gs.move.ToLower() == "s1")
            {
                Utility.Setup1(this);
            }
            if (gs.move.ToLower() == "s2")
            {
                Utility.Setup2(this);
            }

            InputBox.Clear();
            InputBox.Focus();

            MoveList.Content = moveList;
            Valid.Content = "";

            switch (gs.move.Length)
            {
                case 1:
                    if (FlowControl.ProcessCommand(gs.move))
                    {
                        Valid.Content = "Valid command : " + gs.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid command : " + gs.move;
                    }
                    break;

                case 4:
                    if (CheckMoveRegExpression.CheckMoveRegEx(gs.move))
                    {
                        gs.array = ConvertMove.ToInternalArray(gs.move);
                        if (GeneralValidations.ValidateMove(this, gs))
                        {
                            Utility.SetCastlingFlags(this, gs.array[0], gs.array[1], gs);
                            Utility.MovePiece(this, gs.array[0], gs.array[1], gs.array[2], gs.array[3]);
                            if (gs.WKCRS)
                            {
                                Utility.MovePiece(this, 7, 7, 5, 7); //move castle over to complete the castle move
                                gs.WKCRS = false; //and reset the flag
                            }
                            if (gs.WKCQS)
                            {
                                Utility.MovePiece(this, 0, 7, 3, 7); //move castle over to complete the castle move
                                gs.WKCQS = false; //and reset the flag
                            }
                            if (gs.BKCRS)
                            {
                                Utility.MovePiece(this, 7, 0, 5, 0); //move castle over to complete the castle move
                                gs.BKCRS = false; //and reset the flag
                            }
                            if (gs.BKCQS)
                            {
                                Utility.MovePiece(this, 0, 0, 3, 0); //move castle over to complete the castle move
                                gs.BKCQS = false; //and reset the flag
                            }
                            moveCnt++;
                            whitesMove = (moveCnt % 2 == 0);
                            moveList.Append(gs.WM + "-" + gs.move + " ");
                            gs.WM = whitesMove ? "WHITE" : "BLACK";
                            WhoseMove.Content = gs.WM + " to move ...";
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
    }
}