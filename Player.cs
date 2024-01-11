using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Player
    {
        private Circle playerCircle;
        private Vector2 _location;
        private Rectangle playerRect;
        private Vector2 speed;
        private Texture2D playerTexture, playerHealthTexture, playerHealthFrame;
        private float playerRotation, playerHealth;
        private Color healthColor;
        public Player(float rotation, Texture2D texture, Texture2D healthTexture, Texture2D frame, int x, int y, int width, int height, int health)
        {
            playerCircle = new Circle (new Vector2(x / 2, y / 2), width / 3);
            _location = new Vector2(x, y);
            playerRect = new Rectangle(x, y, width, height);
            speed = new Vector2();
            playerTexture = texture;
            playerHealthTexture = healthTexture; 
            playerHealthFrame = frame;
            playerRotation = rotation;
            playerHealth = health;
            healthColor = Color.DarkGreen;
        }
        public void Move(List<Rectangle> barriers, List<Circle> people)
        {
            people.Remove(PlayerCircle);
            //Reset X coord
            _location.X += speed.X;
            playerRect.X = (int)_location.X;
            playerCircle.MiddleX = _location.X;
            foreach (Rectangle barrier in barriers)
                if (Collides(barrier))
                    UndoMoveX();
            foreach(Circle person in people)
                if (Collides(person))
                    UndoMoveX();
            playerRect.X = (int)_location.X;
            playerCircle.MiddleX = _location.X;
            //Reset Y coord
            _location.Y += speed.Y;
            playerRect.Y = (int)_location.Y;
            playerCircle.MiddleY = _location.Y;
            foreach (Rectangle barrier in barriers)
                if (Collides(barrier))
                    UndoMoveY();
            foreach (Circle person in people)
                if (Collides(person))
                    UndoMoveY();
            playerRect.Y = (int)_location.Y;
            playerCircle.MiddleY = _location.Y;
            people.Add(PlayerCircle);
        }
        private void UndoMoveX()
        {
            _location.X -= speed.X;
        }
        private void UndoMoveY()
        {
            _location.Y -= speed.Y;
        }
        private bool Collides(Rectangle rectangle)
        {
            return new Rectangle(playerRect.X - playerTexture.Width / 2, playerRect.Y - playerTexture.Height / 2, playerTexture.Width, playerTexture.Height).Intersects(rectangle);
        }
        private bool Collides(Circle circle)
        {
            return playerCircle.Intersects(circle);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, new Rectangle(playerRect.X - 3, playerRect.Y + 3, playerRect.Width, playerRect.Height), null, Color.Black * 0.7f, playerRotation, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(playerTexture, playerRect, null, Color.White, playerRotation, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(playerHealthTexture, new Rectangle(PlayerRectangle.X - 30, PlayerRectangle.Y - 30, (int)(60 * (playerHealth / 10)), 10), healthColor);
            spriteBatch.Draw(playerHealthFrame, new Rectangle(PlayerRectangle.X - 30, PlayerRectangle.Y - 30, 60, 10), Color.White);
        }
        //Values
        public Vector2 PlayerSpeed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float PlayerRotation
        {
            get { return playerRotation; }
            set { playerRotation = value; }
        }
        public Vector2 PlayerPosition
        {
            get { return _location; }
            set { _location = value; }
        }
        public Rectangle PlayerRectangle
        {
            get { return playerRect; }
        }
        public Circle PlayerCircle
        {
            get { return playerCircle; }
        }
        public float PlayerHealth
        {
            get { return playerHealth; }
            set { playerHealth = value; }
        }
        public Color HealthColor
        {
            get { return healthColor; }
            set { healthColor = value; }
        }
        public Texture2D PlayerTexture
        {
            get { return playerTexture; }
            set { playerTexture = value; }
        }
    }
}
