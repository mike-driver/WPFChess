using System;
using System.Collections.Generic;
using System.Text;

namespace WPFChess.ValidationsAndProcesses
{
    public class MoveProcess
    {
        public static int[] TranslateMove(string movestr)
        {
            int[] array = new int[4];

            var src = movestr.Substring(0, 2);
            var dst = movestr.Substring(2, 2);

            array[0] = GetFile(src);
            array[1] = GetRank(src);
            array[2] = GetFile(dst);
            array[3] = GetRank(dst);

            return array;
        }

        private static int GetFile(string pos)
        {
            //'a' ... 'h' ----->  1 ..... 8
            // subtract 1 for 0 index
            char letter = Convert.ToChar(pos.Substring(0, 1));
            int F = (int)letter - 96 - 1;
            return F;
        }

        private static int GetRank(string pos)
        {
            // 1 ..... 8  -----> 0 .... 7  .... because the board is the other way around in the array
            // and we subtract 1 for the 0 index start
            int swap = 8 + 1 - (int.Parse(pos.Substring(1, 1)));
            int R = swap - 1;
            return R;
        }
    }
}