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
        TimeKeeper endTimeKeeper;
        GameTimeTicker endTimeTicker;
        int timeLoseMultiplier;

        //File IO variables
        PuzzleLoader pzLoader;
        System.Windows.Forms.OpenFileDialog fileOpener;

        //Tile Placement Mode enum
        private enum TilePlacementMode { None, Place, Mark, Erase }
        private TilePlacementMode tilePlacementMode = TilePlacementMode.None;

        //Game Messages
        private string currMessage;
        private string loseMessage = "Game over!";

        #region Initalize Methods
        public GameScreen() : base(){
        }

        public override void Initalize() {
            pzLoader = new PuzzleLoader();
            fileOpener = new System.Windows.Forms.OpenFileDialog();
            CreateMenus();
        }

        public override void Start() {
            //Do base Picross Screen stuff
            base.Start();
            //Setting player state to normal
            playerState = PlayerState.Alive;
            //Preparing file open sequence
            fileOpener.Filter = "PicrossClone Puzzle|*.pic";
            fileOpener.Title = "Open puzzle";
            fileOpener.InitialDirectory = Assets.levelFilePath;
            //Execute the file opener window
            try {
                if (fileOpener.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    //Load specified puzzle if user presses OK
                    puzzle = pzLoader.loadPuzzle(fileOpener.FileName);
                } else {
                    //Load default puzzle
                    puzzle = pzLoader.loadPuzzle(@"Content/levels/puzzle_test.pic");
                }
            } catch {
                //Make a fake puzzle and use that instead
                puzzle.name = "Error Handling Puzzle";
                puzzle.puzzle = new int[4, 4];
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        puzzle.puzzle[i, j] = 1;
                    }
                }
            }
            //Create tileCounter object, place the newly loaded puzzle into it
            tileCounter = new BoardTileCounter(puzzle.puzzle);
            //Use tileCounter to count all the rows and columns of the puzzle board, and return all data onto a count display object
            countPuzzle();
            //Create the game board, a board that the user can manipulate
            board = new GameBoard(puzzle.puzzle.GetLength(0), puzzle.puzzle.GetLength(1));
            //Initalize the timeKeeper object at specified time (first parameter is minutes and second parameter is seconds)
            timeKeeper = new TimeKeeper(20, 0);
            //Initalize the timeTicker object, which will take in the timeKeeper and for every second of gameTime that passes, it will increment the time
            timeTicker = new GameTimeTicker();
            timeTicker.SetTimeKeeper(timeKeeper);
            timeTicker.SetIncrement(-1);
            timeTicker.SetEnabled(true);
            //Setting time lose multiplier to 1
            timeLoseMultiplier = 1;
            //Make the board visisble to the player
            ToggleBoardVisibility(true);
            //Add the GameScreen specific draws into the drawCalls delegate method
            drawCalls += GameScreenRunningDraw;
        }

        private void SetUpEndTimer() {
            //Initalize the end timer (keeper + ticker)
            endTimeKeeper = new TimeKeeper(0, 5);
            endTimeTicker = new GameTimeTicker();
            //Set up the end time ticker to start counting down
            endTimeTicker.SetIncrement(-1);
            endTimeTicker.SetTimeKeeper(endTimeKeeper);
            endTimeTicker.SetEnabled(true);
        }
        #endregion

        #region Counting Methods
        private void countPuzzle() {
            //Figuring out the width and height of puzzle board
            //The total amount of columns is equal to the length of the board horizontally
            int totalColumnAmount = puzzle.puzzle.GetLength(0);
            //Likewise, the total amount of rows is equal to the length of the board vertically
            int totalRowAmount = puzzle.puzzle.GetLength(1);
            //Initalizing string array we will be using to store count strings
            CountData[] countDataArr = new CountData[totalColumnAmount + totalRowAmount];
            //Count horizontally (by going through every row)
            for (int i = 0; i < totalRowAmount; i++) {
                countDataArr[i] = tileCounter.countRow(i);
            }
            //Count vertically (by going through every column)
            for (int i = 0; i < totalColumnAmount; i++) {
                countDataArr[i + totalRowAmount] = tileCounter.countCol(i);
            }
            //Create new CountDisplay object and throw the string array into it along with width and height of the puzzle board
            countDisplay = new CountDisplay();
            countDisplay.setData(totalColumnAmount, totalRowAmount, countDataArr);
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -8));
        }
        #endregion

        #region Select Methods
        protected override void LeftSelect() {
            base.LeftSelect();
            if (playerState == PlayerState.Alive) placeTile();
        }

        protected override void RightSelect() {
            if (playerState == PlayerState.Alive) rightClickTile();
        }

        protected override void SelectRelease() {
            tilePlacementMode = TilePlacementMode.None;
        }
        #endregion

        #region Actions
        protected override void Pause(){
            if (playerState == PlayerState.Alive) {
                base.Pause();
                if (isPaused) {
                    drawCalls -= GameScreenRunningDraw;
                } else {
                    drawCalls += GameScreenRunningDraw;
                }
            }
        }

        protected override void moveGridCursor(int _xDirection, int _yDirection) {
            if (playerState == PlayerState.Alive) {
                base.moveGridCursor(_xDirection, _yDirection); //only move grid cursor using arrow keys/control pad if in-game
            }
        }

        protected override void EscapeHandle() {
            if (playerState == PlayerState.Alive) base.EscapeHandle();
            else Global.GlobalMessenger.CallMessage("ReturnToTitle");
        }

        private void WinAction() {
            playerState = PlayerState.Win;
            timeTicker.SetEnabled(false);
            //We got a winner!
            currMessage = puzzle.name;
            //Remove all X marked tiles
            board.ClearTileColor(3);
            //Add the end screen draw and remove the running draw and count display draw
            drawCalls -= GameScreenRunningDraw;
            drawCalls -= countDisplay.Draw;
            drawCalls -= DrawArrows;
            drawCalls += GameScreenEndDraw;
            //Setup the end timer (once it goes off, user will go back to the title screen)
            SetUpEndTimer();
        }

        private void LoseAction() {
            playerState = PlayerState.Dead;
            timeTicker.SetEnabled(false);
            //We got a loser!
            currMessage = loseMessage;
            //Add the end screen draw and remove the running draw, count display draw, AND board draw
            drawCalls -= GameScreenRunningDraw;
            drawCalls -= countDisplay.Draw;
            drawCalls -= board.Draw;
            drawCalls -= DrawArrows;
            drawCalls += GameScreenEndDraw;
            //Setup the end timer (once it goes off, user will go back to the title screen)
            SetUpEndTimer();
        }
        #endregion

        #region Tile Modifying Methods

        private void placeTile() {
            //Checking if corresponding tile on the puzzle int array is a correct piece
            if (checkIfWithinPuzzleConstraints(mouseGridPoint)) {
                if (puzzle.puzzle[mouseGridPoint.X, mouseGridPoint.Y] != 1) {
                    if (playerState == PlayerState.Alive) {
                        timeKeeper.AddTime(-20 * timeLoseMultiplier); //taking away time for guessing incorrectly
                        CheckForGameOver();
                        timeLoseMultiplier++;
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
                WinAction();
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
        #endregion

        #region Check Conditions
        private bool checkIfWithinPuzzleConstraints(Point _gridPoint) {
            return (_gridPoint.X >= 0 && _gridPoint.X < puzzle.puzzle.GetLength(0)
                && _gridPoint.Y >= 0 && _gridPoint.Y < puzzle.puzzle.GetLength(1));
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
        private void CheckForGameOver() {
            if (playerState == PlayerState.Alive && timeKeeper.Minutes <= 0 && timeKeeper.Seconds <= 0) {
                LoseAction();
            }
        }
        private void CheckForTitleScreenReturn() {
            if ((playerState == PlayerState.Win || playerState == PlayerState.Dead) && endTimeKeeper.Minutes <= 0 && endTimeKeeper.Seconds <= 0) {
                Global.GlobalMessenger.CallMessage("ReturnToTitle");
            }
        }
        #endregion

        #region Game Screen Updates
        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
            //Updating the Game Time Keeper
            CheckForGameOver();
            timeTicker.Update(_gameTime);
            //Updating the End Time Keeper
            CheckForTitleScreenReturn();
            if (endTimeTicker != null) {
                endTimeTicker.Update(_gameTime);
            }
        }
        #endregion

        #region Game Screen Draws
        private void GameScreenRunningDraw(SpriteBatch _spriteBatch) {
            timeKeeper.Draw(_spriteBatch, gameFont.BodyFont, Vector2.Zero + camera.Position);
        }
        private void GameScreenEndDraw(SpriteBatch _spriteBatch) {
            _spriteBatch.DrawString(gameFont.BodyFont, currMessage, new Vector2(200,0) + camera.Position, Color.Black);
        }
        #endregion

        #region Game Screen Unload
        public override void UnloadScreen() {
            //Call base Unload first
            base.UnloadScreen();
            //Nullify the timers
            timeKeeper = null;
            timeTicker = null;
            endTimeKeeper = null;
            endTimeTicker = null;
        }
        #endregion
    }
}
