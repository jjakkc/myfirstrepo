using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;

namespace Fantastic7
{
    public class SpriteBatchPlus : SpriteBatch
    {
        private Texture2D _defaultTexture;
        public SpriteBatchPlus(GraphicsDevice graphicsDevice) : base(graphicsDevice){}
        public void setDefaultTexture(Texture2D defaultTexture) { _defaultTexture = defaultTexture; }
        public Texture2D defaultTexture() { return _defaultTexture; }
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatchPlus spriteBatch;
        GameState gs;
        Room rm;
        //Texture2D plainText;
        Map currMap;
        const int WIDTH = 1280;
        const int HEIGHT = 720;
        SpriteFont mfont;
        SpriteFont sfont;
        GGUI mainMenu;
        GGUI pauseMenu;

        enum GameState
        {
            mainMenu,
            running,
            paused
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gs = GameState.mainMenu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatchPlus(GraphicsDevice);
            Texture2D plainText = new Texture2D(GraphicsDevice, 1, 1);
            plainText.SetData(new[] { Color.White });
            spriteBatch.setDefaultTexture(plainText);
            


            //Creates Test Room
            /*GSprite[] roomSprites = { new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.Gray),
                new NSprite(new Rectangle(100, 100, WIDTH - 200, HEIGHT - 200), Color.LightGray)};
            rm = new Room(roomSprites);

            //rm.addObject(new Entity(new Vector2(500, 500), new NSprite(new Rectangle(500, 500, 50, 50), Color.Wheat)));*/


            //Imports Font
            mfont = Content.Load<SpriteFont>("main");
            sfont = Content.Load<SpriteFont>("second");
            int mHeight = (int)mfont.MeasureString("M").Y;
            int sHeight = (int)sfont.MeasureString("M").Y;


            //Creates Main Menu
            GSprite[] gs = { new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.SandyBrown),
                new NSprite(new Rectangle(0, 0, WIDTH, mHeight * 2), Color.SaddleBrown),
                new SSprite("Maze Crawler", mfont, new Vector2(25,mHeight / 2), Color.Azure)};

            MenuOption[] mo = { new MenuOption(new SSprite("Start Game", sfont, new Vector2(50, mHeight * 2 + sHeight), Color.Azure)),
                new MenuOption(new SSprite("Settings", sfont, new Vector2(50, mHeight * 2 + sHeight * 3), Color.Azure)),
                new MenuOption(new SSprite("Quit Game", sfont, new Vector2(50,mHeight * 2 + sHeight * 5), Color.Azure))};

            mainMenu = new GGUI(gs, mo, Color.Azure);



            //Creates Pause Menu
            GSprite[] pgs = { new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, HEIGHT / 2), Color.SandyBrown),
                new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, mHeight * 2), Color.SaddleBrown),
                new SSprite("Pause Menu", mfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 2, HEIGHT / 8 + mHeight / 2), Color.Azure)};

            MenuOption[] pmo = { new MenuOption(new SSprite("Resume", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight/2), Color.Azure)),
                new MenuOption(new SSprite("Setting", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 2), Color.Azure)),
                new MenuOption(new SSprite("Quit", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 3.5f), Color.Azure))};

            pauseMenu = new GGUI(pgs, pmo, Color.Azure);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void newGame()
        {
            gs = GameState.running;
            currMap = new Map();
            currMap.GenerateMap();
        }

        int elapsedTime = 0;
        int millisecondsPer = 80;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            switch(gs)
            {
                case GameState.mainMenu:
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                    if(elapsedTime > millisecondsPer)
                    {
                        elapsedTime = 0;

                        //Poll inputs
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                            Exit();
                        if (Keyboard.GetState().IsKeyDown(Keys.G)) newGame();
                        if (Keyboard.GetState().IsKeyDown(Keys.Down)) mainMenu.nextOption();
                        if (Keyboard.GetState().IsKeyDown(Keys.Up)) mainMenu.previousOption();
                    }
                    //End inputs 

                    break;
                case GameState.running:
                    //Poll inputs
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) gs = GameState.paused;
                    //End inputs

                    currMap.update(gameTime);
                    
                    break;
                case GameState.paused:
                    //Poll inputs
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) gs = GameState.mainMenu;
                    if (Keyboard.GetState().IsKeyDown(Keys.U)) gs = GameState.running;
                    if (Keyboard.GetState().IsKeyDown(Keys.F)) graphics.IsFullScreen = false; 
                    //End inputs
                    break;
                default: break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //Draws objects depending on the state of the game
            switch (gs)
            {
                case GameState.mainMenu:
                    mainMenu.draw(spriteBatch,1);
                    break;
                case GameState.paused:
                    pauseMenu.draw(spriteBatch, 1);
                    break;
                case GameState.running:

                    currMap.draw(spriteBatch, 1f);
                    break;
                default: break;
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
