﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public abstract void LoadContent(ContentManager _contentManager);
        public abstract void Update(GameTime _gameTime);
        public virtual void UpdateMouse(Vector2 _mousePos) { }
        public virtual bool UpdateInput(int[] _inputState) { return false; }
        public abstract void Draw(SpriteBatch _spriteBatch);
    }
}
