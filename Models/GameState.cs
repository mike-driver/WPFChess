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

        public bool WKM = false;    //white king has moved
        public bool BKM = false;    //black king has moved
        public bool WRK = false;    //white rook moved king side
        public bool WRQ = false;    //white rook moved queen side
        public bool BRK = false;    //black rook moved king side
        public bool BRQ = false;    //black rook moved queen side

        //flags when castling to move the castle (rook)

        public bool WKCRS = false;   //white king castling on rook side
        public bool WKCQS = false;   //white king castling on queen side
        public bool BKCRS = false;   //black king castling on rook side
        public bool BKCQS = false;   //black king castling on queen side

        public int[] array = new int[4];
        public string move = "";
    }
}