using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class GameTimeTicker : ITimeTicker {
        TimeKeeper timeKeeper;
        int increment;
        bool isOn;

        float secondsCount;
        const float MAX_SECONDS_COUNT = 1.0f;
        public void SetTimeKeeper(TimeKeeper _timeKeeper) {
            timeKeeper = _timeKeeper;
        }
        public void SetIncrement(int _increment) {
            increment = _increment;
        }
        public void SetEnabled(bool _expression) {
            isOn = _expression;
        }
        public void Update(GameTime _gameTime) {
            if (isOn) {
                if (secondsCount >= MAX_SECONDS_COUNT) {
                    if (timeKeeper.Minutes <= 0 && timeKeeper.Seconds <= 0) {
                        SetEnabled(false);
                    } else {
                        if (timeKeeper != null) timeKeeper.AddTime(increment);
                    }
                    secondsCount = 0;
                } else {
                    secondsCount += (float)_gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }
}
