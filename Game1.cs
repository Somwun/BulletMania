using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BulletMania
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D ammo, health, armour, speed, projectile;
        Rectangle buffsRect, projectlieRect;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            buffsRect = new Rectangle(10, 10, 60, 60);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ammo = Content.Load<Texture2D>("IconBullet");
            health = Content.Load<Texture2D>("IconHealth");
            armour = Content.Load<Texture2D>("IconShield");
            speed = Content.Load<Texture2D>("IconSpeed");
            projectile = Content.Load<Texture2D>("IconProjectile");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(armour, buffsRect, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}