using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SharpDX.XInput;

namespace ImagineRITGame
{
    public enum MenuTextures
    {
        GeneralButtons = 0,
        TitleCard = 1,
        FishingPostCard = 2,
        AllFish = 3,
        FishInvShadow = 4
    }

    class Menu
    {
        public event MenuButtonActivatedDelegate MenuButtonActivated;
        //public event PlaySoundEffectDelegate PlaySoundEffect;

        // Fields for the menu's various textures
        protected List<Texture2D> textures;
        //protected List<SpriteFont> fonts;

        // Buttons
        protected List<Button> buttons;
        protected List<Button> buttons2;
        protected Button currentButton;

        protected List<string[]> fishInfo;

        protected List<string> strings;

        protected float aspectRatioFactor;

        //Previous Input States
        protected MouseState prevMState;
        protected KeyboardState prevKBState;

        /// <summary>
        /// A set for the previous keyboard state
        /// </summary>
        public KeyboardState PrevKBState
        {
            set { prevKBState = value; }
        }

        /// <summary>
        /// A set for the previous MouseState
        /// </summary>
        public MouseState PrevMState
        {
            set { prevMState = value; }
        }

        /// <summary>
        /// A constructor that defines a default Menu object
        /// </summary>
        /// <param name="textures">list of all textures used by menus</param>
        /// <param name="fonts">list of all fonts</param>
        public Menu(List<Texture2D> textures)
        {
            // Assigning values to the Menu's fields
            this.textures = textures;

            if ((float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) < (float)((float)1920 / (float)1080))
                aspectRatioFactor = (float)((float)((float)1920 / (float)1080) - (float)((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
        }


        /// <summary>
        /// Update various aspects of the Menu class (buttons, mouseState, etc)
        /// </summary>
        public virtual void Update()
        {

            foreach (Button b in buttons)
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
        }

        /// <summary>
        /// Draws the background and buttons of the menu
        /// </summary>
        /// <param name="sb">the spritebatch that allows us to draw</param>
        public virtual void Draw(SpriteBatch sb, Color hoverColor)
        {
            // Background
            //sb.Draw(textures[(int)MenuTextures.Background],
            //    new Rectangle(0, 0, 1280, 960),
            //    Color.White);

            // Buttons
            foreach (Button b in buttons)
                b.Draw(sb, b.IsHovered ? hoverColor : Color.White);
        }

        public float AspectRatioFactor
        {
            get
            {
                return aspectRatioFactor;
            }
        }

        /// <summary>
        /// Calls the MenuButtonActivated event
        /// </summary>
        protected virtual void ButtonActivated(int buttonType)
        {
            //    PlaySoundEffect(SoundEffects.ButtonPressValid);
            MenuButtonActivated?.Invoke(buttonType);
        }

        //protected virtual void PlayEffect(SoundEffects soundEffect)
        //{
        //    PlaySoundEffect?.Invoke(soundEffect);
        //}

        /// <summary>
        /// Aligns a button on the screen
        /// </summary>
        /// <param name="x">Horizontal factor positioning</param>
        /// <param name="y">Vertical factor positioning</param>
        /// <returns></returns>
        protected virtual Point AlignButton(double x, double y)
        {
            return new Point((int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) * x)), (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * y)));
        }

