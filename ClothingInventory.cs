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
        ShirtPantsCombo,    // dress, clown, pumpkin, spooky, witch (2 rows, 1 page)
        Shoes,              // shoes (1 row, 1 page)
        Accessories         // beard, glasses, glasses_sun (11 elements, 1 page)
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

            currentPage = ClothingInventoryPage.ShirtPantsCombo;

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
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .55)),
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
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .25833333)),
                new Rectangle(307, 47, 186, 62),
                Color.White, 0f,
                Vector2.Zero,
                0,
                .0001f);

            // "Customize" word
            sb.Draw(textures[(int)MenuTextures.ClothingInventory],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.423),
                (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.34)),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .4359375),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .25833333)),
                new Rectangle(307, 47, 186, 62),
                Color.White, 0f,
                Vector2.Zero,
                0,
                .0001f);

            switch (page)
            {
                case ClothingInventoryPage.Shirt:
                    DrawInShirts(sb); break;
                case ClothingInventoryPage.Pants:
                    DrawInPants(sb); break;
                case ClothingInventoryPage.Shoes:
                    DrawInShoes(sb); break;
                case ClothingInventoryPage.Hair:
                    DrawInHair(sb); break;
                case ClothingInventoryPage.Hat:
                    DrawInHat(sb); break;
                case ClothingInventoryPage.Body:
                    DrawInBody(sb); break;
                case ClothingInventoryPage.Accessories:
                    DrawInAccessories(sb); break;
                case ClothingInventoryPage.ShirtPantsCombo:
                    DrawInShirtPantsCombo(sb); break;
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
                        DrawItemClothing(sb, allClothes[(int)ClothingType.Shirt][j + ((currentScreen - 1) * 3)], row, 1.95, i, col, 0, 0, 10);
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
                        DrawItemClothing(sb, allClothes[(int)ClothingType.Pants][j + ((currentScreen - 1) * 3)], row, 1.95, i, col, -0.014, 0, 10);
                    }
                }
            }
        }

        /// <summary>
        /// Function to draw in the screens for pants.
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        public void DrawInShoes(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
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
                    if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Shoes].Count))
                    {
                        col++;
                        DrawItemClothing(sb, allClothes[(int)ClothingType.Shoes][j + ((currentScreen - 1) * 3)], row, 1.95, i, col, -0.023, 0, 10);
                    }
                }
            }
        }

        /// <summary>
        /// Function to draw in the screens for pants.
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        public void DrawInHair(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
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
                    if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Hair].Count))
                    {
                        col++;
                        DrawItemClothing(sb, allClothes[(int)ClothingType.Hair][j + ((currentScreen - 1) * 3)], row, 2.6, i, col, 0.064, 0.0115, 14);
                    }
                }
            }
        }

        /// <summary>
        /// Function to draw in the screens for pants.
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        public void DrawInHat(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            int col = 0;
            int row = 1;
            int currentScreen = 1;
            for (int j = 0; j < allClothes[(int)ClothingType.Hat].Count; j++)     // All the elements in the pants list
            {
                if(col == 9)
                    row++;
                if ((j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Hat].Count))
                {
                    col++;
                    if (j < 5)
                        DrawItemClothing(sb, allClothes[(int)ClothingType.Hat][j + ((currentScreen - 1) * 3)], row, 2.6, 1, col, 0.081, 0.0115, 1);
                    else
                        DrawItemClothing(sb, allClothes[(int)ClothingType.Hat][j + ((currentScreen - 1) * 3)], row, 2.6, 1, col, 0.064, 0.0115, 1);
                }
            }
        }


        /// <summary>
        /// Function to draw in the screens for pants.
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        public void DrawInBody(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            int col = 0;
            int row = 0;
            int currentScreen = 1;
            for (int j = 0; j < 3; j++)     // All the elements in the pants list
            {
                col = 0;
                row++;
                if (j == 0)
                {
                    for (int i = 1; i < 11; i++)
                    {
                        if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Body].Count))
                        {
                            col++;
                            DrawItemClothing(sb, allClothes[(int)ClothingType.Body][j + ((currentScreen - 1) * 3)], row, 2.6, i, col, 0.06, 0.0115, 8);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < 11; i++)
                    {
                        if ((j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Body].Count))
                        {
                            col++;
                            DrawHead(sb, allClothes[(int)ClothingType.Body][j + ((currentScreen - 1) * 3)], row, 3.5, i, col, 0.063, 0.0210, 8);
                        }
                    }
                }
            }
        }

        public void DrawInAccessories(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            int col = 0;
            int row = 0;
            int currentScreen = 1;
            for (int j = 0; j < 3; j++)     // All the elements in the pants list
            {
                //if (col == 10)
                //{
                    col = 0;
                    row++;
                //}
                if (j == 0)
                {
                    for (int i = 1; i < 11; i++)
                    {
                        if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Accessories].Count))
                        {
                            col++;
                            DrawItemClothing(sb, allClothes[(int)ClothingType.Accessories][j + ((currentScreen - 1) * 3)], row, 2.6, i, col, 0.05, 0.0115, 14);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < 11; i++)
                    {
                        if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.Accessories].Count))
                        {
                            col++;
                            DrawItemClothing(sb, allClothes[(int)ClothingType.Accessories][j + ((currentScreen - 1) * 3)], row, 2.2, i, col, 0.045, 0.006, 10);
                        }
                    }
                }
            }
        }

        public void DrawInShirtPantsCombo(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            int col = 0;
            int row = 1;
            int currentScreen = 1;
            for (int j = 0; j < 5; j++)     // All the elements in the pants list
            {
                if (col > 8)
                {
                    col = 0;
                    row++;
                }
                if (j == 0)
                {
                    for (int i = 1; i < 11; i++)
                    {
                        if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.ShirtPantsCombo].Count))
                        {
                            col++;
                            DrawItemClothing(sb, allClothes[(int)ClothingType.ShirtPantsCombo][j + ((currentScreen - 1) * 3)], row, 2, i, col, 0, 0.001, 10);
                        }
                    }
                }
                else if (j == 1 || j == 2)
                {
                    for (int i = 1; i < 3; i++)
                    {
                        if (i != 5 && (j + ((currentScreen - 1) * 3)) < (allClothes[(int)ClothingType.ShirtPantsCombo].Count))
                        {
                            col++;
                            DrawItemClothing(sb, allClothes[(int)ClothingType.ShirtPantsCombo][j + ((currentScreen - 1) * 3)], row, 2.4, i, col, 0.022, 0.009, 2);
                        }
                    }
                }
                else
                {
                    col++;
                    DrawItemClothing(sb, allClothes[(int)ClothingType.ShirtPantsCombo][j + ((currentScreen - 1) * 3)], row, 2.4, 1, col, 0.022, 0.009, 1);
                }
            }
        }


        public void DrawItemClothing(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D texture, int row, double sizing, int color, int column, double yAdd, double xAdd, int itemCount)
        {
            sb.Draw(texture,
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.398 + xAdd)) + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .0493 * (column - 1)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (.1348125 + (0.0515 * 2) + yAdd)) + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (0.046 * 2) * (row - 1))),    // First row y position + factor to get to specified row
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / sizing), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / sizing)),
                new Rectangle((texture.Width / itemCount) * (color - 1), 0, 160 / 5, texture.Height / 44),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .00000000001f);
        }

        public void DrawHead(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D texture, int row, double sizing, int color, int column, double yAdd, double xAdd, int itemCount)
        {
            sb.Draw(texture,
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (.398 + xAdd)) + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .0493 * (column - 1)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (.1348125 + (0.0515 * 2) + yAdd)) + (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (0.046 * 2) * (row - 1))),    // First row y position + factor to get to specified row
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / sizing), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / sizing)),
                new Rectangle((texture.Width / itemCount) * (color - 1) + 5, 0, 160 / 5 - 10, (texture.Height / 44) - 11),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .00000000001f);
        }


    }
}
