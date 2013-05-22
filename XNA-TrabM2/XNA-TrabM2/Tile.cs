using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_TrabM2
{
    public enum TileCollision
    {
        Passable, Impassable
    };
    
    class Tile
    {
        public Texture2D texture;
        public TileCollision collision;
        public Vector2 position;
        public Vector2 blockPosition;
        public Vector2 size;

        public Tile(Texture2D texture, Vector2 blockPosition, TileCollision collision)
        {
            this.texture = texture;
            this.collision = collision;
            this.position = blockPosition * 25;
            this.blockPosition = blockPosition;
            this.size.X = texture.Width;
            this.size.Y = texture.Height;
            //this.position.X = blockPosition.X * 25;
            //this.position.Y = blockPosition.Y * 25;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
