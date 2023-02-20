using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Windows.Forms.VisualStyles;

namespace ImagineRITGame
{
    delegate void MenuButtonActivatedDelegate(int state);

    public enum GameState : int
    {
        MainMenu = 1,
        Game = 2,
        Inventory = 3,
        PauseMenu = 4
    }

    public class Game1 : Game
    {

        // Default fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Player objects
        private Player player;

        // Keeps track of the game state so we know which
        // screen of the game to be on
        private GameState gameState;
        // Keeps track of the previous game state
        private GameState prevGameState;


        // ======================================
        // Fields for keeping track of mouse,
        // keyboard, and controller states
        // ======================================
        // Keeps track of the last frame's key press
        private KeyboardState previousKbState;
        // Fields that hold the mouse's current and last state
        private MouseState mouseState;
        private MouseState previousMouseState;


        int _width;
        int _height;

        // Space for sound effects

        // Space for fonts

        // Player texture fields
        private Texture2D playerTexture;
        private Texture2D fishingBobTexture;
        private Texture2D fishTexture;
        private Texture2D buttonTexture;

        // Sprite sheet data
        private int numSpritesInSheet;
        private int widthOfSingleSprite;

        // Animation data
        private int playerCurrentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;

        // Default constructor for a Game1 class
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            SetFullscreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void SetFullscreen()
        {
            _width = Window.ClientBounds.Width;
            _height = Window.ClientBounds.Height;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.HardwareModeSwitch = true;

            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // Default GameState is MainMenu
            gameState = GameState.Game;
            prevGameState = GameState.Game;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();

            numSpritesInSheet = 8;
            widthOfSingleSprite = playerTexture.Width / numSpritesInSheet;

            player = new Player(playerTexture, new Vector2(100, 0), fishingBobTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Always update the animation
            UpdateAnimation(gameTime);

            // Assigning the appropriate current game state
 //           switch (gameState)
 //           {
 //               case GameState.MainMenu:
 //                   if ()
 //           }

            switch (gameState)
            {
                case GameState.MainMenu:
                    //mainMenu.Update(gameTime);
                    break;
                case GameState.PauseMenu:
                    //pauseMenu.Update(gameTime);
                    break;
                case GameState.Inventory:
                    //inventory.Update(gameTime);
                    break;
                case GameState.Game:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Helper for updating the selected player's animation based on time
        /// </summary>
        /// <param name="gameTime">Info about time from MonoGame</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            player.UpdateAnimation(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Making it so that the drawn in assets don't become blurred when enlarged
            _spriteBatch.Begin
                (SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise);

            if (gameState == GameState.Game)
            {
                player.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        public void LoadTextures()
        {
            playerTexture = Content.Load<Texture2D>("char_all");
            fishingBobTexture = Content.Load<Texture2D>("inv_items");
            fishTexture = Content.Load<Texture2D>("fish_shadow_black");
            buttonTexture = Content.Load<Texture2D>("button maker");
        }

        /// <summary>
        /// Returns whether the last key pressed was only pressed one time
        /// </summary>
        /// <param name="key">the key that has been pressed</param>
        /// <returns>if the last key pressed was only pressed one time</returns>
        public static bool SingleKeyPress(Keys key, KeyboardState prevKBState)
        {
            return Keyboard.GetState().IsKeyDown(key) && prevKBState.IsKeyUp(key);
        }

    }
}