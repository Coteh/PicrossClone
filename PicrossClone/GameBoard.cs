using GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class GameBoard : ConcreteBoard {
        public GameBoard(int _gridWidth, int _gridHeight) : base(_gridWidth, _gridHeight) {
            
        }

        public override void Select(SelectEventState _selectState) {
            base.Select(_selectState);
        }

        public override void Highlight() {
            base.Highlight();
        }

        public override void Select_Release() {
            base.Select_Release();
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
        }
    }
}
