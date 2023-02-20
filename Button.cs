using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ImagineRITGame
{

    /// <summary>
    /// An enum for the various types of buttons
    /// </summary>
    public enum ButtonType
    {
        Start = 0,
        Exit = 1,
        Back = 2
    }

    internal class Button
    {
        // Fields that define a button object
        private Rectangle rect;
        private Rectangle textRect;
        private bool isHovered;
        private ButtonType buttonType;
        private Texture2D texture;

        /// <summary>
        /// Whether or not the mouse is currently hovering over the button
        /// </summary>
        public bool IsHovered
        {
            get
            {
                isHovered = rect.Contains(Mouse.GetState().Position);
                return isHovered;
            }
            set { isHovered = value; }
        }

        /// <summary>
        /// The type of button the button is
        /// </summary>
        public ButtonType ButtonType { get { return buttonType; } }

        /// <summary>
        /// Creates a new button object
        /// </summary>
        /// <param name="position">the button's position</param>
        /// <param name="buttonType">the type of button (ie. enter, back, etc)</param>
        /// <param name="texture">the button's texture</param>
        public Button(Point position, ButtonType buttonType, Texture2D texture)
        {
            // Assigning values to fields
            rect = new Rectangle(position, new Point(297, 109));
            this.buttonType = buttonType;
            this.texture = texture;
            // If the button is being hovered over
            isHovered = false;

            // Change the texture to be drawn depending on button type.
            // Buttons are always the same size (775 x 285)
            switch (buttonType)
            {
                default:
                    textRect = new Rectangle(0, 0, 297, 109);
                    break;

                case ButtonType.Start:
                    textRect = new Rectangle(297, 0, 297, 109);
                    break;

                case ButtonType.Exit:
                    textRect = new Rectangle(297, 109, 297, 109);
                    break;

                case ButtonType.Back:
                    textRect = new Rectangle(297, 109, 297, 109);
                    break;
            }


        }


        /// <summary>
        /// Draws the button with a given color
        /// </summary>
        /// <param name="sb">the spritebatch that allows us to draw</param>
        /// <param name="color">the color the button is drawn in</param>
        public virtual void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture,
                rect,
                textRect,
                color);
        }

    }
}
