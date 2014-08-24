using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        protected ICamera2D camera;

        //temp
        protected bool isPaused;
        protected InputState inputState;
        protected ControlInputs controlInputs;
        protected SelectState selectState;
        protected MiscInputs miscInputs;
        protected bool isExit;

        public ConcreteScreen() {
            Initalize();
        }

        /// <summary>
        /// Setting the Screen's Camera2D object
        /// </summary>
        /// <param name="_cam">Camera2D to set.</param>
        public override void setCamera(Camera2D _cam) {
            camera = _cam;
        }

        public override void LoadContent(ContentManager _contentManager) {
            
        }

        /// <summary>
        /// Updates the Game Screen.
        /// </summary>
        /// <param name="_gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime _gameTime) {
        }

        public override bool UpdateInput(int[] _inputState) {
            isExit = false;
            inputState = (InputState)_inputState[0];
            controlInputs = (ControlInputs)_inputState[1];
            selectState = (SelectState)_inputState[2];
            miscInputs = (MiscInputs)_inputState[3];
            if (miscInputs.Has(MiscInputs.ESCAPE)) {
                EscapeHandle();
            }
            return isExit;
        }

        protected virtual void EscapeHandle() {
            isExit = true;
        }

        /// <summary>
        /// Draws Game contents.
        /// </summary>
        /// <param name="_spriteBatch">Settings for drawing sprites.</param>
        public override void Draw(SpriteBatch _spriteBatch) {
            
        }
    }
}
