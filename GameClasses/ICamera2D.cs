using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public interface ICamera2D {
        Vector2 Position { get; set; }
        Vector2 Origin { get; }
        Vector2 ScreenCenter { get; }
        float Roation { get; set; }
        float Scale { get; set; }
        Matrix Transform { get; }
    }
}
