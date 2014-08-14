using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PicrossClone {
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

        //temp
        int inputDelay = 0;
        const int TOTAL_INPUT_DELAY = 25;

        public PicrossScreen()
            : base() {
                mousePos = Vector2.Zero;
                inputActionsDelegate = InGameActions;
        }

        public override void Initalize() {
            int boardWidth = 20, boardHeight = 20;
            board = new ConcreteBoard(boardWidth, boardHeight);
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
            board.OnHighlightEvent += new EventHandler(OnHighlight);
        }

        protected override void OnHighlight(object _sender, EventArgs _e) {
            base.OnHighlight(_sender, _e);
            HighlightPoint();
        }

        protected void InGameActions(InputEventState _state) {
            //switch (_state) {
            //    case InputEventState.MOVE_RIGHT:
            //        moveGridCursor(1, 0);
            //        break;
            //    case InputEventState.MOVE_LEFT:
            //        moveGridCursor(-1, 0);
            //        break;
            //    case InputEventState.MOVE_UP:
            //        moveGridCursor(0, -1);
            //        break;
            //    case InputEventState.MOVE_DOWN:
            //        moveGridCursor(0, 1);
            //        break;
            //    default:
            //        break;
            //}
            switch (_state) {
                case InputEventState.START:
                    Pause();
                    break;
                case InputEventState.LEFT_SELECT:
                    LeftSelect();
                    break;
                case InputEventState.MIDDLE_SELECT:
                    break;
                case InputEventState.RIGHT_SELECT:
                    RightSelect();
                    break;
                default:
                    break;
            }
        }
        protected void PauseActions(InputEventState _state) {
            switch (_state) {
                case InputEventState.START:
                    Pause();
                    break;
            }
        }

        private void UnhighlightPoint() {
            Point prevHighlightedPoint = board.getPrevHighlightedPoint();
            if (board.getTileType(prevHighlightedPoint.X, prevHighlightedPoint.Y) == 2) board.ChangeTileColor(prevHighlightedPoint.X, prevHighlightedPoint.Y, 0);
        }

        protected void HighlightPoint() {
            if (!isPaused) {
                UnhighlightPoint();
                if (board.getTileType(mouseGridPoint.X, mouseGridPoint.Y) == 0) {
                    board.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 2);
                }
                board.setPrevHighlightedCoords(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        protected void moveGridCursor(int _xDirection, int _yDirection) {
            int newX = mouseGridPoint.X + _xDirection, newY = mouseGridPoint.Y + _yDirection;
            //Checks to see if it can make the move first before making it
            if (board.isInBounds(newX, newY)) {
                mouseGridPoint = board.getPrevHighlightedPoint();
                mouseGridPoint.X = newX;
                mouseGridPoint.Y = newY;
                HighlightPoint();
                Console.WriteLine("Now at X: " + mouseGridPoint.X + " and Y: " + mouseGridPoint.Y);
            }
        }

        protected virtual void LeftSelect() { }

        protected virtual void RightSelect() { }

        /// <summary>
        /// Pauses the game if it isn't paused.
        /// Unpauses the game if it is paused.
        /// </summary>
        protected void Pause() {
            isPaused = !isPaused;
            if (isPaused) {
                inputActionsDelegate = PauseActions;
            } else { inputActionsDelegate = InGameActions; }
            UnhighlightPoint();
        }

        /// <summary>
        /// Updates the Picross Game Screen.
        /// </summary>
        /// <param name="_gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
            //Updating gameboard
            board.Update(_gameTime);
            //Grabbing mouse position
            mousePos = inputHelper.GetMousePosition();
            //TEMP-------
            if (inputDelay <= 0) {
                if (inputHelper.CheckForKeyboardHold(Keys.Right)) {
                    moveGridCursor(1, 0);
                    inputDelay = TOTAL_INPUT_DELAY;
                }
                if (inputHelper.CheckForKeyboardHold(Keys.Left)) {
                    moveGridCursor(-1, 0);
                    inputDelay = TOTAL_INPUT_DELAY;
                }
                if (inputHelper.CheckForKeyboardHold(Keys.Up)) {
                    moveGridCursor(0, -1);
                    inputDelay = TOTAL_INPUT_DELAY;
                }
                if (inputHelper.CheckForKeyboardHold(Keys.Down)) {
                    moveGridCursor(0, 1);
                    inputDelay = TOTAL_INPUT_DELAY;
                }
            } else {
                inputDelay -= (int)(_gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            //-----------
            //If mouse moved, provide an update to board's mouse grid point
            if (!isPaused && (prevMousePos == null || mousePos != prevMousePos)) {
                //Converting mouse position to grid points
                mouseGridPoint = board.getMouseToGridCoords(mousePos + camera.Position);
                //Providing mouse grid point update to board
                board.UpdateMousePoint(mouseGridPoint);
                //Setting previous mouse position
                prevMousePos = mousePos;
            }
            //Provide input updates to board
            board.UpdateInput(inputState);
        }

        /// <summary>
        /// Draws Picross Game contents, such as the Board and Count Display.
        /// </summary>
        /// <param name="_spriteBatch">Settings for drawing sprites.</param>
        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
            countDisplay.Draw(_spriteBatch);
            board.Draw(_spriteBatch);
        }
    }
}
