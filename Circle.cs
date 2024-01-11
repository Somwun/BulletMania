using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMania
{
    internal class Circle
    {
        public Vector2 Center;
        public float Radius { get; set; }
        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        public bool Intersects(Circle other)
        {
            return (other.Center - Center).Length() < (other.Radius + Radius);
        }
        public bool Contains(Circle other)
        {
            return (other.Center - Center).Length() < (other.Radius - Radius);
        }
        //Values
        public float MiddleX
        {
            get { return Center.X; }
            set { Center.X = value; }
        }
        public float MiddleY
        {
            get { return Center.Y; }
            set { Center.Y = value; }
        }
        public float radius
        {
            get { return Radius; }
        }

        public Rectangle DrawRect
        {
            get { return new Rectangle((int)(Center.X - Radius), (int)(Center.Y - Radius), (int)Radius * 2, (int)Radius * 2); }
        }
        public Vector2 DrawLocation
        {
            get { return new Vector2(Center.X - Radius, Center.Y - Radius); }
        }
    }
}
