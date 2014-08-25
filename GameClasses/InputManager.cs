using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public enum InputState {
        NONE = 0,
        MOVE_RIGHT = 1,
        MOVE_LEFT = 2,
        MOVE_UP = 4,
        MOVE_DOWN = 8,
        START = 16
    }
    public enum ControlInputs {
        NONE = 0,
        SAVE = 1,
        OPEN = 2,
        NEW = 4,
        ADJUST_RIGHT = 8,
        ADJUST_LEFT = 16,
        ADJUST_UP = 32,
        ADJUST_DOWN = 64
    }
    public enum SelectState {
        NONE = 0,
        LEFT_SELECT = 1,
        RIGHT_SELECT = 2,
        LEFT_HOLD = 4,
        RIGHT_HOLD = 8,
        LEFT_RELEASE = 16,
        RIGHT_RELEASE = 32
    }
    public enum MiscInputs {
        NONE = 0,
        ESCAPE = 1
    }
    
    public class InputManager {
        InputHelper inputHelper;

        InputState inputState;
        ControlInputs controlInputs;
        SelectState selectState;
        MiscInputs miscInputs;

        int inputDelay = 0;
        const int TOTAL_INPUT_DELAY = 100;

        public int[] InputEnums { get { return new int[]{(int)inputState, (int)controlInputs, (int)selectState, (int)miscInputs}; } }

        public Vector2 MousePosition { get { return inputHelper.GetMousePosition();} }

        public InputManager() {
            inputHelper = InputHelper.Instance;
        }

        public void Update(GameTime _gameTime) {
            inputHelper.Update();
            inputState = InputState.NONE;
            controlInputs = ControlInputs.NONE;
            selectState = SelectState.NONE;
            miscInputs = MiscInputs.NONE;
            if (inputHelper.CheckForKeyboardRelease(Keys.Escape)) {
                miscInputs |= MiscInputs.ESCAPE;
            }
            if (inputDelay <= 0) {
                if (inputHelper.CheckForKeyboardHold(Keys.Right)) {
                    inputState |= InputState.MOVE_RIGHT;
                    inputDelay = TOTAL_INPUT_DELAY;
                }
                if (inputHelper.CheckForKeyboardHold(Keys.Left)) {
                    inputState |= InputState.MOVE_LEFT;
                    inputDelay = TOTAL_INPUT_DELAY;
                }
                if (inputHelper.CheckForKeyboardHold(Keys.Up)) {
                    inputState |= InputState.MOVE_UP;
                    inputDelay = TOTAL_INPUT_DELAY;
                }
                if (inputHelper.CheckForKeyboardHold(Keys.Down)) {
                    inputState |= InputState.MOVE_DOWN;
                    inputDelay = TOTAL_INPUT_DELAY;
                }
            } else {
                inputDelay -= (int)(_gameTime.ElapsedGameTime.TotalSeconds * 1000);
            }
            if (inputHelper.CheckForLeftClick() || inputHelper.CheckForKeyboardPress(Keys.Space)) {
                selectState |= SelectState.LEFT_SELECT;
            }
            if (inputHelper.CheckForLeftHold() || inputHelper.CheckForKeyboardHold(Keys.Space)) {
                selectState |= SelectState.LEFT_HOLD;
            }
            if (inputHelper.CheckForRightClick() || inputHelper.CheckForKeyboardPress(Keys.B)) {
                selectState |= SelectState.RIGHT_SELECT;
            }
            if (inputHelper.CheckForRightHold() || inputHelper.CheckForKeyboardHold(Keys.B)) {
                selectState |= SelectState.RIGHT_HOLD;
            }
            if (inputHelper.CheckForLeftRelease() || inputHelper.CheckForKeyboardRelease(Keys.Space)) {
                selectState |= SelectState.LEFT_RELEASE;
            }
            if (inputHelper.CheckForRightRelease() || inputHelper.CheckForKeyboardRelease(Keys.B)) {
                selectState |= SelectState.RIGHT_RELEASE;
            }
            if (inputHelper.CheckForKeyboardRelease(Keys.Enter)) {
                inputState |= InputState.START;
            }
            if (inputHelper.CheckForKeyboardHold(Keys.LeftControl) || inputHelper.CheckForKeyboardHold(Keys.RightControl)) {
                if (inputHelper.CheckForKeyboardRelease(Keys.S)) {
                    controlInputs |= ControlInputs.SAVE;
                } else if (inputHelper.CheckForKeyboardRelease(Keys.O)) {
                    controlInputs |= ControlInputs.OPEN;
                } else if (inputHelper.CheckForKeyboardRelease(Keys.N)) {
                    controlInputs |= ControlInputs.NEW;
                }
                if (inputHelper.CheckForKeyboardRelease(Keys.Right)) {
                    controlInputs |= ControlInputs.ADJUST_RIGHT;
                }
                if (inputHelper.CheckForKeyboardRelease(Keys.Left)) {
                    controlInputs |= ControlInputs.ADJUST_LEFT;
                }
                if (inputHelper.CheckForKeyboardRelease(Keys.Up)) {
                    controlInputs |= ControlInputs.ADJUST_UP;
                }
                if (inputHelper.CheckForKeyboardRelease(Keys.Down)) {
                    controlInputs |= ControlInputs.ADJUST_DOWN;
                }
            }
        }
    }
}
