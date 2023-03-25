﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Dynamic;
using SharpDX.Direct3D9;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ImagineRITGame
{

    // An enum that holds all of the states the fish can be in
    enum FishStates
    {
        FadeIn,
        FadeOut,
        Swim,
        Invisible
    }

    internal class Fish
    {

        // The player's current state (walking, jumping, idle, or dying)
        private FishStates fishState;
        private FishStates prevState;

        private Texture2D texture;
        private Vector2 position;

        // Keyboard states
        private KeyboardState kbState;
        private KeyboardState prevKBState;

        // Fields for Player animation data
        private double fps;
        private double timeCounter;
        private int fishCurrentFrame;
        private int fishMaxFrame;
        private double secondsPerFrame;
        private int widthOfSingleSprite;
        private int animationIndex;

        public Fish(Texture2D sprite, Vector2 location)
        {
            texture = sprite;
            position = location;

            // The player is idle by default because the game starts with them not moving
            fishState = FishStates.Invisible;
            prevState = FishStates.Invisible;

            // Initializing the keyboard states
            prevKBState = new KeyboardState();
            kbState = new KeyboardState();

            // Assigning default values to animation data
            fps = 8.0;
            secondsPerFrame = 1 / fps;
            timeCounter = 0;
            fishCurrentFrame = 0;
            fishMaxFrame = 15;
            widthOfSingleSprite = (240 / 15); // Height is 16
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            // Update the time since the last frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Update secondsPerFrame, as fps could have been changed
            secondsPerFrame = 1 / fps;

            // Check that enough time has passed between frames
            if (timeCounter >= secondsPerFrame)
            {
                // Increment the active frame
                fishCurrentFrame++;

                if (fishCurrentFrame >= fishMaxFrame)
                {
                    fishCurrentFrame = 0;
                }

                // Reset the time counter for a new frame
                timeCounter -= secondsPerFrame;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            // Draw in the fish
            sb.Draw(texture,
               new Rectangle((int)position.X, (int)position.Y, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 16, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 16),
               new Rectangle(fishCurrentFrame * widthOfSingleSprite, 0, widthOfSingleSprite, 16),
               Color.White,
               0f,
               Vector2.Zero,
               0,
               .01f);
        }

    }
}
