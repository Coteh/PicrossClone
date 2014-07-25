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
            int prevCounted = 0, count = 0, rowLength = board.GetLength(0);
            for (int i = 0; i <= rowLength; i++) {
                int counted = (i < rowLength) ? board[i, _row] : 0;
                if (counted != 0) {
                    count++;
                } else if (counted != prevCounted) {
                    if (countedStr != "") {
                        countedStr += " ";
                    }
                    countedStr += count;
                    count = 0;
                }
                prevCounted = counted;
            }
            return countedStr;
        }
        public string countCol(int _col) {
            string countedStr = "";
            int prevCounted = 0, count = 0, colLength = board.GetLength(1);
            for (int i = 0; i <= colLength; i++) {
                int counted = (i < colLength) ? board[_col, i] : 0;
                if (counted != 0) {
                    count++;
                } else if (counted != prevCounted) {
                    countedStr += count;
                    count = 0;
                }
                prevCounted = counted;
            }
            return countedStr;
        }
    }
}
