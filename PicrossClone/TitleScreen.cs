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
        Menu titleMenu;

        Vector2 mousePos;

        public override void Initalize() {
            titleMenu = new Menu();
            titleMenu.SetPosition(new Vector2(100));
        }

        public override void LoadContent(ContentManager _contentManager) {
            base.LoadContent(_contentManager);
            SpriteFont fontToUse = _contentManager.Load<SpriteFont>(@"Fonts/ComicSans");
            FontHolder titleFont = FontHolder.BuildFontHolder(fontToUse, fontToUse);
            titleMenu.SetFonts(titleFont);
        }

        public override void LoadFonts(FontHolder _fontHolder) {
            titleMenu.SetFonts(_fontHolder);
        }

        public void AssignTitleMenuButtons(MenuButton[] _menuBtnArr) {
            titleMenu.AddMultiple(_menuBtnArr);
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
            titleMenu.Update(mousePos + camera.Position, false, false);
        }

        public override void UpdateMouse(Vector2 _mousePos) {
            mousePos = _mousePos;
        }

        public override bool UpdateInput(int[] _inputState) {
            base.UpdateInput(_inputState);
            //If left select OR left held and mouse is in new grid point (to avoid duplicate clicks)
            if (selectState.Has(SelectState.LEFT_RELEASE)) {
                titleMenu.Select();
            }
            if (inputState.Has(InputState.MOVE_UP)) {
                titleMenu.Move(1);
            } else if (inputState.Has(InputState.MOVE_DOWN)) {
                titleMenu.Move(-1);
            }
            return false;
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
            titleMenu.DrawMenu(_spriteBatch);
        }
    }
}
