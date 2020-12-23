using System;

namespace WPFChess.ChessCore
{
    public class ConvertMove
    {
        // this class is to convert from this human readable string form "d2d4" on chess board to internal integer array [3,6,3,4]
        // chess board is letters for the files and numbers for the ranks starting at bottom left
        //internal board array is held in integers
        //    0,0 ...... 7,0   file,rank file,rank file,rank  etc.
        //    0,1 ...... 7,1
        //    0,2 ...... 7,2
        //    0,3 ...... 7,3
        //    0,4 ...... 7,4
        //    0,5 ...... 7,5
        //    0,6 ...... 7,6
        //    0,7  1,7  2,7  3,7  4,7  5,7  6,7  7,7 - like this
        // this is how the positions of the pieces are held internally and the only sensible way to calculate valid moves etc.

        public static int[] ToInternalArray(string MovePieceToFrom)
        {
            // example move d2d4 - whites queen pawn advancing 2 spaces forward

            int[] array = new int[4];

            var source = MovePieceToFrom.Substring(0, 2);  //get 'd2' - source square
            var destination = MovePieceToFrom.Substring(2, 2);  //get 'd4' - destination square

            array[0] = GetFileOfSquare(source);                 // 3
            array[1] = GetRankOfSquare(source);                 // 6
            array[2] = GetFileOfSquare(destination);            // 3
            array[3] = GetRankOfSquare(destination);            // 4

            return array; // [3,6,3,4]
        }

        private static int GetFileOfSquare(string square)
        {
            // the files go from left to right a,b,c,d,e,f,g,h which translates to array values of 0,1,2,3,4,5,6,7
            // charcter 'a' has value of 97 so subtract that to get 0 starting index in array
            return (int)Convert.ToChar(square.Substring(0, 1)) - 97;
        }

        private static int GetRankOfSquare(string square)
        {
            // the ranks go from bottom to top of board, 12345678  translates to 76543210 because board is top to bottom in the array
            // so by subtracting it from 8 gives us the correct rank in the array from top
            return 8 - (int.Parse(square.Substring(1, 1)));
        }
    }
}