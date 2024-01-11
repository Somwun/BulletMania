using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BulletMania
{
    internal class Bullet
    {
        private Rectangle Rect;
        private Vector2 Location;
        private Circle Circle;
        private Texture2D Texture;
        private Vector2 Angle;
        private bool Hit, PlayerSpawned;
        public Bullet(Texture2D texture, Vector2 position, int width, Vector2 angle, bool playerSpawned)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            Circle = new Circle(new Vector2(x / 2, y / 2), width / 2);
            Rect = new Rectangle(x, y, width, width);
            Location = new Vector2(x, y);
            Texture = texture;
            Angle = angle;
            Angle.X = (float)Math.Round(Angle.X, 2);
            Angle.Y = (float)Math.Round(Angle.Y, 2);
            Hit = false;
            PlayerSpawned = playerSpawned;
        }
        public void Move(List<Rectangle> barriers, List<Circle> people, List<Bullet> bullets, Player player, Boss boss)
        {
            Location.X += Angle.X;
            Location.Y += Angle.Y;
            Rect.X = (int)Location.X;
            Rect.Y = (int)Location.Y;
            Circle.MiddleX = Location.X;
            Circle.MiddleY = Location.Y;
            foreach (Rectangle barrier in barriers)
                if (BulletRectangle.Intersects(barrier))
                    bullets.Remove(this);
            if (PlayerSpawned)
                people.Remove(player.PlayerCircle);
            foreach (Circle person in people)
                if (BulletCircle.Intersects(person))
                    Hit = true;
            if (PlayerSpawned)
                people.Add(player.PlayerCircle);
            if (BulletCircle.Intersects(boss.BossCircle))
                Hit = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color.Black);
        }
        //Values
        public Vector2 BulletAngle
        {
            get { return Angle; }
            set { Angle = value; }
        }
        public Vector2 BulletPosition
        {
            get { return Location; }
            set { Location = value; }
        }
        public Rectangle BulletRectangle
        {
            get { return Rect; }
        }
        public Circle BulletCircle
        {
            get { return Circle; }
        }
        public bool BulletHit
        {
            get { return Hit; }
        }
        public bool PlayerShot
        {
            get { return PlayerSpawned; }
        }
    }
}