        protected virtual Point CenterButton(ButtonType buttonType, double y)
        {
            if (buttonType == ButtonType.A || buttonType == ButtonType.B || buttonType == ButtonType.C || buttonType == ButtonType.D)
                return new Point((int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) * .5) - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (11.13 * 2))), (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * y)));
            else if (buttonType == ButtonType.Title)
                return new Point((int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) * .5) - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (3.45 * 2))), (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * y)));
            else
                return new Point((int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) * .5) - (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (7.76 * 2))), (int)(((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) * y)));
        }

        protected virtual List<string> WrapText(string text, int stringLengthFactor)
        {
            strings = new List<string>();
            if (text.Length <= stringLengthFactor)
            {
                strings.Add(text);
                return strings;
            }
            else if (text.Length > stringLengthFactor && text.Length <= (stringLengthFactor * 2))
            {
                int indexOfMiddleSpace = text.LastIndexOf(' ', text.Length / 2);
                strings.Add(text.Substring(0, indexOfMiddleSpace));
                strings.Add(text.Substring(indexOfMiddleSpace + 1));
                return strings;
            }
            else if (text.Length > (stringLengthFactor * 2) && text.Length <= (stringLengthFactor * 3))
            {
                int indexOfFirstThirdSpace = text.LastIndexOf(' ', text.Length / 3);
                int indexOfSecondThirdSpace = text.LastIndexOf(' ', (text.Length / 3) * 2);
                strings.Add(text.Substring(0, indexOfFirstThirdSpace));
                strings.Add(text.Substring(indexOfFirstThirdSpace + 1, indexOfSecondThirdSpace - (indexOfFirstThirdSpace + 1)));
                strings.Add(text.Substring(indexOfSecondThirdSpace + 1));
                return strings;
            }
            else if (text.Length > (stringLengthFactor * 3) && text.Length <= (stringLengthFactor * 4))
            {
                int indexOfFirstQuarterSpace = text.LastIndexOf(' ', text.Length / 4);
                int indexOfSecondQuarterSpace = text.LastIndexOf(' ', (text.Length / 4) * 2);
                int indexOfThirdQuarterSpace = text.LastIndexOf(' ', (text.Length / 4) * 3);
                int len = text.Length;
                strings.Add(text.Substring(0, indexOfFirstQuarterSpace));
                strings.Add(text.Substring(indexOfFirstQuarterSpace + 1, indexOfSecondQuarterSpace - (indexOfFirstQuarterSpace + 1)));
                strings.Add(text.Substring(indexOfSecondQuarterSpace + 1, indexOfThirdQuarterSpace - (indexOfSecondQuarterSpace + 1)));
                strings.Add(text.Substring(indexOfThirdQuarterSpace + 1));
                return strings;
            }
            else if (text.Length > (stringLengthFactor * 4) && text.Length <= (stringLengthFactor * 5))
            {
                int indexOfFirstFifthSpace = text.LastIndexOf(' ', text.Length / 5);
                int indexOfSecondFifthSpace = text.LastIndexOf(' ', (text.Length / 5) * 2);
                int indexOfThirdFifthSpace = text.LastIndexOf(' ', (text.Length / 5) * 3);
                int indexOfFourthFifthSpace = text.LastIndexOf(' ', (text.Length / 5) * 4);
                int len = text.Length;
                strings.Add(text.Substring(0, indexOfFirstFifthSpace));
                strings.Add(text.Substring(indexOfFirstFifthSpace + 1, indexOfSecondFifthSpace - (indexOfFirstFifthSpace + 1)));
                strings.Add(text.Substring(indexOfSecondFifthSpace + 1, indexOfThirdFifthSpace - (indexOfSecondFifthSpace + 1)));
                strings.Add(text.Substring(indexOfThirdFifthSpace + 1, indexOfFourthFifthSpace - (indexOfThirdFifthSpace + 1)));
                strings.Add(text.Substring(indexOfFourthFifthSpace + 1));
                return strings;
            }
            else if (text.Length > (stringLengthFactor * 5) && text.Length <= (stringLengthFactor * 6))
            {
                int indexOfFirstSixthSpace = text.LastIndexOf(' ', text.Length / 6);
                int indexOfSecondSixthSpace = text.LastIndexOf(' ', (text.Length / 6) * 2);
                int indexOfThirdSixthSpace = text.LastIndexOf(' ', (text.Length / 6) * 3);
                int indexOfFourthSixthSpace = text.LastIndexOf(' ', (text.Length / 6) * 4);
                int indexOfFifthSixthSpace = text.LastIndexOf(' ', (text.Length / 6) * 5);
                int len = text.Length;
                strings.Add(text.Substring(0, indexOfFirstSixthSpace));
                strings.Add(text.Substring(indexOfFirstSixthSpace + 1, indexOfSecondSixthSpace - (indexOfFirstSixthSpace + 1)));
                strings.Add(text.Substring(indexOfSecondSixthSpace + 1, indexOfThirdSixthSpace - (indexOfSecondSixthSpace + 1)));
                strings.Add(text.Substring(indexOfThirdSixthSpace + 1, indexOfFourthSixthSpace - (indexOfThirdSixthSpace + 1)));
                strings.Add(text.Substring(indexOfFourthSixthSpace + 1, indexOfFifthSixthSpace - (indexOfFourthSixthSpace + 1)));
                strings.Add(text.Substring(indexOfFifthSixthSpace + 1));
                return strings;
            }
            else
            {
                int indexOfFirstSeventhSpace = text.LastIndexOf(' ', text.Length / 7);
                int indexOfSecondSeventhSpace = text.LastIndexOf(' ', (text.Length / 7) * 2);
                int indexOfThirdSeventhSpace = text.LastIndexOf(' ', (text.Length / 7) * 3);
                int indexOfFourthSeventhSpace = text.LastIndexOf(' ', (text.Length / 7) * 4);
                int indexOfFifthSeventhSpace = text.LastIndexOf(' ', (text.Length / 7) * 5);
                int indexOfSixthSeventhSpace = text.LastIndexOf(' ', (text.Length / 7) * 6);
                int len = text.Length;
                strings.Add(text.Substring(0, indexOfFirstSeventhSpace));
                strings.Add(text.Substring(indexOfFirstSeventhSpace + 1, indexOfSecondSeventhSpace - (indexOfFirstSeventhSpace + 1)));
                strings.Add(text.Substring(indexOfSecondSeventhSpace + 1, indexOfThirdSeventhSpace - (indexOfSecondSeventhSpace + 1)));
                strings.Add(text.Substring(indexOfThirdSeventhSpace + 1, indexOfFourthSeventhSpace - (indexOfThirdSeventhSpace + 1)));
                strings.Add(text.Substring(indexOfFourthSeventhSpace + 1, indexOfFifthSeventhSpace - (indexOfFourthSeventhSpace + 1)));
                strings.Add(text.Substring(indexOfFifthSeventhSpace + 1, indexOfSixthSeventhSpace - (indexOfFifthSeventhSpace + 1)));
                strings.Add(text.Substring(indexOfSixthSeventhSpace + 1));
                return strings;
            }

        }


    }
}