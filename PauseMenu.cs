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
        /// <param name="fonts">list of all fonts</param>
        public PauseMenu(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2560) * 140, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1140) * 1060), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons]),
                new Button(new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2560) * 2090, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1140) * 1060), ButtonType.MainMenu, textures[(int)MenuTextures.GeneralButtons])
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
    }
}
