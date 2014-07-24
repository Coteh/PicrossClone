using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public static class SpriteBatchExtender {
        public static void DrawLine(this SpriteBatch _spriteBatch, Vector2 _pos1, Vector2 _pos2, Color _color, float _stroke) {
            _spriteBatch.Draw(Assets.pixel, _pos1, null, _color, (float)Math.Atan2(_pos2.Y - _pos1.Y, _pos2.X - _pos1.X), new Vector2(0f, Assets.pixel.Height / 2), new Vector2(Vector2.Distance(_pos1, _pos2), _stroke), SpriteEffects.None, 0f);
        }
        public static void DrawRect(this SpriteBatch _spriteBatch, Rectangle _rect, Color _color) {
            _spriteBatch.Draw(Assets.pixel, _rect, _color);
        }
        public static void DrawRect(this SpriteBatch _spriteBatch, Rectangle _rect, Color _color, float _lineStroke) {
            _spriteBatch.Draw(Assets.pixel, _rect, _color);
            _spriteBatch.DrawLine(_rect.Location(), new Vector2(_rect.X + _rect.Width, _rect.Y), Color.Black, _lineStroke);
            _spriteBatch.DrawLine(new Vector2(_rect.X, _rect.Y + _rect.Height), new Vector2(_rect.X + _rect.Width, _rect.Y + _rect.Height), Color.Black, _lineStroke);
            _spriteBatch.DrawLine(_rect.Location(), new Vector2(_rect.X, _rect.Y + +_rect.Height), Color.Black, _lineStroke);
            _spriteBatch.DrawLine(new Vector2(_rect.X + _rect.Width, _rect.Y), new Vector2(_rect.X + _rect.Width, _rect.Y + _rect.Height), Color.Black, _lineStroke);
        }
    }

    public static class RectangleExtender {
        public static Vector2 Location(this Rectangle _rect) {
            return new Vector2(_rect.X, _rect.Y);
        }
    }
}
