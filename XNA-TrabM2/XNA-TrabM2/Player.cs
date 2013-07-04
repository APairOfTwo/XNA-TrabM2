using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace XNA_TrabM2
{
    class Player
    {
        Model model;
        public Matrix _world ;
        public Vector3 oldPosition = Vector3.Zero;
        private Vector3 _position = Vector3.Zero;
        private Vector3 _direction = Vector3.Forward;
        private float _speed = 0;

        public Matrix world
        {
            get { return _world; }
            //   set { _world = value; }
        }

        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 direction
        {
            get { return _direction; }
        }

        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public void SetRotationY(float radians)
        {
            _world *= Matrix.CreateTranslation(-_position);
            _world *= Matrix.CreateRotationY(radians);
            _world *= Matrix.CreateTranslation(_position);
            // Rotate orientation vector
            _direction = Vector3.Transform(_direction, Matrix.CreateRotationY(radians));
            _direction.Normalize();
        }

        public Player()
        {
            //  Posiciona o cubo acima do plano
            _world *= Matrix.CreateTranslation(position);
            _position = new Vector3(0f, .5f, 0f);
            oldPosition = _position;
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>(@"Modelos\Cube");
        }

        public void Update(GameTime time)
        {
            oldPosition = _position;

            //---  Move o cubo
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Left))
                SetRotationY(0.04f);

            if (currentKeyboardState.IsKeyDown(Keys.Right))
                SetRotationY(-0.04f);

            if (currentKeyboardState.IsKeyDown(Keys.Up))
                speed = 0.05f;

            if (currentKeyboardState.IsKeyDown(Keys.Down))
                speed = -0.05f;

            if (currentKeyboardState.IsKeyUp(Keys.Up) && currentKeyboardState.IsKeyUp(Keys.Down))
                speed = 0;

            // Re-calcula a direção "direita", que com as aproximações vai perdendo a precisão
            Vector3 right = Vector3.Cross(_direction, Vector3.Up);

            _position += _direction * _speed;

            // Reconstrói a matriz de mundo (world)
            _world = Matrix.Identity;
            _world.Forward = _direction;
            _world.Up = Vector3.Up;
            _world.Right = right;
            _world.Translation = _position;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world; 
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        public bool CheckForCollisions(Cube modelo)
        {
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                BoundingSphere boundingSphere;
                boundingSphere.Radius = 0.5f;
                boundingSphere.Center = _position;
                for (int j = 0; j < modelo.model.Meshes.Count; j++)
                {
                    BoundingSphere otherBoundingSphere = modelo.model.Meshes[j].BoundingSphere;
                    otherBoundingSphere.Radius = 0.5f;
                    otherBoundingSphere.Center = modelo.position;
                    if (boundingSphere.Intersects(otherBoundingSphere))
                        return true;
                }
            }
            return false;
        }

        // TODO Modo antigo 2D

        //public Texture2D texture;
        //public Vector2 position;
        //public Vector2 blockPosition;
        //public Vector2 oldBlockPosition;
        //public Vector2 size;

        //public Player(Texture2D texture, Vector2 blockPosition)
        //{
        //    this.texture = texture;
        //    this.position = blockPosition * 25;
        //    this.blockPosition = blockPosition;
        //    this.size.X = texture.Width;
        //    this.size.Y = texture.Height;
        //}

        //public void Update()
        //{
        //    position = blockPosition * 25;
        //}

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(texture, position, Color.White);
        //}
    }
}