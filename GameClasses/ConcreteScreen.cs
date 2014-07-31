using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class ConcreteScreen : Screen {
        protected InputHelper inputHelper;
        protected ICamera2D camera;
        protected SelectEventState selectState;
        public ConcreteScreen() {
            inputHelper = InputHelper.Instance;
            Initalize();
        }
        protected virtual void OnSelect(object _sender, EventArgs _e) {
        }
        protected virtual void OnHighlight(object _sender, EventArgs _e) {
        }
        public override void setCamera(Camera2D _cam) {
            camera = _cam;
        }
        public override void Update(GameTime _gameTime) {
            
        }
        public override void Draw(SpriteBatch _spriteBatch) {
            
        }
    }
}
