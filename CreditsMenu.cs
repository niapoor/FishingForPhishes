using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SharpDX.Direct2D1;

namespace ImagineRITGame
{
    internal class CreditsMenu : Menu
    {

        /// <summary>
        /// A constructor that defines a Credits Menu object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        /// <param name="fonts">list of all fonts</param>
        public CreditsMenu(List<Texture2D> textures, List<SpriteFont> fonts) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.02, .77), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons]),
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

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Color hoverColor, List<SpriteFont> fonts)
        {
            base.Draw(sb, hoverColor);
            string text;

            text = "Credits";
            sb.DrawString(fonts[0], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .08)), fonts[0]), Color.DarkGoldenrod);
            text = "Created By...";
            sb.DrawString(fonts[1], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .28)), fonts[1]), Color.DarkGoldenrod);
            text = "Nia Poor, Brandon Keller, Jaime Campanelli,";
            sb.DrawString(fonts[2], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .4)), fonts[2]), Color.DarkGoldenrod);
            text = "Alexa Krempa, Elijah Heilman, Lalitha Donga";
            sb.DrawString(fonts[2], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .47)), fonts[2]), Color.DarkGoldenrod);
            text = "Special Thanks To...";
            sb.DrawString(fonts[1], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .56)), fonts[1]), Color.DarkGoldenrod);
            text = "The National Science Foundation and OPM";
            sb.DrawString(fonts[2], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .68)), fonts[2]), Color.DarkGoldenrod);
            text = "Bo Yuan, Andy Meneely, Rajendra Raj";
            sb.DrawString(fonts[2], text, Game1.CenterText(text, (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .75)), fonts[2]), Color.DarkGoldenrod);
            
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
