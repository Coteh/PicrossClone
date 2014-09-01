using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PicrossClone {
    public class TitleScreen : ConcreteScreen {
        Menu titleMenu, makeMenu, currMenu;

        Vector2 mousePos;

        public override void Initalize() {
            titleMenu = new Menu();
            titleMenu.SetPosition(new Vector2(100));
            titleMenu.SetTitle("PicrossClone");
            makeMenu = new Menu();
            makeMenu.SetPosition(new Vector2(100));
            makeMenu.SetTitle("Make a Puzzle");
            currMenu = titleMenu;
        }

        public override void LoadFonts(FontHolder _fontHolder) {
            titleMenu.SetFonts(_fontHolder);
            makeMenu.SetFonts(_fontHolder);
        }

        public void AssignTitleMenuButtons(MenuButton[] _menuBtnArr) {
            titleMenu.AddMultiple(_menuBtnArr);
        }

        public void AssignMakeMenuButtons(MenuButton[] _menuBtnArr) {
            makeMenu.AddMultiple(_menuBtnArr);
        }

        public void SwitchToMakeMenu() {
            currMenu = makeMenu;
        }

        public void SwitchToTitleMenu() {
            currMenu = titleMenu;
        }

        protected override void EscapeHandle() {
            if (currMenu != titleMenu) {
                SwitchToTitleMenu();
            } else {
                isExit = true;
            }
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
            currMenu.Update(mousePos + camera.Position, false, false);
        }

        public override void UpdateMouse(Vector2 _mousePos) {
            mousePos = _mousePos;
        }

        public override bool UpdateInput(int[] _inputState) {
            base.UpdateInput(_inputState);
            //If left select OR left held and mouse is in new grid point (to avoid duplicate clicks)
            if (selectState.Has(SelectState.LEFT_RELEASE)) {
                currMenu.Select();
            }
            if (inputState.Has(InputState.MOVE_UP)) {
                currMenu.Move(1);
            } else if (inputState.Has(InputState.MOVE_DOWN)) {
                currMenu.Move(-1);
            }
            return isExit;
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
            currMenu.DrawMenu(_spriteBatch);
        }
    }
}
