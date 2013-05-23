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

        public static bool right, left, up, down;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            timeRect = new Rectangle(700, 20, 25, 450);

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
        }

        protected override void UnloadContent() { }

        private void GetInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                left = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                right = true;
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                up = true;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                down = true;
            }
            if (keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.A))
            {
                left = false;
            }
            if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.D))
            {
                right = false;
            }
            if (keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.W))
            {
                up = false;
            }
            if (keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S))
            {
                down = false;
            }
        }


        protected override void Update(GameTime gameTime)
        {
            GetInput();
            player.Update();

            if(timeRect.Height > 0)
                timeRect.Height -= 1;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            
            spriteBatch.Begin();
            {
                player.Draw(spriteBatch);

                foreach (Tile t in tileMap)
                {
                    t.Draw(spriteBatch);
                }

                spriteBatch.Draw(barraTempo, timeRect, Color.White);
            }
            spriteBatch.End();
        }
    }
}
