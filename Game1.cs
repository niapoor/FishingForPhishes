using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace ImagineRITGame
{
    delegate void MenuButtonActivatedDelegate(int state);

    public enum GameState : int
    {
        MainMenu = 1,
        Game = 2,
        Inventory = 3,
        PauseMenu = 4,
        CreditsMenu = 5
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
        private SpriteFont peaberryBaseText1;
        private SpriteFont peaberryBaseText2;
        private SpriteFont peaberryBaseText3;
        private List<SpriteFont> fonts;

        // Menu objects
        private MainMenu mainMenu;
        private PauseMenu pauseMenu;
        private CreditsMenu creditsMenu;
        private GameButtonsOverlay gameButtonsOverlay;
        private List<Texture2D> menuTextures;

        // Texture fields
        private Texture2D playerTexture;
        private Texture2D fishingBobTexture;
        private Texture2D fishTexture;
        private Texture2D buttonTexture;
        private Texture2D titleCardTexture;
        private Texture2D cyberCorpsLogo;


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
            gameState = GameState.MainMenu;
            prevGameState = GameState.MainMenu;

            menuTextures = new List<Texture2D>();
            fonts = new List<SpriteFont>();

            // Game properties
            Window.Title = "ImagineRIT Phishing Game";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();

            // Initializing Menu objects
            mainMenu = new MainMenu(menuTextures);
            pauseMenu = new PauseMenu(menuTextures);
            creditsMenu= new CreditsMenu(menuTextures, fonts);
            gameButtonsOverlay= new GameButtonsOverlay(menuTextures);

            numSpritesInSheet = 8;
            widthOfSingleSprite = playerTexture.Width / numSpritesInSheet;

            player = new Player(playerTexture, new Vector2(100, 0), fishingBobTexture);
        }

        protected override void Update(GameTime gameTime)
        {

            // Always update the animation
            UpdateAnimation(gameTime);

            // Assigning the appropriate current game state and updating the frame based on the game state
            switch (gameState)
            {
                case GameState.MainMenu:
                    mainMenu.Update(gameTime);
                    if (SingleKeyPress(Keys.Escape, previousKbState))
                        Exit();
                    mainMenu.MenuButtonActivated += ChangeGameState;
                    break;
                case GameState.PauseMenu:
                    // If the player selects 'esc' in the pause menu, return to the game
                    pauseMenu.Update(gameTime);
                    if (SingleKeyPress(Keys.Escape, previousKbState))
                        ChangeGameState(-1);
                    pauseMenu.MenuButtonActivated += ChangeGameState;
                    break;
                case GameState.Inventory:
                    //inventory.Update(gameTime);
                    break;
                case GameState.Game:
                    gameButtonsOverlay.Update(gameTime);
                    // If the player selects 'esc' in the game, go to the pause menu
                    if (SingleKeyPress(Keys.Escape, previousKbState))
                        ChangeGameState(19);
                    gameButtonsOverlay.MenuButtonActivated += ChangeGameState;
                    break;
                case GameState.CreditsMenu:
                    creditsMenu.Update(gameTime);
                    if (SingleKeyPress(Keys.Escape, previousKbState))
                        ChangeGameState(2);
                    creditsMenu.MenuButtonActivated += ChangeGameState;
                    break;
            }

            // Updates the previous position of the keyboard / mouse
            // (this is critical for the SingleKeyPress function, a very important function)
            UpdatePrevInputStates();

            base.Update(gameTime);
        }

        // </summary>
        /// Helper for updating the selected player's animation based on time
        /// </summary>
        /// <param name="gameTime">Info about time from MonoGame</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Game:
                    player.UpdateAnimation(gameTime);
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // Making it so that the drawn in assets don't become blurred when enlarged
            _spriteBatch.Begin
                (SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise);

            // Drawing in assets based on the current game state
            switch (gameState)
            {
                case GameState.MainMenu:
                    mainMenu.Draw(_spriteBatch, Color.Goldenrod);
                    break;
                case GameState.PauseMenu:
                    pauseMenu.Draw(_spriteBatch, Color.Goldenrod);
                    break;
                case GameState.Inventory:
                    //draw in
                    break;
                case GameState.Game:
                    gameButtonsOverlay.Draw(_spriteBatch, Color.Goldenrod);
                    player.Draw(_spriteBatch);
                    break;
                case GameState.CreditsMenu:
                    creditsMenu.Draw(_spriteBatch, Color.Goldenrod, fonts);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ChangeGameState(int state)
        {
            // The player pressed esc in the main menu or the exit button
            if (state == 0)
            {
                // If the player pressed the exit button or esc in the main menu, exit the application (the game)
                if (gameState == GameState.MainMenu)
                    this.Exit();
                // If the player pressed the exit button in the pause menu, return to the main menu
                else if (gameState == GameState.PauseMenu)
                {
                    prevGameState = gameState;
                    gameState = GameState.MainMenu;
                }
            }
            // Brings you back to the previous GameState (the player probably presssed the back button but in some cases they may have pressed esc)
            else if (state == -1)
            {
                GameState temp = gameState;
                gameState = prevGameState;
                prevGameState = temp;
            }
            // Pressing the start button triggers this event to start the game
            else if (state == 1)
            {
                prevGameState = gameState;
                gameState = GameState.Game;
            }
            // Pressing "Main Menu" in the pause menu brings the player to the main menu
            else if (state == 2)
            {
                prevGameState = gameState;
                gameState = GameState.MainMenu;
            }
            else if (state == 18)
            {
                prevGameState = GameState.MainMenu;
                gameState = GameState.CreditsMenu;
            }
            else if (state == 19)
            {
                prevGameState = GameState.Game;
                gameState = GameState.PauseMenu;
            }
                

        }

        public void LoadTextures()
        {
            playerTexture = Content.Load<Texture2D>("char_all");
            fishingBobTexture = Content.Load<Texture2D>("inv_items");
            fishTexture = Content.Load<Texture2D>("fish_shadow_black");
            buttonTexture = Content.Load<Texture2D>("button spritesheet");
            menuTextures.Add(buttonTexture);
            titleCardTexture = Content.Load<Texture2D>("phishing_game_logo");
            menuTextures.Add(titleCardTexture);
            // The fonts get really screwy if the aspect ratio is changed. Fonts are annoying in that they cannot be size changed
            // after compile time or dynamically. Therefore, I have created many spritefonts of different sizes, and which ones are
            // used are dependant on the width of the user's screen.
            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 6000)
            {
                                                                                        // Sizes    Sizing Factors
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text22");   // 328      18.286
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text19");   // 210      28.444
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text18");   // 121      51.2
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 6000 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 5120)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text21");   // 280
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text16");   // 188
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text4");    // 105
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 5120 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 4096)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text20");   // 234
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text1");    // 140
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text9");    // 79
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 4096 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 3840)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text19");   // 210
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text1");    // 140
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text9");    // 79
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 3840 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 3440)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text17");   // 188
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text18");   // 121
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text5");    // 67
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 3440 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 3000)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text16");   // 164
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text4");    // 105
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text7");    // 59
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 3000 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 2560)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text1");    // 140
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text2");    // 90
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text3");    // 50
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 2560 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1920)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text4");    // 105
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text5");    // 67   
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text6");    // 37
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1920 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1600)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text2");    // 90
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text7");    // 59
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text8");    // 33
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1600 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1440)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text9");    // 79
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text3");    // 50
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text8");    // 33
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1440 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1360)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text9");    // 79
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text3");    // 50
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text10");   // 27
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1360 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1280)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text5");    // 67
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text3");    // 50
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text10");   // 27
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1280 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1024)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text11");   // 56
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text6");    // 37
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text12");   // 20
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1024 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 800)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text13");   // 44
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text10");   // 27
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text14");   // 16
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 800)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text8");    // 33
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text12");   // 20
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text15");   // 12
            }
            // Adding the chosen fonts to the font list
            fonts.Add(peaberryBaseText1);
            fonts.Add(peaberryBaseText2);
            fonts.Add(peaberryBaseText3);
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

        /// <summary>
        /// Updates the previous state of each input method for each class that needs it
        /// </summary>
        public void UpdatePrevInputStates()
        {
            previousKbState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            player.PrevKBState = previousKbState;
        }

        /// <summary>
        /// Determines the starting location of a string of text 
        /// in order for it be centered horizontally on the screen
        /// </summary>
        /// <param name="str">the text to be centered</param>
        /// <param name="y">the Y location of the text</param>
        /// <param name="font">the font the text will be drawn in</param>
        /// <returns>the location the text should be at</returns>
        public static Vector2 CenterText(string str, float y, SpriteFont font)
        {
            return new Vector2(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - font.MeasureString(str).X) / 2), y);
        }


    }
}