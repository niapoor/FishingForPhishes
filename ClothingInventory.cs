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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SharpDX.DXGI;

namespace ImagineRITGame
{
    enum ClothingInventoryPage
    {
        Body = 0,               // char_all, eyes (2 rows, 1 page)
        Hat = 1,                // hat_cowboy, hat_lucky, hat_pumpkin, hat_pumpkin_purple, hat_witch, mask_clown_blue, mask_clown_red, mask_spooky (8 elements, 1 page)
        Hair = 2,               // bob, braids, buzzcut, curly, emo, extra_long, extra_long_skirt, french_curl, gentleman, long_straight, long_straight_skirt, midiwave, ponytail, spacebuns, wavy (15 rows, 5 pages)
        Shirt = 3,              // basic, floral, sailor, sailor_bow, skull, spaghetti, sporty, stripe, suit, overalls (10 rows, 4 pages)
        Pants = 4,              // pants, skirt, pants_suit (3 rows, 1 page)
        ShirtPantsCombo = 5,    // dress, clown, pumpkin, spooky, witch (2 rows, 1 page)
        Shoes = 6,              // shoes (1 row, 1 page)
        Accessories = 7         // beard, glasses, glasses_sun (11 elements, 1 page)
    }

    internal class ClothingInventory : Menu
    {

        private ClothingInventoryPage currentPage;
        private int currentScreen;

        private List<Button> buttonsSelectedPage;
        private List<Button> buttonsInventoryItems;

        public ClothingInventoryPage CurrentPage
        {
            get
            {
                return currentPage;
            }
            set { currentPage = value; }
        }

        public int CurrentScreen
        {
            get
            {
                return currentScreen;
            }
            set { currentScreen = value; }
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
                new Button(base.AlignButton(.04, .75), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.8191, .6055), ButtonType.RightArrow, textures[(int)MenuTextures.ClothingInventory]),
                new Button(base.AlignButton(.423, .6055), ButtonType.LeftArrow, textures[(int)MenuTextures.ClothingInventory]),
                new Button(base.AlignButton(.224, .354), ButtonType.XButton, textures[(int)MenuTextures.ClothingInventory]),
                new Button(base.AlignButton(.224, .429), ButtonType.XButton, textures[(int)MenuTextures.ClothingInventory]),
                new Button(base.AlignButton(.224, .504), ButtonType.XButton, textures[(int)MenuTextures.ClothingInventory]),
                new Button(base.AlignButton(.352, .504), ButtonType.XButton, textures[(int)MenuTextures.ClothingInventory])
            };
            
