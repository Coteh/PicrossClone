using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework;

namespace PicrossClone {
    public class TitleScreen : ConcreteScreen {
        Menu titleMenu;

        public enum GameToLoad { None, Game, Make }
        GameToLoad gameToLoad;

        public override void Initalize() {
            titleMenu = new Menu();
            MenuButton playBtn, makeBtn;
            playBtn.name = "Play";
            playBtn.menuAction = SetUpForGame;
            makeBtn.name = "Make";
            makeBtn.menuAction = SetUpForMake;
        }

        private void SetUpForGame() {
            gameToLoad = GameToLoad.Game;
        }

        private void SetUpForMake() {
            gameToLoad = GameToLoad.Make;
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
        }
    }
}
