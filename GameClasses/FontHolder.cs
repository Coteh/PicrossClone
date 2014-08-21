using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class FontHolder {
        private SpriteFont titleFont;
        private SpriteFont bodyFont;

        public SpriteFont TitleFont { get { return titleFont; } set { titleFont = value; } }
        public SpriteFont BodyFont { get { return bodyFont; } set { bodyFont = value; } }

        public static FontHolder BuildFontHolder(SpriteFont _titleFont, SpriteFont _bodyFont) {
            FontHolder fontHolder = new FontHolder();
            fontHolder.titleFont = _titleFont;
            fontHolder.bodyFont = _bodyFont;
            return fontHolder;
        }
    }
}
