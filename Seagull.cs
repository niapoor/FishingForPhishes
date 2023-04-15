using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagineRITGame
{
    internal class Seagull
    {

        private Texture2D texture;
        private Vector2 position;

        // Fields for Player animation data
        private double fps;
        private double timeCounter;
        private int gullCurrentFrame;
        private int gullMaxFrame;
        private double secondsPerFrame;
        private int widthOfSingleSprite;
        private int animationIndex;
        private float aspectRatioFactor;

        public float XPos
        {
            get { return position.X; } set { position.X = (float)value; }
        }

        public Seagull(Texture2D seagullTexture, Vector2 location) 
        {
            texture = seagullTexture;
            position = location;

            // Assigning default values to animation data
            fps = 10.0;
            secondsPerFrame = 1 / fps;
            timeCounter = 0;
            gullCurrentFrame = 0;
            gullMaxFrame = 3;
            widthOfSingleSprite = 17;

            if ((float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) < (float)((float)1920 / (float)1080))
                aspectRatioFactor = (float)((float)((float)1920 / (float)1080) - (float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            else
                aspectRatioFactor = 0;
        }

        public int CurrentFrame
        {
            get
            {
                return gullCurrentFrame;
            }
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
                gullCurrentFrame++;

                // Reset which frame is being drawn in once the end is reached
                if (gullCurrentFrame >= gullMaxFrame)
                {
                    gullCurrentFrame = 0;
                }

                // Reset the time counter for a new frame
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Updates the position of the seagull flying across the screen
        /// </summary>
        /// <returns>Whether the seagull has flewn off the screen yet</returns>
        public void UpdatePosition()
        {
            gullMaxFrame = 2;
            fps = 4;
            widthOfSingleSprite = 18;
            // Add the the X position (fly across the screen)
            position.X = position.X + 5;
        }

        public bool ShoulUpdatePosition()
        {
            if (position.X >= (int)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                return false;
            return true;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            // Seagull
            sb.Draw(texture,
               new Rectangle((int)position.X, (int)position.Y + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .03),
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 15, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 15),
               new Rectangle(139 + (gullCurrentFrame * widthOfSingleSprite), 373, widthOfSingleSprite, 17),
               Color.White,
               0f,
               Vector2.Zero,
               0,
               .01f);
        }

        public void DrawFlying(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            // Seagull
            sb.Draw(texture,
               new Rectangle((int)position.X, (int)position.Y + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .03),
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 15, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 15),
               new Rectangle(265 + (gullCurrentFrame * widthOfSingleSprite) + (gullCurrentFrame - 2), 370, widthOfSingleSprite + 1, 20),
               Color.White,
               0f,
               Vector2.Zero,
               SpriteEffects.FlipHorizontally,
               .1f);
        }

    }
}
