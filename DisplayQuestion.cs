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
    internal class DisplayQuestion : Menu
    {

        /// <summary>
        /// A constructor that defines a DisplayQuestion object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        public DisplayQuestion(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.35, .55), ButtonType.A, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.45, .55), ButtonType.B, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.55, .55), ButtonType.C, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.65, .55), ButtonType.D, textures[(int)MenuTextures.GeneralButtons])
            };
        }

    }
}
