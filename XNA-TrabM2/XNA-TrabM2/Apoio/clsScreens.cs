using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics; //  Spritebatch
using Microsoft.Xna.Framework.Content;  //  content


namespace XNA_TrabM2
{
    class Telas : List<Tela>
    {
        public enum Tipo
        {
            Inicial = 0,
            Instrucoes = 1,
            Jogo = 2,
            Vitoria = 3,
            GameOver = 4
        }
        public Tipo TelaAtual = Tipo.Inicial;

        public void Draw(SpriteBatch Renderizador2D)
        {
            this[Convert.ToInt32(TelaAtual)].Draw(Renderizador2D);
        }

        public void Add(Texture2D Tela)
        {
            //Call the 'real' add method 
            this.Add(new Tela(Tela));
        }

        public void Unload()
        {
            foreach(Tela tela in this)
                tela.Unload();
        }
    }
}
