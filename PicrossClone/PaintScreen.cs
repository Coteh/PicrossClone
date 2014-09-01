using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PicrossClone {
    public class PaintScreen : PicrossScreen {
        //Height and width of board
        int boardHeight, boardWidth;

        //Puzzle data
        PuzzleData puzzle;

        //Count stuff
        CountData[] countDataArr;
        CountData blankCount;

        //Puzzle Saver
        PuzzleSaver pzSaver;
        System.Windows.Forms.SaveFileDialog fileSaver;

        //Puzzle Loader
        PuzzleLoader pzLoader;
        System.Windows.Forms.OpenFileDialog fileOpener;

        #region Constructor
        public PaintScreen() : base() {
        }
        #endregion

        #region Initalization
        public override void Initalize() {
            //Initalize the Puzzle Saver object
            pzSaver = new PuzzleSaver();
            fileSaver = new System.Windows.Forms.SaveFileDialog();
            fileSaver.InitialDirectory = Assets.levelFilePath;
            fileSaver.Filter = "PicrossClone Puzzle|*.pic";
            fileSaver.Title = "Save puzzle";
            //Initalize the Puzzle Opener object
            pzLoader = new PuzzleLoader();
            fileOpener = new System.Windows.Forms.OpenFileDialog();
            fileOpener.InitialDirectory = Assets.levelFilePath;
            fileOpener.Filter = "PicrossClone Puzzle|*.pic";
            fileOpener.Title = "Open puzzle";
        }

        private void InitalizeCountDisplay() {
            //Create new CountDisplay object
            countDisplay = new CountDisplay();
            //Set position of count display
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -8));
            //Do the counting
            CountEverything();
        }
        #endregion

        #region Start
        public override void Start() {
            //Base start
            base.Start();
            //Initalize board and puzzle data
            boardHeight = 16;
            boardWidth = 16;
            puzzle = new PuzzleData();
            puzzle.name = "New Puzzle";
            puzzle.puzzle = new int[boardWidth, boardHeight];
            board = new PaintBoard(boardWidth, boardHeight);
            //Create menus
            CreateMenus();
            //Creating blank count
            blankCount.countedData = new int[] { 0 };
            blankCount.strCountedData = "0 ";
            //Initalize the Count Display
            InitalizeCountDisplay();
            //Make board visible
            ToggleBoardVisibility(true);
        }
        #endregion

        #region Board Reset
        private void resetBoard() {
            board.Clear();
            for (int i = 0; i < boardWidth; i++) {
                for (int j = 0; j < boardHeight; j++) {
                    if (tileCounter.Update(i, j, 0)) {
                        countPoint(i, j);
                    }
                }
            }
        }
        #endregion

        #region Board Adjustment
        private void adjustBoardSize(int _xMagnitude, int _yMagnitude) {
            if (boardWidth + _xMagnitude < 2) return; //can't adjust the board horizontally less than this
            if (boardHeight + _yMagnitude < 2) return; //can't adjust the board vertically less than this
            ((PaintBoard)board).AdjustBoard(_xMagnitude, _yMagnitude);
            int[,] oldPuzzle = new int[boardWidth, boardHeight];
            for (int i = 0; i < boardWidth; i++) {
                for (int j = 0; j < boardHeight; j++) {
                    oldPuzzle[i, j] = puzzle.puzzle[i, j];
                }
            }
            int oldWidth = boardWidth, oldHeight = boardHeight;
            boardWidth += _xMagnitude;
            boardHeight += _yMagnitude;
            puzzle.puzzle = new int[boardWidth, boardHeight];
            for (int i = 0; i < boardWidth; i++) {
                for (int j = 0; j < boardHeight; j++) {
                    puzzle.puzzle[i, j] = (i < oldWidth && j < oldHeight) ? oldPuzzle[i, j] : 0;
                }
            }
            if (_xMagnitude < 0 || _yMagnitude < 0) { //if the board was shrunk
                CountEverything(); //recount everything because some of the board blocks may have been lost
            } else { //otherwise... just copy the count data over to a new count data array and fill in the new spots with 0s
                //Create a new instance of tile counter, and put the current puzzle into it
                tileCounter = new BoardTileCounter(puzzle.puzzle);
                CountData[] newCountData = new CountData[boardWidth + boardHeight];
                //Copy all row counts, putting in 0s for new areas
                for (int i = 0; i < boardHeight; i++) {
                    newCountData[i] = (i < oldHeight) ? countDataArr[i] : blankCount;
                }
                for (int i = 0; i < boardWidth; i++) {
                    newCountData[i + boardHeight] = (i < oldWidth) ? countDataArr[i + oldHeight] : blankCount;
                }
                //Create new CountData[] instance for countDataArr and put all values from newCountData into it
                countDataArr = new CountData[boardWidth + boardHeight];
                for (int i = 0; i < countDataArr.Length; i++) {
                    countDataArr[i] = newCountData[i];
                }
                //Set the data to the count display now
                countDisplay.setData(boardWidth, boardHeight, countDataArr);
            }
        }
        #endregion

        #region Counting
        private void CountEverything() {
            //Create a new instance of tile counter, and put the current puzzle into it
            tileCounter = new BoardTileCounter(puzzle.puzzle);
            //Figuring out the width and height of puzzle board
            //The total amount of columns is equal to the length of the board horizontally
            int totalColumnAmount = boardWidth;
            //Likewise, the total amount of rows is equal to the length of the board vertically
            int totalRowAmount = boardHeight;
            //Initalizing string array we will be using to store count strings
            countDataArr = new CountData[totalColumnAmount + totalRowAmount];
            //Count horizontally (by going through every row)
            for (int i = 0; i < totalRowAmount; i++) {
                countDataArr[i] = tileCounter.countRow(i);
            }
            //Count vertically (by going through every column)
            for (int i = 0; i < totalColumnAmount; i++) {
                countDataArr[i + totalRowAmount] = tileCounter.countCol(i);
            }
            //Throw the string array into count display along with width and height of the puzzle board
            countDisplay.setData(totalColumnAmount, totalRowAmount, countDataArr);
        }

        private void countPoint(int _x, int _y) {
            countDataArr[_y] = tileCounter.countRow(_y);
            countDataArr[boardHeight + _x] = tileCounter.countCol(_x);
        }
        #endregion

        #region Paint Tile
        private void paintTile(int _value) {
            //Change tile color to new value
            board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, _value);
            //Update tile counter to reflect tile change. If the tile change is within board constraints,
            //the method will return true and then tile counter can go ahead and count the row and column
            //associated with this tile
            if (tileCounter.Update(mouseGridPoint.X, mouseGridPoint.Y, _value)) {
                countPoint(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }
        #endregion

        #region Select
        protected override void LeftSelect() {
            base.LeftSelect();
            paintTile(1);
        }

        protected override void RightSelect() {
            paintTile(0);
        }
        #endregion

        public void LoadPuzzle() {
            //Take in newly loaded puzzle data from file
            if (fileOpener.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                puzzle = pzLoader.loadPuzzle(fileOpener.FileName);
            }
            //capture old widths and heights
            int oldWidth = boardWidth, oldHeight = boardHeight;
            //set the board width and height to newly loaded puzzle's width and height
            boardWidth = puzzle.puzzle.GetLength(0);
            boardHeight = puzzle.puzzle.GetLength(1);
            //create a fresh new board using the new dimensions
            ((PaintBoard)board).AdjustBoard(boardWidth - oldWidth, boardHeight - oldHeight);
            //fill in all the blocks that are filled in the puzzle
            for (int i = 0; i < boardWidth; i++) {
                for (int j = 0; j < boardHeight; j++) {
                    if (puzzle.puzzle[i,j] == 1) board.ChangeTileColor(i, j, 1);
                }
            }
            //Recount everything now
            CountEverything();
        }

        #region Paint Screen Update
        public override bool UpdateInput(int[] _inputState) {
            base.UpdateInput(_inputState);
            if (controlInputs.Has(ControlInputs.SAVE)) {
                if (fileSaver.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    pzSaver.savePuzzle(puzzle, fileSaver.FileName);
                }
            } else if (controlInputs.Has(ControlInputs.OPEN)){
                LoadPuzzle();
            } else if (controlInputs.Has(ControlInputs.NEW)) {
                resetBoard();
            }
            if (controlInputs.Has(ControlInputs.ADJUST_RIGHT)) {
                adjustBoardSize(1, 0);
            }
            if (controlInputs.Has(ControlInputs.ADJUST_LEFT)) {
                adjustBoardSize(-1, 0);
            }
            if (controlInputs.Has(ControlInputs.ADJUST_UP)) {
                adjustBoardSize(0, -1);
            }
            if (controlInputs.Has(ControlInputs.ADJUST_DOWN)) {
                adjustBoardSize(0, 1);
            }
            return isExit;
        }
        #endregion
    }
}
