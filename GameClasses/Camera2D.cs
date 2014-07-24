using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class Camera2D : ICamera2D {
        private Vector2 position;
        public Camera2D()
            : base() {
                Scale = 1.0f;
                Roation = 0.0f;
        }
        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Origin { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public float Roation { get; set; }
        public float Scale { get; set; }
        public Matrix Transform { get; set; }

        public void Update(GameTime _gameTime) {
            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(Roation) *
                Matrix.CreateScale(Scale);
        }
    }
}
