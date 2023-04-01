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

        /// <summary>
        /// Drawing the text of the fish and 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="fonts"></param>
        /// <param name="fish"></param>
        public void DrawCatchText(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts, Fish fish)
        {
            List<string> currentTextList = new List<string> { };
            string text = fish.CatchInfo[1];

            if (fish.CatchInfo[1][0].ToString() == "\"")
                currentTextList = base.WrapText(text.Substring(1, text.Length - 2), 25);
            else
                currentTextList = base.WrapText(text, 25);

            float textListMax = 0;
            int indexTestListMax = 0;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                if (fonts[2].MeasureString(currentTextList[i]).X > textListMax)
                {
                    textListMax = fonts[2].MeasureString(currentTextList[i]).X;
                    indexTestListMax = i;
                }
            }

            double y = 0.06;

            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)((float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (float)0.5) - (float)((float)textListMax / (float)2) - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.03)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.03)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.06) + (float)textListMax),
                    (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .06 * currentTextList.Count) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.03))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    Game1.CenterText(currentTextList[i], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * ((.06 * i) + y))), fonts[2]),
                    Color.Black);
            }

            // =============
            // Drawing in the fish on screen! TURTLE and BALLAN WRASSE
            // =============
            int indexX = Int32.Parse(fish.CatchInfo[2]);
            int indexY = (indexX / 10);
            indexX = (indexX % 10) - 1;
            if (indexX < 0)
            {
                indexX = 9;
                indexY--;
            }

            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.71), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.46),
                    (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.1825), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.1725) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .1))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            // Drawing the fish to display on the side of the screen
            sb.Draw(textures[(int)MenuTextures.AllFish],
                new Rectangle((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.698) + (int)(((((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10)))),
                (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.245) + (int)((((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10)) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * aspectRatioFactor * .25)),
                (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 1.328) / 9.6), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 1.328) / 9.6)),
                new Rectangle((textures[(int)MenuTextures.AllFish].Width / 10) * indexX, (textures[(int)MenuTextures.AllFish].Height / 10) * indexY,
                (textures[(int)MenuTextures.AllFish].Width / 10), (textures[(int)MenuTextures.AllFish].Height / 10)),
                Color.White);

        }

        public void DrawQuestionIncorrect(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, List<SpriteFont> fonts, Question question)
        {
            string correctAnswer = "tmp";
            for (int i = 0; i < question.NumAnswers(); i++)
            {
                if (question.CheckAnswer(i))
                    correctAnswer = question.AnswerList()[i].Text();
            }

            // Trim the quotes if there are any
            if (correctAnswer[0].ToString() == "\"")
                correctAnswer = correctAnswer.Substring(1, correctAnswer.Length - 2);

            string text = "The fish got away! The correct answer was \"" + correctAnswer +"\".";

            List<string> currentTextList = new List<string> { };
            currentTextList = base.WrapText(text, 25);

            float textListMax = 0;
            int indexTestListMax = 0;
            for (int i = 0; i < currentTextList.Count; i++)
            {
                if (fonts[2].MeasureString(currentTextList[i]).X > textListMax)
                {
                    textListMax = fonts[2].MeasureString(currentTextList[i]).X;
                    indexTestListMax = i;
                }
            }

            // Drawing in the fishing post card backdrop (no fish added)
            sb.Draw(textures[(int)MenuTextures.FishingPostCard],
                new Rectangle((int)((float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * (float)0.5) - (float)((float)textListMax / (float)2) - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.03)), 
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.03)),
                    (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.06) + (float)textListMax),
                    (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * .06 * currentTextList.Count) + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.03))),
                new Rectangle(0, 0, textures[(int)MenuTextures.FishingPostCard].Width, textures[(int)MenuTextures.FishingPostCard].Height),
                Color.White);

            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[2],
                    currentTextList[i],
                    Game1.CenterText(currentTextList[i], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * ((.06 * i) + 0.06))), fonts[2]),
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
