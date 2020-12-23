using System.Text.RegularExpressions;

namespace WPFChess.ChessCore
{
    public static class CheckMoveRegExpression
    {
        public static bool CheckMoveRegEx(string move)
        {
            const string MoveRegExExpression = "([a-h]{1}[1-8]{1}[a-h]{1}[1-8]{1})";

            if (!Regex.Match(move, MoveRegExExpression).Success)
            {
                return false;
            }

            return true;
        }
    }
}