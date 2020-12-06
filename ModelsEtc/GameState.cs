namespace WPFChess
{
    public class GameState
    {
        public GameState()
        {
        }

        //whose move is it?
        private string m;

        public string WM
        {
            get { return m; }
            set { m = value; }
        }

        //checks to alow castling moves
        public bool WKM;    //white king has moved

        public bool BKM;    //black king has moved

        public bool WRK;    //white rook moved king side
        public bool WRQ;    //white rook moved queen side
        public bool BRK;    //black rook moved king side
        public bool BRQ;    //black rook moved queen side

        public int[] array = new int[4];
        public string move = "";
    }
}