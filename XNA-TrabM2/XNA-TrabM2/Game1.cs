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
        List<Tile> tileMap;
        char[][] map;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileBlock = Content.Load<Texture2D>(@"Tiles\Block");
            
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
                }
            }
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            
            spriteBatch.Begin();
            {
                foreach (Tile t in tileMap)
                {
                    t.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}
