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
        public Inventory(List<Texture2D> textures, List<SpriteFont> fonts) : base(textures)
        {
            // Giving positions and sizes to the main menu's buttons
            buttons = new List<Button>() {
                new Button(base.AlignButton(.07, .65), ButtonType.Back, textures[(int)MenuTextures.GeneralButtons])
            };

            buttons2 = new List<Button>();

            fishInfo = new List<string[]>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    string[] initialString = new string[2];
                    initialString[0] = "???";
                    initialString[1] = 0.ToString();
                    buttons2.Add(new Button(new Point((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.3) + (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10) * j),
                        (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.1) + (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10) * i)),
                        ButtonType.Inventory,
                        textures[(int)MenuTextures.GeneralButtons]));
                    fishInfo.Add(initialString);
                }
            }

        }

        public void AddFishToInventory(Fish fish)
        {
            int index = Int32.Parse(fish.CatchInfo[2]);
            // If the fish has been caught the name should be visible
            fishInfo[index - 1][0] = fish.CatchInfo[0];
            // Increment the count of this fish caught
            fishInfo[index - 1][1] = (Int32.Parse(fishInfo[index - 1][1]) + 1).ToString();
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
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.265), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.12),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.4), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.4)),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing shadows of the fish (if they have not been caught)
            sb.Draw(textures[(int)MenuTextures.FishInvShadow],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.3), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.183),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332)),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishInvShadow].Width, textures[(int)MenuTextures.FishInvShadow].Height),
                Color.Goldenrod);

            int k = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(Int32.Parse(fishInfo[k][1]) > 0)
                    {
                        sb.Draw(textures[(int)MenuTextures.AllFish],
                            new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.2995) + (int)(((((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10) * j))),
                                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.102) + (int)((((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10) * i)),
                            (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 9.6), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 9.6)),
                            new Rectangle((textures[(int)MenuTextures.AllFish].Width / 10) * j, (textures[(int)MenuTextures.AllFish].Height / 10) * i, 
                                (textures[(int)MenuTextures.AllFish].Width / 10), (textures[(int)MenuTextures.AllFish].Height / 10)),
                            Color.White);
                    }
                    k++;
                }
            }

        }

        public void DrawFonts(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts)
        {
            int index = 0;

            foreach (Button b in buttons2)
            {
                b.Draw(sb, b.IsHovered ? Color.Goldenrod : Color.White);
                if (b.ButtonType == ButtonType.Inventory && b.IsHovered)
                {
                    sb.DrawString
                        (fonts[2],
                        "Fish Name: " + fishInfo[index][0],
                        Game1.CenterText("Fish Name: " + fishInfo[index][0], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * 0.86)), fonts[2]),
                        Color.DarkGoldenrod);
                    sb.DrawString
                        (fonts[2],
                        "Fish Caught: " + fishInfo[index][1],
                        Game1.CenterText("Fish Caught: " + fishInfo[index][1], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * 0.92)), fonts[2]),
                        Color.DarkGoldenrod);
                }
                index++;
            }

            // Drawing in guiding text
            string text = "Certain fish can only be caught with different question difficulties. You can change the question difficulty in the pause menu.";
            List<string> currentTextList = base.WrapText(text, 9);
            double y = ((8 - currentTextList.Count) * .06) + 0.2;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    new Vector2((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.7), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * ((.06 * i) + y)))),
                    Color.DarkGoldenrod);
            }
        }

    }
}