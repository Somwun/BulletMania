using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace BulletMania
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;
        Texture2D explosion, ammoBuff, armourBuff, speedBuff, unknown, bullet, arena, square, cursor, frame, bomb, skull, blankTexture, circle;
        Texture2D playerMachineGun, bigPlayerMachineGun, playerShotGun, bigPlayerShotGun, currentPlayer, machineGun, shotGun, playerMachineGunGold, bigPlayerMachineGunGold, playerShotGunGold, bigPlayerShotGunGold;
        Texture2D currentBoss, bossBlue, bossRed, helicopter, enemy, leftPunch, rightPunch;
        Rectangle screenRect, cursorRect;
        Vector2 mousePos, playerFace;
        Random random;
        Player player;
        Boss boss;
        Circle bossFace;
        Button continueBTN, playBTN, shotGunBTN, machineGunBTN, quitBTN, dontQuitBTN, deathMenuBTN, deathQuitBTN, tutorialBTN, prevBTN;
        Button armourBuffBTN, armourTimeBTN, speedBuffBTN, speedTimeBTN, ammoBuffBTN, ammoTimeBTN, skipBTN;
        List<Rectangle> borders;
        List<Grunt> grunts;
        List<Circle> people;
        List<Bullet> bullets;
        List<Bomb> bombs;
        List<Buff> buffs;
        SoundEffect shotGunSound, machineGunSound, punchSound, explosionSound, speedUpSound, armourSound, ammoSound, hurt1, hurt2, hurt3, click;
        SoundEffectInstance explosionInstance, clickInstance;
        SpriteFont testFont, menuFont, shopFont, titleFont;
        float playerRotation, bossRotation, ammoBuffAmount, shotGunReloadTime, machineGunReloadTime, bombReloadTime;
        int speed, damage, speedBuffAmount, armourBuffAmount, skillPoints, level;
        int load, tutorialPhase;
        int armourAmountCost, armourTimeCost, ammoAmountCost, ammoTimeCost, speedAmountCost, speedTimeCost;
        double speedBuffTime, ammoBuffTime, armourBuffTime, totalArmourBuffTime, totalAmmoBuffTime, totalSpeedBuffTime;
        double bossBulletTime2, gruntSpawnTime, bossBulletTime, bulletTime, bombTime, gruntPunchTime, startTime;
        bool playerShooting, keepSpawning, exit, prevExitState, startUp, shotGunEquipped, machineGunEquipped;
        bool armourAmountUpgrade, armourTimeUpgrade, speedAmountUpgrade, speedTimeUpgrade, ammoAmountUpgrade, ammoTimeUpgrade;
        StreamWriter acheivments;
        bool defeatedBlueTank, defeatedRedTank, defeatedHelicopter;
        double finishTime;
        List<string> acheivmentsList;
        string cheatCode;

        enum Screen
        {
            shop, battle, dead, exit, intro, tutorial, win
        }
        Screen screen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 900;
            Window.Title = "Cool Game";
            _graphics.ApplyChanges();

            load = 0;

            if (!File.Exists("Acheivments.txt"))
            {
                defeatedBlueTank = false;
                defeatedRedTank = false;
                defeatedHelicopter = false;
                finishTime = 0;               
            }
            acheivmentsList = new List<string>()
                {
                    "False",
                    "False",
                    "False",
                    "0"
                };

            cheatCode = "";

            screen = Screen.intro;

            random = new Random();

            bulletTime = 0;
            damage = 2;
            armourBuffAmount = 1;
            ammoBuffAmount = 0.5f;
            speedBuffAmount = 2;
            skillPoints = 0;
            level = 1;
            tutorialPhase = 0;

            machineGunReloadTime = 100;
            shotGunReloadTime = 850;
            bombTime = 5;
            bombReloadTime = 5;
            bossBulletTime = 0;
            speedBuffTime = 0;
            ammoBuffTime = 0;
            armourBuffTime = 0;
            totalAmmoBuffTime = 0;
            totalArmourBuffTime = 0;
            totalSpeedBuffTime = 0;
            gruntSpawnTime = 0;
            gruntPunchTime = 500;
            startTime = 5;

            armourAmountCost = 1;
            armourTimeCost = 1;
            ammoAmountCost = 1;
            ammoTimeCost = 1;
            speedAmountCost = 1;
            speedTimeCost = 1;

            keepSpawning = true;
            exit = false;
            playerShooting = false;
            startUp = true;
            shotGunEquipped = false;
            machineGunEquipped = true;

            ammoAmountUpgrade = false;
            ammoTimeUpgrade = false;
            armourAmountUpgrade = false;
            armourTimeUpgrade = false;
            speedAmountUpgrade = false;
            speedTimeUpgrade = false;

            bossFace = new Circle(new Vector2(1, 1), 1);
            screenRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            cursorRect = new Rectangle(mouseState.X, mouseState.Y, 33, 33);

            continueBTN = new Button(new Vector2(313, 733), new Rectangle(295, 700, 300, 100), Color.DarkGreen, Color.White, "Continue >", menuFont, frame, square);
            playBTN = new Button(new Vector2(375, 733), new Rectangle(295, 700, 300, 100), Color.DarkGreen, Color.White, "Play >", menuFont, frame, square);
            machineGunBTN = new Button(new Vector2(0, 0), new Rectangle(100, 400, 120, 60), Color.White, Color.Black, "", menuFont, frame, square);
            shotGunBTN = new Button(new Vector2(0, 0), new Rectangle(100, 490, 120, 60), Color.White, Color.Black, "", menuFont, frame, square);
            quitBTN = new Button(new Vector2(415, 375), new Rectangle(350, 355, 200, 75), Color.White, Color.Black, "YES", menuFont, frame, square);
            dontQuitBTN = new Button(new Vector2(425, 475), new Rectangle(350, 455, 200, 75), Color.White, Color.Black, "NO", menuFont, frame, square);
            armourBuffBTN = new Button(new Vector2(60, 445), new Rectangle(37, 420, 225, 70), Color.White, Color.Black, "Damage Reduction\n  -50% > -100%", shopFont, frame, square);
            armourTimeBTN = new Button(new Vector2(91, 525), new Rectangle(37, 500, 225, 70), Color.White, Color.Black, " Buff Time\n3sec > 5sec", shopFont, frame, square);
            ammoBuffBTN = new Button(new Vector2(350, 445), new Rectangle(337, 420, 225, 70), Color.White, Color.Black, "Firing Speed Boost\n      2x > 3x", shopFont, frame, square);
            ammoTimeBTN = new Button(new Vector2(391, 525), new Rectangle(337, 500, 225, 70), Color.White, Color.Black, " Buff Time\n3sec > 5sec", shopFont, frame, square);
            speedBuffBTN = new Button(new Vector2(666, 445), new Rectangle(622, 420, 225, 70), Color.White, Color.Black, " Speed Boost\n+50%  > +100%", shopFont, frame, square);
            speedTimeBTN = new Button(new Vector2(681, 525), new Rectangle(622, 500, 225, 70), Color.White, Color.Black, " Buff Time\n3sec > 5sec", shopFont, frame, square);
            deathQuitBTN = new Button(new Vector2(95, 520), new Rectangle(37, 500, 225, 70), Color.DarkRed, Color.Black, "Quit", menuFont, frame, square);
            deathMenuBTN = new Button(new Vector2(690, 520), new Rectangle(622, 500, 225, 70), Color.DarkGreen, Color.Black, "Menu", menuFont, frame, square);
            tutorialBTN = new Button(new Vector2(580, 700), new Rectangle(560, 680, 250, 70), Color.DarkGreen, Color.White, "Continue", menuFont, frame, square);
            skipBTN = new Button(new Vector2(160, 700), new Rectangle(90, 680, 250, 70), Color.DarkRed, Color.Black, "Skip", menuFont, frame, square);
            prevBTN = new Button(new Vector2(395, 700), new Rectangle(350, 680, 200, 70), Color.DarkBlue, Color.White, "Prev", menuFont, frame, square);

            player = new Player(playerRotation, playerMachineGun, square, frame, _graphics.PreferredBackBufferWidth / 2 + 175, _graphics.PreferredBackBufferHeight / 2 + 10, 50, 35, 10);

            boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 100, _graphics.PreferredBackBufferHeight / 2, 150, 150, 0);

            people = new List<Circle>();
            people.Add(player.PlayerCircle);
            people.Add(boss.BossCircle);

            borders = new List<Rectangle>();
            borders.Add(new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, 85));
            borders.Add(new Rectangle(0, _graphics.PreferredBackBufferHeight - 85, _graphics.PreferredBackBufferWidth, 85));
            borders.Add(new Rectangle(0, 0, 85, _graphics.PreferredBackBufferHeight));
            borders.Add(new Rectangle(_graphics.PreferredBackBufferWidth - 85, 0, 85, _graphics.PreferredBackBufferHeight));

            grunts = new List<Grunt>();

            bullets = new List<Bullet>();
            bombs = new List<Bomb>();

            buffs = new List<Buff>();

            boss.BossSpeed = new Vector2(0, 0);
            speed = 4;
            SoundEffect.MasterVolume = 0.2f;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ammoBuff = Content.Load<Texture2D>("IconBullet");
            armourBuff = Content.Load<Texture2D>("IconShield");
            speedBuff = Content.Load<Texture2D>("IconSpeed");
            bullet = Content.Load<Texture2D>("IconProjectile");
            unknown = Content.Load<Texture2D>("IconMysteryResize");
            arena = Content.Load<Texture2D>("BattleArena");
            square = Content.Load<Texture2D>("Square White");
            cursor = Content.Load<Texture2D>("AimCursor (2)");
            frame = Content.Load<Texture2D>("FrameSimple");
            bomb = Content.Load<Texture2D>("BombCartoonPixel");
            explosion = Content.Load<Texture2D>("GoodExplosion");
            skull = Content.Load<Texture2D>("PixelSkull");
            blankTexture = Content.Load<Texture2D>("Blank");
            circle = Content.Load<Texture2D>("circle");

            shotGun = Content.Load<Texture2D>("ShotGun2");
            machineGun = Content.Load<Texture2D>("MachineGun2");

            playerMachineGun = Content.Load<Texture2D>("PixelManResize");
            bigPlayerMachineGun = Content.Load<Texture2D>("PixelManBigResize");
            playerShotGun = Content.Load<Texture2D>("PixelManShotGunResize");
            bigPlayerShotGun = Content.Load<Texture2D>("PixelManBigShotGunResize");
            currentPlayer = Content.Load<Texture2D>("PixelManBigResize");
            playerMachineGunGold = Content.Load<Texture2D>("PixelManGold");
            bigPlayerMachineGunGold = Content.Load<Texture2D>("PixelManBigGold");
            playerShotGunGold = Content.Load<Texture2D>("PixelManShotGunGold");
            bigPlayerShotGunGold = Content.Load<Texture2D>("PixelManBigShotGunGold");

            bossBlue = Content.Load<Texture2D>("PixelTank2");
            bossRed = Content.Load<Texture2D>("PixelTankRed");
            currentBoss = Content.Load<Texture2D>("PixelTank2");
            helicopter = Content.Load<Texture2D>("PixelHelicopter");

            enemy = Content.Load<Texture2D>("PixelEnemy");
            leftPunch = Content.Load<Texture2D>("PixelEnemyLeftPunch");
            rightPunch = Content.Load<Texture2D>("PixelEnemyRightPunch");

            testFont = Content.Load<SpriteFont>("testFont");
            menuFont = Content.Load<SpriteFont>("menuFont");
            shopFont = Content.Load<SpriteFont>("ShopFont");
            titleFont = Content.Load<SpriteFont>("TitleFont");

            machineGunSound = Content.Load<SoundEffect>("gun-shot_77bpm_D_major");
            shotGunSound = Content.Load<SoundEffect>("shot-old-gun-one-shot");
            explosionSound = Content.Load<SoundEffect>("explosion_F_minor");
            punchSound = Content.Load<SoundEffect>("punch_A_minor");
            explosionInstance = explosionSound.CreateInstance();

            speedUpSound = Content.Load<SoundEffect>("boost-100537");
            ammoSound = Content.Load<SoundEffect>("pistol-cock-6014");
            armourSound = Content.Load<SoundEffect>("shield-guard-6963");

            hurt1 = Content.Load<SoundEffect>("grunt1-68324");
            hurt2 = Content.Load<SoundEffect>("grunt1-84540");
            hurt3 = Content.Load<SoundEffect>("male_hurt7-48124");

            click = Content.Load<SoundEffect>("jumpCoin");
            clickInstance = click.CreateInstance();

        }
        protected override void Update(GameTime gameTime)
        {
            //Set Variables
            if (load == 0) //Load save data
            {
                if (File.Exists("Acheivments.txt"))
                    Load();
                else
                {
                    Save();
                    Load();
                }
                load++;
            }
            prevExitState = exit;
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            if (screen == Screen.intro)
            {
                mousePos.X -= 15;
                mousePos.Y -= 15;
            }
            IsMouseVisible = false;
            playerRotation = GetAngle(player.PlayerPosition, mousePos);
            playerRotation = (float)Math.Round(playerRotation, 2);
            player.PlayerRotation = playerRotation;
            bossRotation = GetAngle(boss.BossPosition, player.PlayerPosition);
            if (level == 6)
                bossRotation += 1.575f;
            boss.BossRotation = bossRotation;
            playerFace = GetBulletAngle(player.PlayerPosition, mousePos);
            playerFace.X = (float)Math.Round(playerFace.X, 2);
            playerFace.Y = (float)Math.Round(playerFace.Y, 2);
            bossFace = new Circle(new Vector2(boss.BossCircle.MiddleX + (GetBulletAngle(boss.BossPosition, player.PlayerPosition).X * 10), boss.BossCircle.MiddleY + (GetBulletAngle(boss.BossPosition, player.PlayerPosition).Y * 10)), boss.BossCircle.Radius);

            // Exit
            if (screen != Screen.dead & screen != Screen.intro)
            {
                if (keyboardState.IsKeyDown(Keys.Escape))
                    exit = true;
                if (exit)
                {
                    if (quitBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        startUp = true;
                        exit = false;
                        screen = Screen.intro;
                    }
                    else if (dontQuitBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        exit = false;
                    }
                }
            }

            if (exit == false)
            {
                //Intro
                if (screen == Screen.intro)
                {
                    if (startUp)
                    {
                        borders.Clear();
                        currentBoss = bossBlue;
                        skillPoints = 0;
                        tutorialPhase = 0;
                        boss.BossHealth = 0;
                        level = 1;
                        speed = 4;
                        damage = 2;
                        machineGunReloadTime = 100;
                        shotGunReloadTime = 500;
                        bombReloadTime = 5;
                        bulletTime = 0;
                        bombTime = 5;
                        bullets.Clear();
                        bombs.Clear();
                        people.Clear();
                        grunts.Clear();
                        buffs.Clear();
                        armourBuffTime = 0;
                        ammoBuffTime = 0;
                        speedBuffTime = 0;
                        totalArmourBuffTime = 0;
                        totalAmmoBuffTime = 0;
                        totalSpeedBuffTime = 0;
                        bossBulletTime = 0;
                        bossBulletTime2 = 0;
                        gruntSpawnTime = 0;
                        machineGunReloadTime = 100;
                        shotGunReloadTime = 850;
                        startTime = 5;
                        keepSpawning = false;
                        
                        if (defeatedHelicopter)
                        {
                            playerMachineGun = playerMachineGunGold;
                            playerShotGun = playerShotGunGold;
                            bigPlayerMachineGun = bigPlayerMachineGunGold;
                            bigPlayerShotGun = bigPlayerShotGunGold;
                        }

                        if (machineGunEquipped)
                            currentPlayer = bigPlayerMachineGun;
                        else
                            currentPlayer = bigPlayerShotGun;
                        player = new Player(playerRotation, currentPlayer, blankTexture, blankTexture, 165, 240, 200, 142, 10);
                        startUp = false;
                    }
                    if (shotGunBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        currentPlayer = bigPlayerShotGun;
                        shotGunEquipped = true;
                        machineGunEquipped = false;
                    }
                    else if (machineGunBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        currentPlayer = bigPlayerMachineGun;
                        shotGunEquipped = false;
                        machineGunEquipped = true;
                    }
                    else if (playBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        if (machineGunEquipped)
                            currentPlayer = playerMachineGun;
                        else
                            currentPlayer = playerShotGun;
                        screen = Screen.tutorial;
                    }
                    player.PlayerTexture = currentPlayer;

                    bulletTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (machineGunBTN.ButtonRectangle.Contains(new Vector2(mousePos.X + 15, mousePos.Y + 15)) == false & shotGunBTN.ButtonRectangle.Contains(new Vector2(mousePos.X + 15, mousePos.Y + 15)) == false & playBTN.ButtonRectangle.Contains(mousePos) == false & mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (machineGunEquipped & bulletTime >= machineGunReloadTime)
                        {
                            playerShooting = true;
                            machineGunSound.Play();
                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, playerFace, playerShooting));
                            playerShooting = false;
                            bulletTime = 0;
                        }
                        if (shotGunEquipped & bulletTime >= shotGunReloadTime)
                        {
                            playerShooting = true;
                            for (int i = 0; i <= 9; i++)
                                switch (i)
                                {
                                    case 0:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, playerFace, playerShooting));
                                        break;
                                    case 1:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X - (playerFace.Y / 3f), playerFace.Y + (playerFace.X / 3f)), playerShooting));
                                        break;
                                    case 2:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X - (playerFace.Y / 3.6f), playerFace.Y + (playerFace.X / 3.6f)), playerShooting));
                                        break;
                                    case 3:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X + (playerFace.Y / 3f), playerFace.Y - (playerFace.X / 3f)), playerShooting));
                                        break;
                                    case 4:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X + (playerFace.Y / 3.6f), playerFace.Y - (playerFace.X / 3.6f)), playerShooting));
                                        break;
                                    case 5:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X - (playerFace.Y / 5.5f), playerFace.Y + (playerFace.X / 5.5f)), playerShooting));
                                        break;
                                    case 6:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X - (playerFace.Y / 10f), playerFace.Y + (playerFace.X / 10f)), playerShooting));
                                        break;
                                    case 7:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X + (playerFace.Y / 5.5f), playerFace.Y - (playerFace.X / 5.5f)), playerShooting));
                                        break;
                                    case 8:
                                        bullets.Add(new Bullet(bullet, PlaceBullet(true), 30, new Vector2(playerFace.X + (playerFace.Y / 10f), playerFace.Y - (playerFace.X / 10f)), playerShooting));
                                        break;
                                }
                            shotGunSound.Play();
                            playerShooting = false;
                            bulletTime = 0;
                        }
                    }

                    for (int i = 0; i < bullets.Count; i++)
                        bullets[i].Move(borders, people, bullets, player, boss);

                    if (keyboardState.IsKeyDown(Keys.Up) & !prevKeyboardState.IsKeyDown(Keys.Up))
                        cheatCode += "U";
                    else if (keyboardState.IsKeyDown(Keys.Down) & !prevKeyboardState.IsKeyDown(Keys.Down))
                        cheatCode += "D";
                    else if (keyboardState.IsKeyDown(Keys.Left) & !prevKeyboardState.IsKeyDown(Keys.Left))
                        cheatCode += "L";
                    else if (keyboardState.IsKeyDown(Keys.Right) & !prevKeyboardState.IsKeyDown(Keys.Right))
                        cheatCode += "R";
                    else if (keyboardState.IsKeyDown(Keys.B) & !prevKeyboardState.IsKeyDown(Keys.B))
                        cheatCode += "B";
                    else if (keyboardState.IsKeyDown(Keys.A) & !prevKeyboardState.IsKeyDown(Keys.A))
                        cheatCode += "A";
                    else if (keyboardState.IsKeyDown(Keys.Escape))
                    {
                        currentBoss = bossBlue;
                        level = 1;
                        cheatCode = "";
                        playBTN.ButtonText = "Play >";
                    }
                    if (cheatCode == "UUDDLRLRBA")
                    {
                        currentBoss = helicopter;
                        level = 6;
                        playBTN.ButtonText = "  6";
                    }
                    if (cheatCode == "DDUURLRLAB")
                    {
                        currentBoss = bossRed;
                        level = 5;
                        playBTN.ButtonText = "  5";
                    }
                }

                //Battle
                else if (prevExitState == false & screen == Screen.battle)
                {
                    //Start Up
                    if (boss.BossHealth <= 0)
                    {
                        if (level > 1)
                            defeatedBlueTank = true;
                        if (level > 5)
                            defeatedRedTank = true;
                        if (level > 6)
                            defeatedHelicopter = true;
                        Save();
                        if (borders.Count == 0)
                        {
                            borders.Add(new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, 85));
                            borders.Add(new Rectangle(0, _graphics.PreferredBackBufferHeight - 85, _graphics.PreferredBackBufferWidth, 85));
                            borders.Add(new Rectangle(0, 0, 85, _graphics.PreferredBackBufferHeight));
                            borders.Add(new Rectangle(_graphics.PreferredBackBufferWidth - 85, 0, 85, _graphics.PreferredBackBufferHeight));
                        }
                        people.Remove(player.PlayerCircle);
                        player = new Player(playerRotation, currentPlayer, square, frame, _graphics.PreferredBackBufferWidth / 2 + 200, _graphics.PreferredBackBufferHeight / 2, 50, 35, 10);
                        people.Add(player.PlayerCircle);
                        bullets.Clear();
                        switch (level)
                        {
                            case 1:
                                currentBoss = bossBlue;
                                boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 150, 150, 50);
                                people.Add(boss.BossCircle);
                                break;
                            case 2:
                                currentBoss = bossBlue;
                                boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 150, 150, 100);
                                people.Add(boss.BossCircle);
                                break;
                            case 3:
                                currentBoss = bossBlue;
                                boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 150, 150, 200);
                                people.Add(boss.BossCircle);
                                break;
                            case 4:
                                currentBoss = bossBlue;
                                boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 150, 150, 300);
                                people.Add(boss.BossCircle);
                                break;
                            case 5:
                                currentBoss = bossRed;
                                boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 150, 150, 500);
                                people.Add(boss.BossCircle);
                                break;
                            case 6:
                                currentBoss = helicopter;
                                boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 100, 250, 500);
                                people.Remove(boss.BossCircle);
                                break;
                        }
                    }

                    if (startTime > 0)
                        startTime -= gameTime.ElapsedGameTime.TotalSeconds;

                    //Play
                    if (startTime <= 0)
                    {
                        //Buffs
                        for (int i = 0; i < buffs.Count; i++)
                            if (buffs[i].BuffCircle.Intersects(player.PlayerCircle))
                            {
                                switch (buffs[i].BuffType)
                                {
                                    case "armour":
                                        if (armourTimeUpgrade)
                                            armourBuffTime = 5;
                                        else
                                            armourBuffTime = 3;
                                        if (damage == 2)
                                            damage -= armourBuffAmount;
                                        player.HealthColor = Color.DarkBlue;
                                        armourSound.Play();
                                        break;
                                    case "ammo":
                                        if (ammoTimeUpgrade)
                                            ammoBuffTime = 5;
                                        else
                                            ammoBuffTime = 3;
                                        if (bombReloadTime == 5)
                                        {
                                            machineGunReloadTime *= ammoBuffAmount;
                                            shotGunReloadTime *= ammoBuffAmount;
                                            bombReloadTime *= ammoBuffAmount;
                                        }
                                        ammoSound.Play();
                                        break;
                                    case "speed":
                                        if (speedTimeUpgrade)
                                            speedBuffTime = 5;
                                        else
                                            speedBuffTime = 3;
                                        if (speed == 4)
                                            speed += speedBuffAmount;
                                        speedUpSound.Play();
                                        break;
                                }
                                buffs.Remove(buffs[i]);
                            }
                        if (armourBuffTime > 0)
                        {
                            totalArmourBuffTime += gameTime.ElapsedGameTime.TotalSeconds;
                            if (totalArmourBuffTime >= armourBuffTime)
                            {
                                armourBuffTime = 0;
                                damage += armourBuffAmount;
                                totalArmourBuffTime = 0;
                                player.HealthColor = Color.DarkGreen;
                            }
                        }
                        if (ammoBuffTime > 0)
                        {
                            totalAmmoBuffTime += gameTime.ElapsedGameTime.TotalSeconds;
                            if (totalAmmoBuffTime >= ammoBuffTime)
                            {
                                machineGunReloadTime /= ammoBuffAmount;
                                shotGunReloadTime /= ammoBuffAmount;
                                bombReloadTime /= ammoBuffAmount;
                                ammoBuffTime = 0;
                                totalAmmoBuffTime = 0;
                            }
                        }
                        if (speedBuffTime > 0)
                        {
                            totalSpeedBuffTime += gameTime.ElapsedGameTime.TotalSeconds;
                            if (totalSpeedBuffTime >= speedBuffTime)
                            {
                                speed -= speedBuffAmount;
                                speedBuffTime = 0;
                                totalSpeedBuffTime = 0;
                            }
                        }

                        //Player Shooting
                        bulletTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (machineGunEquipped)
                            if (mouseState.LeftButton == ButtonState.Pressed & bulletTime >= machineGunReloadTime)
                            {
                                playerShooting = true;
                                bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, playerFace, playerShooting));
                                machineGunSound.Play();
                                playerShooting = false;
                                bulletTime = 0;
                            }
                        if (shotGunEquipped)
                            if (mouseState.LeftButton == ButtonState.Pressed & bulletTime >= shotGunReloadTime)
                            {
                                playerShooting = true;
                                for (int i = 0; i <= 9; i++)
                                    switch (i)
                                    {
                                        case 0:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, playerFace, playerShooting));
                                            break;
                                        case 1:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X - (playerFace.Y / 3f), playerFace.Y + (playerFace.X / 3f)), playerShooting));
                                            break;
                                        case 2:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X - (playerFace.Y / 3.6f), playerFace.Y + (playerFace.X / 3.6f)), playerShooting));
                                            break;
                                        case 3:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X + (playerFace.Y / 3f), playerFace.Y - (playerFace.X / 3f)), playerShooting));
                                            break;
                                        case 4:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X + (playerFace.Y / 3.6f), playerFace.Y - (playerFace.X / 3.6f)), playerShooting));
                                            break;
                                        case 5:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X - (playerFace.Y / 5.5f), playerFace.Y + (playerFace.X / 5.5f)), playerShooting));
                                            break;
                                        case 6:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X - (playerFace.Y / 10f), playerFace.Y + (playerFace.X / 10f)), playerShooting));
                                            break;
                                        case 7:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X + (playerFace.Y / 5.5f), playerFace.Y - (playerFace.X / 5.5f)), playerShooting));
                                            break;
                                        case 8:
                                            bullets.Add(new Bullet(bullet, PlaceBullet(true), 15, new Vector2(playerFace.X + (playerFace.Y / 10f), playerFace.Y - (playerFace.X / 10f)), playerShooting));
                                            break;
                                    }
                                playerShooting = false;
                                shotGunSound.Play();
                                bulletTime = 0;
                            }
                        for (int i = 0; i < bullets.Count; i++)
                            bullets[i].Move(borders, people, bullets, player, boss);
                        bombTime += gameTime.ElapsedGameTime.TotalSeconds;
                        if (mouseState.RightButton == ButtonState.Pressed)
                            if (bombTime >= bombReloadTime)
                            {
                                bombs.Add(new Bomb(bomb, explosion, (int)player.PlayerPosition.X, (int)player.PlayerPosition.Y, 20));
                                bombTime = 0;
                            }

                        //Boss Shooting
                        bossBulletTime += gameTime.ElapsedGameTime.TotalSeconds;
                        bossBulletTime2 += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (level == 1)
                        {
                            if (people.Contains(boss.BossCircle) == false)
                                people.Add(boss.BossCircle);
                            if (bossBulletTime >= 3)
                            {
                                bullets.Add(new Bullet(bullet, PlaceBullet(false), 30, GetBulletAngle(boss.BossPosition, player.PlayerPosition), playerShooting));
                                machineGunSound.Play();
                                bossBulletTime = 0;
                            }
                            boss.BossSpeed = GetSpeed(boss.BossPosition, player.PlayerPosition);
                            boss.Move(people);
                        }
                        else if (level == 2)
                        {
                            if (bossBulletTime >= 2.5f)
                            {
                                bullets.Add(new Bullet(bullet, PlaceBullet(false), 30, GetBulletAngle(boss.BossPosition, player.PlayerPosition), playerShooting));
                                machineGunSound.Play();
                                bossBulletTime = 0;
                            }
                            boss.BossSpeed = GetSpeed(boss.BossPosition, player.PlayerPosition);
                            boss.Move(people);
                        }
                        else if (level == 3)
                        {
                            if (bossBulletTime >= 2)
                            {
                                bullets.Add(new Bullet(bullet, PlaceBullet(false), 30, GetBulletAngle(boss.BossPosition, player.PlayerPosition), playerShooting));
                                machineGunSound.Play();
                                bossBulletTime = 0;
                            }
                            boss.BossSpeed = GetSpeed(boss.BossPosition, player.PlayerPosition);
                            boss.Move(people);
                        }
                        else if (level == 4)
                        {
                            if (bossBulletTime >= 1.5f)
                            {
                                bullets.Add(new Bullet(bullet, PlaceBullet(false), 30, GetBulletAngle(boss.BossPosition, player.PlayerPosition), playerShooting));
                                machineGunSound.Play();
                                bossBulletTime = 0;
                            }
                            boss.BossSpeed = GetSpeed(boss.BossPosition, player.PlayerPosition) * 1.5f;
                            boss.Move(people);
                        }
                        else if (level == 5)
                        {
                            if (bossBulletTime >= 1f)
                                if (bossBulletTime2 >= 1000)
                                {
                                    bullets.Add(new Bullet(bullet, PlaceBullet(false), 30, GetBulletAngle(boss.BossPosition, player.PlayerPosition), playerShooting));
                                    machineGunSound.Play();
                                    bossBulletTime2 = 0;
                                }
                            if (bossBulletTime >= 3)
                            {
                                bombs.Add(new Bomb(bomb, explosion, (int)boss.BossPosition.X, (int)boss.BossPosition.Y, 20));
                                bossBulletTime = 0;
                            }
                            boss.BossSpeed = GetSpeed(boss.BossPosition, player.PlayerPosition) * 2;
                            boss.Move(people);
                        }
                        else if (level == 6)
                        {
                            if (bossBulletTime >= 1)
                                if (bossBulletTime2 >= 200)
                                {
                                    bullets.Add(new Bullet(bullet, PlaceBullet(false), 30, GetBulletAngle(boss.BossPosition, player.PlayerPosition), playerShooting));
                                    machineGunSound.Play();
                                    bossBulletTime2 = 0;
                                }
                            if (bossBulletTime >= 2)
                            {
                                bombs.Add(new Bomb(bomb, explosion, (int)boss.BossPosition.X, (int)boss.BossPosition.Y, 20));
                                bossBulletTime = 0;
                            }
                            boss.BossSpeed = GetSpeed(boss.BossPosition, player.PlayerPosition);
                            boss.Move(people);
                        }                       

                        //Spawning Grunts
                        gruntSpawnTime += gameTime.ElapsedGameTime.TotalSeconds;
                        if (Math.Round(gruntSpawnTime % 5) == 0 & gruntSpawnTime > 1)
                        {
                            if (grunts.Count < level)
                                if (keepSpawning == true)
                                {
                                    for (int i = 0; i < 1; i++)
                                    {
                                        grunts.Add(new Grunt(enemy, square, frame, random.Next(200, 700), random.Next(200, 700), 60, level * 7));
                                        for (int a = 0; a < people.Count; a++)
                                            if (grunts[grunts.Count - 1].GruntCircle.Intersects(people[a]))
                                            {
                                                grunts.Remove(grunts[grunts.Count - 1]);
                                                i--;
                                                break;
                                            }
                                    }
                                    people.Add(grunts[grunts.Count - 1].GruntCircle);
                                    keepSpawning = false;
                                }
                        }
                        else
                            keepSpawning = true;

                        //Grunt Movement
                        if (level == 1 || level == 2)
                        {
                            foreach (Grunt grunt in grunts)
                            {
                                grunt.GruntSpeed = GetSpeed(grunt.GruntPosition, player.PlayerPosition) * 2;
                                grunt.Move(borders, people, player);
                            }
                            foreach (Grunt grunt in grunts)
                            {
                                bossRotation = GetAngle(grunt.GruntPosition, player.PlayerPosition);
                                grunt.GruntRotation = bossRotation;
                            }
                        }
                        else
                        {
                            foreach (Grunt grunt in grunts)
                            {
                                grunt.GruntSpeed = GetSpeed(grunt.GruntPosition, player.PlayerPosition) * (int)(level / 2 + 1);
                                grunt.Move(borders, people, player);
                            }
                            foreach (Grunt grunt in grunts)
                            {
                                bossRotation = GetAngle(grunt.GruntPosition, player.PlayerPosition);
                                grunt.GruntRotation = bossRotation;
                            }
                        }

                        //Grunt Punching
                        gruntPunchTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        foreach (Grunt grunt in grunts)
                            if (grunt.Intersects & Math.Round(gruntPunchTime % 500) == 0)
                            {
                                player.PlayerHealth -= 1;
                                switch (random.Next(1, 4))
                                {
                                    case 1:
                                        hurt1.Play();
                                        break;
                                    case 2:
                                        hurt2.Play();
                                        break;
                                    case 3:
                                        hurt3.Play();
                                        break;
                                }
                                punchSound.Play();
                            }

                        //Player Movement
                        player.PlayerSpeed = GetPlayerSpeed(keyboardState);
                        player.Move(borders, people);

                        //Damage Checks
                        for (int i = 0; i < bullets.Count; i++)
                        {
                            if (bullets[i].BulletHit)
                            {
                                if (player.PlayerCircle.Intersects(bullets[i].BulletCircle) & bullets[i].PlayerShot == false)
                                {
                                    player.PlayerHealth -= damage;
                                    switch (random.Next(1, 4))
                                    {
                                        case 1:
                                            hurt1.Play();
                                            break;
                                        case 2:
                                            hurt2.Play();
                                            break;
                                        case 3:
                                            hurt3.Play();
                                            break;
                                    }
                                    bullets.Remove(bullets[i]);
                                    break;
                                }
                                if (boss.BossCircle.Intersects(bullets[i].BulletCircle) || bossFace.Intersects(bullets[i].BulletCircle))
                                {
                                    boss.BossHealth -= 1;
                                    bullets.Remove(bullets[i]);
                                    break;
                                }
                                foreach (Grunt grunt in grunts)
                                    if (grunt.GruntCircle.Intersects(bullets[i].BulletCircle) & bullets[i].PlayerShot)
                                    {
                                        grunt.TakeDamage();
                                        bullets.Remove(bullets[i]);
                                        break;
                                    }
                            }
                        }

                        foreach (Bomb bomb in bombs)
                        {
                            bomb.Fuse(gameTime, bombs);
                            if (bomb.Exploding)
                            {
                                if (player.PlayerCircle.Intersects(bomb.BombCircle))
                                    player.PlayerHealth -= 1;
                                if (boss.BossCircle.Intersects(bomb.BombCircle) & level != 6)
                                    boss.BossHealth -= 1;
                                foreach (Grunt grunt in grunts)
                                    if (grunt.GruntCircle.Intersects(bomb.BombCircle))
                                        grunt.TakeDamage();
                                if (explosionInstance.State != SoundState.Playing & bomb.FuseTime < 0.5)
                                    explosionInstance.Play();
                            }
                            if (bomb.FuseTime >= 2 & bomb.Exploding)
                            {
                                bombs.Remove(bomb);
                                break;
                            }
                        }

                        if (player.PlayerHealth <= 0)
                            screen = Screen.dead;
                        if (boss.BossHealth <= 0)
                        {
                            explosionInstance.Stop();
                            explosionInstance.Play();
                            skillPoints ++;
                            level ++;
                            screen = Screen.win;
                        }
                        for (int i = 0; i < grunts.Count; i++)
                        {
                            if (grunts[i].GruntHealth <= 0)
                            {
                                string buffType = "";
                                switch (random.Next(1, 5))
                                {
                                    case 1:
                                        break;
                                    case 2:
                                        buffType = "armour";
                                        break;
                                    case 3:
                                        buffType = "speed";
                                        break;
                                    case 4:
                                        buffType = "ammo";
                                        break;
                                }
                                if (buffType != "")
                                    buffs.Add(new Buff(new Circle(new Vector2(grunts[i].GruntCircle.MiddleX, grunts[i].GruntCircle.MiddleY), grunts[i].GruntCircle.Radius * 1.3f), buffType, armourBuff, speedBuff, ammoBuff));
                                people.Remove(grunts[i].GruntCircle);
                                grunts.Remove(grunts[i]);
                            }
                        }
                    }
                }

                //Shop
                else if (prevExitState == false & screen == Screen.shop)
                {
                    speed = 4;
                    damage = 2;
                    machineGunReloadTime = 100;
                    shotGunReloadTime = 500;
                    bombReloadTime = 0;
                    bullets.Clear();
                    bombs.Clear();
                    people.Clear();
                    grunts.Clear();
                    buffs.Clear();
                    armourBuffTime = 0;
                    ammoBuffTime = 0;
                    speedBuffTime = 0;
                    totalArmourBuffTime = 0;
                    totalAmmoBuffTime = 0;
                    totalSpeedBuffTime = 0;
                    bossBulletTime = 0;
                    bossBulletTime2 = 0;
                    gruntSpawnTime = 0;
                    machineGunReloadTime = 100;
                    shotGunReloadTime = 850;
                    bombReloadTime = 5;
                    startTime = 5;
                    keepSpawning = false;

                    if (armourBuffBTN.Click(mouseState, prevMouseState) & armourAmountUpgrade == false & skillPoints >= armourAmountCost)
                    {
                        clickInstance.Play();
                        armourBuffAmount += 1;
                        armourAmountUpgrade = true;
                        skillPoints -= armourAmountCost;
                        armourTimeCost++;
                    }
                    else if (armourTimeBTN.Click(mouseState, prevMouseState) & armourTimeUpgrade == false & skillPoints >= armourTimeCost)
                    {
                        clickInstance.Play();
                        armourTimeUpgrade = true;
                        skillPoints -= armourTimeCost;
                        armourAmountCost++;
                    }
                    else if (ammoBuffBTN.Click(mouseState, prevMouseState) & ammoAmountUpgrade == false & skillPoints >= ammoAmountCost)
                    {
                        clickInstance.Play();
                        ammoBuffAmount -= 0.17f;
                        ammoAmountUpgrade = true;
                        skillPoints -= ammoAmountCost;
                        ammoTimeCost++;
                    }
                    else if (ammoTimeBTN.Click(mouseState, prevMouseState) & ammoTimeUpgrade == false & skillPoints >= ammoTimeCost)
                    {
                        clickInstance.Play();
                        ammoTimeUpgrade = true;
                        skillPoints -= ammoTimeCost;
                        ammoAmountCost++;
                    }
                    else if (speedBuffBTN.Click(mouseState, prevMouseState) & speedAmountUpgrade == false & skillPoints >= speedAmountCost)
                    {
                        clickInstance.Play();
                        speedBuffAmount += 2;
                        speedAmountUpgrade = true;
                        skillPoints -= speedAmountCost;
                        speedTimeCost++;
                    }
                    else if (speedTimeBTN.Click(mouseState, prevMouseState) & speedTimeUpgrade == false & skillPoints >= speedTimeCost)
                    {
                        clickInstance.Play();
                        speedTimeUpgrade = true;
                        skillPoints -= speedTimeCost;
                        speedAmountCost++;
                    }
                    if (continueBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        screen = Screen.battle;
                    }
                }
                
                //Death
                else if (screen == Screen.dead)
                {
                    if (deathQuitBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        Exit();
                    }
                    if (deathMenuBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        startUp = true;
                        screen = Screen.intro;
                    }
                }

                //Tutorial
                else if (screen == Screen.tutorial)
                {
                    if (borders.Count == 0)
                    {
                        borders.Add(new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, 85));
                        borders.Add(new Rectangle(0, _graphics.PreferredBackBufferHeight - 85, _graphics.PreferredBackBufferWidth, 85));
                        borders.Add(new Rectangle(0, 0, 85, _graphics.PreferredBackBufferHeight));
                        borders.Add(new Rectangle(_graphics.PreferredBackBufferWidth - 85, 0, 85, _graphics.PreferredBackBufferHeight));
                    }
                    people.Remove(player.PlayerCircle);
                    player = new Player(playerRotation, currentPlayer, square, frame, _graphics.PreferredBackBufferWidth / 2 + 200, _graphics.PreferredBackBufferHeight / 2, 50, 35, 10);
                    people.Add(player.PlayerCircle);
                    bullets.Clear();
                    if (level != 6)
                        boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 150, 150, 1);
                    else
                    {
                        bossRotation += 1.575f;
                        boss = new Boss(currentBoss, square, frame, _graphics.PreferredBackBufferWidth / 2 - 200, _graphics.PreferredBackBufferHeight / 2, 100, 250, 1);
                    }
                    people.Add(boss.BossCircle);
                    if (skipBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        boss.BossHealth = 0;
                        screen = Screen.battle;
                    }
                    else if (tutorialBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        tutorialPhase++;
                    }
                    else if (prevBTN.Click(mouseState, prevMouseState) & tutorialPhase > 1)
                    {
                        clickInstance.Play();
                        tutorialPhase--;
                    }
                    if (tutorialPhase == 9)
                    {
                        boss.BossHealth = 0;
                        screen = Screen.battle;
                    }
                }
                
                //Win
                else if (screen == Screen.win)
                {
                    boss.BossTexture = explosion;
                    if (continueBTN.Click(mouseState, prevMouseState))
                    {
                        clickInstance.Play();
                        if (level <= 6)
                            screen = Screen.shop;
                        else
                        {
                            startUp = true;
                            player = new Player(playerRotation, currentPlayer, blankTexture, blankTexture, 165, 240, 200, 142, 10);
                            screen = Screen.intro;
                        }
                    }
                }
            }

            //Cursor Movement
            cursorRect.X = mouseState.X - 18;
            cursorRect.Y = mouseState.Y - 18;
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            
            //Battle*
            if (screen == Screen.battle || screen == Screen.dead || screen == Screen.tutorial || screen == Screen.win)
            {
                //Battle
                _spriteBatch.Draw(arena, screenRect, Color.White);
                foreach (Buff buff in buffs)
                    buff.Draw(_spriteBatch);
                foreach (Bomb bomb in bombs)
                    bomb.Draw(_spriteBatch);
                for (int i = 0; i < bullets.Count; i++)
                    bullets[i].Draw(_spriteBatch);
                foreach (Grunt grunt in grunts)
                {
                    if (grunt.Intersects)
                    {
                        if (Math.Round(gruntPunchTime % 500) == 0 & grunt.GruntTexture == enemy || Math.Round(gruntPunchTime % 500) == 0 & grunt.GruntTexture == rightPunch)
                            grunt.GruntTexture = leftPunch;
                        else if (Math.Round(gruntPunchTime % 500) == 0 & grunt.GruntTexture == leftPunch)
                            grunt.GruntTexture = rightPunch;
                    }
                    else
                        grunt.GruntTexture = enemy;
                    grunt.Draw(_spriteBatch);
                }
                if (player.PlayerHealth > 0 & startTime > 0)
                {
                    _spriteBatch.Draw(square, screenRect, Color.Black * 0.3f);
                    boss.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    if (bombTime < 5)
                        _spriteBatch.Draw(square, new Rectangle((int)player.PlayerPosition.X - 30, (int)player.PlayerPosition.Y + 30, (int)(58 * (bombTime / 5)), 10), Color.White * 0.7f);
                    else
                        _spriteBatch.Draw(square, new Rectangle((int)player.PlayerPosition.X - 30, (int)player.PlayerPosition.Y + 30, 58, 10), Color.White * 0.7f);
                    if (screen != Screen.tutorial)
                        _spriteBatch.DrawString(titleFont, Math.Round(startTime) + "", new Vector2(440, 450), Color.White);
                }
                if (player.PlayerHealth > 0 & startTime <= 0)
                {
                    player.Draw(_spriteBatch);
                    if (bombTime < 5)
                        _spriteBatch.Draw(square, new Rectangle((int)player.PlayerPosition.X - 30, (int)player.PlayerPosition.Y + 30, (int)(58 * (bombTime / 5)), 10), Color.White * 0.7f);
                    else
                        _spriteBatch.Draw(square, new Rectangle((int)player.PlayerPosition.X - 30, (int)player.PlayerPosition.Y + 30, 58, 10), Color.White * 0.7f);
                }
                if (boss.BossHealth > 0 & startTime <= 0)
                    boss.Draw(_spriteBatch);
                foreach (Rectangle border in borders)
                    _spriteBatch.Draw(square, border, Color.White * 0f);

                //Dead
                if (screen == Screen.dead)
                {
                    _spriteBatch.Draw(square, screenRect, Color.Black * 0.3f);
                    _spriteBatch.Draw(skull, new Rectangle(270, 275, 350, 350), Color.White);
                    deathQuitBTN.Draw(_spriteBatch, mouseState);
                    deathMenuBTN.Draw(_spriteBatch, mouseState);
                }

                //Tutorial
                if (screen == Screen.tutorial)
                {
                    tutorialBTN.Draw(_spriteBatch, mouseState);
                    skipBTN.Draw(_spriteBatch, mouseState);
                    if (tutorialPhase > 1)
                        prevBTN.Draw(_spriteBatch, mouseState);
                    switch (tutorialPhase)
                    {
                        case 0:
                            _spriteBatch.DrawString(menuFont, "      Welcome to Cool Game\nWould you like see the tutorial?", new Vector2(20, 200), Color.White);
                            break;
                        case 1:
                            //Explain the player
                            _spriteBatch.DrawString(menuFont, "         This is you\n      What's your goal?\nDon't die and kill that thing", new Vector2(60, 200), Color.White);
                            _spriteBatch.DrawString(shopFont, "-(That thing)           (You)-", new Vector2(310, 443), Color.White);
                            break;
                        case 2:
                            //Explain the tank
                            _spriteBatch.DrawString(menuFont, "       That thing's\ngoing to try and kill you\n  Don't let that happen", new Vector2(110, 200), Color.White);
                            _spriteBatch.DrawString(shopFont, "(Don't let this hit you)", new Vector2(300, 427), Color.White);
                            _spriteBatch.DrawString(shopFont, "*pew*", new Vector2(310, 443), Color.White);

                            _spriteBatch.Draw(bullet, new Rectangle(360, 435, 30, 30), Color.Black);
                            break;
                        case 3:
                            //What grunts are
                            _spriteBatch.Draw(rightPunch, new Rectangle(580, 420, 60, 60), Color.White);
                            _spriteBatch.DrawString(menuFont, "Sometimes those things will spawn", new Vector2(5, 200), Color.White);
                            _spriteBatch.DrawString(shopFont, "*punching sounds*", new Vector2(500, 500), Color.White);

                            break;
                        case 4:
                            //Powerups
                            _spriteBatch.DrawString(menuFont, " If you kill them\nthey drop powerups\n(Most of the time)", new Vector2(200, 200), Color.White);
                            _spriteBatch.Draw(ammoBuff, new Rectangle(580, 420, 60, 60), Color.White);
                            _spriteBatch.Draw(ammoBuff, new Rectangle(280, 500, 60, 60), Color.White);
                            _spriteBatch.DrawString(shopFont, "+Firing Speed", new Vector2(230, 560), Color.White);
                            _spriteBatch.Draw(armourBuff, new Rectangle(430, 500, 60, 60), Color.White);
                            _spriteBatch.DrawString(shopFont, "+Damage Res", new Vector2(400, 560), Color.White);
                            _spriteBatch.Draw(speedBuff, new Rectangle(580, 500, 60, 60), Color.White);
                            _spriteBatch.DrawString(shopFont, "+Speed", new Vector2(575, 560), Color.White);
                            break;
                        case 5:
                            //How bombs work
                            _spriteBatch.DrawString(menuFont, "Right click to place bombs\n     Carefull  though\n     They hurt you to", new Vector2(100, 200), Color.White);
                            _spriteBatch.Draw(bomb, new Rectangle(400, 450, 30, 30), Color.White);
                            _spriteBatch.Draw(explosion, new Rectangle(500, 450, 30, 30), Color.White);
                            _spriteBatch.DrawString(shopFont, "pre boom", new Vector2(370, 500), Color.White);
                            _spriteBatch.DrawString(shopFont, "post boom", new Vector2(475, 500), Color.White);
                            _spriteBatch.DrawString(shopFont, "<-(bomb recharge\n       time)", new Vector2(680, 478), Color.White);
                            break;
                        case 6:
                            //How the shop works part 1

                            //Draw the shop

                            _spriteBatch.Draw(square, screenRect, Color.White);

                            _spriteBatch.DrawString(menuFont, $"Skill Points:{skillPoints}", new Vector2(37, 150), Color.Black);

                            //Icons
                            _spriteBatch.Draw(armourBuff, new Rectangle(0, 150, 300, 300), Color.White);
                            _spriteBatch.Draw(ammoBuff, new Rectangle(300, 150, 300, 300), Color.White);
                            _spriteBatch.Draw(speedBuff, new Rectangle(585, 150, 300, 300), Color.White);

                            //Buttons
                            armourBuffBTN.Draw(_spriteBatch, mouseState);
                            ammoBuffBTN.Draw(_spriteBatch, mouseState);
                            speedBuffBTN.Draw(_spriteBatch, mouseState);
                            armourTimeBTN.Draw(_spriteBatch, mouseState);
                            ammoTimeBTN.Draw(_spriteBatch, mouseState);
                            speedTimeBTN.Draw(_spriteBatch, mouseState);

                            //Cost
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(armourBuffBTN.ButtonRectangle.X + 10, armourBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(armourTimeBTN.ButtonRectangle.X + 10, armourTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(ammoBuffBTN.ButtonRectangle.X + 10, ammoBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(ammoTimeBTN.ButtonRectangle.X + 10, ammoTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(speedBuffBTN.ButtonRectangle.X + 10, speedBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(speedTimeBTN.ButtonRectangle.X + 10, speedTimeBTN.ButtonRectangle.Y + 10), Color.Black);

                            //Tutorial
                            skipBTN.Draw(_spriteBatch, mouseState);
                            prevBTN.Draw(_spriteBatch, mouseState);
                            tutorialBTN.Draw(_spriteBatch, mouseState);
                            _spriteBatch.DrawString(menuFont, "       This is  The Shop\n       After every level\nyou can upgrade the powerups here", new Vector2(5, 50), Color.Black);
                            break;
                        case 7:
                            //How the shop works part 2

                            //Draw the shop

                            _spriteBatch.Draw(square, screenRect, Color.White);

                            _spriteBatch.DrawString(menuFont, $"Skill Points:{skillPoints}", new Vector2(37, 150), Color.Black);

                            //Icons
                            _spriteBatch.Draw(armourBuff, new Rectangle(0, 150, 300, 300), Color.White);
                            _spriteBatch.Draw(ammoBuff, new Rectangle(300, 150, 300, 300), Color.White);
                            _spriteBatch.Draw(speedBuff, new Rectangle(585, 150, 300, 300), Color.White);

                            //Buttons
                            armourBuffBTN.Draw(_spriteBatch, mouseState);
                            ammoBuffBTN.Draw(_spriteBatch, mouseState);
                            speedBuffBTN.Draw(_spriteBatch, mouseState);
                            armourTimeBTN.Draw(_spriteBatch, mouseState);
                            ammoTimeBTN.Draw(_spriteBatch, mouseState);
                            speedTimeBTN.Draw(_spriteBatch, mouseState);

                            //Cost
                            _spriteBatch.DrawString(shopFont, "Bought", new Vector2(armourBuffBTN.ButtonRectangle.X + 10, armourBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 2", new Vector2(armourTimeBTN.ButtonRectangle.X + 10, armourTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 2", new Vector2(ammoBuffBTN.ButtonRectangle.X + 10, ammoBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Bought", new Vector2(ammoTimeBTN.ButtonRectangle.X + 10, ammoTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(speedBuffBTN.ButtonRectangle.X + 10, speedBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                            _spriteBatch.DrawString(shopFont, "Cost: 1", new Vector2(speedTimeBTN.ButtonRectangle.X + 10, speedTimeBTN.ButtonRectangle.Y + 10), Color.Black);

                            //Tutorial
                            skipBTN.Draw(_spriteBatch, mouseState);
                            prevBTN.Draw(_spriteBatch, mouseState);
                            tutorialBTN.Draw(_spriteBatch, mouseState);
                            _spriteBatch.DrawString(menuFont, "Once you upgrade a powerup once\n        the second one\n     will cost twice as much", new Vector2(30, 50), Color.Black);
                            break;
                        case 8:
                            //Exit
                            _spriteBatch.DrawString(menuFont, "Well that's it from me\n      Good luck!", new Vector2(150, 200), Color.White);
                            break;
                    }
                }

                //Win
                if (screen == Screen.win)
                {
                    _spriteBatch.Draw(square, screenRect, Color.Black * 0.3f);
                    boss.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    continueBTN.Draw(_spriteBatch, mouseState);
                }
            }
            
            //Shop
            else if (screen == Screen.shop)
            {
                _spriteBatch.DrawString(menuFont, $"Skill Points:{skillPoints}", new Vector2(37, 150), Color.Black);

                //Icons
                _spriteBatch.Draw(armourBuff, new Rectangle(0, 150, 300, 300), Color.White);
                _spriteBatch.Draw(ammoBuff, new Rectangle(300, 150, 300, 300), Color.White);
                _spriteBatch.Draw(speedBuff, new Rectangle(585, 150, 300, 300), Color.White);

                //Buttons
                armourBuffBTN.Draw(_spriteBatch, mouseState);
                ammoBuffBTN.Draw(_spriteBatch, mouseState);
                speedBuffBTN.Draw(_spriteBatch, mouseState);
                armourTimeBTN.Draw(_spriteBatch, mouseState);
                ammoTimeBTN.Draw(_spriteBatch, mouseState);
                speedTimeBTN.Draw(_spriteBatch, mouseState);

                //Armour Cost
                if (armourAmountUpgrade == false)
                    _spriteBatch.DrawString(shopFont, $"Cost: {armourAmountCost}", new Vector2(armourBuffBTN.ButtonRectangle.X + 10, armourBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                else
                    _spriteBatch.DrawString(shopFont, "Bought", new Vector2(armourBuffBTN.ButtonRectangle.X + 10, armourBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                if (armourTimeUpgrade == false)
                    _spriteBatch.DrawString(shopFont, $"Cost: {armourTimeCost}", new Vector2(armourTimeBTN.ButtonRectangle.X + 10, armourTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                else
                    _spriteBatch.DrawString(shopFont, "Bought", new Vector2(armourTimeBTN.ButtonRectangle.X + 10, armourTimeBTN.ButtonRectangle.Y + 10), Color.Black);

                //Ammo Cost
                if (ammoAmountUpgrade == false)
                    _spriteBatch.DrawString(shopFont, $"Cost: {ammoAmountCost}", new Vector2(ammoBuffBTN.ButtonRectangle.X + 10, ammoBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                else
                    _spriteBatch.DrawString(shopFont, "Bought", new Vector2(ammoBuffBTN.ButtonRectangle.X + 10, armourBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                if (ammoTimeUpgrade == false)
                    _spriteBatch.DrawString(shopFont, $"Cost: {ammoTimeCost}", new Vector2(ammoTimeBTN.ButtonRectangle.X + 10, ammoTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                else
                    _spriteBatch.DrawString(shopFont, "Bought", new Vector2(ammoTimeBTN.ButtonRectangle.X + 10, ammoTimeBTN.ButtonRectangle.Y + 10), Color.Black);

                //Speed Cost
                if (speedAmountUpgrade == false)
                    _spriteBatch.DrawString(shopFont, $"Cost: {speedAmountCost}", new Vector2(speedBuffBTN.ButtonRectangle.X + 10, speedBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                else
                    _spriteBatch.DrawString(shopFont, "Bought", new Vector2(speedBuffBTN.ButtonRectangle.X + 10, speedBuffBTN.ButtonRectangle.Y + 10), Color.Black);
                if (speedTimeUpgrade == false)
                    _spriteBatch.DrawString(shopFont, $"Cost: {speedTimeCost}", new Vector2(speedTimeBTN.ButtonRectangle.X + 10, speedTimeBTN.ButtonRectangle.Y + 10), Color.Black);
                else
                    _spriteBatch.DrawString(shopFont, "Bought", new Vector2(speedTimeBTN.ButtonRectangle.X + 10, speedTimeBTN.ButtonRectangle.Y + 10), Color.Black);

                //Continue
                continueBTN.Draw(_spriteBatch, mouseState);
            }
            
            //Intro
            else if (screen == Screen.intro)
            {
                for (int i = 0; i < bullets.Count; i++)
                    bullets[i].Draw(_spriteBatch);
                player.Draw(_spriteBatch);
                machineGunBTN.Draw(_spriteBatch, mouseState);
                shotGunBTN.Draw(_spriteBatch, mouseState);
                _spriteBatch.DrawString(shopFont, "Machine Gun", new Vector2(100, 385), Color.Black);
                _spriteBatch.DrawString(shopFont, "Shot Gun", new Vector2(117, 475), Color.Black);
                playBTN.Draw(_spriteBatch, mouseState);
                _spriteBatch.Draw(machineGun, machineGunBTN.ButtonRectangle, Color.White);
                _spriteBatch.Draw(shotGun, shotGunBTN.ButtonRectangle, Color.White);
                _spriteBatch.DrawString(titleFont, "Cool Game", new Vector2(285, 170), Color.Black);
                _spriteBatch.DrawString(menuFont, "Mouse - Aim\nWASD/Arrow Keys - Move\nLeft Click - Shoot\nRight Click - Bomb\nEscape - Pause", new Vector2(290, 320), Color.Black);
                if (defeatedBlueTank == false)
                    _spriteBatch.Draw(unknown, new Rectangle(270, 500, 150, 150), Color.White);
                else
                    _spriteBatch.Draw(bossBlue, new Rectangle(270, 500, 150, 150), Color.White);
                if (defeatedRedTank == false)
                    _spriteBatch.Draw(unknown, new Rectangle(440, 500, 150, 150), Color.White);
                else
                    _spriteBatch.Draw(bossRed, new Rectangle(440, 500, 150, 150), Color.White);
                if (defeatedHelicopter == false)
                    _spriteBatch.Draw(unknown, new Rectangle(610, 500, 150, 150), Color.White);
                else
                    _spriteBatch.Draw(helicopter, new Rectangle(715, 575, 100, 250), null, Color.White, 1.575f, new Vector2(helicopter.Width / 2, helicopter.Height / 2), SpriteEffects.None, 0f);

            }
            
            //Exit
            if (exit == true)
            {
                _spriteBatch.Draw(square, screenRect, Color.Black * 0.3f);
                if (screen == Screen.battle)
                    player.Draw(_spriteBatch);
                _spriteBatch.DrawString(menuFont, "Quit to Menu?", new Vector2(280, 250), Color.Black);
                _spriteBatch.DrawString(menuFont, "(Your progress won't be saved)", new Vector2(50, 280), Color.Black);
                quitBTN.Draw(_spriteBatch, mouseState);
                dontQuitBTN.Draw(_spriteBatch, mouseState);
            }

            _spriteBatch.Draw(cursor, cursorRect, Color.Black);

            //Debugers
                //Nothing
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        //Custom methods
        public float GetAngle(Vector2 originPoint, Vector2 secondPoint)
        {
            float rise = secondPoint.Y - originPoint.Y;
            float run = secondPoint.X - originPoint.X;
            if (originPoint.X <= secondPoint.X && originPoint.Y <= secondPoint.Y || originPoint.X <= secondPoint.X && originPoint.Y >= secondPoint.Y)
                return (float)Math.Atan(rise / run);
            else
                return (float)(Math.PI + Math.Atan(rise / run));
        }
        public Vector2 GetSpeed(Vector2 originPos, Vector2 goalPos)
        {
            Vector2 difference = originPos - goalPos;
            difference.Normalize();
            difference *= -0.66f;
            return difference;
        }
        public Vector2 GetPlayerSpeed(KeyboardState keyboardState)
        {
            Vector2 finalSpeed;
            float speedY = 0, speedX = 0;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                speedX -= speed;
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                speedX += speed;
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                speedY -= speed;
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                speedY += speed;
            finalSpeed = new Vector2(speedX, speedY);
            if (speedX != 0 || speedY != 0)
                finalSpeed.Normalize();
            finalSpeed *= speed;
            return finalSpeed;
        }
        public Vector2 GetBulletAngle(Vector2 originPos, Vector2 goalPos)
        {
            Vector2 difference = originPos - goalPos;
            difference.Normalize();
            difference *= -5f;
            return difference;
        }
        public Vector2 PlaceBullet(bool playerShot)
        {
            float bulletX, bulletY;
            if (playerShot)
            {
                bulletX = (float)(player.PlayerPosition.X + Math.Cos(player.PlayerRotation) * 10);
                bulletY = (float)(player.PlayerPosition.Y + Math.Sin(player.PlayerRotation) * 10);
                bulletX -= 5;
                bulletY -= 5;
            }
            else
            {
                if (level != 6)
                {
                    bulletX = (float)(boss.BossPosition.X + Math.Cos(boss.BossRotation) * 70);
                    bulletY = (float)(boss.BossPosition.Y + Math.Sin(boss.BossRotation) * 70);
                    bulletX -= 10;
                    bulletY -= 10;
                }
                else
                {
                    bulletX = (float)(boss.BossPosition.X + Math.Cos(boss.BossRotation - 1.575f) * 70);
                    bulletY = (float)(boss.BossPosition.Y + Math.Sin(boss.BossRotation - 1.575f) * 70);
                    bulletX -= 10;
                    bulletY -= 10;
                }
            }

            return new Vector2(bulletX, bulletY);
        }
        public void Save()
        {
            acheivments = new StreamWriter("Acheivments.txt");
            acheivmentsList[0] =(Convert.ToString(defeatedBlueTank));
            acheivmentsList[1] =(Convert.ToString(defeatedRedTank));
            acheivmentsList[2] = (Convert.ToString(defeatedHelicopter));
            acheivmentsList[3] =(Convert.ToString(finishTime));
            foreach (string line in acheivmentsList)
                acheivments.WriteLine(line);
            acheivments.Close();
        }
        public void Load()
        {
            int i = 0;
            if (File.Exists("Acheivments.txt"))
                foreach (string line in File.ReadLines(@"Acheivments.txt"))
                {
                    if (i == 0)
                        defeatedBlueTank = Convert.ToBoolean(line.ToLower());
                    else if (i == 1)
                        defeatedRedTank = Convert.ToBoolean(line.ToLower());
                    else if (i == 2)
                        defeatedHelicopter = Convert.ToBoolean(line.ToLower());
                    else if (i == 3)
                        finishTime = Convert.ToDouble(line);
                    i++;
                }
            else
                Save();
        }
    }
}