using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using static System.Net.Mime.MediaTypeNames;

namespace ImagineRITGame
{
    internal class PauseMenu : Menu
    {
        /// <summary>
        /// A constructor that defines a Pause Menu object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        public PauseMenu(List<Texture2D> textures, List<SpriteFont> fonts) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.07, .2), ButtonType.ChangeMode, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.07, .4), ButtonType.MainMenu, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.07, .6), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons])
            };
        }

        /// <summary>
        /// Updating the planet's animation and updating information about the buttons
        /// </summary>
        /// <param name="gameTime">info about the time from MonoGame</param>
        public void Update(GameTime gameTime)
        {
            base.Update();
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Color hoverColor, List<SpriteFont> fonts, Difficulty currentDifficulty)
        {
            base.Draw(sb, hoverColor);
            string text;

            DrawMode(sb, fonts, currentDifficulty);

            DrawInstructions(sb, fonts);
            // This is a debug thing to see the screen size when drawing text
            /*
            text = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height.ToString();
            sb.DrawString(fonts[2], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .8)), fonts[2]), Color.DarkGoldenrod);
            text = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width.ToString();
            sb.DrawString(fonts[2], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .85)), fonts[2]), Color.DarkGoldenrod);
            */

            //   sb.Draw(textures[(int)MenuTextures.TitleCard],
            //       new Rectangle(800, 100, 894, 588),
            //       new Rectangle(0, 0, 149, 98),
            //       Color.White);
        }

        public void DrawMode(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts, Difficulty currentDifficulty)
        {
            string text;
            text = "Current Mode:";
            sb.DrawString(fonts[2], text, new Vector2((int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) * .23), (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .22))), Color.DarkGoldenrod);

            text = currentDifficulty.ToString();
            sb.DrawString(fonts[1], text, new Vector2((int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) * .23), (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .26))), Color.DarkGoldenrod);
        }

        public void DrawInstructions(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts)
        {
            // Drawing in the fishing post card backdrop
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.515), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.16),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.25), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.17) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .1))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing tutorial 1
            string text = "When a fish appears, press the spacebar to try to catch it";
            List<string> currentTextList = base.WrapText(text, 17);
            double y = ((8 - currentTextList.Count) * .06) - 0.05;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    new Vector2((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.54), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * ((.06 * i) + y)))),
                    Color.Black);
            }

            // Drawing in the fishing post card backdrop
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.515), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.51),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.25), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.17) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .1))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing tutorial 2
            text = "Press the spacebar to try and catch another fish";
            currentTextList = base.WrapText(text, 15);
            y = ((8 - currentTextList.Count) * .06) + 0.3;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    new Vector2((int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.54)), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * ((.06 * i) + y)))),
                    Color.Black);
            }
        }

    }
}