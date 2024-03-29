﻿using System;
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
        public MainMenu(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.35, .55), ButtonType.Start, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.52, .55), ButtonType.Exit, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.CenterButton(ButtonType.Title, .15), ButtonType.Title, textures[(int)MenuTextures.GeneralButtons])
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
