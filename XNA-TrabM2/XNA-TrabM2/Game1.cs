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
        SpriteFont verdana;

        Texture2D timeBar;

        Player player;
        Cube cube;
        Plano plano;

        ChaseCamera camera;
        Boolean cameraSpringEnabled = true;

        static List<Tile> tileMap;
        static List<Cube> cubeMap;
        static List<Booster> timeBoosters;

        char[][] map;
        char[][] map2;
        char[][] map3;

        int mapCont = 1;

        FinalBlock finalBlock;

        Rectangle timeRect;

        Telas telas;
        Botoes botoes;

        long totalTime = 1000;
        long timePassed = 0;
        int totalSecondsLeft = 10;
        int score = 0;

        bool startGame = false;
        bool gameOver = false;
        bool gameWin = false;
        bool playOnce = false;

        SoundEffect menuMusic;
        SoundEffectInstance menuMusicInstance;
        SoundEffect gameMusic;
        SoundEffectInstance gameMusicInstance;
        SoundEffect sfxGameWin;
        SoundEffectInstance sfxGameWinInstance;
        SoundEffect sfxGameOver;
        SoundEffectInstance sfxGameOverInstance;
        SoundEffect sfxPickup;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            player = new Player();
            cube = new Cube(new Vector3(0, .5f, -3));
            plano = new Plano(GraphicsDevice);

            // Cria a "chasing camera", que segue o cubo
            InicializaCamera(graphics.GraphicsDevice);

            telas = new Telas();
            timeRect = new Rectangle(700, 40, 25, 100);
            botoes = new Botoes();

            base.Initialize();
        }

        public void InicializaCamera(GraphicsDevice device)
        {
            camera = new ChaseCamera();
            // Set the camera offsets
            camera.DesiredPositionOffset = new Vector3(0.0f, 2.0f, 3.0f);
            camera.LookAtOffset = new Vector3(0.0f, 1.0f, 0.0f);
            // Set the camera aspect ratio
            camera.AspectRatio = (float)device.Viewport.Width / device.Viewport.Height;

            // Perform an inital reset on the camera so that it starts at the resting
            // position. If we don't do this, the camera will start at the origin and
            // race across the world to get behind the chased object.
            // This is performed here because the aspect ratio is needed by Reset.
            UpdateCamera(new GameTime(), player.position, player.direction);
            camera.Reset();
        }

        protected override void LoadContent()
        {
            player.LoadContent(Content);
            cube.LoadContent(Content);

            plano.LoadContent(Content.Load<Texture2D>(@"Texturas\PlaneTexture"));

            menuMusic = Content.Load<SoundEffect>(@"Audio\MenuMusic");
            menuMusicInstance = menuMusic.CreateInstance();
            gameMusic = Content.Load<SoundEffect>(@"Audio\GameMusic");
            gameMusicInstance = gameMusic.CreateInstance();
            sfxGameWin = Content.Load<SoundEffect>(@"Audio\GameWin");
            sfxGameWinInstance = sfxGameWin.CreateInstance();
            sfxGameOver = Content.Load<SoundEffect>(@"Audio\GameOver");
            sfxGameOverInstance = sfxGameOver.CreateInstance();
            sfxPickup = Content.Load<SoundEffect>(@"Audio\Pickup");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            verdana = Content.Load<SpriteFont>(@"Fonts\Verdana");
            timeBar = Content.Load<Texture2D>(@"Sprites\TimeBar");
            
            tileMap = new List<Tile>();
            cubeMap = new List<Cube>();
            timeBoosters = new List<Booster>();

            map = File.ReadAllLines(@"Content/TextFiles/mapa.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();
            map2 = File.ReadAllLines(@"Content/TextFiles/mapa2.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();
            map3 = File.ReadAllLines(@"Content/TextFiles/mapa3.txt").Select(l => l.Split(',').Select(i => char.Parse(i)).ToArray()).ToArray();

            loadMap(map);

            botoes.Add(new clsButton(this, Content.Load<Texture2D>(@"botoes\yellow_button_start"), Content.Load<Texture2D>(@"botoes\blue_button_start"), new Vector2(330, 200), new EventHandler(BotaoAzul_Click)));
            botoes.Add(new clsButton(this, Content.Load<Texture2D>(@"botoes\yellow_button_instructions"), Content.Load<Texture2D>(@"botoes\blue_button_instructions"), new Vector2(330, 280), new EventHandler(BotaoLaranja_Click)));
            botoes.Add(new clsButton(this, Content.Load<Texture2D>(@"botoes\yellow_button_exit"), Content.Load<Texture2D>(@"botoes\blue_button_exit"), new Vector2(330, 360), new EventHandler(BotaoVermelho_Click)));

            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Inicial"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Instrucoes"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Jogo"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_Vitoria"));
            telas.Add(Content.Load<Texture2D>(@"telas\Tela_GameOver"));
            telas.TelaAtual = Telas.Tipo.Inicial;

            playMenuMusic();
        }

        protected override void UnloadContent()
        {
            plano.UnloadContent();
        }

        protected void GetInputs(GameTime gameTime)
        {
            Input.Update(gameTime);

            if (Input.KeyboardEscapeJustPressed)
            {
                telas.TelaAtual = Telas.Tipo.Inicial;
                startGame = false;
                gameOver = false;
                gameWin = false;
                playOnce = false;
                clearStage();
                mapCont = 1;
                playMenuMusic();
                loadMap(map);
                gameMusicInstance.Pause();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            botoes.Update();

            GetInputs(gameTime);

            if (!gameOver)
            {
                if (startGame)
                {
                    //  atualiza a posi��o do cubo
                    player.Update(gameTime);
                    cube.Update(gameTime);

                    foreach (Cube c in cubeMap)
                    {
                        c.Update(gameTime);

                        //  Se o cubo est� se movendo e bateu na parede
                        if (player.CheckForCollisions(c) && player.speed != 0f)
                        {
                            player.position = player.oldPosition;
                        }
                    }

                    foreach (Booster b in timeBoosters)
                    {
                        b.Update(gameTime);
                        if (b.active && player.CheckForCollisions(b))
                        {
                            sfxPickup.Play();
                            b.active = false;
                            timeRect.Height += 30;
                            totalSecondsLeft += 3;
                        }
                    }

                    finalBlock.Update(gameTime);
                    if (player.CheckForCollisions(finalBlock))
                    {
                        clearStage();

                        if (mapCont == 1)
                        {
                            loadMap(map2);
                            mapCont = 2;
                            return;
                        }
                        if (mapCont == 2)
                        {
                            loadMap(map3);
                            mapCont = 3;
                            return;
                        }
                        if (mapCont == 3)
                        {
                            gameWin = true;
                            gameOver = true;
                        }
                    }

                    // Atualiza a c�mera para "perseguir" seu alvo
                    UpdateCamera(gameTime, player.position, player.direction);

                    timePassed += gameTime.ElapsedGameTime.Milliseconds;
                    gameMusicInstance.Pitch = -1 * ((float)totalSecondsLeft / 10) + 1;

                    if (timePassed >= totalTime)
                    {
                        timePassed = 0;
                        totalSecondsLeft--;

                        if (timeRect.Height > 0)
                            timeRect.Height -= 10;
                        else
                        {
                            gameOver = true;
                            gameMusicInstance.Pause();
                        }
                    }
                }
            }
            else
            {
                if (gameWin)
                {
                    if (!playOnce)
                    {
                        playOnce = true;
                        telas.TelaAtual = Telas.Tipo.Vitoria;
                        gameMusicInstance.Pause();
                        sfxGameWinInstance.Play();
                    }
                }
                else
                {
                    if (!playOnce)
                    {
                        playOnce = true;
                        telas.TelaAtual = Telas.Tipo.GameOver;
                        sfxGameOverInstance.Play();
                    }
                }
            }

            base.Update(gameTime);
        }

        private void UpdateCamera(GameTime gameTime, Vector3 posicao, Vector3 direcao)
        {
            // The chase camera's update behavior is the springs, but we can
            // use the Reset method to have a locked, spring-less camera
            if (cameraSpringEnabled)
                camera.Update(gameTime);
            else
                camera.Reset();

            //  Update the chase target
            camera.ChasePosition = posicao;
            camera.ChaseDirection = direcao;

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.S))
                cameraSpringEnabled = !cameraSpringEnabled;

            if (keyboard.IsKeyDown(Keys.PageUp))
                camera.DesiredPositionOffset = new Vector3(0.0f, camera.DesiredPositionOffset.Y + 0.5f, camera.DesiredPositionOffset.Z + 0.5f);

            if (keyboard.IsKeyDown(Keys.PageDown))
                camera.DesiredPositionOffset = new Vector3(0.0f, camera.DesiredPositionOffset.Y - 0.5f, camera.DesiredPositionOffset.Z - 0.5f);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            
            spriteBatch.Begin();
            {
                if (!startGame || gameOver)
                    telas.Draw(spriteBatch);

                if (telas.TelaAtual == Telas.Tipo.Inicial)
                    botoes.Draw(spriteBatch);

                if (startGame && !gameOver)
                {
                    spriteBatch.DrawString(verdana, "Time: "+totalSecondsLeft+"s", new Vector2(680, 5), Color.Red);

                    GraphicsDevice.BlendState = BlendState.Opaque;
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    plano.Draw(camera.View, camera.Projection);

                    foreach (Cube c in cubeMap)
                    {
                        c.Draw(camera.View, camera.Projection);
                    }

                    cube.Draw(camera.View, camera.Projection);

                    foreach (Booster b in timeBoosters)
                    {
                        if(b.active)
                            b.Draw(camera.View, camera.Projection);
                    }

                    finalBlock.Draw(camera.View, camera.Projection);
                    
                    player.Draw(camera.View, camera.Projection);

                    spriteBatch.Draw(timeBar, timeRect, Color.White);
                }

                if (startGame && gameWin)
                {
                    spriteBatch.DrawString(verdana, "Score: " + score, new Vector2(350, 230), Color.White);
                }
            }
            spriteBatch.End();
        }

        #region Play Audio
        public void playMenuMusic()
        {
            if (menuMusicInstance.State == SoundState.Stopped)
            {
                menuMusicInstance.Volume = 0.75f;
                menuMusicInstance.IsLooped = true;
                menuMusicInstance.Play();
            }
            else
                menuMusicInstance.Resume();
        }

        public void playGameMusic()
        {
            if (gameMusicInstance.State == SoundState.Stopped)
            {
                gameMusicInstance.Volume = 0.75f;
                gameMusicInstance.IsLooped = true;
                gameMusicInstance.Play();
            }
            else
                gameMusicInstance.Resume();
        }
        #endregion

        #region Eventos dos Bot�es
        public void BotaoAzul_Click(object sender, EventArgs e)
        {
            menuMusicInstance.Pause();
            telas.TelaAtual = Telas.Tipo.Jogo;
            startGame = true;
            playGameMusic();
        }

        public void BotaoLaranja_Click(object sender, EventArgs e)
        {
            telas.TelaAtual = Telas.Tipo.Instrucoes;
        }

        public void BotaoVermelho_Click(object sender, EventArgs e)
        {
            this.Exit();
        }
        #endregion

        public void clearStage()
        {
            score += totalSecondsLeft;
            tileMap.Clear();
            cubeMap.Clear();
            timeBoosters.Clear();
            timePassed = 0;
            totalSecondsLeft = 10;
            timeRect.Height = 100;
            gameMusicInstance.Pitch = 0;
        }
        
        public void loadMap(char[][] map)
        {
            int rows = map.Length;
            int cols = map.Max(subArray => subArray.Length);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (map[i][j] == 'P')
                    {
                        Cube c = new Cube(new Vector3(j, 0.5f, i));
                        c.LoadContent(Content);
                        cubeMap.Add(c);
                    }
                    if (map[i][j] == 'I')
                    {
                        player.position = new Vector3(j, 0.5f, i);
                    }
                    if (map[i][j] == 'T')
                    {
                        Booster b = new Booster(new Vector3(j, 0.5f, i));
                        b.LoadContent(Content);
                        timeBoosters.Add(b);
                    }
                    if (map[i][j] == 'F')
                    {
                        finalBlock = new FinalBlock(new Vector3(j, 0.5f, i));
                        finalBlock.LoadContent(Content);
                    }
                }
            }
        }
    }
}

