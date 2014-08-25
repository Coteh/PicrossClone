using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;

namespace PicrossClone {
    public class GameScreen : PicrossScreen {
        //Puzzle to solve
        PuzzleData puzzle;

        //Player variables
        private enum PlayerState { Alive, Dead, Win }
        PlayerState playerState;

        //Time variables
        TimeKeeper timeKeeper;
        GameTimeTicker timeTicker;

        //File IO variables
        PuzzleLoader pzLoader;
        System.Windows.Forms.OpenFileDialog fileOpener;

        //Misc variables
        private enum TilePlacementMode { None, Place, Mark, Erase }
        private TilePlacementMode tilePlacementMode = TilePlacementMode.None;

        public GameScreen() : base(){
        }

        public override void Initalize() {
            pzLoader = new PuzzleLoader();
            fileOpener = new System.Windows.Forms.OpenFileDialog();
            CreateMenus();
        }

        public override void Start() {
            fileOpener.Filter = "PicrossClone Puzzle|*.pic";
            fileOpener.Title = "Open puzzle";
            fileOpener.InitialDirectory = System.IO.Path.GetPathRoot(Environment.SystemDirectory);
            if (fileOpener.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                puzzle = pzLoader.loadPuzzle(fileOpener.FileName);
            } else {
                puzzle = pzLoader.loadPuzzle(@"Content/levels/puzzle_test.pic");
            }
            tileCounter = new BoardTileCounter(puzzle.puzzle);
            countPuzzle();
            board = new GameBoard(puzzle.puzzle.GetLength(0), puzzle.puzzle.GetLength(1));
            timeKeeper = new TimeKeeper(1, 0);
            timeTicker = new GameTimeTicker();
            timeTicker.SetTimeKeeper(timeKeeper);
            timeTicker.SetIncrement(-1);
            timeTicker.SetEnabled(true);
            ToggleBoardVisibility(true);
            drawCalls += GameScreenRunningDraw;
        }

        private bool checkIfWithinPuzzleConstraints(Point _gridPoint) {
            return (_gridPoint.X >= 0 && _gridPoint.X < puzzle.puzzle.GetLength(0)
                && _gridPoint.Y >= 0 && _gridPoint.Y < puzzle.puzzle.GetLength(1));
        }

        private void countPuzzle() {
            //Figuring out the width and height of puzzle board
            int totalTilesHorizontal = puzzle.puzzle.GetLength(0);
            int totalTilesVertical = puzzle.puzzle.GetLength(1);
            //Initalizing string array we will be using to store count strings
            CountData[] countDataArr = new CountData[totalTilesHorizontal + totalTilesVertical];
            //Count horizontally
            for (int i = 0; i < totalTilesHorizontal; i++) {
                countDataArr[i] = tileCounter.countRow(i);
            }
            //Count vertically
            for (int i = 0; i < totalTilesVertical; i++) {
                countDataArr[i + totalTilesHorizontal] = tileCounter.countCol(i);
            }
            //Create new CountDisplay object and throw the string array into it along with width and height of the puzzle board
            countDisplay = new CountDisplay(totalTilesHorizontal, totalTilesVertical, countDataArr);
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -8));
        }

        private bool checkForCompletion() {
            for (int i = 0; i < puzzle.puzzle.GetLength(0); i++) {
                for (int j = 0; j < puzzle.puzzle.GetLength(1); j++) {
                    int puzzleTileColor = puzzle.puzzle[i, j];
                    if (puzzleTileColor != 0) //don't check empty spots in the puzzle
                        if (board.getTileType(i, j) != puzzleTileColor) return false;
                }
            }
            return true;
        }

        protected override void LeftSelect() {
            base.LeftSelect();
            placeTile();
        }

        protected override void RightSelect() {
            rightClickTile();
        }

        protected override void SelectRelease() {
            tilePlacementMode = TilePlacementMode.None;
        }

        protected override void Pause(){
            base.Pause();
            if (isPaused) {
                drawCalls -= GameScreenRunningDraw;
            } else {
                drawCalls += GameScreenRunningDraw;
            }
        }

        private void placeTile() {
            //Checking if corresponding tile on the puzzle int array is a correct piece
            if (checkIfWithinPuzzleConstraints(mouseGridPoint)) {
                if (puzzle.puzzle[mouseGridPoint.X, mouseGridPoint.Y] != 1) {
                    if (playerState == PlayerState.Alive) {
                        timeKeeper.AddTime(-20); //taking away time for guessing incorrectly
                        CheckForGameOver();
                    }
                    return;
                }
            } else {
                return;
            }
            //At this point, the tile is correct and we will make it black.
            board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 1);
            //Checking for Win Condition (All the correct puzzle tiles are filled in)
            if (playerState == PlayerState.Alive && checkForCompletion()) {
                playerState = PlayerState.Win;
                timeTicker.SetEnabled(false);
                Console.WriteLine("We got a winner!");
            }
        }

        private void eraseTile() {
            //Erasing tile at mouseGridPoint location
            board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 0);
        }

        private void markTile() {
            //Marking the tile at mouseGridPoint location
            board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 3);
        }

        private void rightClickTile() {
            if (!checkIfWithinPuzzleConstraints(mouseGridPoint)) return;
            if (tilePlacementMode == TilePlacementMode.None) {
                //When user begins to hold right mouse on the board,
                //We will figure out if the tile that mouse is currently over
                //is erased or not
                int currTile = board.getTileType(mouseGridPoint.X, mouseGridPoint.Y);
                if (currTile <= -1) return;
                else if (currTile != 0 && currTile != 2) tilePlacementMode = TilePlacementMode.Erase;
                else tilePlacementMode = TilePlacementMode.Mark;
            }
            //After user begins holding right mouse button on board, and after we figure out what kind of tiles to place,
            //we place them
            switch (tilePlacementMode) {
                case TilePlacementMode.Mark:
                    markTile();
                    break;
                case TilePlacementMode.Erase:
                    eraseTile();
                    break;
                default:
                    break;
            }
        }

        private void CheckForGameOver() {
            if (playerState == PlayerState.Alive && timeKeeper.Minutes <= 0 && timeKeeper.Seconds <= 0) {
                Console.WriteLine("Game over.");
                playerState = PlayerState.Dead;
            }
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
            CheckForGameOver();
            timeTicker.Update(_gameTime);
        }

        private void GameScreenRunningDraw(SpriteBatch _spriteBatch) {
            timeKeeper.Draw(_spriteBatch, gameFont.BodyFont, new Vector2(300, 100));
        }
    }
}
