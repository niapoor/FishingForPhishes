using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Dynamic;
using SharpDX.Direct3D9;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using SharpDX.Direct2D1;
using SharpDX.MediaFoundation;

namespace ImagineRITGame
{
    // An enum that holds all of the states the player can be in
    enum PlayerStates
    {
        Fishing,
        HoldingFish,
        Idle,
        CatchFish,
        PutAway
    }

    // An enum that holds all of the outfit pieces a player can have
    enum OutfitPieces
    {
        Body = 0,
        Shirt = 1,
        Pants = 2,
        Shoes = 3,
        Hair = 4,
        Eyes = 5
    }

    class Player : DynamicObject
    {

        // The player's current state (walking, jumping, idle, or dying)
        private PlayerStates playerState;
        private PlayerStates prevState;

        private List<Texture2D> textures;
        private List<Texture2D> notOutfitTextures;
        private double xLoc;
        private double yLoc;

        // Keyboard states
        private KeyboardState kbState;
        private KeyboardState prevKBState;

        // Fields for Player animation data
        private double fps;
        private double timeCounter;
        private int playerCurrentFrame;
        private int playerMaxFrame;
        private double secondsPerFrame;
        private int widthOfSingleSprite;
        private int animationIndex;
        private bool faceRight;
        private int animationType;
        private bool drawFishingBob;
        private bool fishingRodSoundEffect;

        private float aspectRatioFactor;
        
        // For a sound effect
        public event PlaySoundEffectDelegate PlaySoundEffect;

        /// <summary>
        /// Getter and setter for the playerState
        /// </summary>
        public PlayerStates PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

        public int CurrentFrame
        {
            get { return playerCurrentFrame; }
        }

        /// <summary>
        /// A set for the previous keyboard state
        /// </summary>
        public KeyboardState PrevKBState
        {
            set { prevKBState = value; }
        }


