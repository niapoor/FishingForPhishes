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
    internal class TutorialsAndInfo : Menu
    {

        public TutorialsAndInfo(List<Texture2D> textures, List<SpriteFont> fonts) : base(textures)
        {
        }

        public void DrawCatchText(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts, Fish fish)
        {
            List<string> currentTextList = new List<string> { };
            string text = fish.CatchInfo[1];

            if (fish.CatchInfo[1][0].ToString() == "\"")
                currentTextList = base.WrapText(text.Substring(1, text.Length - 2), 25);
            else
                currentTextList = base.WrapText(text, 25);

            double y = ((4 - currentTextList.Count) * .06);

            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    Game1.CenterText(currentTextList[i], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * ((.06 * i) + y))), fonts[2]),
                    Color.Black);
            }
        }
    }
}
