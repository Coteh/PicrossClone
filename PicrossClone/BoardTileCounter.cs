using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class BoardTileCounter : ITileCounter {
        private int[,] board;
        private int boardWidth, boardHeight, maxCountWidth, maxCountHeight;

        int[] currCountedArr;
        int currCountedCursor = 0; //curses through the above int array

        private enum CountAlignment{
            Horizontal = 0,
            Vertical = 1
        }

        public BoardTileCounter(int[,] _board) {
            board = _board;
            boardWidth = board.GetLength(0);
            boardHeight = board.GetLength(1);
            maxCountWidth = boardWidth / 2; //the most amount of counts that the rows can possibly have
            maxCountHeight = boardHeight / 2; //the most amount of counts that the columns can possibly have
        }

        public CountData countRow(int _row) {
            return count(boardWidth, CountAlignment.Horizontal, _row);
        }
        public CountData countCol(int _col) {
            return count(boardWidth, CountAlignment.Vertical, _col);
        }
        private CountData count(int _length, CountAlignment _alignment, int _disposition) {
            CountData countData;
            currCountedCursor = 0;
            //Declaring local variables to use
            int useHorizontal = 0, useVertical = 0, countAmount = 0, prevCounted = 0;
            //Setting up for appropriate alignment
            switch (_alignment) {
                case CountAlignment.Vertical:
                    currCountedArr = new int[boardHeight / 2];
                    useVertical = 1;
                    break;
                case CountAlignment.Horizontal:
                default:
                    currCountedArr = new int[boardWidth / 2];
                    useHorizontal = 1;
                    break;
            }
            //Loop through each value of the row/column
            for (int i = 0; i <= _length; i++) {
                int counted = (i < _length) ? board[(useVertical * _disposition) + (useHorizontal * i), (useHorizontal * _disposition) + (useVertical * i)] : 0;
                if (counted != 0) {
                    countAmount++;
                } else if (counted != prevCounted) {
                    currCountedArr[currCountedCursor] = countAmount; //add counted amount into array
                    currCountedCursor++;
                    countAmount = 0;
                }
                prevCounted = counted;
            }
            //If nothing was counted up to this point
            if (currCountedCursor == 0) {
                countData.countedData = new int[] { 0 }; //finish it off with an int array that just has 0 in it
            } else {
                countData.countedData = dumpIntoIntArr(); //dump the array contents into this array
            }
            countData.strCountedData = turnIntoString(countData.countedData);
            return countData;
        }

        private int[] dumpIntoIntArr() {
            //Dumps contents from the counted int array into this int array, then returns the array
            int[] arrToReturn = new int[currCountedCursor]; //whatever amount currCountedCursor is is the amount of ints we are going to put into this int array
            for (int i = 0; i < currCountedCursor; i++) {
                arrToReturn[i] = currCountedArr[i];
            }
            return arrToReturn;
        }

        private string turnIntoString(int[] _intArr) {
            string strToReturn = "";
            for (int i = 0; i < _intArr.Length; i++) {
                strToReturn += _intArr[i];
                if (i < _intArr.Length) {
                    strToReturn += " ";
                }
            }
            return strToReturn;
        }

        public bool Update(int _xIndex, int _yIndex, int _value) {
            if (_xIndex >= 0 && _xIndex < boardWidth
                && _yIndex >= 0 && _yIndex < boardHeight) {
                board[_xIndex, _yIndex] = _value;
                return true;
            }
            return false;
        }
    }
}