        public Player(List<Texture2D> sprites, List<Texture2D> sprites2, double x, double y)
        {
            textures = sprites;
            notOutfitTextures = sprites2;
            xLoc = x;
            yLoc = y;

            // The player is idle by default because the game starts with them not moving
            playerState = PlayerStates.Fishing;
            prevState = PlayerStates.Idle;

            faceRight = true;
            fishingRodSoundEffect = true;

            // Initializing the keyboard states
            prevKBState = new KeyboardState();
            kbState = new KeyboardState();

            // Assigning default values to animation data
            fps = 10.0;
            secondsPerFrame = 1 / fps;
            timeCounter = 0;
            playerCurrentFrame = 0;
            playerMaxFrame = 4;
            widthOfSingleSprite = 160;
            animationIndex = 2;

            if ((float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) < (float)((float)1920 / (float)1080))
                aspectRatioFactor = (float)((float)((float)1920 / (float)1080) - (float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            else
                aspectRatioFactor = 0;
        }


        /// <summary>
        /// Helper for updating the player's animation based on time
        /// </summary>
        /// <param name="gameTime">Info about time from MonoGame</param>
        public void UpdateAnimation(GameTime gameTime)
        {
            // Update the time since the last frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // ================================================================
            // For the different states below,
            // vars change based on the current animation...
            // playerMaxFrame: how many frames long the current animation is
            // widthOfSingleSprite: some animations are "thinner" than others
            // animationIndex: index of current animation on the spritesheet
            // fps: some animations should run faster than others
            // ================================================================

            if (playerState == PlayerStates.Fishing || playerState == PlayerStates.CatchFish)
            {
                playerMaxFrame = 5;
                widthOfSingleSprite = 160 / 5;
                if (playerState == PlayerStates.Fishing)
                    animationIndex = 0;
                else
                    animationIndex = 4;
                fps = 7.0;
                animationType = 45;
            }
            else if (playerState == PlayerStates.HoldingFish || playerState == PlayerStates.PutAway)
            {
                playerMaxFrame = 6;
                widthOfSingleSprite = 160 / 5;
                animationIndex = 1;
                fps = 10.0;
                animationType = 8;
            }
            else if (playerState == PlayerStates.Idle)
            {
                playerMaxFrame = 1; //1
                widthOfSingleSprite = 160 / 5;
                animationIndex = 2;
                fps = 8.0;
                animationType = 0; // 0
            }

            // Update secondsPerFrame, as fps could have been changed
            secondsPerFrame = 1 / fps;

            // Check that enough time has passed between frames
            if (timeCounter >= secondsPerFrame)
            {
                switch (playerState)
                {
                    case PlayerStates.Fishing:
                        UpdateFishingAnimation();
                        break;
                    case PlayerStates.HoldingFish:
                        UpdateHoldingAnimation();
                        break;
                    case PlayerStates.Idle:
                        UpdateIdleAnimation();
                        break;
                    case PlayerStates.CatchFish:
                        UpdateCatchFishAnimation();
                        break;
                    case PlayerStates.PutAway:
                        UpdatePutAwayAnimation();
                        break;
                }

                // Reset the time counter for a new frame
                timeCounter -= secondsPerFrame;
            }
        }

        public void UpdateFishingAnimation()
        {
            if (fishingRodSoundEffect)
            {
                PlaySoundEffect?.Invoke(SoundEffects.CastRod);
                fishingRodSoundEffect = false;
            }

            // Increment the active frame if the player is still casting their rod
            // Otherwise, the player will fish standing still
            if (playerCurrentFrame != 4)
            {
                playerCurrentFrame++;
                drawFishingBob = true;
            }

            if (playerCurrentFrame >= playerMaxFrame)
            {
                playerCurrentFrame = 0;
                drawFishingBob = false;
            }
        }

        public void UpdateHoldingAnimation()
        {
            // Increment the active frame if the player is still casting their rod
            // Otherwise, the player will fish standing still
            if (playerCurrentFrame != 4)
            {
                playerCurrentFrame++;
                drawFishingBob = true;
            }

            if (playerCurrentFrame >= playerMaxFrame)
            {
                playerCurrentFrame = 0;
                drawFishingBob = false;
            }
        }

        public void UpdateIdleAnimation()
        {
            playerCurrentFrame = 0;
        }

        public void UpdateCatchFishAnimation()
        {
            if (fishingRodSoundEffect)
            {
                PlaySoundEffect?.Invoke(SoundEffects.CastRod);
                fishingRodSoundEffect = false;
            }

            // Increment the active frame if the player is still casting their rod
            // Otherwise, the player will fish standing still
            if (playerCurrentFrame != 0)
                playerCurrentFrame--;
            else
            {
                UpdatePlayerState(PlayerStates.HoldingFish);
            }
        }

        public void UpdatePutAwayAnimation()
        {
            // Increment the active frame if the player is still casting their rod
            // Otherwise, the player will fish standing still
            if (playerCurrentFrame != 0)
                playerCurrentFrame--;
            else
            {
                UpdatePlayerState(PlayerStates.Fishing);
            }
        }

        /// <summary>
        /// Update the player's state for animation purposes
        /// </summary>
        /// <param name="state"></param>
        public void UpdatePlayerState(PlayerStates state)
        {
            // Most animations will start on the first frame
            if (state != PlayerStates.CatchFish && state != PlayerStates.PutAway)
                playerCurrentFrame = 0;
            else
                playerCurrentFrame = 4;
            prevState = playerState;
            playerState = state;
            fishingRodSoundEffect = true;
            drawFishingBob = false;
        }

        public void DrawFish(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Fish fish)
        {
            int indexX = Int32.Parse(fish.CatchInfo[2]);
            int indexY = (indexX / 10);
            indexX = (indexX % 10) - 1;
            if (indexX < 0)
            {
                indexX = 9;
                indexY--;
            }

            // Drawing the fish for the player to hold
            sb.Draw(notOutfitTextures[0],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.444) + (int)(((((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10)))),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.251) + (int)((((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10)) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .3)),
                (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.664) / 9.6), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.664) / 9.6)),
                new Rectangle((notOutfitTextures[0].Width / 10) * indexX, (notOutfitTextures[0].Height / 10) * indexY,
                (notOutfitTextures[0].Width / 10), (notOutfitTextures[0].Height / 10)),
                Color.White);
        }

        /// <summary>
        /// Draws in the player facing the correct direction with the correct
        /// frame of their current animation
        /// </summary>
        /// <param name="sb">the spritebatch that allows us to draw</param>
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            SpriteEffects spe = SpriteEffects.None;
            if (!faceRight)
                spe = SpriteEffects.FlipHorizontally;

            double conditionalY = 0;
            if (playerState == PlayerStates.Idle)
                conditionalY = 0.04;

            // Drawing in the player
            for (int i = 0; i < textures.Count; i++)
                sb.Draw(textures[i],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * xLoc), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (yLoc - conditionalY)),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765)),
                new Rectangle((playerCurrentFrame * widthOfSingleSprite) + (3 * 8 * widthOfSingleSprite), (animationType * 32), widthOfSingleSprite, textures[i].Height / 44),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .826f);

            //if (drawFishingBob == true)
            //{
            //    sb.Draw(texture,
            //        new Rectangle(500, 100, widthOfSingleSprite * 9, (int)(texture.Height / 5)),
            //        new Rectangle((playerCurrentFrame) * widthOfSingleSprite, (animationType * 32), widthOfSingleSprite, texture.Height / 44),
            //        Color.White,
            //        0f,
            //        Vector2.Zero,
            //        0,
            //        .826f);
            //}

        }


        /// <summary>
        /// Determines if the key being held was just released
        /// </summary>
        /// <param name="key">the key being held</param>
        /// <returns>if the key being held was just released</returns>
        private bool JustReleased(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key) && prevKBState.IsKeyDown(key);
        }

    }

}