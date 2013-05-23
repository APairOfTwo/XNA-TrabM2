using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;  //  for Vector2

namespace XNA_TrabM2
{
    
    class Tela
    {
        private Texture2D fundo;         //  Textura da tela

        public Tela(Texture2D Textura)
        {
            fundo = Textura;
        }

        public void Draw(SpriteBatch Renderizador2D)
        {
            Renderizador2D.Draw(fundo, Vector2.Zero, Color.White);
        }

        public void Unload()
        {
            fundo.Dispose();
        }
    }
}



