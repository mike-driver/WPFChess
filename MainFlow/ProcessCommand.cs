using System;

namespace WPFChess.MainFlow

{
    public static class FlowControl
    {
        public static bool ProcessCommand(GameState gs)
        {
            if (gs.move == "q" || gs.move == "f" || gs.move == "r" || gs.move == "s")
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