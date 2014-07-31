using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PicrossClone {
    public class PaintScreen : ConcreteScreen {
        //General Game Objects
        PaintBoard paintBoard;

        //Height and width of board
        int boardHeight, boardWidth;

        //Puzzle data
        PuzzleData puzzle;

        //Count stuff
        ITileCounter tileCounter;
        CountData[] countDataArr;
        CountDisplay countDisplay;

        //Mouse position converted to grid point
        private Point mouseGridPoint;

        public PaintScreen() : base() {
        }

        public override void Initalize() {
            base.Initalize();
            boardHeight = 16;
            boardWidth = 16;
            puzzle = new PuzzleData();
            puzzle.name = "New Puzzle";
            puzzle.puzzle = new int[boardWidth, boardHeight];
            paintBoard = new PaintBoard(boardWidth, boardHeight);
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
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -4));
            //select events
            paintBoard.OnSelectEvent += new EventHandler(OnSelect);
            paintBoard.OnHighlightEvent += new EventHandler(OnHighlight);
        }

        //private void adjustBoardSize(int _width, int _height) {
        //    boardHeight = _height;
        //    boardWidth = _width;
        //}

        private void calculatePoint(int _x, int _y) {
            countDataArr[_y] = tileCounter.countRow(_y);
            countDataArr[boardHeight + _x] = tileCounter.countCol(_x);
        }

        protected override void OnSelect(object _sender, EventArgs _e) {
            base.OnSelect(_sender, _e);
            SelectEventArgs mouseE = ((SelectEventArgs)_e);
            int valueToUse = 0;
            switch (mouseE.SelectState) {
                case SelectEventState.LEFT_SELECT:
                    valueToUse = 1;
                    break;
                case SelectEventState.MIDDLE_SELECT:
                    break;
                case SelectEventState.RIGHT_SELECT:
                    valueToUse = 0;
                    break;
                default:
                    break;
            }
            paintBoard.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, valueToUse);
            if (tileCounter.update(mouseGridPoint.X, mouseGridPoint.Y, valueToUse)) {
                calculatePoint(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        protected override void OnHighlight(object _sender, EventArgs _e) {
            base.OnHighlight(_sender, _e);
            Point prevHighlightedPoint = paintBoard.getPrevHighlightedPoint();
            if (paintBoard.getTileType(prevHighlightedPoint.X, prevHighlightedPoint.Y) == 2) paintBoard.ChangeTileColor(prevHighlightedPoint.X, prevHighlightedPoint.Y, 0);
            if (paintBoard.isInBounds(mouseGridPoint.X, mouseGridPoint.Y)
                && paintBoard.getTileType(mouseGridPoint.X, mouseGridPoint.Y) == 0) {
                    paintBoard.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 2);
                    paintBoard.setPrevHighlightedCoords(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        public override void Update(GameTime _gameTime) {
            //Grabbing mouse position
            Vector2 mousePos = inputHelper.GetMousePosition();
            //Converting mouse position to grid points
            mouseGridPoint = paintBoard.getMouseToGridCoords(mousePos + camera.Position);
            //Updating paintBoard, providing the mouse grid point as well as select state
            if (inputHelper.CheckForLeftHold()) {
                selectState = SelectEventState.LEFT_SELECT;
            } else if (inputHelper.CheckForRightHold()) {
                selectState = SelectEventState.RIGHT_SELECT;
            } else {
                selectState = SelectEventState.NONE;
            }
            paintBoard.Update(_gameTime, mouseGridPoint, selectState);
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            countDisplay.Draw(_spriteBatch);
            paintBoard.Draw(_spriteBatch);
        }
    }
}
