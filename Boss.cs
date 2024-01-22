using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Boss
    {
        private Rectangle Rect;
        private Vector2 speed, Location;
        private Circle Circle;
        private Texture2D Texture, HealthTexture, Frame;
        private float Rotation, Health, initialHealth;
        public Boss(Texture2D texture, Texture2D healthBar, Texture2D frame, int x, int y, int width, int height, int health)
        {
            Circle = new Circle(new Vector2(x / 2, y / 2), width / 4);
            Rect = new Rectangle(x, y, width, height);
            speed = new Vector2();
            Location = new Vector2(x, y);
            Texture = texture;
            Health = health;
            initialHealth = health;
            HealthTexture = healthBar;
            Frame = frame;
        }
        public void Move(List<Circle> people)
        {
            bool contains = false;
            if (people.Contains(BossCircle))
                contains = true;
            people.Remove(BossCircle);
            //X Coord
            Location.X += speed.X;
            Rect.X = (int)Location.X;
            Circle.MiddleX = Location.X;
            if (contains)
                foreach (Circle person in people)
                    if (Collides(person))
                        UndoMoveX();
            Rect.X = (int)Location.X;
            Circle.MiddleX = Location.X;
            //Y Coord
            Location.Y += speed.Y;
            Rect.Y = (int)Location.Y;
            Circle.MiddleY = Location.Y;
            if (contains)
                foreach (Circle person in people)
                    if (Collides(person))
                        UndoMoveY();
            Rect.Y = (int)Location.Y;
            Circle.MiddleY = Location.Y;
            if (contains)
                people.Add(BossCircle);
        }
        private void UndoMoveX()
        {
            Location.X -= speed.X;
        }
        private void UndoMoveY()
        {
            Location.Y -= speed.Y;
        }
        private bool Collides(Circle circle)
        {
            return BossCircle.Intersects(circle);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(Rect.X - 10, Rect.Y + 10, Rect.Width, Rect.Height), null, Color.Black * 0.7f, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture, Rect, null, Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthTexture, new Rectangle(BossRectangle.X - 60, BossRectangle.Y - 75, (int)(120 * (BossHealth / initialHealth)), 10), Color.Red);
            spriteBatch.Draw(Frame, new Rectangle(BossRectangle.X - 60, BossRectangle.Y - 75, 120, 10), Color.White);
        }
        //Values
        public Vector2 BossSpeed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float BossRotation
        {
            get { return Rotation; }
            set { Rotation = value; }
        }
        public Vector2 BossPosition
        {
            get { return Location; }
            set { Location = value; }
        }
        public Rectangle BossRectangle
        {
            get { return Rect; }
            set { Rect = value; }
        }
        public Circle BossCircle
        {
            get { return Circle; }
        }
        public float BossHealth
        {
            get { return Health; }
            set { Health = value; }
        }
        public Texture2D BossTexture
        {
            get { return Texture; }
            set { Texture = value; }
        }
    }
}
