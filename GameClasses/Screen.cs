using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public abstract class Screen {
        public virtual void Initalize() {
            
        }
        /// <summary>
        /// Events that happen when selecting something on the board
        /// </summary>
        //protected virtual void OnSelect(object _sender, EventArgs _e) {

        //}
        /// <summary>
        /// Events that happen when highlighting something on the board
        /// </summary>
        //protected virtual void OnHighlight(object _sender, EventArgs _e) {

        //}
        /// <summary>
        /// Events that happen when releasing the left mouse after selecing something on the board
        /// </summary>
        //protected virtual void OnSelectRelease(object _sender, EventArgs _e) {

        //}
        public abstract void setCamera(Camera2D _cam);
        public abstract void Update(GameTime _gameTime);
        public abstract void Draw(SpriteBatch _spriteBatch);
    }
}
