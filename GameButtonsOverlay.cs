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
    internal class GameButtonsOverlay : Menu
    {
        /// <summary>
        /// A constructor that defines a Pause Menu object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        public GameButtonsOverlay(List<Texture2D> textures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.17, .03), ButtonType.ViewItems, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.02, .03), ButtonType.Pause, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.7, .03), ButtonType.OutfitShop, textures[(int)MenuTextures.GeneralButtons])
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

        public void DrawStoreCredit(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Shop shop, List<SpriteFont> fonts)//177, 116
        {
            // width: 2732, height: 2048
            // Drawing in the RIT CyberCorps logo in the bottom right. This will always be drawn in regardless of other settings.
            // Drawing in size and position is dynamic based on the screen size
            sb.Draw(textures[(int)MenuTextures.ClothingInventory], new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .845), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .03),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.0609375 * 2.3)), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.06041667 * 2.3))), new Rectangle(304, 432, 177, 116), Color.White);

            sb.DrawString
            (fonts[2],
            shop.CurrentBalance.ToString(),
            new Vector2((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.885), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.12)),
            Color.DarkGoldenrod);
        }

    }
}
