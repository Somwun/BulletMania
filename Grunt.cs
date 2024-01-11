using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Grunt
    {
        private Circle gruntCircle;
        private Vector2 location;
        private Rectangle gruntRect;
        private Vector2 speed;
        private Texture2D gruntTexture, healthTexture, Frame;
        private float gruntRotation, Health, initialHealth;
        private bool intersects;
        public Grunt(Texture2D texture, Texture2D healthBar, Texture2D frame, int x, int y, int width, int health)
        {
            gruntCircle = new Circle(new Vector2(x, y), width / 3);
            location = new Vector2(x, y);
            gruntRect = new Rectangle(x, y, width, width);
            speed = new Vector2();
            gruntTexture = texture;
            healthTexture = healthBar;
            gruntRotation = 0;
            Health = health;
            initialHealth = health;
            Frame = frame;
            intersects = false;
        }
        public void Move(List<Rectangle> barriers, List<Circle> people, Player player)
        {
            people.Remove(GruntCircle);
            //Set X coord
            intersects = false;
            location.X += speed.X;
            gruntRect.X = (int)location.X;
            gruntCircle.MiddleX = location.X;
            foreach (Rectangle barrier in barriers)
                if (Collides(barrier))
                    UndoMoveX();
            foreach (Circle person in people)
                if (Collides(person))
                {
                    if (person == player.PlayerCircle)
                        intersects = true;
                    UndoMoveX();
                }
            gruntRect.X = (int)location.X;
            gruntCircle.MiddleX = location.X;
            //Set Y coord
            location.Y += speed.Y;
            gruntRect.Y = (int)location.Y;
            gruntCircle.MiddleY = location.Y;
            foreach (Rectangle barrier in barriers)
                if (Collides(barrier))
                    UndoMoveY();
            foreach (Circle person in people)
                if (Collides(person))
                {
                    if (person == player.PlayerCircle)
                        intersects = true;
                    UndoMoveY();
                }
            gruntRect.Y = (int)location.Y;
            gruntCircle.MiddleY = location.Y;
            people.Add(GruntCircle);
        }
        public void TakeDamage()
        {
            Health -= 1;
        }
        private void UndoMoveX()
        {
            location.X -= speed.X;
        }
        private void UndoMoveY()
        {
            location.Y -= speed.Y;
        }
        private bool Collides(Rectangle rectangle)
        {
            return new Rectangle(gruntRect.X - gruntTexture.Width / 2, gruntRect.Y - gruntTexture.Height / 2, gruntTexture.Width, gruntTexture.Height).Intersects(rectangle);
        }
        public bool Collides(Circle circle)
        {
            return gruntCircle.Intersects(circle);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gruntTexture, new Rectangle(gruntRect.X - 3, gruntRect.Y + 3, gruntRect.Width, gruntRect.Height), null, Color.Black * 0.7f, gruntRotation, new Vector2(gruntTexture.Width / 2, gruntTexture.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(gruntTexture, gruntRect, null, Color.White, gruntRotation, new Vector2(gruntTexture.Width / 2, gruntTexture.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(healthTexture, new Rectangle(gruntRect.X - 30, gruntRect.Y - 30, (int)(60 * (GruntHealth / initialHealth)), 10), Color.Red);
            spriteBatch.Draw(Frame, new Rectangle(gruntRect.X - 30, gruntRect.Y - 30, 60, 10), Color.White);
        }
        //Values
        public float GruntHealth
        {
            get { return Health; }
        }
        public Vector2 GruntSpeed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float GruntRotation
        {
            get { return gruntRotation; }
            set { gruntRotation = value; }
        }
        public Vector2 GruntPosition
        {
            get { return location; }
        }
        public Rectangle GruntRectangle
        {
            get { return gruntRect; }
        }
        public Circle GruntCircle
        {
            get { return gruntCircle; }
        }
        public Texture2D GruntTexture
        {
            get { return  gruntTexture; }
            set { gruntTexture = value; }
        }
        public bool Intersects
        {
            get { return intersects; }
        }
    }
}
