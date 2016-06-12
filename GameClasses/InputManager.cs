using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameClasses {
    public class InputManager {
        /*INSTANCE*/
        private static InputManager instance;
        /*FIELDS*/
        private GamePadState currGamePadState;
        private GamePadState prevGamePadState;
#if(!XBOX)
        private MouseState currMouseState;
        private MouseState prevMouseState;
        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;
#endif
        private PlayerIndex index;

        public static InputManager Instance {
            get {
                if (instance == null) {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        public PlayerIndex GamepadPlayerIndex {
            get { return index; }
            set { index = value; }
        }

        public Vector2 MousePosition { get { return new Vector2(currMouseState.X, currMouseState.Y); } }

        public Vector2 DeltaMousePosition { get { return new Vector2(currMouseState.X - prevMouseState.X, currMouseState.Y - prevMouseState.Y); } }

        public int MouseScrollValue { get { return currMouseState.ScrollWheelValue; } }

        private InputManager() {
            index = PlayerIndex.One;
        }

        public void Update(GameTime _gameTime) {
            if (prevGamePadState == null && currGamePadState == null) {
                prevGamePadState = currGamePadState = GamePad.GetState(index);
            } else {
                prevGamePadState = currGamePadState;
                currGamePadState = GamePad.GetState(index);
            }
            if (prevMouseState == null && currMouseState == null) {
                prevMouseState = currMouseState = Mouse.GetState();
            } else {
                prevMouseState = currMouseState;
                currMouseState = Mouse.GetState();
            }
            if (prevKeyboardState == null && currKeyboardState == null) {
                prevKeyboardState = currKeyboardState = Keyboard.GetState();
            } else {
                prevKeyboardState = currKeyboardState;
                currKeyboardState = Keyboard.GetState();
            }
        }
        //Gamepad Methods
        public bool CheckForGamepadPress(Buttons _button) {
            return (currGamePadState.IsButtonDown(_button) && prevGamePadState.IsButtonUp(_button));
        }
        public bool CheckForGamepadHold(Buttons _button) {
            return (currGamePadState.IsButtonDown(_button) && prevGamePadState.IsButtonDown(_button));
        }
        public bool CheckForGamepadRelease(Buttons _button) {
            return (currGamePadState.IsButtonUp(_button) && prevGamePadState.IsButtonDown(_button));
        }
        //Keyboard Methods
        public bool CheckForKeyboardPress(Keys _key) {
            return (currKeyboardState.IsKeyDown(_key) && prevKeyboardState.IsKeyUp(_key));
        }
        public bool CheckForKeyboardHold(Keys _key) {
            return (currKeyboardState.IsKeyDown(_key) && prevKeyboardState.IsKeyDown(_key));
        }
        public bool CheckForKeyboardRelease(Keys _key) {
            return (currKeyboardState.IsKeyUp(_key) && prevKeyboardState.IsKeyDown(_key));
        }
        public bool CheckForCtrlHold() {
            return (CheckForKeyboardHold(Keys.LeftControl) || CheckForKeyboardHold(Keys.RightControl));
        }
        //Mouse Methods
        public bool CheckForLeftMouseClick() {
            return (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released);
        }
        public bool CheckForLeftMouseHold() {
            return (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed);
        }
        public bool CheckForLeftMouseRelease() {
            return (currMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed);
        }
        public bool CheckForRightMouseClick() {
            return (currMouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released);
        }
        public bool CheckForRightMouseHold() {
            return (currMouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Pressed);
        }
        public bool CheckForRightMouseRelease() {
            return (currMouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed);
        }
    }
}
