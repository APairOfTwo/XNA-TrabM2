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
        
        char[][] map2;
        char[,] map;

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

            map2 = File.ReadAllLines(@"Content/TextFiles/mapa.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();
            map = JaggedToMultidimensional(map2);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'P')
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

        private char[,] JaggedToMultidimensional(char[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray.Max(subArray => subArray.Length);
            char[,] array = new char[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }
            return array;
        }
    }
}
