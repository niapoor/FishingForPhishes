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
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;

namespace ImagineRITGame
{
    enum ClothingType
    {
        Body = 0,               // char_all, eyes (2 rows, 1 page)
        Hat = 1,                // hat_cowboy, hat_lucky, hat_pumpkin, hat_pumpkin_purple, hat_witch, mask_clown_blue, mask_clown_red, mask_spooky (8 elements, 1 page)
        Hair = 2,               // bob, braids, buzzcut, curly, emo, extra_long, extra_long_skirt, french_curl, gentleman, long_straight, long_straight_skirt, midiwave, ponytail, spacebuns, wavy (15 rows, 5 pages)
        Shirt = 3,              // basic, floral, overalls, sailor, sailor_bow, skull, spaghetti, sporty, stripe, suit (10 rows, 4 pages)
        Pants = 4,              // pants, pants_suit, skirt (3 rows, 1 page)
        ShirtPantsCombo = 5,    // clown, dress, pumpkin, spooky, witch (2 rows, 1 page)
        Shoes = 6,              // shoes (1 row, 1 page)
        Accessories = 7         // beard, earring (x4), glasses, glasses_sun (7 elements, 1 page)
    }

    internal class Outfit
    {
        private List<List<Texture2D>> allClothes;

        private Texture2D currentBody;
        private Texture2D currentHat;
        private Texture2D currentHair;
        private Texture2D currentShirt;
        private Texture2D currentPants;
        private Texture2D currentShoes;
        private Texture2D currentAccessories;



        public Outfit(List<List<Texture2D>> allOutfitTextures)
        {
            allClothes = allOutfitTextures;
            currentBody = allClothes[(int)ClothingType.Body][0];
            currentHair = allClothes[(int)ClothingType.Hair][0];
            currentShirt = allClothes[(int)ClothingType.Shirt][0];
            currentPants = allClothes[(int)ClothingType.Pants][0];
            currentShoes = allClothes[(int)ClothingType.Shoes][0];

            currentHat = allClothes[(int)ClothingType.Hat][0];
            currentAccessories = allClothes[(int)ClothingType.Accessories][1];
        }

        public void DrawOutfitInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            double xLoc = .343;
            double yLoc = .392;

            //1, 2, 10, 14, 8 (just characters)
            DrawArticleInInventory(sb, currentShirt, .3295, .255, false, ClothingType.Shirt);
            //DrawArticleIdle(sb, currentShirt, .3433, .324, false, 10);
            DrawArticleInInventory(sb, currentPants, .3295, .31, false, ClothingType.Pants);
            DrawArticleInInventory(sb, currentShoes, .3295, .375, false, ClothingType.Shoes);
            DrawArticleInInventory(sb, currentHat, .2145, .335, true, ClothingType.Hat);
            DrawArticleInInventory(sb, currentHair, .2145, .39, true, ClothingType.Hair);
            DrawArticleInInventory(sb, currentAccessories, .2031, .435, false, ClothingType.Accessories);
        }

        public void DrawArticleInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D article, double xLoc, double yLoc, bool makeSmaller, ClothingType type)
        {
            if (article != null)
            {
                // If the accessory is the beard there are 14 options
                if (article.Width == 3584)
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 14);
                // If the accessory is a type of glasses there are 10 options
                else if (article.Width == 2560)
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 10);
                else if (article.Width == 512)
                {
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 2);
                }
                // If the article has 1 option
                else
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 1);
            }
        }

        public void DrawOutfitOnPlayer(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Player player, int currentFrame, int animationType, double xLoc, double yLoc)
        {
            double conditionalY = 0;
            if (player.PlayerState == PlayerStates.Idle)
                conditionalY = 0.04;

            List<Texture2D> outfitForPlayer = new List<Texture2D>();
            outfitForPlayer.Add(currentBody);
            if (currentHair != null)
                outfitForPlayer.Add(currentHair);
            outfitForPlayer.Add(currentShirt);
            outfitForPlayer.Add(currentPants);
            if (currentShoes != null)
                outfitForPlayer.Add(currentShoes);
            if (currentHat != null)
                outfitForPlayer.Add(currentHat);
            if (currentAccessories != null)
                outfitForPlayer.Add(currentAccessories);

            for (int i = 0; i < outfitForPlayer.Count; i++)
                sb.Draw(outfitForPlayer[i],
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * xLoc), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (yLoc - conditionalY)),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765)),
                    new Rectangle((currentFrame * (160 / 5)) + (0 * 8 * (160 / 5)), (animationType * 32), 160 / 5, outfitForPlayer[i].Height / 44),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .86f);
        }

        public void DrawInventoryPlayer(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            List<Texture2D> outfitForPlayer = new List<Texture2D>();
            outfitForPlayer.Add(currentBody);
            if (currentHair != null)
                outfitForPlayer.Add(currentHair);
            outfitForPlayer.Add(currentShirt);
            outfitForPlayer.Add(currentPants);
            if (currentShoes != null)
                outfitForPlayer.Add(currentShoes);
            if (currentHat != null)
                outfitForPlayer.Add(currentHat);
            if (currentAccessories != null)
                outfitForPlayer.Add(currentAccessories);

            for (int i = 0; i < outfitForPlayer.Count; i++)
                sb.Draw(outfitForPlayer[i],
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .2525), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .32),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / 1.5), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / 1.5)),
                    new Rectangle((0 * 8 * (160 / 5)), 0, 160 / 5, outfitForPlayer[i].Height / 44),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .00000000001f);
        }

        public void DrawArticleIdle(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D article, double xLoc, double yLoc, bool makeSmaller, int spriteCount)
        {
            double sizing = 1.95;
            if (makeSmaller)
            {
                sizing = 2.6;
            }

            sb.Draw(article,
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * xLoc), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * yLoc),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / sizing), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / sizing)),
                new Rectangle((article.Width / spriteCount) * 0, 0, 160 / 5, article.Height / 44),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .0000000000000000000000000000000000000000000000001f);
        }

    }
}
