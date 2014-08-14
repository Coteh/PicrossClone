using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    /* Concrete Screen
     * Screen behaviour that is shared by any game screen
     */
    public class ConcreteScreen : Screen {
        protected InputHelper inputHelper;
        protected ICamera2D camera;
        protected InputEventState inputState;

        protected delegate void InputActions(InputEventState _state);
        protected InputActions inputActionsDelegate;

        //temp
        protected bool isPaused;

        public ConcreteScreen() {
            inputHelper = InputHelper.Instance;
            Initalize();
        }

        /// <summary>
        /// Setting the Screen's Camera2D object
        /// </summary>
        /// <param name="_cam">Camera2D to set.</param>
        public override void setCamera(Camera2D _cam) {
            camera = _cam;
        }

        /// <summary>
        /// Events that happen when selecting something.
        /// </summary>
        protected virtual void OnSelect(object _sender, EventArgs _e) {}

        /// <summary>
        /// Events that happen when highlighting something.
        /// </summary>
        protected virtual void OnHighlight(object _sender, EventArgs _e) {}

        /// <summary>
        /// Events that happen when releasing the left mouse after selecing something.
        /// </summary>
        protected virtual void OnSelectRelease(object _sender, EventArgs _e) {}

        /// <summary>
        /// Updates the Game Screen.
        /// </summary>
        /// <param name="_gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime _gameTime) {
            inputState = InputEventState.NONE;
            if (inputHelper.CheckForLeftHold() || inputHelper.CheckForKeyboardHold(Keys.Space)) {
                inputState = InputEventState.LEFT_SELECT;
            }
            if (inputHelper.CheckForRightHold() || inputHelper.CheckForKeyboardHold(Keys.B)) {
                inputState = InputEventState.RIGHT_SELECT;
            }
            if (inputHelper.CheckForKeyboardRelease(Keys.Enter)) {
                inputState = InputEventState.START;
            }
            
        }

        /// <summary>
        /// Draws Game contents.
        /// </summary>
        /// <param name="_spriteBatch">Settings for drawing sprites.</param>
        public override void Draw(SpriteBatch _spriteBatch) {
            
        }
    }
}
