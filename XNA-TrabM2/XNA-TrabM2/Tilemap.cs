using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_TrabM2
{
    class Tilemap
    {
        List<Tile> tiles = new List<Tile>();

        public void loadMap(String fileName)
        {
            String line;
            int lineNumber = 0;

            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        char c = line[i];
                        if (c.Equals('P') || c.Equals('p'))
                        {
                            tiles.Add(new Tile(new Vector2(i, lineNumber)));
                        }
                    }

                    lineNumber++;
                }
            }
        }

        public void onDraw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                tile.onDraw(spriteBatch);
            }
        }
    }
}
