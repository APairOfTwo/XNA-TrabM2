﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XNA_TrabM2
{
    class Cube
    {
        Model model;
        public Matrix world;
        private Vector3 position = Vector3.Zero;

        public Cube(Vector3 position)
        {
            this.position = position;
            world *= Matrix.CreateTranslation(position);
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>(@"Modelos\Cube");
        }

        public void Update(GameTime time)
        {
            world = Matrix.Identity;
            world.Translation = position;
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
    }
}