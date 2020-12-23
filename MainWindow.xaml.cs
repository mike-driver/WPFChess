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
        public StringBuilder MoveList = new StringBuilder();
        public Collection<ChessPiece> PiecesTaken;

        private int MOVECOUNT = 0;
        private bool WHITESMOVE = true;
        public GameState GS = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            InputBox.Focus();

            this.PiecesTaken = new Collection<ChessPiece>();

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

            _ = Utility.SetInitialGameState();

            WhoseMove.Content = "WHITE to move ...";
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
            MoveListLabel.Content = InputBox.Text;
            GS.move = InputBox.Text;

            InputBox.Clear();
            InputBox.Focus();

            MoveListLabel.Content = MoveList;
            Valid.Content = "";

            switch (GS.move.Length)
            {
                case 1:
                    if (FlowControl.ProcessCommand(GS))
                    {
                        Valid.Content = "Valid command : " + GS.move;
                    }
                    else
                    {
                        Valid.Content = "Invalid command : " + GS.move;
                    }
                    break;

                case 2:
                    if (GS.move.ToLower() == "s1")
                    {
                        Utility.Setup1(this);
                    }
                    if (GS.move.ToLower() == "s2")
                    {
                        Utility.Setup2(this);
                    }
                    break;

                case 4:
                    if (CheckMoveRegExpression.CheckMoveRegEx(GS.move))
                    {
                        GS.array = ConvertMove.ToInternalArray(GS.move);
                        if (GeneralValidations.ValidateMove(this, GS))
                        {
                            Utility.SetCastlingFlags(this, GS.array[0], GS.array[1], GS);
                            Utility.MovePiece(this, GS.array[0], GS.array[1], GS.array[2], GS.array[3]);
                            Utility.MoveCastleWhenCastling(this, GS);

                            MOVECOUNT++;
                            WHITESMOVE = (MOVECOUNT % 2 == 0);
                            MoveList.Append(GS.WM + "-" + GS.move + " ");
                            GS.WM = WHITESMOVE ? "WHITE" : "BLACK";
                            WhoseMove.Content = GS.WM + " to move ...";
                            Valid.Content = "Valid move : " + GS.move;
                        }
                        else
                        {
                            Valid.Content = "Invalid move : " + GS.move;
                        }
                    }
                    else
                    {
                        Valid.Content = "Invalid move : " + GS.move;
                    }
                    break;
            }
        }
    }
}