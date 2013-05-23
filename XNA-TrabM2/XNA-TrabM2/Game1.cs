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

        Texture2D tileBlock;
        Texture2D playerSprite;
        Texture2D barraTempo;

        Player player;
        List<Tile> tileMap;
        char[][] map;

        Rectangle timeRect;

        Telas telas;

        bool startGame = true;
        bool gameOver = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            telas = new Telas();
            timeRect = new Rectangle(700, 20, 25, 500);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileBlock = Content.Load<Texture2D>(@"Tiles\Block");
            playerSprite = Content.Load<Texture2D>(@"Sprites\Character");
            barraTempo = Content.Load<Texture2D>(@"Sprites\TimeBar");
            
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

            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            //telas.Add(Content.Load<Texture2D>(@"telas\Tela_Instrucoes"));
            //telas.Add(Content.Load<Texture2D>(@"telas\Tela_Jogo"));
            //telas.Add(Content.Load<Texture2D>(@"telas\Tela_Vitoria"));
            //telas.Add(Content.Load<Texture2D>(@"telas\Tela_GameOver"));
            telas.TelaAtual = Telas.Tipo.Inicial;
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
            player.Update();

            if (!gameOver)
            {
                if (startGame)
                {
                    #region Player Inputs
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
                    #endregion

                    if (timeRect.Height > 0)
                        timeRect.Height -= 1;
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

                if (startGame && !gameOver)
                {
                    player.Draw(spriteBatch);

                    foreach (Tile t in tileMap)
                    {
                        t.Draw(spriteBatch);
                    }

                    spriteBatch.Draw(barraTempo, timeRect, Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