            // These buttons only exists to be hovered over. It is the raised, selected page button
            buttonsSelectedPage = new List<Button>() {
                new Button(base.AlignButton(.474 + (.0493 * 0), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.04946 * 1), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 2), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 3), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 4), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 5), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.04929 * 6), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 7), .165), ButtonType.OutfitShopCurrentPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 0), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.04946 * 1), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 2), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 3), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 4), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 5), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.04929 * 6), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.474 + (.0493 * 7), .1982), ButtonType.OutfitShopAnotherPage, textures[(int)MenuTextures.GeneralButtons])
            };

            // These buttons are used to switch between the different inventory pages
            buttonsInventoryItems = new List<Button>();

            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    buttonsInventoryItems.Add(new Button(base.AlignButton(0.42105 + ((0.0071076825 + .04211956) * row), 0.3365 + ((.07487923 + 0.01684782) * col)), ButtonType.OutfitInventorySlot, textures[(int)MenuTextures.ClothingInventory]));
                }
            }

            // The default page will always be the body
            currentPage = ClothingInventoryPage.Body;
            
            // Absolutely all of the outfit textures!
            allClothes = allOutfitTextures;
            // The current screen in the current inventory page
            currentScreen = 1;
        }

        /// <summary>
        /// Updating the inventory (i.e. the buttons)
        /// </summary>
        /// <param name="gameTime">The current time in the game</param>
        public void Update(GameTime gameTime)
        {

            // === Buttons for currently selected page ===
            foreach (Button b in buttonsSelectedPage)
            {
                if (b.IsHovered)
                    currentButton = b;
            }

            // Check to see if the button should be activated
            if (currentButton != null)
            {
                if ((Mouse.GetState().LeftButton == ButtonState.Pressed &&
                prevMState.LeftButton == ButtonState.Released)
                && currentButton.IsHovered)
                {
                    ButtonActivated((int)currentButton.ButtonType);
                }
            }

            // === Buttons for all current inventory slots ===
            foreach (Button b in buttonsInventoryItems)
            {
                if (b.IsHovered)
                    currentButton = b;
            }

            // Check to see if the button should be activated
            if (currentButton != null)
            {
                if ((Mouse.GetState().LeftButton == ButtonState.Pressed &&
                prevMState.LeftButton == ButtonState.Released)
                && currentButton.IsHovered)
                {
                    ButtonActivated((int)currentButton.ButtonType);
                }
            }

            // Updating the "back" button
            base.Update();

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            DrawClothingInventoryBackgrop(sb, currentPage);

            base.ConditionalDraw(sb, Color.Goldenrod, ShouldDrawRightArrow(), ShouldDrawLeftArrow());

            DrawPageSelectionButtons(sb);
        }

        public void DrawPageSelectionButtons(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            buttonsSelectedPage[(int)currentPage].Draw(sb, buttonsSelectedPage[(int)currentPage].IsHovered ? new Color(Color.DarkGoldenrod, 0.45f) : Color.Transparent);
            for (int i = 0; i < 8; i++)
            {
                if (i != (int)currentPage)
                    buttonsSelectedPage[i + 8].Draw(sb, buttonsSelectedPage[i + 8].IsHovered ? new Color(Color.DarkGoldenrod, 0.45f) : Color.Transparent);

            }
        }
        
        public bool ShouldDrawRightArrow()
        {
            if ((currentPage == ClothingInventoryPage.Shirt && currentScreen < 4) || (currentPage == ClothingInventoryPage.Hair && currentScreen < 5))
                return true;
            return false;
        }

        public bool ShouldDrawLeftArrow()
        {
            if ((currentPage == ClothingInventoryPage.Shirt && currentScreen > 1) || (currentPage == ClothingInventoryPage.Hair && currentScreen > 1))
                return true;
            return false;
        }

        public void UpdateCurrentPage()
        {
            for (int i = 0; i < 8; i++)
            {
                if (buttonsSelectedPage[i + 8].IsHovered)
                {
                    currentPage = (ClothingInventoryPage)i;
                    currentScreen = 1;
                }
            }
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
            //sb.Draw(textures[(int)MenuTextures.ClothingInventory],
            //    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.423),
            //    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.34)),
            //    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .4359375),
            //    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .25833333)),
            //    new Rectangle(307, 47, 186, 62),
            //    Color.White, 0f,
            //    Vector2.Zero,
            //    0,
            //    .0001f);
            DrawBoxes(sb);


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
                            DrawItemClothing(sb, allClothes[(int)ClothingType.Body][j + ((currentScreen - 1) * 3)], row, 2.6, i, col, 0.06, 0.0115, 14);
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
            for (int j = 0; j < 5; j++)     // All the elements in the pants list
            {
                if (col > 8)
                {
                    col = 0;
                    row++;
                }
                if (j == 0) // These if statements are hard coded. This is because we know our list of outfit textures; it is static and will not change.
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

        public void DrawBoxes(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            int numBoxes = 0;
            int currentBox = 0;
            switch (currentPage)
            {
                case ClothingInventoryPage.Shirt:
                    numBoxes = 27;
                    if (currentScreen == 4)
                        numBoxes = 9;
                    break;
                case ClothingInventoryPage.Pants:
                    numBoxes = 27;
                    break;
                case ClothingInventoryPage.Shoes:
                    numBoxes = 9;
                    break;
                case ClothingInventoryPage.Hair:
                    numBoxes = 27;
                    break;
                case ClothingInventoryPage.Hat:
                    numBoxes = 8;
                    break;
                case ClothingInventoryPage.Body:
                    numBoxes = 17;
                    break;
                case ClothingInventoryPage.Accessories:
                    numBoxes = 27;
                    break;
                case ClothingInventoryPage.ShirtPantsCombo:
                    numBoxes = 15;
                    break;
            }

            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    currentBox++;
                    if (currentBox <= numBoxes)
                    {
                        sb.Draw(textures[(int)MenuTextures.ClothingInventory],
                            new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (0.423 + ((0.0071076825 + .04211956) * row))),
                            (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (0.34 + ((.07487923 + 0.01684782) * col))),
                            (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .04211956),
                            (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .07487923)),
                            new Rectangle(307, 47, 18, 18),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            0,
                            .0001f);
                        buttonsInventoryItems[currentBox - 1].Draw(sb, buttonsInventoryItems[currentBox - 1].IsHovered ? Color.DarkGoldenrod : Color.Transparent);
                    }
                }
            }

        }

        public void LeftArrowPress()
        {
            if (currentPage == ClothingInventoryPage.Shirt && currentScreen > 1)
                currentScreen--;
            else if (currentPage == ClothingInventoryPage.Hair && currentScreen > 1)
                currentScreen--;
        }

        public void RightArrowPress()
        {
            if (currentPage == ClothingInventoryPage.Shirt && currentScreen < 4)
                currentScreen++;
            else if (currentPage == ClothingInventoryPage.Hair && currentScreen < 5)
                currentScreen++;
        }

        public int FindIndexButtonSelected()
        {
            int numBoxes = 0;
            int currentBox = 0;
            switch (currentPage)
            {
                case ClothingInventoryPage.Shirt:
                    numBoxes = 27;
                    if (currentScreen == 4)
                        numBoxes = 9;
                    break;
                case ClothingInventoryPage.Pants:
                    numBoxes = 27;
                    break;
                case ClothingInventoryPage.Shoes:
                    numBoxes = 9;
                    break;
                case ClothingInventoryPage.Hair:
                    numBoxes = 27;
                    break;
                case ClothingInventoryPage.Hat:
                    numBoxes = 8;
                    break;
                case ClothingInventoryPage.Body:
                    numBoxes = 17;
                    break;
                case ClothingInventoryPage.Accessories:
                    numBoxes = 27;
                    break;
                case ClothingInventoryPage.ShirtPantsCombo:
                    numBoxes = 15;
                    break;
            }

            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    currentBox++;
                    if (currentBox <= numBoxes)
                    {
                        if (buttonsInventoryItems[currentBox - 1].IsHovered)
                            return currentBox - 1;
                    }
                }
            }
            return -1;
        }

        public Outfit EditArticle(Outfit outfit)
        {
            int buttonSelected = FindIndexButtonSelected();
            switch (currentPage)
            {
                case ClothingInventoryPage.Shirt:
                    if (buttonSelected >= 4)
                        buttonSelected++;
                    if (buttonSelected >= 14)
                        buttonSelected++;
                    if (buttonSelected >= 24)
                        buttonSelected++;
                    outfit.ChangeCurrentArticle(ClothingType.Shirt, (int)Math.Floor((double)(buttonSelected / (double)10)) + ((currentScreen - 1) * 3), buttonSelected % 10);
                    break;
                case ClothingInventoryPage.Pants:
                    if (buttonSelected >= 4)
                        buttonSelected++;
                    if (buttonSelected >= 14)
                        buttonSelected++;
                    if (buttonSelected >= 24)
                        buttonSelected++;
                    outfit.ChangeCurrentArticle(ClothingType.Pants, (int)Math.Floor((double)buttonSelected / (double)10), buttonSelected % 10);
                    break;
                case ClothingInventoryPage.Shoes:
                    if (buttonSelected >= 4)
                        buttonSelected++;
                    if (buttonSelected >= 14)
                        buttonSelected++;
                    if (buttonSelected >= 24)
                        buttonSelected++;
                    outfit.ChangeCurrentArticle(ClothingType.Shoes, 0, buttonSelected);
                    break;
                case ClothingInventoryPage.Hair:
                    if (buttonSelected >= 4)
                        buttonSelected++;
                    if (buttonSelected >= 14)
                        buttonSelected++;
                    if (buttonSelected >= 24)
                        buttonSelected++;
                    outfit.ChangeCurrentArticle(ClothingType.Hair, (int)Math.Floor((double)buttonSelected / (double)10) + ((currentScreen - 1) * 3), buttonSelected % 10);
                    break;
                case ClothingInventoryPage.Hat:
                    outfit.ChangeCurrentArticle(ClothingType.Hat, buttonSelected, 0);
                    break;
                case ClothingInventoryPage.Body:
                    if (buttonSelected < 9)
                    {
                        if (buttonSelected >= 4)
                            buttonSelected++;
                        outfit.ChangeCurrentArticle(ClothingType.Body, 0, buttonSelected);
                    }
                    else
                        outfit.ChangeCurrentArticle(ClothingType.Body, 1, buttonSelected - 9);
                    break;
                case ClothingInventoryPage.Accessories:
                    if (buttonSelected >= 4)
                        buttonSelected++;
                    if (buttonSelected >= 14)
                        buttonSelected++;
                    if (buttonSelected >= 24)
                        buttonSelected++;
                    outfit.ChangeCurrentArticle(ClothingType.Accessories, (int)Math.Floor((double)buttonSelected / (double)10), buttonSelected % 10);
                    break;
                case ClothingInventoryPage.ShirtPantsCombo:
                    if (buttonSelected >= 4)
                        buttonSelected++;
                    if (buttonSelected < 10)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 0, buttonSelected % 10);
                    if (buttonSelected == 10)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 1, 0);
                    if (buttonSelected == 11)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 1, 1);
                    if (buttonSelected == 12)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 2, 0);
                    if (buttonSelected == 13)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 2, 1);
                    if (buttonSelected == 14)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 3, 0);
                    if (buttonSelected == 15)
                        outfit.ChangeCurrentArticle(ClothingType.ShirtPantsCombo, 4, 0);
                    break;
            }
            return outfit;
        }

        public Outfit RemoveArticle(Outfit outfit)
        {
            if (buttons[3].IsHovered)
                outfit.ChangeCurrentArticle(ClothingType.Hat, 0, -1);
            if (buttons[4].IsHovered)
                outfit.ChangeCurrentArticle(ClothingType.Hair, 0, -1);
            if (buttons[5].IsHovered)
                outfit.ChangeCurrentArticle(ClothingType.Accessories, 0, -1);
            if (buttons[6].IsHovered)
                outfit.ChangeCurrentArticle(ClothingType.Shoes, 0, -1);
            return outfit;

        }

    }
}
