﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;

namespace PicrossClone {
    public class GameScreen : ConcreteScreen {
        //General Game Objects
        GameBoard gameBoard;
        PuzzleLoader puzzleLoader;

        //Puzzle to solve
        PuzzleData puzzle;
        ITileCounter tileCounter;
        CountData[] countDataArr;

        //Mouse position converted to grid point
        private Point mouseGridPoint;

        //Player variables
        const int STARTING_LIVES = 3;
        int lives = STARTING_LIVES;

        public GameScreen() : base(){
        }

        public override void Initalize() {
            base.Initalize();
            puzzleLoader = new PuzzleLoader();
            puzzle = puzzleLoader.loadPuzzle(@"Content/levels/puzzle_test.pic");
            tileCounter = new BoardTileCounter(puzzle.puzzle);
            countPuzzle();
            gameBoard = new GameBoard(puzzle.puzzle.GetLength(0), puzzle.puzzle.GetLength(1));
            gameBoard.OnSelectEvent += new EventHandler(OnSelect);
            gameBoard.OnHighlightEvent += new EventHandler(OnHighlight);
        }

        private bool checkIfWithinPuzzleConstraints(Point _gridPoint) {
            return (_gridPoint.X >= 0 && _gridPoint.X < puzzle.puzzle.GetLength(0)
                && _gridPoint.Y >= 0 && _gridPoint.Y < puzzle.puzzle.GetLength(1));
        }

        private void countPuzzle() {
            int totalTilesHorizontal = puzzle.puzzle.GetLength(0);
            int totalTilesVertical = puzzle.puzzle.GetLength(1);
            countDataArr = new CountData[totalTilesHorizontal + totalTilesVertical];
            for (int i = 0; i < totalTilesHorizontal; i++) {
                countDataArr[i].countedStr = tileCounter.countRow(i);
                countDataArr[i].amtOfSpaces = countDataArr[i].countedStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
            }
            for (int i = 0; i < totalTilesVertical; i++) {
                countDataArr[i + totalTilesHorizontal].countedStr = tileCounter.countCol(i);
                countDataArr[i].amtOfSpaces = countDataArr[i].countedStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
            }
        }

        private bool checkForCompletion() {
            for (int i = 0; i < puzzle.puzzle.GetLength(0); i++) {
                for (int j = 0; j < puzzle.puzzle.GetLength(1); j++) {
                    int puzzleTileColor = puzzle.puzzle[i, j];
                    if (puzzleTileColor != 0) //don't check empty spots in the puzzle
                        if (gameBoard.getTileType(i, j) != puzzleTileColor) return false;
                }
            }
            return true;
        }

        protected override void OnSelect(object _sender, EventArgs _e) {
            base.OnSelect(_sender, _e);
            //Checking if corresponding tile on the puzzle int array is a correct piece
            if (checkIfWithinPuzzleConstraints(mouseGridPoint)) {
                if (puzzle.puzzle[mouseGridPoint.X, mouseGridPoint.Y] != 1) {
                    lives--;
                    return;
                }
            } else {
                return;
            }
            //At this point, the tile is correct and we will make it black.
            gameBoard.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 1);
            if (checkForCompletion()) {
                Console.WriteLine("We got a winner!");
            }
        }

        protected override void OnHighlight(object _sender, EventArgs _e) {
            base.OnHighlight(_sender, _e);
            Point prevHighlightedPoint = gameBoard.getPrevHighlightedPoint();
            if (gameBoard.getTileType(prevHighlightedPoint.X, prevHighlightedPoint.Y) == 2) gameBoard.ChangeTileColor(prevHighlightedPoint.X, prevHighlightedPoint.Y, 0);
            if (gameBoard.isInBounds(mouseGridPoint.X, mouseGridPoint.Y)
                && gameBoard.getTileType(mouseGridPoint.X, mouseGridPoint.Y) == 0) {
                    gameBoard.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 2);
                    gameBoard.setPrevHighlightedCoords(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        public override void Update(GameTime _gameTime) {
            //Grabbing mouse position
            Vector2 mousePos = inputHelper.GetMousePosition();
            //Converting mouse position to grid points
            mouseGridPoint = gameBoard.getMouseToGridCoords(mousePos + camera.Position);
            //Updating gameboard, providing the mouse grid point as well as left hold check
            gameBoard.Update(_gameTime, mouseGridPoint, inputHelper.CheckForLeftHold());
        }
        public override void Draw(SpriteBatch _spriteBatch) {
            for (int i = 0; i < puzzle.puzzle.GetLength(0); i++) {
                _spriteBatch.DrawString(Assets.font, countDataArr[i].countedStr, new Vector2(-16 - (countDataArr[i].amtOfSpaces * 15), -4 + (i * 16)), Color.Black);
            }
            gameBoard.Draw(_spriteBatch);
        }
    }
}