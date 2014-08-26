using GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class PaintBoard : ConcreteBoard {

        public PaintBoard(int _gridWidth, int _gridHeight)
            : base(_gridWidth, _gridHeight) {
                
        }

        public void AdjustBoard(int _xMagnitude, int _yMagnitude){
            if (gridWidth + _xMagnitude < 0) return; //can't adjust the board horizontally less than this
            if (gridHeight + _yMagnitude < 0) return; //can't adjust the board vertically less than this
            gridWidth += _xMagnitude;
            gridHeight += _yMagnitude;
            RecalibrateBoard();
        }

        public override void Update(GameTime _gameTime) {
            base.Update(_gameTime);
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
        }
    }
}
