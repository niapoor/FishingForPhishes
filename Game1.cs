using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;
using System.IO;

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

        // Cooldowns for button presses and fish spawning
        private double cooldownTime = 0;
        private double cooldownTime2 = 0;
        private double cooldownTime3 = 0;
        private double fishSpawnTime = 0;

        private Random random;
        private int randTmp;

        // Tutorial bools
        private bool firstTutorial;
        private bool secondTutorial;

        // Space for sound effects
        private bool currentlyPlaying;
        private SoundManager soundManager;
        private List<Song> songs;
        private System.Collections.Generic.List<Microsoft.Xna.Framework.Audio.SoundEffect> soundEffects;
        private Song gameSong;
        private SoundEffect fishSplash;
        private SoundEffect castRod;
        private SoundEffect catchFish;
        private SoundEffect buttonClick;

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
        private Inventory inventory;
        private TutorialsAndInfo tutorialsAndInfo;
        private List<Texture2D> menuTextures;

        private Texture2D background;
        private Texture2D darkenBackground;

        // Should only be drawn at the proper time during the game
        private bool drawInQuestion;
        private bool drawInFishType;
        private bool canOpenQuestion;

        private FishPack fishPack;
        private Fish currentFish;
        private List<Texture2D> fishTextures;

        private Seagull seagull;
        private bool finalSeagullFrame;

        // The current difficulty
        private Difficulty currentDifficulty;
        private Difficulty prevDifficulty;

        // Question variables
        private DisplayQuestion displayQuestion;
        private QuestionPack questionPack;
        private Question currentQuestion;
        private bool correctOrIncorrect;
        private List<IEnumerable<String>> questionCSVFiles;
        private IEnumerable<String> easyCSV;
        private IEnumerable<String> mediumCSV;
        private IEnumerable<String> hardCSV;

        // Texture fields
        private Texture2D fishingBobTexture;
        private Texture2D fishTexture;
        private Texture2D buttonTexture;
        private Texture2D titleCardTexture;
        private Texture2D cyberCorpsLogo;
        private Texture2D fishingPostCard;
        private Texture2D allFish;
        private Texture2D fishInvShadow;
        private Texture2D allTiles;

        // Player outfit texture fields
        private Texture2D playerTexture;
        private Texture2D shirt;
        private Texture2D pants;
        private Texture2D shoes;
        private Texture2D hair;
        private Texture2D eyes;
        private List<Texture2D> outfitTextures;


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

            // The default difficulty for questions is easy
            currentDifficulty = Difficulty.Easy;
            prevDifficulty = Difficulty.Hard;

            menuTextures = new List<Texture2D>();
            fonts = new List<SpriteFont>();
            outfitTextures = new List<Texture2D>();
            fishTextures = new List<Texture2D>();
            outfitTextures = new List<Texture2D>();
            songs = new List<Song>();
            soundEffects = new System.Collections.Generic.List<Microsoft.Xna.Framework.Audio.SoundEffect>();


            this.random = new Random();

            // Game properties
            Window.Title = "ImagineRIT Phishing Game";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();

            questionPack = new QuestionPack();
            fishPack = new FishPack();
            // Initializing Menu objects
            mainMenu = new MainMenu(menuTextures);
            pauseMenu = new PauseMenu(menuTextures, fonts);
            creditsMenu = new CreditsMenu(menuTextures, fonts);
            gameButtonsOverlay = new GameButtonsOverlay(menuTextures);
            inventory = new Inventory(menuTextures, fonts);
            displayQuestion = new DisplayQuestion(menuTextures, fonts);
            tutorialsAndInfo = new TutorialsAndInfo(menuTextures, fonts);

            soundManager = new SoundManager(songs, soundEffects);

            // Fish for testing
            // testFish = new Fish(fishTexture, new Vector2((float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .47), (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .84)));

            // A fish to start off with so the game doesn't freak out about an empty variable
            currentFish = new Fish(fishTextures,
                new Vector2((float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .47),
                (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .84)),
                fishPack.FetchRandomFish(currentDifficulty), fonts);
            currentFish.PlaySoundEffect += soundManager.PlaySoundEffect;

            seagull = new Seagull(allTiles,
                new Vector2((float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .54),
                (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .59)));
            finalSeagullFrame = false;

            firstTutorial = true;
            secondTutorial = true;

            currentlyPlaying = false;

            // TODO: add question pack
            //questionPack = new QuestionPack();

            numSpritesInSheet = 8;
            widthOfSingleSprite = playerTexture.Width / numSpritesInSheet;

            player = new Player(outfitTextures, 0.42, 0.4);
            player.PlaySoundEffect += soundManager.PlaySoundEffect;
        }

        protected override void Update(GameTime gameTime)
        {

            // Always update the animation
            UpdateAnimation(gameTime);

            // Cooldown between pressing buttons
            cooldownTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            cooldownTime2 += gameTime.ElapsedGameTime.TotalMilliseconds;
            cooldownTime3 += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!currentlyPlaying)
            {
                currentlyPlaying = true;
                soundManager.PlaySong(gameSong, true);
            }

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
                    inventory.Update(gameTime);
                    if (SingleKeyPress(Keys.Escape, previousKbState))
                        ChangeGameState(-1);
                    inventory.MenuButtonActivated += ChangeGameState;
                    break;
                case GameState.Game:
                    gameButtonsOverlay.Update(gameTime);
                    // If the player selects 'esc' in the game, go to the pause menu
                    if (SingleKeyPress(Keys.Escape, previousKbState))
                        ChangeGameState(19);
                    else if (SingleKeyPress(Keys.Space, previousKbState) && drawInQuestion == false && drawInFishType == false && canOpenQuestion)
                    {
                        currentQuestion = questionPack.FetchRandomQuestion(currentDifficulty);
                        displayQuestion.SetUpQuestion(currentQuestion);
                        drawInQuestion = true;
                        currentFish = new Fish(fishTextures,
                            new Vector2((float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .47),
                            (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .84)),
                            fishPack.FetchRandomFish(currentDifficulty), fonts);
                        currentFish.PlaySoundEffect += soundManager.PlaySoundEffect;
                    }
                    else if (SingleKeyPress(Keys.Space, previousKbState) && drawInFishType)
                    {
                        drawInFishType = false;
                        canOpenQuestion = false;
                        fishSpawnTime = 0;
                        firstTutorial= false;
                    }
                    if (drawInQuestion)
                    {
                        displayQuestion.Update(gameTime);
                        displayQuestion.MenuButtonActivated += ChangeGameState;
                    }
                    if (!drawInQuestion && !drawInFishType && !canOpenQuestion)
                    {
                        fishSpawnTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (fishSpawnTime > 1250)//2000)
                        {
                            randTmp = random.Next(1, 50);
                            if (randTmp == 25)
                            {
                                canOpenQuestion = true;
                                currentFish.UpdateState(FishStates.FadeIn);
                            }
                        }
                    }
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
                    currentFish.UpdateAnimation(gameTime);
                    if (cooldownTime3 > 1500 || finalSeagullFrame)
                    {
                        if (finalSeagullFrame)
                        {
                            if (seagull.CurrentFrame != 0)
                            {
                                seagull.UpdateAnimation(gameTime);
                            }
                            else if (seagull.CurrentFrame == 0)
                            {
                                finalSeagullFrame = false;
                                cooldownTime3 = 0;
                            }
                            break;
                        }
                        randTmp = random.Next(1, 5);
                        if (randTmp == 2)
                        {
                            seagull.UpdateAnimation(gameTime);
                            finalSeagullFrame = true;
                        }
                    }
                    //testFish.UpdateAnimation(gameTime);
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

            // Draw in the game's background
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), new Rectangle(0, 0, 1649, 924), Color.White, 0f,
                Vector2.Zero,
                0,
                .9f);

            // Darken the background if the game is "paused" (essentially)
            if (gameState == GameState.CreditsMenu || (gameState == GameState.Game && drawInQuestion) || gameState == GameState.PauseMenu || gameState == GameState.Inventory)
                _spriteBatch.Draw(darkenBackground, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), new Rectangle(110, 0, 1700, 960), new Color(Color.Black, 0.6f), 0f,
               Vector2.Zero,
               0,
               .0001f);

            // width: 2732, height: 2048
            // Drawing in the RIT CyberCorps logo in the bottom right. This will always be drawn in regardless of other settings.
            // Drawing in size and position is dynamic based on the screen size
            _spriteBatch.Draw(cyberCorpsLogo, new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .82), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .02),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.075 * 2.3)), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.0149 * 2.3))), new Rectangle(0, 0, 750, 149), Color.White);


            // Drawing in assets based on the current game state
            switch (gameState)
            {

                case GameState.MainMenu:
                    mainMenu.Draw(_spriteBatch, Color.Goldenrod);
                    break;
                case GameState.PauseMenu:
                    pauseMenu.Draw(_spriteBatch, Color.Goldenrod, fonts, currentDifficulty);
                    break;
                case GameState.Inventory:
                    inventory.DrawFonts(_spriteBatch, fonts);
                    inventory.Draw(_spriteBatch, Color.Goldenrod);
                    inventory.DrawFonts(_spriteBatch, fonts);
                    break;
                case GameState.Game:
                    gameButtonsOverlay.Draw(_spriteBatch, Color.Goldenrod);
                    if (drawInQuestion)
                    {
                        displayQuestion.Draw(_spriteBatch, Color.Goldenrod, currentQuestion, fonts);
                        if(!firstTutorial && secondTutorial)
                            secondTutorial= false;
                    }
                    // If a question is able to be brought up / answered, draw in in the fish
                    if (canOpenQuestion && !(drawInFishType && correctOrIncorrect))
                    {
                        currentFish.DrawShadow(_spriteBatch);
                    }
                    if (drawInFishType)
                    {
                        if (correctOrIncorrect)
                            tutorialsAndInfo.DrawCatchText(_spriteBatch, fonts, currentFish);
                        else
                            tutorialsAndInfo.DrawQuestionIncorrect(_spriteBatch, fonts, currentQuestion);
                        if (firstTutorial)
                            tutorialsAndInfo.DrawFishAgainInstructions(_spriteBatch, fonts);
                    }
                    if(!drawInQuestion && !drawInFishType)
                    {
                        if (firstTutorial)
                            tutorialsAndInfo.DrawGanericInstructions(_spriteBatch, fonts);
                        else if (secondTutorial)
                            tutorialsAndInfo.DrawMoreTutorials(_spriteBatch, fonts);
                    }
                    player.Draw(_spriteBatch);
                    seagull.Draw(_spriteBatch);
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
            else if (state == 3)
            {
                prevGameState = GameState.Game;
                gameState = GameState.Inventory;
            }
            else if (state == 8)
            {
                if (SingleMousePress(mouseState, previousMouseState) && cooldownTime >= 3)
                {
                    cooldownTime = 0;
                    if (currentDifficulty == Difficulty.Easy && prevDifficulty == Difficulty.Hard)
                    {
                        currentDifficulty = Difficulty.Medium;
                        prevDifficulty = Difficulty.Easy;
                    }
                    else if (currentDifficulty == Difficulty.Medium && prevDifficulty == Difficulty.Easy)
                    {
                        currentDifficulty = Difficulty.Hard;
                        prevDifficulty = Difficulty.Medium;
                    }
                    else
                    {
                        currentDifficulty = Difficulty.Easy;
                        prevDifficulty = Difficulty.Hard;
                    }
                }
            }
            else if (state >= 12 && state <= 17)
            {
                foreach (Answer a in currentQuestion.AnswerList())
                {
                    if (a.Text() == "True" && currentQuestion.NumAnswers() == 2)
                    {
                        correctOrIncorrect = currentQuestion.CheckAnswer(state - 12);
                        drawInQuestion = false;
                        drawInFishType = true;
                        break;
                    }
                }
                correctOrIncorrect = currentQuestion.CheckAnswer(state - 14);
                drawInQuestion = false;
                drawInFishType = true;
                if (cooldownTime2 >= 10 && correctOrIncorrect)
                {
                    cooldownTime2 = 0;
                    inventory.AddFishToInventory(currentFish);
                    soundManager.PlaySoundEffect(SoundEffects.Award);
                }
                else if (cooldownTime2 >= 10 && !correctOrIncorrect)
                {
                    currentFish.UpdateState(FishStates.FadeOut);
                    //soundManager.PlaySoundEffect(SoundEffects.FishEscape);
                }
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
            fishingBobTexture = Content.Load<Texture2D>("inv_items");
            fishTexture = Content.Load<Texture2D>("fish_shadow_black");
            buttonTexture = Content.Load<Texture2D>("button_spritesheet");
            menuTextures.Add(buttonTexture);
            titleCardTexture = Content.Load<Texture2D>("phishing_game_logo");
            menuTextures.Add(titleCardTexture);
            fishingPostCard = Content.Load<Texture2D>("fishing_postcard_blank");
            menuTextures.Add(fishingPostCard);
            allFish = Content.Load<Texture2D>("fish_all");
            menuTextures.Add(allFish);
            fishInvShadow = Content.Load<Texture2D>("inv_fish_shadow");
            menuTextures.Add(fishInvShadow);
            fishTextures.Add(fishTexture);
            fishTextures.Add(allFish);
            // Outfit / Player textures
            playerTexture = Content.Load<Texture2D>("char_all");
            shirt = Content.Load<Texture2D>("basic");
            pants = Content.Load<Texture2D>("pants");
            shoes = Content.Load<Texture2D>("shoes");
            hair = Content.Load<Texture2D>("bob ");
            eyes = Content.Load<Texture2D>("eyes");
            outfitTextures.Add(playerTexture);
            outfitTextures.Add(shirt);
            outfitTextures.Add(pants);
            outfitTextures.Add(shoes);
            outfitTextures.Add(hair);
            outfitTextures.Add(eyes);
            gameSong = Content.Load<Song>("Water_1");
            songs.Add(gameSong);
            fishSplash = Content.Load<SoundEffect>("416710__inspectorj__splash-small-a");
            castRod = Content.Load<SoundEffect>("51755__erkanozan__whip-01");
            catchFish = Content.Load<SoundEffect>("109662__grunz__success");
            buttonClick = Content.Load<SoundEffect>("448080__breviceps__wet-click");
            soundEffects.Add(fishSplash);
            soundEffects.Add(castRod);
            soundEffects.Add(catchFish);
            soundEffects.Add(buttonClick);
            cyberCorpsLogo = Content.Load<Texture2D>("cybercorps_scholarship_for_service_hor_k1");
            background = Content.Load<Texture2D>("backgroundv3");
            darkenBackground = Content.Load<Texture2D>("black");
            allTiles = Content.Load<Texture2D>("tiles_all");
            //easyCSV = Content.Load<IEnumerable<String>>("EasyTemp");
            //mediumCSV = Content.Load<IEnumerable<String>>("MediumTemp");
            //hardCSV = Content.Load<IEnumerable<String>>("HardTemp");
            //questionCSVFiles.Add(easyCSV);
            //questionCSVFiles.Add(mediumCSV);
            //questionCSVFiles.Add(hardCSV);
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
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_4");        // 105
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 5120 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 4096)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text20");   // 234
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_title");    // 140
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text9");    // 79
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 4096 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 3840)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text19");   // 210
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_title");    // 140
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
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_4");        // 105
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text7");    // 59
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 3000 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 2560)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_text1");    // 140
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_2");        // 90
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text3");    // 50
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 2560 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1920)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_4");        // 105
                peaberryBaseText2 = Content.Load<SpriteFont>("peaberry_base_text5");    // 67   
                peaberryBaseText3 = Content.Load<SpriteFont>("peaberry_base_text6");    // 37
            }
            else if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1920 && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width >= 1600)
            {
                peaberryBaseText1 = Content.Load<SpriteFont>("peaberry_base_2");        // 90
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
        /// Returns whether the last key pressed was only pressed one time
        /// </summary>
        /// <param name="key">the key that has been pressed</param>
        /// <returns>if the last key pressed was only pressed one time</returns>
        public static bool SingleMousePress(MouseState mouseState, MouseState prevMouseState)
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released;
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