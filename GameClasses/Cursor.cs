using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class Cursor {
        Vector2 pos;
        Vector2 shadowPos;
        int shadowOffset = 70; //how far away should the shadow be from the cursor
        float scale;

        Texture2D[] imageArr;

        public Cursor(Vector2 _pos) {
            imageArr = new Texture2D[2]; //there are two images, regular cursor and shadow cursor
            pos = _pos;
            shadowPos = new Vector2(_pos.X + shadowOffset, _pos.Y + shadowOffset);
            scale = 0.1f;
        }

        public void LoadContent(ContentManager _content) {
            imageArr[0] = _content.Load<Texture2D>(@"GUI/cursor/Cursor");
            imageArr[1] = _content.Load<Texture2D>(@"GUI/cursor/Cursor_Shadow");
        }

        public void setCursorPoints(Vector2 _pos){
            pos = _pos;
            shadowPos = new Vector2(pos.X + (shadowOffset * 2 * scale), pos.Y + (shadowOffset * scale));
        }

        public void Update(GameTime _gameTime, Vector2 _mousePos) {
            setCursorPoints(_mousePos);
        }

        public void Draw(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(imageArr[1], shadowPos, null, Color.White * 0.5f, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            _spriteBatch.Draw(imageArr[0], pos, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
