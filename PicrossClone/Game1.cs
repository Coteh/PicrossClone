#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
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

        //Fonts used by the game
        SpriteFont gameFont;
        FontHolder gameFontHolder;

        //Camera object
        Camera2D cam;

        //Cursor object
        Cursor cursor;

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
            //Initalizing Input Manager
            inputManager = InputManager.Instance;
            //Initalizing static assets
            Assets.pixel = new Texture2D(GraphicsDevice, 1, 1);
            Assets.pixel.SetData(new[] { Color.White });
            Assets.levelFilePath = System.Windows.Forms.Application.StartupPath + "\\" + Content.RootDirectory + "\\" + "levels";

            screenManager = new ScreenManager();
            titleScreen = screenManager.AddScreen(new TitleScreen());
            gameScreen = screenManager.AddScreen(new GameScreen());
            createScreen = screenManager.AddScreen(new PaintScreen());
            currScreen = screenManager.ChangeScreen(titleScreen);

            cam = new Camera2D();
            cam.Position = new Vector2(-200, -100);
            currScreen.setCamera(cam);

            cursor = new Cursor(Vector2.Zero + cam.Position);
            screenManager.setCursorToScreens(cursor);

            MenuButton playBtn, makeBtn, makeNewBtn, makeLoadBtn;
            playBtn.name = "Play";
            playBtn.menuAction = PlayGame;
            makeBtn.name = "Make";
            makeBtn.menuAction = ((TitleScreen)currScreen).SwitchToMakeMenu;
            makeNewBtn.name = "Start a new puzzle";
            makeNewBtn.menuAction = MakePuzzle;
            makeLoadBtn.name = "Load a puzzle";
            makeLoadBtn.menuAction = LoadPuzzleToMake;
            ((TitleScreen)currScreen).AssignTitleMenuButtons(new MenuButton[] { playBtn, makeBtn });
            ((TitleScreen)currScreen).AssignMakeMenuButtons(new MenuButton[] { makeNewBtn, makeLoadBtn });

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load in Game Logo
            Assets.logo = Content.Load<Texture2D>(@"Images/Logo");
            // Load in Indicator Arrow
            Assets.arrow = Content.Load<Texture2D>(@"GUI/arrow/Arrow");
            // Load the fonts that will be used
            gameFont = Content.Load<SpriteFont>(@"Fonts/ComicSans");
            gameFontHolder = FontHolder.BuildFontHolder(gameFont, gameFont);
            // Load in fonts into the current screen
            LoadFontsToScreen();
            // Load in the cursor!
            cursor.LoadContent(Content);
        }

        private void LoadFontsToScreen() {
            currScreen.LoadFonts(gameFontHolder);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        private void PlayGame() {
            currScreen = screenManager.ChangeScreen(gameScreen);
            currScreen.setCamera(cam);
            // Load in fonts into the current screen
            LoadFontsToScreen();
        }

        private void MakePuzzle() {
            currScreen = screenManager.ChangeScreen(createScreen);
            currScreen.setCamera(cam);
            // Load in fonts into the current screen
            LoadFontsToScreen();
        }

        private void LoadPuzzleToMake() {
            MakePuzzle();
            ((PaintScreen)currScreen).LoadPuzzle();
        }

        private void ReturnToTitleScreen() {
            currScreen = screenManager.ChangeScreen(titleScreen);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            inputManager.Update(gameTime);
            currScreen.UpdateMouse(inputManager.MousePosition);
            if (currScreen.UpdateInput()) {
                if (screenManager.getCurrentScreenID() == titleScreen) {
                    Exit();
                } else {
                    ReturnToTitleScreen();
                }
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
            GraphicsDevice.Clear(Color.SpringGreen);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cam.Transform);
            currScreen.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
