using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    /* Abstract Screen
     * The parent Screen object that all Screens inherit from
     */
    public abstract class Screen {
        public virtual void Initalize() {
            
        }
        public abstract void setCamera(Camera2D _cam);
        public abstract void Update(GameTime _gameTime);
        public abstract void Draw(SpriteBatch _spriteBatch);
    }
}
