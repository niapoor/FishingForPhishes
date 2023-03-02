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
    internal class MainMenu : Menu
    {

        /// <summary>
        /// A constructor that defines a Main Menu object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        /// <param name="fonts">list of all fonts</param>
        public MainMenu(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(new Point(790, 765), ButtonType.Start, textures[(int)MenuTextures.GeneralButtons]),
                new Button(new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2560) * 1390, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1140) * 765), ButtonType.Exit, textures[(int)MenuTextures.GeneralButtons]),
                new Button(new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2560) * 860, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1140) * 265), ButtonType.Title, textures[(int)MenuTextures.GeneralButtons])
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

        public void Draw(SpriteBatch sb, Color hoverColor)
        {
            base.Draw(sb, hoverColor);

         //   sb.Draw(textures[(int)MenuTextures.TitleCard],
         //       new Rectangle(800, 100, 894, 588),
         //       new Rectangle(0, 0, 149, 98),
         //       Color.White);
        }

    }
}
