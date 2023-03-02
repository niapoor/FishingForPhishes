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
                new Button(new Point(140, 1060), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons])
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
            sb.DrawString(fonts[1], text, Game1.CenterText(text, 150, fonts[1]), Color.DarkGoldenrod);

            //   sb.Draw(textures[(int)MenuTextures.TitleCard],
            //       new Rectangle(800, 100, 894, 588),
            //       new Rectangle(0, 0, 149, 98),
            //       Color.White);
        }


    }
}
