using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class BoardTileCounter : ITileCounter {
        int[,] board;

        public BoardTileCounter(int[,] _board) {
            board = _board;
        }

        public string countRow(int _row) {
            string countedStr = "";
            int prevCounted = 0, count = 0, rowWidth = board.GetLength(0);
            for (int i = 0; i <= rowWidth; i++) {
                int counted = (i < rowWidth) ? board[i, _row] : 0;
                if (counted != 0) {
                    count++;
                } else if (counted != prevCounted) {
                    countedStr += count;
                    if (i < rowWidth - 1) countedStr += " ";
                    count = 0;
                }
                prevCounted = counted;
            }
            return countedStr;
        }
        public string countCol(int _col) {
            string countedStr = "";
            int prevCounted = 0, count = 0, colHeight = board.GetLength(0);
            for (int i = 0; i <= colHeight; i++) {
                int counted = (i < colHeight) ? board[_col, i] : 0;
                if (counted != 0) {
                    count++;
                } else if (counted != prevCounted) {
                    countedStr += count;
                    if (i < colHeight - 1) countedStr += " ";
                    count = 0;
                }
                prevCounted = counted;
            }
            return countedStr;
        }
    }
}
