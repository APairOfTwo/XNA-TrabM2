﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_TrabM2
{   
    class Tile
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 blockPosition;
        public Vector2 size;

        public Tile(Texture2D texture, Vector2 blockPosition)
        {
            this.texture = texture;
            this.position = blockPosition * 25;
            this.blockPosition = blockPosition;
            this.size.X = texture.Width;
            this.size.Y = texture.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
