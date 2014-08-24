using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public interface ITimeTicker {
        void Update(GameTime _gameTime);
    }
}
