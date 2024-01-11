using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Buff
    {
        private Circle buffCircle;
        private Rectangle buffRect;
        private string buffType;
        private Texture2D armourTexture, speedTexture, ammoTexture;
        public Buff(Circle circle, string spawn, Texture2D armour, Texture2D speed, Texture2D ammo) 
        {
            buffCircle = circle;
            buffRect = new Rectangle((int)(circle.MiddleX - circle.Radius), (int)(circle.MiddleY - circle.Radius), (int)(circle.Radius * 2), (int)(circle.Radius * 2));
            buffType = spawn.ToLower();
            armourTexture = armour;
            speedTexture = speed;
            ammoTexture = ammo;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (buffType == "armour")
                spriteBatch.Draw(armourTexture, buffRect, Color.White);
            else if (buffType == "speed")
                spriteBatch.Draw(speedTexture, buffRect, Color.White);
            else if (buffType == "ammo")
                spriteBatch.Draw(ammoTexture, buffRect, Color.White);
        }
        public string BuffType
        {
            get { return buffType; }
        }
        public Circle BuffCircle
        {
            get { return buffCircle; }
        }
    }
}
