using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WPFChess
{
    public partial class MoveValidations
    {
        public static bool GeneralMoveFormatOK(string move)
        {
            const string MoveRegExExpression = "([a-h]{1}[1-8]{1}[a-h]{1}[1-8]{1})";

            if (!Regex.Match(move, MoveRegExExpression).Success)
            {
                return false;
            }

            return true;
        }

        //2 or 3 char
        public static bool ProcessCastle(string move)
        {
            if (move == "00" || move == "000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}