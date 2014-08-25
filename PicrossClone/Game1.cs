#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using GameClasses;
#endregion

namespace PicrossClone {
    /// <summary>
    /// PicrossClone
    /// By James Cote
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        InputManager inputManager;

        //Screen Manager
        ScreenManager screenManager;
        //All the screens in the game
        int titleScreen;
        int gameScreen;
        int createScreen;
        //Refers to the current screen being accessed
        Screen currScreen;

        Camera2D cam;

        public Game1()
            : base() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            inputManager = new InputManager();
            Assets.pixel = new Texture2D(GraphicsDevice, 1, 1);
            Assets.pixel.SetData(new[] { Color.White });

            screenManager = new ScreenManager();
            titleScreen = screenManager.AddScreen(new TitleScreen());
            gameScreen = screenManager.AddScreen(new GameScreen());
            createScreen = screenManager.AddScreen(new PaintScreen());
            currScreen = screenManager.ChangeScreen(createScreen);

            cam = new Camera2D();
            cam.Position = new Vector2(-200, -100);
            currScreen.setCamera(cam);

            //MenuButton playBtn, makeBtn;
            //playBtn.name = "Play";
            //playBtn.menuAction = PlayGame;
            //makeBtn.name = "Make";
            //makeBtn.menuAction = MakePuzzle;
            //((TitleScreen)currScreen).AssignTitleMenuButtons(new MenuButton[] { playBtn, makeBtn });

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Now the screen will have a LoadContent method for
            //loading in fonts and such
            currScreen.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        private void PlayGame() {
            screenManager.ChangeScreen(1);
        }

        private void MakePuzzle() {
            screenManager.ChangeScreen(2);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            inputManager.Update(gameTime);
            currScreen.UpdateMouse(inputManager.MousePosition);
            if (currScreen.UpdateInput(inputManager.InputEnums)) {
                Exit();
            }
            currScreen.Update(gameTime);
            cam.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cam.Transform);
            currScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
