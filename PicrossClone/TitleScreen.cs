using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework;

namespace PicrossClone {
    public class TitleScreen : ConcreteScreen {
        Menu titleMenu;

        public override void Initalize() {
            titleMenu = new Menu();
        }

        public void AssignTitleMenuButtons(MenuButton[] _menuBtnArr) {
            titleMenu.AddMultiple(_menuBtnArr);
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
        }
    }
}
