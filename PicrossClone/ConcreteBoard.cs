using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;

namespace PicrossClone {
    /* Concrete Board
     * Board behaviours that all boards have in common
     */
    public class ConcreteBoard : Board {
        protected int[,] board;
        protected float scale = 1.0f;
        protected int tileWidth = 16, tileHeight = 16;
        protected Rectangle[,] tilesRectArr;
        protected Color[] colorArr;

        protected Point currMousePoint, prevHighlightedPoint, prevClickedPoint;
        protected bool isMouseHeld;

        public int getTileType(int _xIndex, int _yIndex) {
            if (!isInBounds(_xIndex, _yIndex)) return -1;
            return board[_xIndex, _yIndex];
        }

        public Point getMouseToGridCoords(Vector2 _mousePos) {
            Vector2 mousePosRelative = _mousePos;
            Point gridPoint;
            gridPoint.X = (int)(mousePosRelative.X / tileWidth);
            gridPoint.Y = (int)(mousePosRelative.Y / tileHeight);
            return gridPoint;
        }

        public Point getPrevHighlightedPoint() {
            return prevHighlightedPoint;
        }

        public void setPrevHighlightedCoords(int _xIndex, int _yIndex) {
            prevHighlightedPoint.X = _xIndex;
            prevHighlightedPoint.Y = _yIndex;
        }

        public ConcreteBoard(int _gridWidth, int _gridHeight)
            : base() {
            board = new int[_gridWidth, _gridHeight];
            colorArr = new Color[] { Color.White, Color.Black, Color.LightGray, Color.Yellow };
            CalibrateRects();
            clearPrevClickPoint(); //more liek initalize the point
        }

        protected void CalibrateRects() {
            tilesRectArr = new Rectangle[board.GetLength(0), board.GetLength(1)];
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    tilesRectArr[i, j] = new Rectangle((int)((i * (tileWidth * scale))), (int)((j * (tileHeight * scale))), tileWidth, tileHeight);
                }
            }
        }

        private void clearPrevClickPoint() {
            prevClickedPoint = new Point(-1, -1);
        }

        /// <summary>
        /// Change Tile Color by supplying the x and y indexes of the tile
        /// and then the color that is desired.
        /// </summary>
        /// <param name="_xIndex">X Index of Game Borard</param>
        /// <param name="_yIndex">Y Index of Game Board</param>
        /// <param name="_tileColor">Tile color that is desired.</param>
        public void ChangeTileColor(int _xIndex, int _yIndex, int _tileColor) {
            if (isInBounds(_xIndex, _yIndex)) {
                board[_xIndex, _yIndex] = _tileColor;
            }
        }

        public override void Select(InputEventState _selectState) {
            if (currMousePoint == prevClickedPoint) {
                return;
            }
            OnSelect(new InputEventArgs(_selectState));
            prevClickedPoint = currMousePoint;
        }

        public override void Highlight() {
            OnHighlight(EventArgs.Empty);
        }

        public override void Select_Release() {
            clearPrevClickPoint(); //empty out prev clicked point
            OnSelectRelease(EventArgs.Empty);
        }

        public bool isInBounds(int _xIndex, int _yIndex) {
            return (_xIndex >= 0 && _xIndex < board.GetLength(0)
                && _yIndex >= 0 && _yIndex < board.GetLength(1));
        }

        public void UpdateMousePoint(Point _mouseGridPoint) {
            //Assigning the mouse point to passed in point
            currMousePoint = _mouseGridPoint;
            //Check conditions for Highlight function
            if (isInBounds(_mouseGridPoint.X, _mouseGridPoint.Y)) {
                Highlight();
            }
        }

        public void UpdateInput(InputEventState _inputState) {
            //Checking if selected
            if (_inputState != InputEventState.NONE) {
                isMouseHeld = true;
                //Select function
                Select(_inputState);
            } else {
                if (isMouseHeld) {
                    Select_Release();
                    isMouseHeld = false;
                    //Console.WriteLine("Mouse released");
                }
            }
        }

        public override void Update(GameTime _gameTime) {

        }

        public override void Draw(SpriteBatch _spriteBatch) {
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    _spriteBatch.DrawRect(tilesRectArr[i, j], colorArr[board[i, j]]);
                }
            }
        }
    }
}
