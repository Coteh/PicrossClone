using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PicrossClone {
    public class PaintScreen : ConcreteScreen {
        //General Game Objects
        PaintBoard paintBoard;

        //Mouse position converted to grid point
        private Point mouseGridPoint;

        public PaintScreen() : base() {
        }

        public override void Initalize() {
            base.Initalize();
            paintBoard = new PaintBoard(16, 16);
            paintBoard.OnSelectEvent += new EventHandler(OnSelect);
            paintBoard.OnHighlightEvent += new EventHandler(OnHighlight);
        }

        protected override void OnSelect(object _sender, EventArgs _e) {
            base.OnSelect(_sender, _e);
            paintBoard.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 1);
        }

        protected override void OnHighlight(object _sender, EventArgs _e) {
            base.OnHighlight(_sender, _e);
            Point prevHighlightedPoint = paintBoard.getPrevHighlightedPoint();
            if (paintBoard.getTileType(prevHighlightedPoint.X, prevHighlightedPoint.Y) == 2) paintBoard.ChangeTileColor(prevHighlightedPoint.X, prevHighlightedPoint.Y, 0);
            if (paintBoard.isInBounds(mouseGridPoint.X, mouseGridPoint.Y)
                && paintBoard.getTileType(mouseGridPoint.X, mouseGridPoint.Y) == 0) {
                    paintBoard.ChangeTileColor(mouseGridPoint.X, mouseGridPoint.Y, 2);
                    paintBoard.setPrevHighlightedCoords(mouseGridPoint.X, mouseGridPoint.Y);
            }
        }

        public override void Update(GameTime _gameTime) {
            //Grabbing mouse position
            Vector2 mousePos = inputHelper.GetMousePosition();
            //Converting mouse position to grid points
            mouseGridPoint = paintBoard.getMouseToGridCoords(mousePos + camera.Position);
            //Updating paintBoard, providing the mouse grid point as well as left hold check
            paintBoard.Update(_gameTime, mouseGridPoint, inputHelper.CheckForLeftHold());
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            paintBoard.Draw(_spriteBatch);
        }
    }
}
