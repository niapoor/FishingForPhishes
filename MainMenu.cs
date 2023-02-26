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
        /// <param name="fonts">list of all fonts</param>
        public MainMenu(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(new Point(790, 265), ButtonType.Start, textures[(int)MenuTextures.GeneralButtons]),
                new Button(new Point(790, 765), ButtonType.Exit, textures[(int)MenuTextures.GeneralButtons])
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