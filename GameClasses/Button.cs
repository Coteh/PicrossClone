using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class Button {
        /*FIELDS*/
        protected Texture2D[] btnImages;
        protected Texture2D currBtnImage;

        protected Vector2 position;

        protected float scale;

        /*PROPERTIES*/
        public Vector2 Position { get { return position; } set { position = value; } }

        public Rectangle ButtonRect {
            get {
                return new Rectangle((int)(position.X), (int)(position.Y),
                    Convert.ToInt32(currBtnImage.Width * scale), Convert.ToInt32(currBtnImage.Height * scale));
            }
        }

        public int Width { get { return btnImages[0].Width; } }

        public int Height { get { return btnImages[0].Height; } }

        /*CONSTRUCTOR*/
        public Button(Vector2 _pos) {
            position = _pos;
        }

        /*METHODS*/
        public virtual void InitButton(string _btnText) {
            
        }

        public virtual void LoadContent(ContentManager _content) {
            
        }

        public virtual bool UpdateButtonState(Vector2 _mousePos, bool _isLeftClick) {
            if (WithinMouseBounds(_mousePos)) {
                if (_isLeftClick) {
                    return true;
                }
            }
            return false;
        }

        public bool WithinMouseBounds(Vector2 _mousePos) {
            return (_mousePos.X > ButtonRect.X && _mousePos.Y > ButtonRect.Y
                && _mousePos.X < (ButtonRect.X + ButtonRect.Width) && _mousePos.Y < (ButtonRect.Y + ButtonRect.Height));
        }

        public virtual void Draw(SpriteBatch _spriteBatch){
            _spriteBatch.Draw(currBtnImage, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }

    public class StandardButton : Button {

        protected SpriteFont spriteFont;
        protected string btnText;

        public StandardButton(Vector2 _pos, string _btnText)
            : base(_pos) {
                InitButton(_btnText);
        }

        public override void InitButton(string _btnText) {
            btnImages = new Texture2D[3]; //reserving space for 3 textures: button, button_highlighted, and button_pressed
            btnText = _btnText;
            scale = 0.5f;
        }

        public override void LoadContent(ContentManager _content) {
            btnImages[0] = _content.Load<Texture2D>(@"GUI/buttons/Button");
            btnImages[1] = _content.Load<Texture2D>(@"GUI/buttons/Button_Highlighted");
            btnImages[2] = _content.Load<Texture2D>(@"GUI/buttons/Button_Pressed");
            currBtnImage = btnImages[0];
            spriteFont = _content.Load<SpriteFont>(@"Fonts/ComicSans");
        }

        public override bool UpdateButtonState(Vector2 _mousePos, bool _isLeftClick) {
            if (WithinMouseBounds(_mousePos)) {
                if (_isLeftClick) {
                    currBtnImage = btnImages[2];
                } else {
                    currBtnImage = btnImages[1];
                }
            } else {
                currBtnImage = btnImages[0];
            }
            return base.UpdateButtonState(_mousePos, _isLeftClick);
        }

        public override void Draw(SpriteBatch _spriteBatch) {
            base.Draw(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, btnText, new Vector2(position.X + (currBtnImage.Width * scale / 8), position.Y + (currBtnImage.Height * scale / 4)), Color.White);
        }
    }

    public class ImageButton : Button {

        string[] imageNameArr;
        const int AMOUNT_OF_IMAGE_STATES = 3; //stationary, highlighted, pressed
        int amountOfImages = 1; //1 by default
        string[] uniqueImageNameArr;
        int buttonImgIndex = 0;

        public int ButtonImgIndex {
            set {
                buttonImgIndex = (value >= 0 && value <= amountOfImages) ? value : 0;
                currBtnImage = btnImages[buttonImgIndex * AMOUNT_OF_IMAGE_STATES];
            }
        }

        public ImageButton(Vector2 _pos, string _imageName)
            : base(_pos) {
                imageNameArr = new string[3]; //only going to be three images in this case (one for stationary, one for highlighted, and one for pressed), so just make array size of 3
                btnImages = new Texture2D[imageNameArr.Length]; //same for the image array
                uniqueImageNameArr = new string[1]; //only one unique image in this case (whatever _imageName is)
                uniqueImageNameArr[0] = _imageName;
                InitalizeButtonNames();
                scale = 0.1f;
        }

        public ImageButton(Vector2 _pos, string _imageName, string _alternateImageName)
            : base(_pos) {
            imageNameArr = new string[6]; //two different images so make array of 6 (3 for each image)
            btnImages = new Texture2D[imageNameArr.Length]; //same for the image array
            amountOfImages = 2;
            uniqueImageNameArr = new string[amountOfImages];
            uniqueImageNameArr[0] = _imageName;
            uniqueImageNameArr[1] = _alternateImageName;
            InitalizeButtonNames();
            scale = 0.1f;
        }

        void InitalizeButtonNames() {
            for (int i = 0; i < amountOfImages; i++) {
                imageNameArr[(i * AMOUNT_OF_IMAGE_STATES) + 0] = uniqueImageNameArr[i]; //put image name into name array
                imageNameArr[(i * AMOUNT_OF_IMAGE_STATES) + 1] = uniqueImageNameArr[i] + "_Highlighted"; //put highlighted image name into array
                imageNameArr[(i * AMOUNT_OF_IMAGE_STATES) + 2] = uniqueImageNameArr[i] + "_Pressed"; //put pressed image name into array
            }
        }

        public override void LoadContent(ContentManager _content) {
            for (int i = 0; i < imageNameArr.Length; i++) {
                btnImages[i] = _content.Load<Texture2D>(@"GUI/buttons/" + imageNameArr[i]);
            }
            currBtnImage = btnImages[0]; //make first button image current button image by default
        }

        public override bool UpdateButtonState(Vector2 _mousePos, bool _isLeftClick) {
            if (WithinMouseBounds(_mousePos)) {
                if (_isLeftClick) {
                    currBtnImage = btnImages[(buttonImgIndex * AMOUNT_OF_IMAGE_STATES) + 2];
                } else {
                    currBtnImage = btnImages[(buttonImgIndex * AMOUNT_OF_IMAGE_STATES) + 1];
                }
            } else {
                currBtnImage = btnImages[(buttonImgIndex * AMOUNT_OF_IMAGE_STATES) + 0];
            }
            return base.UpdateButtonState(_mousePos, _isLeftClick);
        }
    }
}
