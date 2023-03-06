using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

// TODO:    Hovering over a fish shows its name and how many of it have been caught
//          Fish are only drawn in to the postcard once they have been caught

namespace ImagineRITGame
{
    internal class Inventory : Menu
    {
        /// <summary>
        /// A constructor that defines an Inventory object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        public Inventory(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.07, .65), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons])
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

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Color hoverColor)
        {
            base.Draw(sb, hoverColor);

            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.3), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.12),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.4), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.4)),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing shadows of the fish (if they have not been caught)
            sb.Draw(textures[(int)MenuTextures.FishInvShadow],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.335), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.183),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332)),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishInvShadow].Width, textures[(int)MenuTextures.FishInvShadow].Height),
                Color.Goldenrod);

            
            // Drawing in the fish on top of the port card (if they have been caught)
            sb.Draw(textures[(int)MenuTextures.AllFish],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.335), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.183),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332)),
                new Rectangle(0, 0, textures[(int)MenuTextures.AllFish].Width, textures[(int)MenuTextures.AllFish].Height),
                Color.White);
            
        }

    }
}
