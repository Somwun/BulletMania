using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Bomb
    {
        private Rectangle Rect, explodeRect;
        private Circle explodeCircle;
        private Texture2D Texture, Explosion;
        private float seconds;
        private bool exploding, doneExploding;
        public Bomb(Texture2D texture, Texture2D explosion, int x, int y, int width)
        {
            Rect = new Rectangle(x, y, width, width);
            explodeCircle = new Circle(new Vector2(x, y), width);
            explodeRect = new Rectangle(x - (width / 2), y - (width / 2), width * 2, width * 2);
            Explosion = explosion;
            Texture = texture;
            seconds = 0;
            exploding = false;
        }
        public void Fuse(GameTime gameTime, List<Bomb> bombs)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (exploding == false)
                if (seconds >= 2)
                {
                    exploding = true;
                    seconds = 0;
                }
            else
                if (seconds >= 2)
                {
                    exploding = false;
                    doneExploding = true;
                    bombs.Remove(this);
                }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Exploding == false)
                spriteBatch.Draw(Texture, Rect, Color.White);
            else
                spriteBatch.Draw(Explosion, explodeRect, Color.White);

        }
        //Values
        public Circle BombCircle
        {
            get { return explodeCircle; }
        }
        public bool Exploding
        {
            get { return exploding; }
        }
        public bool DoneExploding
        {
            get { return doneExploding; }
        }
        public int FuseTime
        {
            get { return (int)seconds; }
        }
    }
}
