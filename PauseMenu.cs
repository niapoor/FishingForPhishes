using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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

            text = currentDifficulty.ToString();
            sb.DrawString(fonts[0], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .08)), fonts[0]), Color.DarkGoldenrod);

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

    }
}
