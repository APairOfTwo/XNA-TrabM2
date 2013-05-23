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
        Texture2D playerSprite;
        Texture2D timeBar;

        Player player;
        List<Tile> tileMap;
        char[][] map;

        Rectangle timeRect;

        Telas telas;
        Botoes botoes;

        long totalTime = 1000;
        long timePassed = 0;
        int totalSecondsLeft = 10;

        bool startGame = false;
        bool gameOver = false;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            verdana = Content.Load<SpriteFont>(@"Fonts\Verdana");
            tileBlock = Content.Load<Texture2D>(@"Tiles\Block");
            playerSprite = Content.Load<Texture2D>(@"Sprites\Character");
            timeBar = Content.Load<Texture2D>(@"Sprites\TimeBar");
            
            tileMap = new List<Tile>();

            map = File.ReadAllLines(@"Content/TextFiles/mapa.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();
            int rows = map.Length;
            int cols = map.Max(subArray => subArray.Length);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (map[i][j] == 'P')
                    {
                        tileMap.Add(new Tile(tileBlock, new Vector2(j, i), TileCollision.Impassable));
                    }
                    if (map[i][j] == 'I')
                    {
                        player = new Player(playerSprite, new Vector2(j, i));
                    }
                }
            }

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
        }

        protected override void UnloadContent() { }

        protected void GetInputs(GameTime gameTime)
        {
            Input.Update(gameTime);

            if (Input.KeyboardEscapeJustPressed)
            {
                telas.TelaAtual = Telas.Tipo.Inicial;
                startGame = false;
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
            GetInputs(gameTime);

            botoes.Update();
            player.Update();

            if (!gameOver)
            {
                if (startGame)
                {
                    timePassed += gameTime.ElapsedGameTime.Milliseconds;

                    if (timePassed >= totalTime)
                    {
                        timePassed = 0;
                        totalSecondsLeft--;
                        
                        if (timeRect.Height > 0)
                            timeRect.Height -= 10;
                        else
                            gameOver = true;
                    }
                }
            }
            else
            {
                telas.TelaAtual = Telas.Tipo.GameOver;
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

                    spriteBatch.Draw(timeBar, timeRect, Color.White);
                }
            }
            spriteBatch.End();
        }

        #region Eventos dos Botões
        public void BotaoAzul_Click(object sender, EventArgs e)
        {
            telas.TelaAtual = Telas.Tipo.Jogo;
            startGame = true;
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
    }
}
