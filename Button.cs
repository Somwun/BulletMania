using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Button
    {
        private Texture2D frame, square;
        private Vector2 position;
        private Rectangle rect;
        private Color backColor, textColor;
        private SpriteFont font;
        private string text;
        public Button(Vector2 TextLocation, Rectangle Rectangle, Color BackColor, Color TextColor, string Text, SpriteFont Font, Texture2D Frame, Texture2D Square)
        {
            position = TextLocation;
            rect = Rectangle;
            square = Square;
            frame = Frame;
            backColor = BackColor;
            textColor = TextColor;
            font = Font;
            text = Text;
        }
        public bool Click(MouseState mouseState, MouseState prevState)
        {
            bool click = false;
            if (rect.Contains(mouseState.X, mouseState.Y) & mouseState.LeftButton == ButtonState.Pressed & prevState.LeftButton == ButtonState.Released)
                click = true;
            return click;
        }
        public void Draw(SpriteBatch spriteBatch, MouseState mouseState)
        {
            spriteBatch.Draw(square, rect, backColor);
            spriteBatch.Draw(frame, rect, Color.White);
            if (rect.Contains(new Vector2(mouseState.X, mouseState.Y)))
            {
                spriteBatch.Draw(square, rect, Color.Black * 0.2f);
            }
            spriteBatch.DrawString(font, text, position, textColor);
        }
        public Vector2 ButtonPosition
        {
            get { return position; }
        }
        public Rectangle ButtonRectangle
        {
            get { return rect; }
        }
        public string ButtonText
        {
            get { return text; }
            set { text = value; }
        }
    }
}
