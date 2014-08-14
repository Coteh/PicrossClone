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

        public PaintScreen() : base() {
        }

        public override void Initalize() {
            boardHeight = 16;
            boardWidth = 16;
            puzzle = new PuzzleData();
            puzzle.name = "New Puzzle";
            puzzle.puzzle = new int[boardWidth, boardHeight];
            board = new PaintBoard(boardWidth, boardHeight);
            //temporary
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
            //select events
            board.OnSelectEvent += new EventHandler(OnSelect);
            board.OnHighlightEvent += new EventHandler(OnHighlight);
        }

        //private void adjustBoardSize(int _width, int _height) {
        //    boardHeight = _height;
        //    boardWidth = _width;
        //}

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
            paintTile(1);
        }

        protected override void RightSelect() {
            paintTile(0);
        }

        protected override void OnSelect(object _sender, EventArgs _e) {
            base.OnSelect(_sender, _e);
            InputEventArgs mouseE = ((InputEventArgs)_e);
            inputActionsDelegate(mouseE.InputState);
        }
    }
}
