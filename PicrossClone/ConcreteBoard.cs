using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;

namespace PicrossClone {
    /* Concrete Board
     * Stores basic board information
     * Contains various publicly accessible methods that help retrieve/set information from/to the board
     */
    public class ConcreteBoard : Board {
        protected int[,] board;
        protected float scale = 1.0f;
        protected int tileWidth = 16, tileHeight = 16, gridWidth = 16, gridHeight = 16;
        protected Rectangle[,] tilesRectArr;
        protected Color[] colorArr;

        /// <summary>
        /// Gets tile type of tile at (X, Y)
        /// </summary>
        /// <param name="_xIndex">X coord of grid tile</param>
        /// <param name="_yIndex">Y coord of grid tile</param>
        /// <returns>Tile type as int.</returns>
        public int getTileType(int _xIndex, int _yIndex) {
            if (!isInBounds(_xIndex, _yIndex)) return -1;
            return board[_xIndex, _yIndex];
        }

        /// <summary>
        /// Gets the grid point relative to mouse coords.
        /// </summary>
        /// <param name="_mousePos">Mouse position in Vector2 format.</param>
        /// <returns>Grid Point relative to mouse position.</returns>
        public Point getMouseToGridCoords(Vector2 _mousePos) {
            Vector2 mousePosRelative = _mousePos;
            Point gridPoint;
            gridPoint.X = (int)(mousePosRelative.X / tileWidth);
            gridPoint.Y = (int)(mousePosRelative.Y / tileHeight);
            return gridPoint;
        }

        public Vector2 getGridCoordToMousePos(int _x, int _y) {
            return new Vector2(_x * tileWidth, _y * tileHeight);
        }

        public ConcreteBoard(int _gridWidth, int _gridHeight)
            : base() {
            gridWidth = _gridWidth;
            gridHeight = _gridHeight;
            Clear();
            colorArr = new Color[] { Color.White, Color.Black, Color.LightGray, Color.Yellow };
            CalibrateRects();
        }

        protected void CalibrateRects() {
            tilesRectArr = new Rectangle[board.GetLength(0), board.GetLength(1)];
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    tilesRectArr[i, j] = new Rectangle((int)((i * (tileWidth * scale))), (int)((j * (tileHeight * scale))), tileWidth, tileHeight);
                }
            }
        }

        public void Clear() {
            board = new int[gridWidth, gridHeight];
            CalibrateRects();
        }

        public void RecalibrateBoard() {
            int[,] newBoard = new int[gridWidth, gridHeight];
            int oldWidth = board.GetLength(0), oldHeight = board.GetLength(1);
            //Copying board values into new version of board (adding 0s where indexes exceed original length)
            for (int i = 0; i < gridWidth; i++) {
                for (int j = 0; j < gridHeight; j++) {
                    newBoard[i, j] = (i < oldWidth && j < oldHeight) ? board[i, j] : 0;
                }
            }
            Clear(); //clear out board and calibrate the rects
            //Copying original board values back into the board
            for (int i = 0; i < gridWidth; i++) {
                for (int j = 0; j < gridHeight; j++) {
                    board[i, j] = (i < gridWidth && j < gridHeight) ? newBoard[i, j] : 0;
                }
            }
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

        /// <summary>
        /// Clears all tiles of a certain color.
        /// </summary>
        /// <param name="_tileColor">Tile color to clear.</param>
        public void ClearTileColor(int _tileColor) {
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    if (board[i, j] == _tileColor) board[i, j] = 0;
                }
            }
        }

        public bool isInBounds(int _xIndex, int _yIndex) {
            return (_xIndex >= 0 && _xIndex < board.GetLength(0)
                && _yIndex >= 0 && _yIndex < board.GetLength(1));
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
