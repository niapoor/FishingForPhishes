using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace ImagineRITGame
{
    internal class Shop
    {

        private List<int> currentPrices;
        private int currentBalance;

        public int CurrentBalance
        {
            get
            {
                return currentBalance;
            }
        }

        public Shop(List<SpriteFont> fonts)
        {
            currentPrices = new List<int>();
            AddAmountToPrices(0, 17);           // Prices for the eyes and skins is 0
            currentPrices.Add(100);             // Hard coded hat prices
            currentPrices.Add(100);
            currentPrices.Add(120);
            currentPrices.Add(120);
            currentPrices.Add(200);
            currentPrices.Add(350);
            currentPrices.Add(350);
            currentPrices.Add(500);
            currentPrices.Add(0);               // First hair is free
            AddAmountToPrices(50, 8);           // Adding the prices for the rest of the colors / styles
            AddAmountToPrices(100, 9);
            AddAmountToPrices(70, 9);
            AddAmountToPrices(120, 9);
            AddAmountToPrices(70, 9);
            AddAmountToPrices(150, 36);
            AddAmountToPrices(250, 18);
            AddAmountToPrices(150, 9);
            AddAmountToPrices(300, 18);
            AddAmountToPrices(150, 9);
            currentPrices.Add(0);               // First shirt is free
            AddAmountToPrices(60, 8);           // Adding the prices for the rest of the colors / styles
            AddAmountToPrices(150, 18);
            AddAmountToPrices(100, 18);
            AddAmountToPrices(150, 9);
            AddAmountToPrices(100, 27);
            AddAmountToPrices(200, 9);
            currentPrices.Add(0);               // First pants are free
            AddAmountToPrices(80, 26);          // Adding the prices for the rest of the colors / styles
            AddAmountToPrices(150, 9);          // Adding the prices for the dresses / costumes
            AddAmountToPrices(200, 4);
            AddAmountToPrices(300, 2);
            currentPrices.Add(0);               // First shoes are free
            AddAmountToPrices(50, 8);           // Adding the prices for the rest of the shoes
            AddAmountToPrices(100, 9);          // Adding accessory prices
            AddAmountToPrices(50, 9);
            AddAmountToPrices(100, 9);

            currentBalance = 0;
        }

        // A helper method to add prices
        public void AddAmountToPrices(int priceToAdd, int amountToAdd)
        {
            for (int i = 0; i < amountToAdd; i++)
            {
                currentPrices.Add(priceToAdd);
            }
        }

        /// <summary>
        /// Add the the player's balance when a fish is caught
        /// </summary>
        /// <param name="difficulty">The harder the difficulty, the more credits will be added</param>
        public void AddToBalance(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    currentBalance += 10;
                    break;
                case Difficulty.Medium:
                    currentBalance += 20;
                    break;
                case Difficulty.Hard:
                    currentBalance += 30;
                    break;
            }
        }

    }
}
