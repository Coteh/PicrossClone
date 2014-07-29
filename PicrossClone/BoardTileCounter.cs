using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class BoardTileCounter : ITileCounter {
        private int[,] board;
        private int boardWidth, boardHeight;

        public BoardTileCounter(int[,] _board) {
            board = _board;
            boardWidth = board.GetLength(0);
            boardHeight = board.GetLength(1);
        }

        public string countRow(int _row) {
            string countedStr = "";
            int prevCounted = 0, count = 0;
            for (int i = 0; i <= boardWidth; i++) {
                int counted = (i < boardWidth) ? board[i, _row] : 0;
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
            //If nothing was counted up to this point
            if (countedStr == "") {
                countedStr = "0"; //fill row with 0
            }
            return countedStr;
        }
        public string countCol(int _col) {
            string countedStr = "";
            int prevCounted = 0, count = 0;
            for (int i = 0; i <= boardHeight; i++) {
                int counted = (i < boardHeight) ? board[_col, i] : 0;
                if (counted != 0) {
                    count++;
                } else if (counted != prevCounted) {
                    countedStr += count;
                    count = 0;
                }
                prevCounted = counted;
            }
            //If nothing was counted up to this point
            if (countedStr == "") {
                countedStr = "0"; //fill column with 0
            }
            return countedStr;
        }
        public bool update(int _xIndex, int _yIndex, int _value) {
            if (_xIndex >= 0 && _xIndex < boardWidth
                && _yIndex >= 0 && _yIndex < boardHeight) {
                board[_xIndex, _yIndex] = _value;
                return true;
            }
            return false;
        }
    }
}
