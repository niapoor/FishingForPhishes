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
        Exit = 0,
        Start = 1,
        MainMenu = 2,
        ViewItems = 3,
        NewSave = 4,
        LoadSave = 5,
        Options = 6,
        Settings = 7,
        ChangeMode = 8,
        Easy = 9,
        Medium = 10,
        Hard = 11,
        True = 12,
        False = 13,
        A = 14,
        B = 15,
        C = 16,
        D = 17,
        Title = 18,
        Pause = 19,
        Inventory = 20,
        OutfitShop = 21,
        OutfitShopCurrentPage = 22,
        OutfitShopAnotherPage = 23,
        Back = -1
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
            if (buttonType == ButtonType.A || buttonType == ButtonType.B || buttonType == ButtonType.C || buttonType == ButtonType.D)
                rect = new Rectangle(position, new Point((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 11.13), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 5.53)));
            else if (buttonType == ButtonType.Title)
                rect = new Rectangle(position, new Point((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 3.45), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 3)));
            else if (buttonType == ButtonType.Inventory)
                rect = new Rectangle(position, new Point((int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.332) / 10)));
            else if (buttonType == ButtonType.OutfitShopCurrentPage)
                rect = new Rectangle(position, new Point((int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.375) / 10), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 1.13) / 10)));
            else if (buttonType == ButtonType.OutfitShopAnotherPage)
                rect = new Rectangle(position, new Point((int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.375) / 10), (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8) / 10)));
            else
                rect = new Rectangle(position, new Point((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 7.76), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 5.53)));
            // Assigning values to fields
            this.buttonType = buttonType;
            this.texture = texture;
            // If the button is being hovered over
            isHovered = false;

            // Change the texture to be drawn depending on button type.
            // Buttons are always the same size (775 x 285)
            switch (buttonType)
            {
                default:
                    textRect = new Rectangle(310, 0, 104, 81);
                    break;

                case ButtonType.MainMenu:
                    textRect = new Rectangle(0, 0, 104, 81);
                    break;

                case ButtonType.True:
                    textRect = new Rectangle(104, 0, 104, 81);
                    break;

                case ButtonType.False:
                    textRect = new Rectangle(208, 0, 104, 81);
                    break;

                case ButtonType.Pause:
                    textRect = new Rectangle(310, 0, 104, 81);
                    break;

                case ButtonType.A:
                    textRect = new Rectangle(0, 81, 74, 80);
                    break;

                case ButtonType.B:
                    textRect = new Rectangle(74, 81, 74, 80);
                    break;

                case ButtonType.C:
                    textRect = new Rectangle(148, 81, 75, 80);
                    break;

                case ButtonType.D:
                    textRect = new Rectangle(223, 81, 75, 80);
                    break;

                case ButtonType.Start:
                    textRect = new Rectangle(0, 161, 104, 80);
                    break;

                case ButtonType.Exit:
                    textRect = new Rectangle(103, 161, 104, 80);
                    break;

                case ButtonType.ViewItems:
                    textRect = new Rectangle(207, 161, 104, 80);
                    break;

                case ButtonType.Back:
                    textRect = new Rectangle(310, 161, 104, 80);
                    break;

                case ButtonType.NewSave:
                    textRect = new Rectangle(0, 239, 104, 82);
                    break;

                case ButtonType.LoadSave:
                    textRect = new Rectangle(103, 239, 104, 82);
                    break;

                case ButtonType.Options:
                    textRect = new Rectangle(207, 239, 104, 82);
                    break;

                case ButtonType.Settings:
                    textRect = new Rectangle(310, 239, 104, 82);
                    break;

                case ButtonType.ChangeMode:
                    textRect = new Rectangle(0, 320, 104, 81);
                    break;

                case ButtonType.Easy:
                    textRect = new Rectangle(103, 320, 104, 81);
                    break;

                case ButtonType.Medium:
                    textRect = new Rectangle(207, 320, 104, 81);
                    break;

                case ButtonType.Hard:
                    textRect = new Rectangle(310, 320, 104, 81);
                    break;

                case ButtonType.OutfitShop:
                    textRect = new Rectangle(310, 401, 104, 81);
                    break;

                case ButtonType.OutfitShopCurrentPage:
                    textRect = new Rectangle(212, 325, 1, 1);
                    break;

                case ButtonType.OutfitShopAnotherPage:
                    textRect = new Rectangle(212, 325, 1, 1);
                    break;

                case ButtonType.Title:
                    textRect = new Rectangle(0, 401, 256, 167);
                    break;

                case ButtonType.Inventory:
                    textRect = new Rectangle(1, 1, 1, 1);
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
