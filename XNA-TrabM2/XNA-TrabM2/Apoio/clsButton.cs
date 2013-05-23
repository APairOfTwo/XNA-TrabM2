using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;  //  for Vector2

namespace XNA_TrabM2
{
    class clsButton 
    {
        private Texture2D texturaNaoSelecionado; //  botão em não selecionado
        private Texture2D texturaSelecionado;    //  botão em estado selecionado
        private Vector2 posicao;                 //  posição na tela do botão
        private EventHandler eventoClick;        //  Evento a ser chamado automaticamente quando botão for selecionado

        public enum TipoStatus 
        {
            NaoSelecionado = 0,
            Selecionado = 1
        }
        public TipoStatus Status = TipoStatus.NaoSelecionado;

        public Texture2D Textura
        {
            get
            {
                if (Status == TipoStatus.Selecionado)
                    return texturaSelecionado;
                else
                    return texturaNaoSelecionado;
            }
        }

        public Vector2 Posicao
        {
            get
            {
                return posicao;
            }
        }

        public Rectangle retangulo
        {
            get
            {
                return new Rectangle((int)posicao.X, (int)posicao.Y, Textura.Width, Textura.Height);
            }
        }

        public clsButton(Game game, Texture2D TexturaNaoSelecionado, Texture2D TexturaSelecionado, Vector2 Posicao, EventHandler evento)
        {
            texturaNaoSelecionado = TexturaNaoSelecionado;
            texturaSelecionado = TexturaSelecionado;
            posicao = Posicao;
            eventoClick = evento;
        }

        public void Unload()
        {
            texturaNaoSelecionado.Dispose();
            texturaSelecionado.Dispose();
        }

        public void Draw(SpriteBatch Renderizador2D)
        {
            Renderizador2D.Draw(Textura, posicao, Color.White);
        }

        public void Executa()
        {
            //  chama o evento do botão
            eventoClick(null, null);
        }

    }
}



