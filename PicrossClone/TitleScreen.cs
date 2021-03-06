﻿using System;
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
        Vector2 logoPos, bottomTextPos;
        string bottomText;
        SpriteFont titleFont;

        public override void Initalize() {
            titleMenu = new Menu();
            titleMenu.SetPosition(new Vector2(100,180));
            makeMenu = new Menu();
            makeMenu.SetPosition(new Vector2(100,180));
            makeMenu.SetTitle("Make a Puzzle");
            currMenu = titleMenu;
            logoPos = new Vector2(-50, -50);
            bottomText = "2014 James Cote";
            bottomTextPos = new Vector2(50, 350);
        }

        public override void LoadFonts(FontHolder _fontHolder) {
            titleMenu.SetFonts(_fontHolder);
            makeMenu.SetFonts(_fontHolder);
            titleFont = _fontHolder.BodyFont;
        }

        public void AssignTitleMenuButtons(MenuButton[] _menuBtnArr) {
            titleMenu.AddMultiple(_menuBtnArr);
            MenuButton exitBtn;
            exitBtn.name = "Exit";
            exitBtn.menuAction = EscapeHandle;
            titleMenu.Add(exitBtn);
        }

        public void AssignMakeMenuButtons(MenuButton[] _menuBtnArr) {
            makeMenu.AddMultiple(_menuBtnArr);
            MenuButton backBtn;
            backBtn.name = "Back";
            backBtn.menuAction = EscapeHandle;
            makeMenu.Add(backBtn);
        }

        public void SwitchToMakeMenu() {
            currMenu = makeMenu;
            cursor.setCursorPoints(currMenu.GetCurrentMenuItemPosition());
        }

        public void SwitchToTitleMenu() {
            currMenu = titleMenu;
            cursor.setCursorPoints(currMenu.GetCurrentMenuItemPosition());
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
            if (mousePos != prevMousePos) cursor.Update(_gameTime, mousePos + camera.Position);
            prevMousePos = mousePos;
        }

        public override void UpdateMouse(Vector2 _mousePos) {
            base.UpdateMouse(_mousePos);
        }

        public override bool UpdateInput(int[] _inputState) {
            base.UpdateInput(_inputState);
            //If left select OR left held and mouse is in new grid point (to avoid duplicate clicks)
            if (selectState.Has(SelectState.LEFT_RELEASE) || inputState.Has(InputState.START)) {
                currMenu.Select();
            }
            if (inputState.Has(InputState.MOVE_UP)) {
                currMenu.Move(-1);
                cursor.setCursorPoints(currMenu.GetCurrentMenuItemPosition());
            } else if (inputState.Has(InputState.MOVE_DOWN)) {
                currMenu.Move(1);
                cursor.setCursorPoints(currMenu.GetCurrentMenuItemPosition());
            }
            return isExit;
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
            currMenu.DrawMenu(_spriteBatch);
            _spriteBatch.Draw(Assets.logo, logoPos, null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(titleFont, bottomText, bottomTextPos, Color.Black);
        }
    }
}
