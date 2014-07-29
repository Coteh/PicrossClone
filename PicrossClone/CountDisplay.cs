using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public class CountDisplay {
        int boardWidth, boardHeight, spacing = 16;
        Vector2 leftPosStart, topPosStart;
        string[] countedStrArr;

        public CountDisplay(int _boardWidth, int _boardHeight, string[] _countedStrArr) {
            boardWidth = _boardWidth;
            boardHeight = _boardHeight;
            countedStrArr = _countedStrArr;
            SetPositions(Vector2.Zero, Vector2.Zero);
        }

        public void SetPositions(Vector2 _leftSidePos, Vector2 _topSidePos){
            leftPosStart = _leftSidePos;
            topPosStart = _topSidePos;
        }

        public void Draw(SpriteBatch _spriteBatch) {
            for (int i = 0; i < boardWidth; i++) {
                _spriteBatch.DrawString(Assets.font, countedStrArr[i], new Vector2(leftPosStart.X, leftPosStart.Y + (i * spacing)), Alignment.Left, Color.Black);
            }
            for (int i = 0; i < boardHeight; i++) {
                int colCountLength = countedStrArr[i + boardWidth].Length;
                for (int j = colCountLength - 1; j >= 0; j--) {
                    _spriteBatch.DrawString(Assets.font, countedStrArr[i + boardWidth][j].ToString(), new Vector2(topPosStart.X + (i * spacing), 16 + topPosStart.Y - (spacing * (colCountLength - j))), Alignment.Top, Color.Black);
                }
            }
        }
    }
}
