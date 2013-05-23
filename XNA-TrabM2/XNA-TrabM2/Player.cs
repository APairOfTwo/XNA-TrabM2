using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNA_TrabM2
{
    class Player
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 blockPosition;
        public Vector2 size;

        public Player(Texture2D texture, Vector2 blockPosition)
        {
            this.texture = texture;
            this.position = blockPosition * 25;
            this.blockPosition = blockPosition;
            this.size.X = texture.Width;
            this.size.Y = texture.Height;
        }

        public void Update()
        {
            if (Game1.left)
            {
                blockPosition.X -= 1;
            }
            if (Game1.right)
            {
                blockPosition.X += 1;
            }
            if (Game1.up)
            {
                blockPosition.Y -= 1;
            }
            if (Game1.down)
            {
                blockPosition.Y += 1;
            }

            position = blockPosition * 25;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}