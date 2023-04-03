using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Reflection;
using SharpDX.Direct2D1;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagineRITGame
{
    enum ClothingInventoryPage
    {
        Body,               // char_all, eyes (2 rows, 1 page)
        Hat,                // hat_cowboy, hat_lucky, hat_pumpkin, hat_pumpkin_purple, hat_witch, mask_clown_blue, mask_clown_red, mask_spooky (8 elements, 1 page)
        Hair,               // bob, braids, buzzcut, curly, emo, extra_long, extra_long_skirt, french_curl, gentleman, long_straight, long_straight_skirt, midiwave, ponytail, spacebuns, wavy (15 rows, 5 pages)
        Shirt,              // basic, floral, sailor, sailor_bow, skull, spaghetti, sporty, stripe, suit, overalls (10 rows, 4 pages)
        Pants,              // pants, skirt, pants_suit (3 rows, 1 page)
        ShirtPantsCombo,    // clown, dress, pumpkin, spooky, witch (2 rows, 1 page)
        Shoes,              // shoes (1 row, 1 page)
        Accessories         // beard, earring (x4), glasses, glasses_sun (7 elements, 1 page)
    }

    internal class ClothingInventory : Menu
    {

        private ClothingInventoryPage currentPage;

        public ClothingInventoryPage CurrentPage
        {
            get
            {
                return currentPage;
            }
            set { currentPage = value; }
        }

        private List<List<Texture2D>> allClothes;

        /// <summary>
        /// A constructor that defines an Inventory object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        public ClothingInventory(List<Texture2D> textures, List<SpriteFont> fonts, List<List<Texture2D>> allOutfitTextures) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.04, .75), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons])
            };

            currentPage = ClothingInventoryPage.Shirt;

            allClothes = allOutfitTextures;
        }

        public void Update(GameTime gameTime)
        {
            base.Update();
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Draw(sb, Color.Goldenrod);
            DrawClothingInventoryBackgrop(sb, currentPage);
        }

        public void DrawClothingInventoryBackgrop(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, ClothingInventoryPage page)
        {
            int yChange = 0;
            switch (page)
            {
                case ClothingInventoryPage.Hat:
                    yChange= 132 + (12 * 1);
                    break;
                case ClothingInventoryPage.Hair:
                    yChange = (132 * 2) + (12 * 2);
                    break;
                case ClothingInventoryPage.Shirt:
                    yChange = (132 * 3) + (12 * 3);
                    break;
                case ClothingInventoryPage.Pants:
                    yChange = (132 * 4) + (12 * 4);
                    break;
                case ClothingInventoryPage.ShirtPantsCombo:
                    yChange = (132 * 5) + (12 * 5);
                    break;
                case ClothingInventoryPage.Shoes:
                    yChange = (132 * 6) + (12 * 6);
                    break;
                case ClothingInventoryPage.Accessories:
                    yChange = (132 * 7) + (12 * 7);
                    break;
            }

            // Drawing in the inventory background
            sb.Draw(textures[(int)MenuTextures.ClothingInventory],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.2),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.14)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .68671875)),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .309375)),
                new Rectangle(0, yChange, 293, 132),
                Color.White, 0f,
               Vector2.Zero,
               0,
               .0001f);

            // Drawing in the background boxes
            sb.Draw(textures[(int)MenuTextures.ClothingInventory],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.423),
                (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.34)),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .4359375),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .1453125)),
                new Rectangle(307, 47, 186, 62),
                Color.White, 0f,
                Vector2.Zero,
                0,
                .0001f);

            switch (page)
            {
                case ClothingInventoryPage.Shirt:
                    DrawInShirts(sb);
                    break;
                case ClothingInventoryPage.Pants:
                    DrawInPants(sb);
                    break;
            }
        }

        /// <summary>
        /// Function to draw in the screens for shirts. Change the "currentScreen" variable to change screens (4 total)
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        public void DrawInShirts(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {

            int col = 0;
            int row = 0;
            int currentScreen = 1;
            for (int j = 0; j < 3; j++)     // All the elements in the shirts list
            {
                col = 0;
                row++;
                for (int i = 1; i < 11; i++)
                {
                    if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Shirt].Count))
                    {
                        col++;
                        Draw10ItemClothing(sb, allClothes[(int)ClothingType.Shirt][j + ((currentScreen - 1) * 3)], row, 1.95, i, col, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Function to draw in the screens for pants.
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        public void DrawInPants(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {

            int col = 0;
            int row = 0;
            int currentScreen = 1;
            for (int j = 0; j < 3; j++)     // All the elements in the pants list
            {
                col = 0;
                row++;
                for (int i = 1; i < 11; i++)
                {
                    if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Pants].Count))
                    {
                        col++;
                        Draw10ItemClothing(sb, allClothes[(int)ClothingType.Pants][j + ((currentScreen - 1) * 3)], row, 1.95, i, col, -0.009);
                    }
                }
            }
        }

        public void Draw10ItemClothing(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D texture, int row, double sizing, int color, int column, double yAdd)
        {
            sb.Draw(texture,
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .398) + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .0493 * (column - 1)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.1348125 + yAdd)) + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (0.0515) * (row - 1))),    // First row y position + factor to get to specified row
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / sizing), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / sizing)),
                new Rectangle((texture.Width / 10) * (color - 1), 0, 160 / 5, texture.Height / 44),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .00000000001f);
        }

    }
}
