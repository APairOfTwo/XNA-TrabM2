using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_TrabM2
{
    class Tile
    {
        public Vector2 position;
        public Vector2 blockPosition;
        int largura, altura;
        public Texture2D sprite;

        public Tile(Vector2 blockPosition)
        {
            this.blockPosition = blockPosition;
            this.position.X = blockPosition.X * 25;
            this.position.Y = blockPosition.Y * 25;
        }

        public void onDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
