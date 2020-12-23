using System;

namespace WPFChess.MainFlow

{
    public static class FlowControl
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