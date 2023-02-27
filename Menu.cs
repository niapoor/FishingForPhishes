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
        Background = 1
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
        protected Button currentButton;

        //Previous Input States
        protected MouseState prevMState;
        protected KeyboardState prevKBState;
        protected bool test;

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
        }


        /// <summary>
        /// Update various aspects of the Menu class (buttons, mouseState, etc)
        /// </summary>
        public virtual void Update()
        {

            foreach (Button b in buttons)
            {
                if (b.IsHovered)
                    currentButton= b;
            }

            // Check to see if the button should be activated
            if (((Mouse.GetState().LeftButton == ButtonState.Pressed &&
                prevMState.LeftButton == ButtonState.Released)
                && currentButton.IsHovered) ||
                Game1.SingleKeyPress(Keys.Enter, prevKBState))
                if (currentButton != null)
                {
                    ButtonActivated((int)currentButton.ButtonType);
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

    }
}
