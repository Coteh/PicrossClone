using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    /* Input Event State
     * Enum for game input
     */
    public enum InputEventState {
        NONE = 0,
        LEFT_SELECT = 1,
        MIDDLE_SELECT = 2,
        RIGHT_SELECT = 4,
        MOVE_LEFT = 8,
        MOVE_RIGHT = 16,
        MOVE_UP = 32,
        MOVE_DOWN = 64,
        START = 128
    }

    /* Input Event Args
     * Inherited from EventArgs; contains Input state information
     * that is then passed on to event handler methods to process
     */
    public class InputEventArgs : EventArgs {
        private InputEventState inputState;
        public InputEventArgs(InputEventState _state) {
            inputState = _state;
        }

        public InputEventState InputState { get { return inputState; } }
    }
}