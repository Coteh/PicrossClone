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

        //temp
        PuzzleSaver pzSaver;
        System.Windows.Forms.SaveFileDialog fileSaver;

        public PaintScreen() : base() {
        }

        public override void Initalize() {
            boardHeight = 16;
            boardWidth = 16;
            puzzle = new PuzzleData();
            puzzle.name = "New Puzzle";
            puzzle.puzzle = new int[boardWidth, boardHeight];
            board = new PaintBoard(boardWidth, boardHeight);
            CreateMenus();
            InitalizeCountDisplay();
            pzSaver = new PuzzleSaver();
            fileSaver = new System.Windows.Forms.SaveFileDialog();
            fileSaver.Filter = "PicrossClone Puzzle|*.pic";
            fileSaver.Title = "Save puzzle";
            ToggleBoardVisibility(true);
        }

        private void resetBoard() {
            board.Clear();
            for (int i = 0; i < boardWidth; i++) {
                for (int j = 0; j < boardHeight; j++) {
                    if (tileCounter.Update(i, j, 0)) {
                        calculatePoint(i, j);
                    }
                }
            }
        }

        private void adjustBoardSize(int _xMagnitude, int _yMagnitude) {
            ((PaintBoard)board).AdjustBoard(_xMagnitude, _yMagnitude);
            boardWidth += _xMagnitude;
            boardHeight += _yMagnitude;
            InitalizeCountDisplay();
        }

        private void InitalizeCountDisplay() {
            tileCounter = new BoardTileCounter(puzzle.puzzle);
            countDataArr = new CountData[boardWidth + boardHeight];
            for (int i = 0; i < boardWidth; i++) {
                countDataArr[i] = tileCounter.countRow(i);
            }
            for (int i = 0; i < boardHeight; i++) {
                countDataArr[boardWidth + i] = tileCounter.countRow(i);
            }
            countDisplay = new CountDisplay(boardWidth, boardHeight, countDataArr);
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -8));
        }

        private void calculatePoint(int _x, int _y) {
            countDataArr[_y] = tileCounter.countRow(_y);
            countDataArr[boardHeight + _x] = tileCounter.countCol(_x);
        }

        private void paintTile(int _value) {
            //Change tile color to new value
            board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, _value);
            //Update tile counter to reflect tile change. If the tile change is within board constraints,
            //the method will return true and then tile counter can go ahead and count the row and column
            //associated with this tile
            if (tileCounter.Update(mouseGridPoint.X, mouseGridPoint.Y, _value)) {
                calculatePoint(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        protected override void LeftSelect() {
            base.LeftSelect();
            paintTile(1);
        }

        protected override void RightSelect() {
            paintTile(0);
        }

        public override bool UpdateInput(int[] _inputState) {
            base.UpdateInput(_inputState);
            if (controlInputs.Has(ControlInputs.SAVE)) {
                if (fileSaver.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    pzSaver.savePuzzle(puzzle, fileSaver.FileName);
                }
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
    }
}
