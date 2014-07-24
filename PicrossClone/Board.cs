using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public delegate void EventHandler(object _sender, EventArgs _e);
    public abstract class Board {
        public event EventHandler OnSelectEvent, OnHighlightEvent, OnSelectReleaseEvent;
        protected virtual void OnSelect(EventArgs _e) {
            if (OnSelectEvent != null) {
                OnSelectEvent(this, _e);
            }
        }
        protected virtual void OnHighlight(EventArgs _e) {
            if (OnHighlightEvent != null) {
                OnHighlightEvent(this, _e);
            }
        }
        protected virtual void OnSelectRelease(EventArgs _e) {
            if (OnSelectReleaseEvent != null) {
                OnSelectReleaseEvent(this, _e);
            }
        }
        public abstract void Select();
        public abstract void Highlight();
        public abstract void Select_Release();
        public abstract void Update(GameTime _gameTime);
        public abstract void Draw(SpriteBatch _spriteBatch);
    }
}
