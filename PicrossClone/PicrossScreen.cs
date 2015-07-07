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
        //Grid point for the arrows to see
        protected Point mouseGridArrowPoint;

        //Offset vector for the image cursor (used when input methods other than the mouse navigate the grid)
        Vector2 imageCursorOffsetVec;

        //Pause Menu
        Menu pauseMenu;
        protected bool isGoingToExit;

        //Font set used for the game
        protected FontHolder gameFont;

        private Point lastLeftClickedPoint, lastRightClickedPoint, prevHighlightedPoint;
        private float selectDelay;
        private const float MAX_SELECT_DELAY = 0.1f;

        private int holdDownDelay;
        private const int MAX_HOLD_DOWN_DELAY = 700;
        private int moveDelay;
        private const int MAX_MOVE_DELAY = 50;

        //Delegate methods
        public delegate void DrawCalls(SpriteBatch _spriteBatch);
        protected DrawCalls drawCalls;
        public delegate void UpdateCalls(GameTime _gameTime);
        private UpdateCalls updateCalls;
        private delegate void LeftSelectActions();
        private LeftSelectActions leftSelectActions;
        private delegate void RightSelectActions();
        private RightSelectActions rightSelectActions;

        public PicrossScreen()
            : base() {
                mousePos = Vector2.Zero;
                imageCursorOffsetVec = new Vector2(9.4f, 9.4f);
        }

        public override void Initalize() {
            //This Initalize only happens if this class is initalized directly, rather than a subclass
            int boardWidth = 20, boardHeight = 20;
            board = new ConcreteBoard(boardWidth, boardHeight);
            CreateMenus();
            int[,] fakeBoard = new int[boardWidth, boardHeight];
            tileCounter = new BoardTileCounter(fakeBoard);
            CountData[] countDataArr = new CountData[boardWidth + boardHeight];
            for (int i = 0; i < boardWidth; i++) {
                countDataArr[i] = tileCounter.countRow(i);
            }
            for (int i = 0; i < boardHeight; i++) {
                countDataArr[boardWidth + i] = tileCounter.countRow(i);
            }
            countDisplay = new CountDisplay();
            countDisplay.setData(boardWidth, boardHeight, countDataArr);
            countDisplay.SetPositions(new Vector2(-16, 10), new Vector2(6, -8));
            ToggleBoardVisibility(true);
        }

        public override void LoadContent(ContentManager _contentManager) {
            base.LoadContent(_contentManager);
            //nothing here yet
            //maybe take this out?
        }

        public override void LoadFonts(FontHolder _fontHolder) {
            gameFont = _fontHolder;
            countDisplay.SetFont(gameFont.BodyFont);
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
        }

        public override void Start() {
            //setting delegates for left and right selects
            leftSelectActions += LeftSelect;
            rightSelectActions += RightSelect;
        }

        protected void ToggleBoardVisibility(bool _expression) {
            if (_expression) {
                drawCalls += countDisplay.Draw;
                drawCalls += board.Draw;
                drawCalls += DrawArrows;
            } else {
                drawCalls -= countDisplay.Draw;
                drawCalls -= board.Draw;
                drawCalls -= DrawArrows;
            }
        }

        private void setPrevHighlightedCoords(int _xIndex, int _yIndex) {
            prevHighlightedPoint.X = _xIndex;
            prevHighlightedPoint.Y = _yIndex;
        }

        protected void HighlightPoint() {
            if (!isPaused) {
                board.SetHighlightedPoint(new Point(mouseGridPoint.X, mouseGridPoint.Y));
                setPrevHighlightedCoords(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        protected virtual void setGridCursor(Point _mouseGridPoint) {
            //Checks to see if it can make the move first before making it
            if (board.isInBounds(_mouseGridPoint.X, _mouseGridPoint.Y)) {
                mouseGridPoint = _mouseGridPoint;
                HighlightPoint();
                mouseGridArrowPoint = mouseGridPoint; //let the arrows know where we are
                cursor.setCursorPoints(board.getGridCoordToMousePos(mouseGridPoint.X, mouseGridPoint.Y) + imageCursorOffsetVec); //update the mouse cursor as well
            }
        }

        protected virtual void moveGridCursor(int _xDirection, int _yDirection) {
            int newX = prevHighlightedPoint.X + _xDirection, newY = prevHighlightedPoint.Y + _yDirection;
            setGridCursor(new Point(newX, newY));
        }

        protected virtual void LeftSelect() { }

        protected virtual void RightSelect() { }

        protected virtual void SelectRelease() { }

        private void PauseChecks() {
            if (isPaused) {
                updateCalls += PauseUpdate;
                drawCalls += pauseMenu.DrawMenu;
                leftSelectActions -= LeftSelect;
                rightSelectActions -= RightSelect;
                ToggleBoardVisibility(false);
            } else {
                updateCalls -= PauseUpdate;
                drawCalls -= pauseMenu.DrawMenu;
                leftSelectActions += LeftSelect;
                rightSelectActions += RightSelect;
                selectDelay = 0.001f;
                ToggleBoardVisibility(true);
                //Move the image cursor back to where it was before
                cursor.setCursorPoints(board.getGridCoordToMousePos(prevHighlightedPoint.X, prevHighlightedPoint.Y) + imageCursorOffsetVec); //update the mouse cursor to where the currently highlighted grid point is
            }
        }

        /// <summary>
        /// Pauses the game if it isn't paused.
        /// Unpauses the game if it is paused.
        /// </summary>
        protected virtual void Pause() {
            isPaused = !isPaused;
            PauseChecks();
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
                setGridCursor(board.getMouseToGridCoords(mousePos + camera.Position));
                //Updating cursor
                cursor.Update(_gameTime, mousePos + camera.Position);
                //Setting previous mouse position
                prevMousePos = mousePos;
            }
            //Select Delay hack
            /*When unpausing, select is disabled for a little bit of a second
             due to readding the left select action to the delegate left select*/
            if (selectDelay > 0) {
                if (selectDelay >= MAX_SELECT_DELAY) {
                    selectDelay = 0;
                } else {
                    selectDelay += (float)_gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (holdDownDelay > 0) {
                if (holdDownDelay < MAX_HOLD_DOWN_DELAY) {
                    holdDownDelay += (int)(_gameTime.ElapsedGameTime.TotalSeconds * 1000);
                }
            }
            if (moveDelay > 0) {
                if (moveDelay >= MAX_MOVE_DELAY) {
                    moveDelay = 0;
                } else {
                    moveDelay += (int)(_gameTime.ElapsedGameTime.TotalSeconds * 1000);
                }
            }
            if (updateCalls != null) updateCalls(_gameTime);
        }

        private void PauseUpdate(GameTime _gameTime) {
            //Updating pause menu
            pauseMenu.Update(mousePos + camera.Position, false, false);
            if (inputManager.CheckForLeftMouseRelease()) {
                pauseMenu.Select();
            }  
            if (inputManager.CheckForKeyboardPress(Keys.Up)) {
                pauseMenu.Move(-1);
                cursor.setCursorPoints(pauseMenu.GetCurrentMenuItemPosition());
            } else if (inputManager.CheckForKeyboardPress(Keys.Down)) {
                pauseMenu.Move(1);
                cursor.setCursorPoints(pauseMenu.GetCurrentMenuItemPosition());
            }
            //Updating cursor
            if (mousePos != prevMousePos) cursor.Update(_gameTime, mousePos + camera.Position);
        }

        public override void UpdateMouse(Vector2 _mousePos) {
            base.UpdateMouse(_mousePos);
        }

        public override bool UpdateInput() {
            base.UpdateInput();
            isExit = isGoingToExit;
            isGoingToExit = false;
            if (inputManager.CheckForKeyboardPress(Keys.Enter)) {
                Pause();
            }
            if (!isPaused) {
                if (holdDownDelay <= 0) {
                    if (inputManager.CheckForKeyboardPress(Keys.Right)) {
                        moveGridCursor(1, 0);
                        holdDownDelay = 1;
                    }
                    if (inputManager.CheckForKeyboardPress(Keys.Left)) {
                        moveGridCursor(-1, 0);
                        holdDownDelay = 1;
                    }
                    if (inputManager.CheckForKeyboardPress(Keys.Up)) {
                        moveGridCursor(0, -1);
                        holdDownDelay = 1;
                    }
                    if (inputManager.CheckForKeyboardPress(Keys.Down)) {
                        moveGridCursor(0, 1);
                        holdDownDelay = 1;
                    }
                } else if (holdDownDelay >= MAX_HOLD_DOWN_DELAY && moveDelay == 0) {
                    if (inputManager.CheckForKeyboardHold(Keys.Right)) {
                        moveGridCursor(1, 0);
                        moveDelay = 1;
                    }
                    if (inputManager.CheckForKeyboardHold(Keys.Left)) {
                        moveGridCursor(-1, 0);
                        moveDelay = 1;
                    }
                    if (inputManager.CheckForKeyboardHold(Keys.Up)) {
                        moveGridCursor(0, -1);
                        moveDelay = 1;
                    }
                    if (inputManager.CheckForKeyboardHold(Keys.Down)) {
                        moveGridCursor(0, 1);
                        moveDelay = 1;
                    }
                    if (!inputManager.CheckForKeyboardHold(Keys.Up) && !inputManager.CheckForKeyboardHold(Keys.Down) && !inputManager.CheckForKeyboardHold(Keys.Left) && !inputManager.CheckForKeyboardHold(Keys.Right)) {
                        holdDownDelay = 0;
                    }
                }
            }
            //If left select OR left held and mouse is in new grid point (to avoid duplicate clicks)
            if (inputManager.CheckForLeftMouseClick() || inputManager.CheckForKeyboardPress(Keys.Space) || ((inputManager.CheckForLeftMouseHold() || inputManager.CheckForKeyboardHold(Keys.Space)) && lastLeftClickedPoint != mouseGridPoint)) {
                if (leftSelectActions != null) leftSelectActions();
                lastLeftClickedPoint = mouseGridPoint;
            }
            //If right select OR right held and mouse is in new grid point (to avoid duplicate clicks)
            if (inputManager.CheckForRightMouseClick() || (inputManager.CheckForRightMouseHold() && lastRightClickedPoint != mouseGridPoint)) {
                if (rightSelectActions != null) rightSelectActions();
                lastRightClickedPoint = mouseGridPoint;
            }
            //If left or right released (to be split later)
            if (inputManager.CheckForLeftMouseRelease() || inputManager.CheckForRightMouseRelease()) {
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

        protected void DrawArrows(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(Assets.arrow, Vector2.Zero - new Vector2(20, 0 - (mouseGridArrowPoint.Y * 16)), null, Color.White, 0.0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0.0f);
            _spriteBatch.Draw(Assets.arrow, Vector2.Zero - new Vector2(-16 - (mouseGridArrowPoint.X * 16), 16), null, Color.White, (float)Math.PI / 2, Vector2.Zero, 0.1f, SpriteEffects.None, 0.0f);
        }

        public override void UnloadScreen() {
            //Run base unload methods
            base.UnloadScreen();
            //Run pause check, get rid of pause menu and such
            PauseChecks();
            //Set arrow point back to 0
            mouseGridArrowPoint = Point.Zero;
            //Nullify delegate methods
            leftSelectActions = null;
            rightSelectActions = null;
            drawCalls = null;
            updateCalls = null;
        }
    }
}
