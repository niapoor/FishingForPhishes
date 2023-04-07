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
using SharpDX.DXGI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        Accessories = 7,         // beard, earring (x4), glasses, glasses_sun (7 elements, 1 page)
        Eyes = 8
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
        private Texture2D currentEyes;
        private Texture2D currentShirtPantsCombo;
        private Texture2D currentTmpArticle;
        private Texture2D currentTmpSecond;

        public int currentBodyOption;
        private int currentHatOption;
        private int currentHairOption;
        private int currentShirtOption;
        private int currentPantsOption;
        private int currentShoesOption;
        private int currentAccessoriesOption;
        private int currentShirtPantsComboOption;
        private int currentEyesOption;
        private int currentTmpArticleOption;
        private int currentTmpSecondOption;

        private bool drawShirt;
        private bool drawPants;
        private bool drawShirtPantsCombo;

        private ClothingType typeOfCurrentTmp;


        public Outfit(List<List<Texture2D>> allOutfitTextures)
        {
            allClothes = allOutfitTextures;
            currentBody = allClothes[(int)ClothingType.Body][1];
            currentHair = allClothes[(int)ClothingType.Hair][0];
            currentShirt = allClothes[(int)ClothingType.Shirt][0];
            currentPants = allClothes[(int)ClothingType.Pants][0];
            currentShoes = allClothes[(int)ClothingType.Shoes][0];
            currentEyes = allClothes[(int)ClothingType.Body][0];
            currentShirtPantsCombo = allClothes[(int)ClothingType.ShirtPantsCombo][0];
            currentTmpArticle = allClothes[(int)ClothingType.Shirt][0];
            currentTmpSecond = allClothes[(int)ClothingType.Pants][0];

            currentBodyOption = 0;
            currentHatOption = -1;
            currentHairOption = 0;
            currentShirtOption = 0;
            currentPantsOption = 0;
            currentShoesOption = 0;
            currentAccessoriesOption = -1;
            currentShirtPantsComboOption = -1;
            currentEyesOption = 0;
            currentTmpArticleOption = -1;
            currentTmpSecondOption = -1;
            //currentHat = allClothes[(int)ClothingType.Hat][0];
            //currentAccessories = allClothes[(int)ClothingType.Accessories][1];
        }

        public void DrawOutfitInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D inventoryTexture)
        {
            double xLoc = .343;
            double yLoc = .392;

            //1, 2, 10, 14, 8 (just characters)
            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Shirt)
            {
                DrawArticleInInventory(sb, currentTmpArticle, .3295, .255, false, ClothingType.Shirt);
                if (currentPantsOption == -1)
                    DrawArticleInInventory(sb, allClothes[(int)ClothingType.Pants][0], .3295, .31, false, ClothingType.Pants);
            }
            else if (currentShirtPantsComboOption == -1 && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.ShirtPantsCombo))
                DrawArticleInInventory(sb, currentShirt, .3295, .255, false, ClothingType.Shirt);

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Pants)
            {
                DrawArticleInInventory(sb, currentTmpArticle, .3295, .31, false, ClothingType.Pants);
                if (currentShirtOption == -1)
                    DrawArticleInInventory(sb, allClothes[(int)ClothingType.Shirt][0], .3295, .255, false, ClothingType.Shirt);
            }
            else if (currentShirtPantsComboOption == -1 && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.ShirtPantsCombo))
                DrawArticleInInventory(sb, currentPants, .3295, .31, false, ClothingType.Pants);

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Shoes)
                DrawArticleInInventory(sb, currentTmpArticle, .3295, .375, false, ClothingType.Shoes);
            else
                DrawArticleInInventory(sb, currentShoes, .3295, .375, false, ClothingType.Shoes);

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Hat)
                DrawArticleInInventory(sb, currentTmpArticle, .2145, .335, true, ClothingType.Hat);
            else
                DrawArticleInInventory(sb, currentHat, .2145, .335, true, ClothingType.Hat);

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Hair)
                DrawArticleInInventory(sb, currentTmpArticle, .2145, .39, true, ClothingType.Hair);
            else
                DrawArticleInInventory(sb, currentHair, .2145, .39, true, ClothingType.Hair);

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Accessories)
                DrawArticleInInventory(sb, currentTmpArticle, .2031, .435, false, ClothingType.Accessories);
            else
                DrawArticleInInventory(sb, currentAccessories, .2031, .435, false, ClothingType.Accessories);

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.ShirtPantsCombo)
            {
                DrawArticleInInventory(sb, currentTmpArticle, .3295, .255, false, ClothingType.ShirtPantsCombo);
                DrawArticleInInventory(sb, currentTmpArticle, .3295, .31, false, ClothingType.ShirtPantsCombo);
            }
            else if ((currentPantsOption == -1 && currentShirtOption == -1) && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.Shirt) && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.Pants))
            {
                DrawArticleInInventory(sb, currentShirtPantsCombo, .3295, .255, false, ClothingType.ShirtPantsCombo);
                DrawArticleInInventory(sb, currentShirtPantsCombo, .3295, .31, false, ClothingType.ShirtPantsCombo);
            }

            DrawEmptySlot(sb, inventoryTexture);
        }

        public void ChangeCurrentArticle(ClothingType page, int newArticle, int newOption)
        {
            switch (page)
            {
                case ClothingType.Shirt:
                    currentShirt = allClothes[(int)ClothingType.Shirt][newArticle];
                    currentShirtOption = newOption;
                    currentShirtPantsComboOption = -1;
                    if (currentPantsOption == -1)
                    {
                        currentPants = allClothes[(int)ClothingType.Pants][0];
                        currentPantsOption = 0;
                    }
                    break;
                case ClothingType.Pants:
                    currentPants = allClothes[(int)ClothingType.Pants][newArticle];
                    currentPantsOption = newOption;
                    currentShirtPantsComboOption = -1;
                    if (currentShirtOption == -1)
                    {
                        currentShirt = allClothes[(int)ClothingType.Shirt][0];
                        currentShirtOption = 0;
                    }
                    break;
                case ClothingType.Shoes:
                    currentShoes = allClothes[(int)ClothingType.Shoes][newArticle];
                    currentShoesOption = newOption;
                    break;
                case ClothingType.Hair:
                    currentHair = allClothes[(int)ClothingType.Hair][newArticle];
                    currentHairOption = newOption;
                    break;
                case ClothingType.Hat:
                    currentHat = allClothes[(int)ClothingType.Hat][newArticle];
                    currentHatOption = newOption;
                    break;
                case ClothingType.Body:
                    if (newArticle == 1)
                        currentBodyOption = newOption;
                    else
                        currentEyesOption = newOption;
                    break;
                case ClothingType.Accessories:
                    currentAccessories = allClothes[(int)ClothingType.Accessories][newArticle];
                    currentAccessoriesOption = newOption;
                    break;
                case ClothingType.ShirtPantsCombo:
                    currentShirtPantsCombo = allClothes[(int)ClothingType.ShirtPantsCombo][newArticle];
                    currentShirtPantsComboOption = newOption;
                    currentShirtOption = -1;
                    currentPantsOption = -1;
                    break;

            }
        }

        public void UpdateTmpArticle(ClothingType page, int newArticle, int newOption)
        {
            switch (page)
            {
                case ClothingType.Shirt:
                    currentTmpArticle = allClothes[(int)ClothingType.Shirt][newArticle];
                    currentTmpArticleOption = newOption;
                    drawShirtPantsCombo = false;
                    if (currentTmpSecondOption == -1)
                    {
                        currentTmpSecond = allClothes[(int)ClothingType.Pants][0];
                        currentTmpSecondOption = 0;
                    }
                    drawShirt = true;
                    drawPants = true;
                    typeOfCurrentTmp = ClothingType.Shirt;
                    break;
                case ClothingType.Pants:
                    currentTmpArticle = allClothes[(int)ClothingType.Pants][newArticle];
                    currentTmpArticleOption = newOption;
                    drawShirtPantsCombo = false;
                    if (currentTmpSecondOption == -1)
                    {
                        currentTmpSecond = allClothes[(int)ClothingType.Shirt][0];
                        currentTmpSecondOption = 0;
                    }
                    drawShirt = true;
                    drawPants = true;
                    typeOfCurrentTmp = ClothingType.Pants;
                    break;
                case ClothingType.Shoes:
                    currentTmpArticle = allClothes[(int)ClothingType.Shoes][newArticle];
                    currentTmpArticleOption = newOption;
                    typeOfCurrentTmp = ClothingType.Shoes;
                    break;
                case ClothingType.Hair:
                    currentTmpArticle = allClothes[(int)ClothingType.Hair][newArticle];
                    currentTmpArticleOption = newOption;
                    typeOfCurrentTmp = ClothingType.Hair;
                    break;
                case ClothingType.Hat:
                    currentTmpArticle = allClothes[(int)ClothingType.Hat][newArticle];
                    currentTmpArticleOption = newOption;
                    typeOfCurrentTmp = ClothingType.Hat;
                    break;
                case ClothingType.Body:
                    if (newArticle == 1)
                    {
                        currentTmpArticleOption = newOption;
                        typeOfCurrentTmp = ClothingType.Body;
                    }
                    else if (newArticle == 0)
                    {
                        currentTmpSecondOption = newOption;
                        typeOfCurrentTmp = ClothingType.Eyes;
                    }
                    break;
                case ClothingType.Accessories:
                    currentTmpArticle = allClothes[(int)ClothingType.Accessories][newArticle];
                    currentTmpArticleOption = newOption;
                    typeOfCurrentTmp = ClothingType.Accessories;
                    break;
                case ClothingType.ShirtPantsCombo:
                    currentTmpArticle = allClothes[(int)ClothingType.ShirtPantsCombo][newArticle];
                    currentTmpArticleOption = newOption;
                    drawShirt = false;
                    drawPants = false;
                    drawShirtPantsCombo = true;
                    typeOfCurrentTmp = ClothingType.ShirtPantsCombo;
                    break;
            }
            if (newOption == -1)
            {
                currentTmpArticleOption = -1;
                currentTmpSecondOption = -1;
            }
        }

        public void DrawArticleInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D article, double xLoc, double yLoc, bool makeSmaller, ClothingType type)
        {
            int option = -1;
            switch (type)
            {
                case ClothingType.Shirt:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Shirt)
                        option = currentTmpArticleOption;
                    else if (currentTmpSecondOption != -1 && typeOfCurrentTmp == ClothingType.Pants)
                        option = currentTmpSecondOption;
                    else
                        option = currentShirtOption;
                    break;
                case ClothingType.Pants:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Pants)
                        option = currentTmpArticleOption;
                    else if (currentTmpSecondOption != -1 && typeOfCurrentTmp == ClothingType.Shirt)
                        option = currentTmpSecondOption;
                    else
                        option = currentPantsOption;
                    break;
                case ClothingType.Shoes:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Shoes)
                        option = currentTmpArticleOption;
                    else
                        option = currentShoesOption;
                    break;
                case ClothingType.Hair:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Hair)
                        option = currentTmpArticleOption;
                    else
                        option = currentHairOption;
                    break;
                case ClothingType.Hat:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Hat)
                        option = currentTmpArticleOption;
                    else
                        option = currentHatOption;
                    break;
                case ClothingType.Body:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Body)
                        option = currentTmpArticleOption;
                    else
                        option = currentBodyOption;
                    break;
                case ClothingType.Accessories:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Accessories)
                        option = currentTmpArticleOption;
                    else
                        option = currentAccessoriesOption;
                    break;
                case ClothingType.ShirtPantsCombo:
                    if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.ShirtPantsCombo)
                        option = currentTmpArticleOption;
                    else
                        option = currentShirtPantsComboOption;
                    break;
            }
            if (option != -1 || (currentTmpSecondOption != -1 && (type == ClothingType.Shirt || type == ClothingType.Pants))) //DrawArticleInInventory(sb, allClothes[(int)ClothingType.Shirt][0], .3295, .31, false, ClothingType.Shirt);
            {
                // If the accessory is the beard there are 14 options
                if (article.Width == 3584)
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 14, option);
                // If the accessory is a type of glasses there are 10 options
                else if (article.Width == 2560)
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 10, option);
                else if (article.Width == 512)
                {
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 2, option);
                }
                // If the article has 1 option
                else
                    DrawArticleIdle(sb, article, xLoc, yLoc, makeSmaller, 1, option);
            }
        }

        /// <summary>
        /// Draws the proper icon in an empty current outfit inventory slot
        /// </summary>
        /// <param name="sb">Allows us to draw</param>
        /// <param name="inventoryTexture">Textures for the inventory</param>
        public void DrawEmptySlot(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D inventoryTexture)
        {
            if ((currentHat == null || currentHatOption == -1) && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.Hat))
                sb.Draw(inventoryTexture,
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .2168), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .365),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (float)((float).06 / (float)1.2)), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (float)((float).105 / (float)1.2))),
                    new Rectangle(300, 120, 20, 20),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .00000001f);
            if ((currentHair == null || currentHairOption == -1) && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.Hair))
                sb.Draw(inventoryTexture,
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .217), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .427),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (float)((float).06 / (float)1.2)), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (float)((float).105 / (float)1.2))),
                    new Rectangle(300, 135, 20, 20),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .00000001f);
            if ((currentAccessories == null || currentAccessoriesOption == -1) && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.Accessories))
                sb.Draw(inventoryTexture,
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .2131), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .5),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (float)((float).1 / (float)1.2)), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (float)((float).13 / (float)1.2))),
                    new Rectangle(300, 150, 30, 30),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .00000001f);
            if ((currentShoes == null || currentShoesOption == -1) && (currentTmpArticleOption == -1 || typeOfCurrentTmp != ClothingType.Shoes))
                sb.Draw(inventoryTexture,
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .3345), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .522),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (float)((float).1 / (float)1.4)), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (float)((float).13 / (float)1.4))),
                    new Rectangle(350, 156, 30, 30),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .00000001f);
        }

        public void DrawOutfitOnPlayer(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Player player, int currentFrame, int animationType, double xLoc, double yLoc)
        {
            double conditionalY = 0;
            if (player.PlayerState == PlayerStates.Idle)
                conditionalY = 0.04;

            List<Texture2D> outfitForPlayer = new List<Texture2D>();
            List<int> outfitOptions = new List<int>();

            outfitForPlayer.Add(currentBody);
            outfitOptions.Add(currentBodyOption);
            outfitForPlayer.Add(currentEyes);
            outfitOptions.Add(currentEyesOption);
            if (currentHair != null)
            {
                outfitForPlayer.Add(currentHair);
                outfitOptions.Add(currentHairOption);
            }
            if (currentShirtOption != -1)
            {
                outfitForPlayer.Add(currentShirt);
                outfitOptions.Add(currentShirtOption);
                outfitForPlayer.Add(currentPants);
                outfitOptions.Add(currentPantsOption);
            }
            else
            {
                outfitForPlayer.Add(currentShirtPantsCombo);
                outfitOptions.Add(currentShirtPantsComboOption);
            }
            if (currentShoes != null && currentShoesOption != -1)
            {
                outfitForPlayer.Add(currentShoes);
                outfitOptions.Add(currentShoesOption);
            }
            if (currentAccessories != null && currentAccessoriesOption != -1)
            {
                outfitForPlayer.Add(currentAccessories);
                outfitOptions.Add(currentAccessoriesOption);
            }
            if (currentHat != null && currentHatOption != -1)
            {
                outfitForPlayer.Add(currentHat);
                outfitOptions.Add(currentHatOption);
            }

            for (int i = 0; i < outfitForPlayer.Count; i++) {
                //if ((int)outfitOptions[0] == 1)
                sb.Draw(outfitForPlayer[i],
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * xLoc), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * (yLoc - conditionalY)),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765)),
                    new Rectangle((currentFrame * (160 / 5)) + (outfitOptions[i] * 8 * (160 / 5)), (animationType * 32), 160 / 5, outfitForPlayer[i].Height / 44),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .86f);
            }
        }

        public void DrawInventoryPlayer(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {

            List<Texture2D> outfitForPlayer = new List<Texture2D>();
            List<int> outfitOptions = new List<int>();

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Body)
            {       //BODY
                outfitForPlayer.Add(allClothes[(int)ClothingType.Body][1]);
                outfitOptions.Add(currentTmpArticleOption);
            }
            else
            {
                outfitForPlayer.Add(currentBody);
                outfitOptions.Add(currentBodyOption);
            }

            if (currentTmpSecondOption != -1 && typeOfCurrentTmp == ClothingType.Eyes)
            {       // EYES
                outfitForPlayer.Add(allClothes[(int)ClothingType.Body][0]);
                outfitOptions.Add(currentTmpSecondOption);
            }
            else
            {
                outfitForPlayer.Add(currentEyes);
                outfitOptions.Add(currentEyesOption);
            }

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Hair)         // HAIR
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
            }
            else if (currentHair != null)
            {
                outfitForPlayer.Add(currentHair);
                outfitOptions.Add(currentHairOption);
            }

            // Drawing in tmp shirt
            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Shirt)        // SHIRT / PANTS / SHIRT PANTS COMBO
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
                if (currentPantsOption != -1)
                {
                    outfitForPlayer.Add(currentPants);
                    outfitOptions.Add(currentPantsOption);
                }
                else
                {
                    outfitForPlayer.Add(currentTmpSecond);
                    outfitOptions.Add(currentTmpSecondOption);
                }
            }
            // Drawing in tmp pants
            else if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Pants)
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
                if (currentShirtOption != -1)
                {
                    outfitForPlayer.Add(currentShirt);
                    outfitOptions.Add(currentShirtOption);
                }
                else
                {
                    outfitForPlayer.Add(currentTmpSecond);
                    outfitOptions.Add(currentTmpSecondOption);
                }
            }
            // Drawing in tmp shirt / pants combo
            else if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.ShirtPantsCombo)
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
            }
            // If there is no tmp, see if the shirt / pants can be drawn in
            else if (currentShirtOption != -1)
            {
                outfitForPlayer.Add(currentShirt);
                outfitOptions.Add(currentShirtOption);
                outfitForPlayer.Add(currentPants);
                outfitOptions.Add(currentPantsOption);
            }
            // If all else fails, draw in the shirt / pants combo
            else
            {
                outfitForPlayer.Add(currentShirtPantsCombo);
                outfitOptions.Add(currentShirtPantsComboOption);
            }


            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Shoes)        // SHOES
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
            }
            else if (currentShoes != null && currentShoesOption != -1)
            {
                outfitForPlayer.Add(currentShoes);
                outfitOptions.Add(currentShoesOption);
            }

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Accessories)  // ACCESSORIES
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
            }
            else if (currentAccessories != null && currentAccessoriesOption != -1)
            {
                outfitForPlayer.Add(currentAccessories);
                outfitOptions.Add(currentAccessoriesOption);
            }

            if (currentTmpArticleOption != -1 && typeOfCurrentTmp == ClothingType.Hat)          // HAT
            {
                outfitForPlayer.Add(currentTmpArticle);
                outfitOptions.Add(currentTmpArticleOption);
            }
            else if (currentHat != null && currentHatOption != -1)
            {
                outfitForPlayer.Add(currentHat);
                outfitOptions.Add(currentHatOption);
            }





            for (int i = 0; i < outfitForPlayer.Count; i++)
                sb.Draw(outfitForPlayer[i],
                    new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * .2525), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * .32),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / 1.5), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / 1.5)),
                    new Rectangle((outfitOptions[i] * 8 * (160 / 5)), 0, 160 / 5, outfitForPlayer[i].Height / 44),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0,
                    .00000000001f);

        }

        public void DrawArticleIdle(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Texture2D article, double xLoc, double yLoc, bool makeSmaller, int spriteCount, int option)
        {
            double sizing = 1.95;
            if (makeSmaller)
            {
                sizing = 2.6;
            }

            sb.Draw(article,
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * xLoc), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * yLoc),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 5.565 / sizing), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2.765 / sizing)),
                new Rectangle((article.Width / spriteCount) * option, 0, 160 / 5, article.Height / 44),
                Color.White,
                0f,
                Vector2.Zero,
                0,
                .0000000000000000000000000000000000000000000000001f);
        }

    }
}
