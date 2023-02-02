using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ImagineRITGame
{
    public class Game1 : Game
    {
     
        
        public enum GameState : int
        {
            MainMenu = 1,
            FishingPost = 2,
            FishPostCard = 3,
            PauseMenu = 4
        }


        // Default fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


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
        // Gamepad stats
        private GamePadState previousGPState;
        private bool isController;


        int _width = 0;
        int _height = 0;

        // Space for sound effects

        // Space for fonts

        // Player texture fields
        private Texture2D playerTexture;

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
            //_graphics.IsFullScreen = true;
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("char_free_tmp");
            numSpritesInSheet = 8;
            widthOfSingleSprite = playerTexture.Width / numSpritesInSheet;

            // Set up animation data, too
            fps = 7.0;
            secondsPerFrame = 1.0 / fps;
            timeCounter = 0;
            playerCurrentFrame = 1;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Always update the animation
            UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Helper for updating the selected player's animation based on time
        /// </summary>
        /// <param name="gameTime">Info about time from MonoGame</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            // ElapsedGameTime is how the last GAME frame took
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time passed to flip to the next frame?
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active,
                // ensuring we go back to 1 eventually
                playerCurrentFrame++;
                if (playerCurrentFrame >= 8) // hardcoded here because I KNOW what my spritesheet looks like
                {
                    playerCurrentFrame = 1;
                }

                // Reset the time counter
                timeCounter -= secondsPerFrame;
            }
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

            _spriteBatch.Draw(playerTexture,
                new Rectangle(500, 345, widthOfSingleSprite * 9, (int)(playerTexture.Height / 1.5)),
                new Rectangle((playerCurrentFrame) * widthOfSingleSprite, 0, widthOfSingleSprite, playerTexture.Height / 12),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .826f);

            _spriteBatch.Draw(playerTexture,
                new Rectangle(500, 345, widthOfSingleSprite * 9, (int)(playerTexture.Height / 1.5)),
                new Rectangle((playerCurrentFrame) * widthOfSingleSprite, (playerTexture.Height / 12) * 2, widthOfSingleSprite, playerTexture.Height / 12),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .826f);

            _spriteBatch.Draw(playerTexture,
                new Rectangle(500, 345, widthOfSingleSprite * 9, (int)(playerTexture.Height / 1.5)),
                new Rectangle((playerCurrentFrame) * widthOfSingleSprite, (playerTexture.Height / 12) * 4, widthOfSingleSprite, playerTexture.Height / 12),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .826f);

            _spriteBatch.Draw(playerTexture,
                new Rectangle(500, 345, widthOfSingleSprite * 9, (int)(playerTexture.Height / 1.5)),
                new Rectangle((playerCurrentFrame) * widthOfSingleSprite, (playerTexture.Height / 12) * 6, widthOfSingleSprite, playerTexture.Height / 12),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .826f);

            _spriteBatch.Draw(playerTexture,
                new Rectangle(500, 345, widthOfSingleSprite * 9, (int)(playerTexture.Height / 1.5)),
                new Rectangle((playerCurrentFrame) * widthOfSingleSprite, (playerTexture.Height / 12) * 8, widthOfSingleSprite, playerTexture.Height / 12),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .826f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}