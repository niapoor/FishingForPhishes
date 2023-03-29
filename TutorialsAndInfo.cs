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

        public void DrawQuestionIncorrect(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts, Question question)
        {
            string correctAnswer = "tmp";
            for (int i = 0; i < question.NumAnswers(); i++)
            {
                if (question.CheckAnswer(i))
                    correctAnswer = question.AnswerList()[i].Text();
            }
            string text = "The fish got away! The correct answer was \"" + correctAnswer +"\".";

            List<string> currentTextList = new List<string> { };
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

        public void DrawGanericInstructions(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts)
        {
            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.065), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.46),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.25), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.17) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .1))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing in guiding text
            string text = "When a fish appears, press the spacebar to try to catch it";
            List<string> currentTextList = base.WrapText(text, 17);
            double y = ((8 - currentTextList.Count) * .06) + 0.25;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    new Vector2((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.09), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * ((.06 * i) + y)))),
                    Color.Black);
            }
        }

        public void DrawFishAgainInstructions(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts)
        {
            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.065), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.46),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.25), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.17) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .1))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing in guiding text
            string text = "Press the spacebar to try and catch another fish";
            List<string> currentTextList = base.WrapText(text, 15);
            double y = ((8 - currentTextList.Count) * .06) + 0.25;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    new Vector2((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.09), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * ((.06 * i) + y)))),
                    Color.Black);
            }
        }

        public void DrawMoreTutorials(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts)
        {
            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.065), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.46),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.25), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.17) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .1))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing in guiding text
            string text = "To see tutorials again later, press the \"Pause\" button";
            List<string> currentTextList = base.WrapText(text, 15);
            double y = ((8 - currentTextList.Count) * .06) + 0.25;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    new Vector2((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.09), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * ((.06 * i) + y)))),
                    Color.Black);
            }
        }


    }
}
