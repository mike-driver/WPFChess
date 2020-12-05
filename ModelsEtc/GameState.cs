namespace WPFChess
{
    public class GameState
    {
        public GameState()
        {
        }

        private string m;

        public string MM
        {
            get { return m; }
            set { m = value; }
        }

        public int[] array = new int[4];
        public string move = "";
    }
}