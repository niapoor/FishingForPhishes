using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using static System.Net.Mime.MediaTypeNames;

namespace ImagineRITGame
{
    internal class DisplayQuestion : Menu
    {

        /// <summary>
        /// A constructor that defines a DisplayQuestion object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        public DisplayQuestion(List<Texture2D> textures, List<SpriteFont> fonts) : base(textures)
        {
            buttons = new List<Button>() {
                new Button(base.AlignButton(.35, .52), ButtonType.A, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.45, .52), ButtonType.B, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.55, .52), ButtonType.C, textures[(int)MenuTextures.GeneralButtons]),
                new Button(base.AlignButton(.65, .52), ButtonType.D, textures[(int)MenuTextures.GeneralButtons])
            };
        }

        public void SetUpQuestion(Question q)
        {
            // The number and type of buttons will vary based on the number and type of answer choices
            if (q.NumAnswers() == 2)
            {
                // If the answer choices are true/false
                if (q.AnswerList()[0].Text() == "true") // q.AnswerList()[0].Text().ToLower().Contains("true")
                {
                    buttons = new List<Button>() {
                        new Button(base.AlignButton(.35, .65), ButtonType.True, textures[(int)MenuTextures.GeneralButtons]),
                        new Button(base.AlignButton(.45, .65), ButtonType.False, textures[(int)MenuTextures.GeneralButtons]),
                    };
                }
                // If the answer choices are false/true
                else if (q.AnswerList()[0].Text() == "False") // q.AnswerList()[0].Text().ToLower().Contains("false")
                {
                    buttons = new List<Button>() {
                        new Button(base.AlignButton(.35, .65), ButtonType.False, textures[(int)MenuTextures.GeneralButtons]),
                        new Button(base.AlignButton(.45, .65), ButtonType.True, textures[(int)MenuTextures.GeneralButtons]),
                    };
                }
                // If there are two answer choices, not true/false
                else
                {
                    buttons = new List<Button>() {
                        new Button(base.AlignButton(.4, .65), ButtonType.A, textures[(int)MenuTextures.GeneralButtons]),
                        new Button(base.AlignButton(.5, .65), ButtonType.B, textures[(int)MenuTextures.GeneralButtons]),
                    };
                }
            }
            // If there are 3 answer choices
            else if (q.NumAnswers() == 3)
            {
                buttons = new List<Button>() {
                    new Button(base.AlignButton(.35, .65), ButtonType.A, textures[(int)MenuTextures.GeneralButtons]),
                    new Button(base.AlignButton(.45, .65), ButtonType.B, textures[(int)MenuTextures.GeneralButtons]),
                    new Button(base.AlignButton(.55, .65), ButtonType.C, textures[(int)MenuTextures.GeneralButtons])
                };
            }
            // If there are 4 answer choices
            else
            {
                buttons = new List<Button>() {
                    new Button(base.AlignButton(.3, .65), ButtonType.A, textures[(int)MenuTextures.GeneralButtons]),
                    new Button(base.AlignButton(.4, .65), ButtonType.B, textures[(int)MenuTextures.GeneralButtons]),
                    new Button(base.AlignButton(.5, .65), ButtonType.C, textures[(int)MenuTextures.GeneralButtons]),
                    new Button(base.AlignButton(.6, .65), ButtonType.D, textures[(int)MenuTextures.GeneralButtons])
                };
            }
        }

        /// <summary>
        /// Updating any animation information and updating information about the buttons
        /// </summary>
        /// <param name="gameTime">info about the time from MonoGame</param>
        public void Update(GameTime gameTime)
        {
            base.Update();
        }

        /// <summary>
        /// Drawing in the question on screen along with the answer choice buttons
        /// </summary>
        /// <param name="sb">The spritebatch that allows us to draw</param>
        /// <param name="hoverColor">In this case most often used as the color the text will be</param>
        /// <param name="q">The question we will draw in</param>
        /// <param name="fonts">The list of fonts that can be used</param>
        public void Draw(SpriteBatch sb, Color hoverColor, Question q, List<SpriteFont> fonts)
        {
            // Drawing in the buttons themselves based off the base menu function
            base.Draw(sb, hoverColor);

            List<string> currentTextList = new List<string> { };

            // Write in each line of the question (minus the quotes), properly formatted to different lines when it is too long
            // If anyone finds this confusing (hint: it is) and has any questions feel free to message me (Nia)
            if (q.QuestionText()[0].ToString() == "\"")
                currentTextList = base.WrapText(q.QuestionText().Substring(1, q.QuestionText().Length - 2), 30);
            else
                currentTextList = base.WrapText(q.QuestionText(), 30);

            // Make sure to assign the y value of the question based off of how many lines it is
            double y = ((5 - currentTextList.Count) * .08) + 0.2;

            if (currentTextList.Count == 1)
                y = (2 * 0.08) + 0.2;
            else if (currentTextList.Count == 2)
                y = (1.5 * 0.08) + 0.2;
            else if (currentTextList.Count == 3)
                y = (1 * 0.08) + 0.2;
            else if (currentTextList.Count == 4)
                y = (0.5 * 0.08) + 0.2;
            else if (currentTextList.Count == 5)
                y = 0.2;

            // Draw in the question
            for (int i = 0; i < currentTextList.Count; i++)
            {
                sb.DrawString
                    (fonts[1],
                    currentTextList[i],
                    Game1.CenterText(currentTextList[i], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * ((.08 * i) + y))), fonts[1]),
                    Color.DarkGoldenrod);
            }

            // Write in each line of the currently hovered over answer choice, properly formatted to different lines when it is too long\
            // If anyone finds this confusing (hint: it is) and has any questions feel free to message me (Nia)
            for (int i = 0; i < q.NumAnswers(); i++)
            {
                if (buttons[i].IsHovered)
                {
                    if (q.AnswerList()[i].Text()[0].ToString() == "\"")
                        currentTextList = base.WrapText(q.AnswerList()[i].Text().Substring(1, q.AnswerList()[i].Text().Length - 2), 70);
                    else
                        currentTextList = base.WrapText(q.AnswerList()[i].Text(), 70);

                    for (int j = 0; j < currentTextList.Count; j++)
                    {
                        sb.DrawString
                            (fonts[2],
                            currentTextList[j],
                            Game1.CenterText(currentTextList[j], (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * ((.06 * j) + 0.86))), fonts[2]),
                            Color.DarkGoldenrod);
                    }
                }
            }
        }

    }
}