using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class TimeKeeper {
        private int minutes, seconds;
        private string timeStr;

        public int Minutes {
            get { return minutes; }
        }

        public int Seconds {
            get { return seconds; }
        }

        public TimeKeeper(int _mins, int _secs) {
            minutes = _mins;
            seconds = _secs;
            convertToString();
        }

        private void convertToString() {
            string minStr = (minutes < 10) ? "0" + minutes : minutes + "";
            string secStr = (seconds < 10) ? "0" + seconds : seconds + "";
            timeStr = minStr + ":" + secStr;
        }

        public void AddTime(int _seconds) {
            if (minutes <= 0 && seconds <= 0) return;
            seconds += _seconds;
            int minutesToAdd = (int)Math.Floor((float)seconds / 60);
            minutes += minutesToAdd;
            seconds -= (60 * minutesToAdd);
            if (minutes < 0) {
                minutes = 0;
                if (seconds > 0) seconds = 0;
            }
            convertToString();
            //Console.WriteLine("Current time: " + timeStr);
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont _font, Vector2 _pos) {
            _spriteBatch.DrawString(_font, timeStr, _pos, Color.Black);
        }
    }
}
