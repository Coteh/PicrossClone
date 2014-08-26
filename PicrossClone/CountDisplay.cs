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
        CountData[] countedStrArr;

        SpriteFont font;

        public CountDisplay() {
            SetPositions(Vector2.Zero, Vector2.Zero);
        }

        public void setData(int _boardWidth, int _boardHeight, CountData[] _countedStrArr) {
            boardWidth = _boardWidth;
            boardHeight = _boardHeight;
            countedStrArr = _countedStrArr;
        }

        public void SetPositions(Vector2 _leftSidePos, Vector2 _topSidePos){
            leftPosStart = _leftSidePos;
            topPosStart = _topSidePos;
        }

        public void SetFont(SpriteFont _font) {
            font = _font;
        }

        public void Draw(SpriteBatch _spriteBatch) {
            if (font != null) {
                for (int i = 0; i < boardHeight; i++) {
                    _spriteBatch.DrawString(font, countedStrArr[i].strCountedData, new Vector2(leftPosStart.X, leftPosStart.Y + (i * spacing)), Alignment.Left, Color.Black);
                }
                for (int i = 0; i < boardWidth; i++) {
                    int colCountLength = countedStrArr[i + boardHeight].countedData.Length;
                    for (int j = colCountLength - 1; j >= 0; j--) {
                        _spriteBatch.DrawString(font, "" + countedStrArr[i + boardHeight].countedData[j], new Vector2(topPosStart.X + (i * spacing), 16 + topPosStart.Y - (spacing * (colCountLength - j))), Alignment.Top, Color.Black);
                    }
                }
            }
        }
    }
}
