using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PicrossClone {
    /* Picross Game States */
    public enum PicrossState {
        Normal = 0,
        Paused = 1,
        Finished = 2
    }
    /* Picross Screen
     * Screen behaviour that is shared by any game screen in PicrossClone
     */
    public class PicrossScreen : ConcreteScreen {
        //General Game Objects
        protected ConcreteBoard board;

        //Count stuff
        protected ITileCounter tileCounter;
        protected CountDisplay countDisplay;

        //Mouse position converted to grid point
        protected Point mouseGridPoint;

        //Mouse position vectors
        private Vector2 mousePos;
        private Vector2 prevMousePos;

        //Pause Menu
        Menu pauseMenu;
        bool isGoingToExit;

        //Font set used for the game
        protected FontHolder gameFont;

        private Point lastLeftClickedPoint, lastRightClickedPoint, prevHighlightedPoint;
        private float selectDelay;
        private const float MAX_SELECT_DELAY = 0.1f;

        //Delegate methods
        public delegate void DrawCalls(SpriteBatch _spriteBatch);
        protected DrawCalls drawCalls;
        public delegate void UpdateCalls(GameTime _gameTime);
        private UpdateCalls updateCalls;
        private delegate void LeftSelectActions();
        private LeftSelectActions leftSelectActions;

        public PicrossScreen()
            : base() {
                mousePos = Vector2.Zero;
        }

        public override void Initalize() {
            int boardWidth = 20, boardHeight = 20;
            board = new ConcreteBoard(boardWidth, boardHeight);
            CreateMenus();
            //temporary
            int[,] fakeBoard = new int[boardWidth, boardHeight];
            tileCounter = new BoardTileCounter(fakeBoard);
            CountData[] countDataArr = new CountData[boardWidth + boardHeight];
            for (int i = 0; i < boardWidth; i++) {
                countDataArr[i] = tileCounter.countRow(i);
            }
            for (int i = 0; i < boardHeight; i++) {
                countDataArr[boardWidth + i] = tileCounter.countRow(i);
            }
            countDisplay = new CountDisplay(boardWidth, boardHeight, countDataArr);
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -8));
            ToggleBoardVisibility(true);
        }

        public override void LoadContent(ContentManager _contentManager) {
            base.LoadContent(_contentManager);
            SpriteFont fontToUse = _contentManager.Load<SpriteFont>(@"Fonts/ComicSans");
            gameFont = FontHolder.BuildFontHolder(fontToUse, fontToUse);
            countDisplay.SetFont(fontToUse);
            pauseMenu.SetFonts(gameFont);
        }

        protected virtual void CreateMenus() {
            //create pause menu
            pauseMenu = new Menu();
            pauseMenu.SetPosition(new Vector2(200));
            pauseMenu.SetTitle("PAUSED");
            MenuButton resumeBtn, exitBtn;
            exitBtn.name = "Exit";
            exitBtn.menuAction = ExitGame;
            resumeBtn.name = "Resume";
            resumeBtn.menuAction = Pause;
            pauseMenu.AddMultiple(new MenuButton[] { resumeBtn, exitBtn });
            //temp
            leftSelectActions += LeftSelect;
        }

        protected void ToggleBoardVisibility(bool _expression) {
            if (_expression) {
                drawCalls += countDisplay.Draw;
                drawCalls += board.Draw;
            } else {
                drawCalls -= countDisplay.Draw;
                drawCalls -= board.Draw;
            }
        }

        private void UnhighlightPoint() {
            if (board.getTileType(prevHighlightedPoint.X, prevHighlightedPoint.Y) == 2) board.ChangeTileColor(prevHighlightedPoint.X, prevHighlightedPoint.Y, 0);
        }

        private void setPrevHighlightedCoords(int _xIndex, int _yIndex) {
            prevHighlightedPoint.X = _xIndex;
            prevHighlightedPoint.Y = _yIndex;
        }

        protected void HighlightPoint() {
            if (!isPaused) {
                UnhighlightPoint();
                if (board.getTileType(mouseGridPoint.X, mouseGridPoint.Y) == 0) {
                    board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 2);
                }
                setPrevHighlightedCoords(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        protected void moveGridCursor(int _xDirection, int _yDirection) {
            int newX = mouseGridPoint.X + _xDirection, newY = mouseGridPoint.Y + _yDirection;
            //Checks to see if it can make the move first before making it
            if (board.isInBounds(newX, newY)) {
                mouseGridPoint = prevHighlightedPoint;
                mouseGridPoint.X = newX;
                mouseGridPoint.Y = newY;
                HighlightPoint();
                Console.WriteLine("Now at X: " + mouseGridPoint.X + " and Y: " + mouseGridPoint.Y);
            }
        }

        protected virtual void LeftSelect() { }

        protected virtual void RightSelect() { }

        protected virtual void SelectRelease() { }

        /// <summary>
        /// Pauses the game if it isn't paused.
        /// Unpauses the game if it is paused.
        /// </summary>
        protected virtual void Pause() {
            isPaused = !isPaused;
            if (isPaused) {
                UnhighlightPoint();
                updateCalls += PauseUpdate;
                drawCalls += pauseMenu.DrawMenu;
                leftSelectActions += PauseLeftSelect;
                leftSelectActions -= LeftSelect;
                ToggleBoardVisibility(false);
            } else {
                HighlightPoint();
                updateCalls -= PauseUpdate;
                drawCalls -= pauseMenu.DrawMenu;
                leftSelectActions -= PauseLeftSelect;
                selectDelay = 0.001f;
                ToggleBoardVisibility(true);
            }
        }

        private void PauseLeftSelect() {
            pauseMenu.Select();
        }

        /// <summary>
        /// Updates the Picross Game Screen.
        /// </summary>
        /// <param name="_gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
            //Updating gameboard
            board.Update(_gameTime);
            //If mouse moved, provide an update to board's mouse grid point
            if (!isPaused && (prevMousePos == null || mousePos != prevMousePos)) {
                //Converting mouse position to grid points
                mouseGridPoint = board.getMouseToGridCoords(mousePos + camera.Position);
                //Check for highlight
                if (board.isInBounds(mouseGridPoint.X, mouseGridPoint.Y)) {
                    HighlightPoint();
                }
                //Setting previous mouse position
                prevMousePos = mousePos;
            }
            //Select Delay hack
            /*When unpausing, select is disabled for a little bit of a second
             due to readding the left select action to the delegate left select*/
            if (selectDelay > 0) {
                if (selectDelay >= MAX_SELECT_DELAY) {
                    selectDelay = 0;
                    leftSelectActions += LeftSelect;
                } else {
                    selectDelay += (float)_gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (updateCalls != null) updateCalls(_gameTime);
        }

        private void PauseUpdate(GameTime _gameTime) {
            //Updating pause menu
            pauseMenu.Update(mousePos + camera.Position, false, false);
            if (inputState.Has(InputState.MOVE_UP)) {
                pauseMenu.Move(1);
            } else if (inputState.Has(InputState.MOVE_DOWN)) {
                pauseMenu.Move(-1);
            }
        }

        public override void UpdateMouse(Vector2 _mousePos) {
            //Grabbing mouse position
            mousePos = _mousePos;
        }

        public override bool UpdateInput(int[] _inputState) {
            base.UpdateInput(_inputState);
            isExit = isGoingToExit;
            isGoingToExit = false;
            if (inputState.Has(InputState.START)) {
                Pause();
            }
            if (!isPaused) {
                if (inputState.Has(InputState.MOVE_RIGHT)) {
                    moveGridCursor(1, 0);
                }
                if (inputState.Has(InputState.MOVE_LEFT)) {
                    moveGridCursor(-1, 0);
                }
                if (inputState.Has(InputState.MOVE_UP)) {
                    moveGridCursor(0, -1);
                }
                if (inputState.Has(InputState.MOVE_DOWN)) {
                    moveGridCursor(0, 1);
                }
            }
            //If left select OR left held and mouse is in new grid point (to avoid duplicate clicks)
            if (selectState.Has(SelectState.LEFT_SELECT) || (selectState.Has(SelectState.LEFT_HOLD) && lastLeftClickedPoint != mouseGridPoint)) {
                if (leftSelectActions != null) leftSelectActions();
                lastLeftClickedPoint = mouseGridPoint;
            }
            //If right select OR right held and mouse is in new grid point (to avoid duplicate clicks)
            if (selectState.Has(SelectState.RIGHT_SELECT) || (selectState.Has(SelectState.RIGHT_HOLD) && lastRightClickedPoint != mouseGridPoint)) {
                RightSelect();
                lastRightClickedPoint = mouseGridPoint;
            }
            //If left or right released (to be split later)
            if (selectState.Has(SelectState.LEFT_RELEASE) || selectState.Has(SelectState.RIGHT_RELEASE)) {
                SelectRelease();
            }
            return isExit;
        }

        protected override void EscapeHandle() {
            if (isPaused) {
                isGoingToExit = true;
            } else {
                Pause();
            }
        }

        private void ExitGame() {
            isGoingToExit = true;
        }

        /// <summary>
        /// Draws Picross Game contents, such as the Board and Count Display.
        /// </summary>
        /// <param name="_spriteBatch">Settings for drawing sprites.</param>
        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
            if (drawCalls != null) drawCalls(_spriteBatch);
        }
    }
}
