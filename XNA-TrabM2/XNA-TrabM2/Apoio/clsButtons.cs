using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics; //  Spritebatch
using Microsoft.Xna.Framework;  //  for Game
using Microsoft.Xna.Framework.Input;  //  for Mouse, keyboard and gamepad

namespace XNA_TrabM2
{
    class Botoes : List<clsButton>
    {
        public int BotaoAtivo = 0;


        public void Draw(SpriteBatch Renderizador2D)
        {
            foreach (clsButton Button in (List<clsButton>)this)
            {
                Renderizador2D.Draw(Button.Textura, Button.Posicao, Color.White);
            }
           
        }

        public void Update() {
            //   ---------------------------------------------------------
            //   --- Movimenta o foco entre os bot�es
            //   ---------------------------------------------------------
            int incremento = 0;

            // ---  GAMEPAD --------------------------------------------------
            //  M�todo sem uso da classe helper - N�O FUNCIONA ADEQUADAMENTE, � para mostrar como exemplo
            //  Muda o bot�o selecionado com os bot�es de "shoulder"
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            if (gamePad.Buttons.LeftShoulder > 0)   
                incremento = -1;

            if (gamePad.Buttons.RightShoulder > 0)
                incremento = +1;

            //  M�todo com uso da classe helper - DESCOMENTAR ESSAS LINHAS e comentar o anterior para funcionar
            //if(Input.GamePadLeftShoulderJustPressed)
            //    incremento = -1;
            //if (Input.GamePadRightShoulderJustPressed)
            //    incremento = +1;


            // ---  TECLADO --------------------------------------------------
            //  M�todo sem uso da classe helper - N�O FUNCIONA ADEQUADAMENTE, � para mostrar como exemplo
            //  muda a posi��o usando o teclado
            KeyboardState keyboardState = Keyboard.GetState();
            //if (keyboardState.IsKeyDown(Keys.Up))
            //    incremento = -1;
            //if (keyboardState.IsKeyDown(Keys.Down))
            //    incremento = +1;
            //if (keyboardState.IsKeyDown(Keys.Left))
            //    incremento = -1;
            //if (keyboardState.IsKeyDown(Keys.Right))
            //    incremento = +1;

            //M�todo com uso da classe helper - DESCOMENTAR ESSAS LINHAS e comentar o anterior para funcionar
            if (Input.KeyboardUpJustPressed)
                incremento = -1;
            if (Input.KeyboardDownJustPressed)
                incremento = +1;
            if (Input.KeyboardLeftJustPressed)
                incremento = -1;
            if (Input.KeyboardRightJustPressed)
                incremento = +1;

            // ---  MOUSE --------------------------------------------------
            //  M�todo com uso da classe helper - DESCOMENTAR ESSAS LINHAS para funcionar
            //  muda a posi��o usando o mouse 
            //for (int i = 0; i < this.Count; i++)
            //{
            //    if (Input.MouseInBox(this[i].retangulo))
            //        BotaoAtivo = i;
            //}

            //  Atualiza o n�mero do bot�o ativo
            BotaoAtivo += incremento;
            if (BotaoAtivo < 0)
                BotaoAtivo = this.Count - 1;

            if (BotaoAtivo >= this.Count)
                BotaoAtivo = 0;

            //  Seleciona o bot�o ativo, desseleciona todos os outros
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Status = clsButton.TipoStatus.NaoSelecionado;
            }
            this[BotaoAtivo].Status = clsButton.TipoStatus.Selecionado;

            //   ---------------------------------------------------------
            //   --- Verifica se o bot�o foi "clicado"  (ENTER no teclado, A no gamePad ou clique no mouse)
            //   ---------------------------------------------------------
            if (keyboardState.IsKeyDown(Keys.Enter))
                    this[BotaoAtivo].Executa();

            if (gamePad.Buttons.A == ButtonState.Pressed)
                    this[BotaoAtivo].Executa();

            // ---  MOUSE --------------------------------------------------
            //  M�todo com uso da classe helper - DESCOMENTAR ESSAS LINHAS para funcionar
            //if (Input.MouseLeftButtonJustPressed)
            //{
            //    if (Input.MouseInBox(this[BotaoAtivo].retangulo))
            //        this[BotaoAtivo].Executa();
            //}

        }

        public void Unload()
        {
            foreach (clsButton Button in this)
            {
                Button.Unload();
            }
        }
    }
}
