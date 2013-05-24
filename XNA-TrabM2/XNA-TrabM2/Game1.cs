using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace XNA_TrabM2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont verdana;

        Texture2D tileBlock;
        Texture2D finalSprite;
        Texture2D playerSprite;
        Texture2D timeBar;
        Texture2D boosterSprite;

        Player player;
        static List<Tile> tileMap;
        static List<Booster> timeBoosters;
        char[][] map;
        char[][] map2;
        char[][] map3;

        int mapCont = 1;

        FinalBlock finalBlock;

        Rectangle timeRect;

        Telas telas;
        Botoes botoes;

        long totalTime = 1000;
        long timePassed = 0;
        int totalSecondsLeft = 10;

        bool startGame = false;
        bool gameOver = false;
        bool gameWin = false;
        bool playOnce = false;

        SoundEffect menuMusic;
        SoundEffectInstance menuMusicInstance;
        SoundEffect gameMusic;
        SoundEffectInstance gameMusicInstance;
        SoundEffect sfxGameWin;
        SoundEffectInstance sfxGameWinInstance;
        SoundEffect sfxGameOver;
        SoundEffectInstance sfxGameOverInstance;
        SoundEffect sfxPickup;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            telas = new Telas();
            timeRect = new Rectangle(700, 40, 25, 100);
            botoes = new Botoes();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            menuMusic = Content.Load<SoundEffect>(@"Audio\MenuMusic");
            menuMusicInstance = menuMusic.CreateInstance();
            gameMusic = Content.Load<SoundEffect>(@"Audio\GameMusic");
            gameMusicInstance = gameMusic.CreateInstance();
            sfxGameWin = Content.Load<SoundEffect>(@"Audio\GameWin");
            sfxGameWinInstance = sfxGameWin.CreateInstance();
            sfxGameOver = Content.Load<SoundEffect>(@"Audio\GameOver");
            sfxGameOverInstance = sfxGameOver.CreateInstance();
            sfxPickup = Content.Load<SoundEffect>(@"Audio\Pickup");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            verdana = Content.Load<SpriteFont>(@"Fonts\Verdana");
            tileBlock = Content.Load<Texture2D>(@"Tiles\Block");
            playerSprite = Content.Load<Texture2D>(@"Sprites\Character");
            timeBar = Content.Load<Texture2D>(@"Sprites\TimeBar");
            boosterSprite = Content.Load<Texture2D>(@"Sprites\Booster");
            finalSprite = Content.Load<Texture2D>(@"Sprites\FinalBlock");
            
            tileMap = new List<Tile>();
            timeBoosters = new List<Booster>();

            map = File.ReadAllLines(@"Content/TextFiles/mapa.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();
            map2 = File.ReadAllLines(@"Content/TextFiles/mapa2.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();
            map3 = File.ReadAllLines(@"Content/TextFiles/mapa3.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();

            loadMap(map);

            botoes.Add(new clsButton(this, Content.Load<Texture2D>(@"botoes\blackBall"),
                                     Content.Load<Texture2D>(@"botoes\blueBall"), new Vector2(380, 200),
                                     new EventHandler(BotaoAzul_Click)));
            botoes.Add(new clsButton(this, Content.Load<Texture2D>(@"botoes\blackBall"),
                                     Content.Load<Texture2D>(@"botoes\orangeBall"), new Vector2(380, 280),
                                     new EventHandler(BotaoLaranja_Click)));
            botoes.Add(new clsButton(this, Content.Load<Texture2D>(@"botoes\blackBall"),
                                     Content.Load<Texture2D>(@"botoes\redBall"), new Vector2(380, 360),
                                     new EventHandler(BotaoVermelho_Click)));

            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Instrucoes"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Jogo"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Vitoria"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_GameOver"));
            telas.TelaAtual = Telas.Tipo.Inicial;

            playMenuMusic();
        }

        protected override void UnloadContent() { }

        protected void GetInputs(GameTime gameTime)
        {
            Input.Update(gameTime);

            if (Input.KeyboardEscapeJustPressed)
            {
                telas.TelaAtual = Telas.Tipo.Inicial;
                startGame = false;
                gameOver = false;
                gameWin = false;
                playOnce = false;
                clearStage();
                mapCont = 1;
                playMenuMusic();
                loadMap(map);
                gameMusicInstance.Pause();
            }

            if (startGame)
            {
                if (Input.KeyboardLeftJustPressed)
                {
                    player.blockPosition.X -= 1;
                }
                if (Input.KeyboardRightJustPressed)
                {
                    player.blockPosition.X += 1;
                }
                if (Input.KeyboardUpJustPressed)
                {
                    player.blockPosition.Y -= 1;
                }
                if (Input.KeyboardDownJustPressed)
                {
                    player.blockPosition.Y += 1;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            botoes.Update();

            player.oldBlockPosition = player.blockPosition;
            GetInputs(gameTime);
            player.Update();

            foreach(Tile t in tileMap)
            {
                if (player.blockPosition == t.blockPosition)
                {
                    player.blockPosition = player.oldBlockPosition;
                }
            }

            foreach (Booster b in timeBoosters)
            {
                if (b.active && player.blockPosition == b.blockPosition)
                {
                    sfxPickup.Play();
                    b.active = false;
                    timeRect.Height += 30;
                    totalSecondsLeft += 3;
                }
            }

            if (!gameOver)
            {
                if (startGame)
                {
                    timePassed += gameTime.ElapsedGameTime.Milliseconds;
                    gameMusicInstance.Pitch = -1 * ((float)totalSecondsLeft / 10) + 1;

                    if (timePassed >= totalTime)
                    {
                        timePassed = 0;
                        totalSecondsLeft--;

                        if (timeRect.Height > 0)
                            timeRect.Height -= 10;
                        else
                        {
                            gameOver = true;
                            gameMusicInstance.Pause();
                        }
                    }
                }
            }
            else
            {
                if (gameWin)
                {
                    if (!playOnce)
                    {
                        playOnce = true;
                        telas.TelaAtual = Telas.Tipo.Vitoria;
                        gameMusicInstance.Pause();
                        sfxGameWinInstance.Play();
                    }
                }
                else
                {
                    if (!playOnce)
                    {
                        playOnce = true;
                        telas.TelaAtual = Telas.Tipo.GameOver;
                        sfxGameOverInstance.Play();
                    }
                }
            }

            if (player.blockPosition == finalBlock.blockPosition)
            {
                clearStage();

                if (mapCont == 1)
                {
                    loadMap(map2);
                    mapCont = 2;
                    return;
                }
                if (mapCont == 2)
                {
                    loadMap(map3);
                    mapCont = 3;
                    return;
                }
                if (mapCont == 3)
                {
                    gameWin = true;
                    gameOver = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            
            spriteBatch.Begin();
            {
                telas.Draw(spriteBatch);

                if (telas.TelaAtual == Telas.Tipo.Inicial)
                    botoes.Draw(spriteBatch);

                if (startGame && !gameOver)
                {
                    spriteBatch.DrawString(verdana, "Time: "+totalSecondsLeft+"s", new Vector2(680, 5), Color.Red);
                    
                    player.Draw(spriteBatch);

                    foreach (Tile t in tileMap)
                    {
                        t.Draw(spriteBatch);
                    }
                    foreach (Booster b in timeBoosters)
                    {
                        b.Draw(spriteBatch);
                    }

                    spriteBatch.Draw(timeBar, timeRect, Color.White);
                    finalBlock.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        #region Play Audio
        public void playMenuMusic()
        {
            if (menuMusicInstance.State == SoundState.Stopped)
            {
                menuMusicInstance.Volume = 0.75f;
                menuMusicInstance.IsLooped = true;
                menuMusicInstance.Play();
            }
            else
                menuMusicInstance.Resume();
        }

        public void playGameMusic()
        {
            if (gameMusicInstance.State == SoundState.Stopped)
            {
                gameMusicInstance.Volume = 0.75f;
                gameMusicInstance.IsLooped = true;
                gameMusicInstance.Play();
            }
            else
                gameMusicInstance.Resume();
        }

        public void playSfx(SoundEffectInstance sfxInstance)
        {

        }
        #endregion

        #region Eventos dos Botões
        public void BotaoAzul_Click(object sender, EventArgs e)
        {
            menuMusicInstance.Pause();
            telas.TelaAtual = Telas.Tipo.Jogo;
            startGame = true;
            playGameMusic();
        }

        public void BotaoLaranja_Click(object sender, EventArgs e)
        {
            telas.TelaAtual = Telas.Tipo.Instrucoes;
        }

        public void BotaoVermelho_Click(object sender, EventArgs e)
        {
            this.Exit();
        }
        #endregion

        public void clearStage()
        {
            tileMap.Clear();
            timeBoosters.Clear();
            timePassed = 0;
            totalSecondsLeft = 10;
            timeRect.Height = 100;
            gameMusicInstance.Pitch = 0;
        }
        
        public void loadMap(char[][] map)
        {
            int rows = map.Length;
            int cols = map.Max(subArray => subArray.Length);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (map[i][j] == 'P')
                    {
                        tileMap.Add(new Tile(tileBlock, new Vector2(j, i)));
                    }
                    if (map[i][j] == 'I')
                    {
                        player = new Player(playerSprite, new Vector2(j, i));
                    }
                    if (map[i][j] == '2')
                    {
                        timeBoosters.Add(new Booster(boosterSprite, new Vector2(j, i)));
                    }
                    if (map[i][j] == 'F')
                    {
                        finalBlock = new FinalBlock(finalSprite, new Vector2(j, i));
                    }
                }
            }
        }
    }
}

