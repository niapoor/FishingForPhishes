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

namespace ImagineRITGame
{
    // An enum that holds all of the states the player can be in
    enum PlayerStates
    {
        Walking,
        Idle,
        Fishing,
        HoldingFish
    }

    class Player : DynamicObject
    {

        // The player's current state (walking, jumping, idle, or dying)
        private PlayerStates playerState;
        private PlayerStates prevState;

        private Texture2D texture;
        private Vector2 position;

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

        /// <summary>
        /// Getter and setter for the playerState
        /// </summary>
        public PlayerStates PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

        /// <summary>
        /// A set for the previous keyboard state
        /// </summary>
        public KeyboardState PrevKBState
        {
            set { prevKBState = value; }
        }


        public Player(Texture2D sprite, Vector2 location, Texture2D fishingBob)
        {
            texture = sprite;
            position = location;

            // The player is idle by default because the game starts with them not moving
            playerState = PlayerStates.Idle;
            prevState = PlayerStates.Idle;

            faceRight = true;

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

            playerState = PlayerStates.Fishing;

            if (playerState == PlayerStates.Fishing)
            {
                playerMaxFrame = 5;
                widthOfSingleSprite = 160 / 5;
                animationIndex = 0;
                fps = 7.0;
                animationType = 45;
            }
            else if (playerState == PlayerStates.Walking)
            {
                playerMaxFrame = 6;
                widthOfSingleSprite = 160 / 5;
                animationIndex = 1;
                fps = 10.0;
                animationType = 0;
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
                // Increment the active frame
                if((playerState == PlayerStates.Fishing && playerCurrentFrame != 4) || playerState != PlayerStates.Fishing)
                {
                    playerCurrentFrame++;
                    drawFishingBob = true;
                }
                
                if (playerCurrentFrame >= playerMaxFrame)
                {
                    playerCurrentFrame = 0;
                    drawFishingBob = false;
                }

                // Reset the time counter for a new frame
                timeCounter -= secondsPerFrame;
            }
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
            
            // Drawing in the player
            sb.Draw(texture,
                new Rectangle(500, 100, widthOfSingleSprite * 9, (int)(texture.Height / 5)),
                new Rectangle((playerCurrentFrame) * widthOfSingleSprite, (animationType * 32), widthOfSingleSprite, texture.Height / 44),
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
