using System;

namespace WPFChess
{
    public partial class Validations
    {
        public static bool ProcessCommand(string move)
        {
            if (move == "q" || move == "f" || move == "r" || move == "s")
            {
                Environment.Exit(0);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}