using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics; // BasicEffect, Vertex*, GraphicsDevice
using Microsoft.Xna.Framework;  // Matrix
using Microsoft.Xna.Framework.Content;  //  Content Manager

namespace XNA_TrabM2
{
    class Plano
    {
        private GraphicsDevice device;

        // Vertex buffer
        private VertexBuffer vertexBuffer;

        // Texture used to draw the plane
        private Texture2D _texture;

        private BasicEffect effect;

        public Matrix worldMatrix = Matrix.Identity;

        public Plano(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
        }

        public void UnloadContent()
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }
        }

        public void LoadContent(Texture2D texture)
        {
            //  Load the texture
            _texture = texture;

            //  Create the Plane
            CreatePlane();

            //  Create the effect that will be used to draw the plane
            effect = new BasicEffect(device);

            //  Enable and inform texture
            effect.TextureEnabled = true;
            effect.Texture = _texture;
 
            effect.LightingEnabled = false;
            //effect.EnableDefaultLighting();
        }

        private void CreatePlane()
        {
            //  size of 3D Axis 
            float PlaneLenght = 10f;
            float repetition = 6f;
            //  Number of vertices we´ll use
            int vertexCount = 4;

            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[vertexCount];
            // X axis
            vertices[0] = new VertexPositionNormalTexture(new Vector3(-PlaneLenght, 0.0f, -PlaneLenght), Vector3.Up, new Vector2(0f, repetition));
            vertices[1] = new VertexPositionNormalTexture(new Vector3(PlaneLenght, 0.0f, -PlaneLenght), Vector3.Up, new Vector2(repetition, repetition));
            vertices[2] = new VertexPositionNormalTexture(new Vector3(-PlaneLenght, 0.0f, PlaneLenght), Vector3.Up, new Vector2(0f, 0f));
            vertices[3] = new VertexPositionNormalTexture(new Vector3(PlaneLenght, 0.0f, PlaneLenght), Vector3.Up, new Vector2(repetition, 0f));
            //  fill the vertex buffer with the vertices
            vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration,
                                                vertexCount, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            effect.World = worldMatrix;
            effect.View = view;
            effect.Projection = projection;
            device.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
            {
                CurrentPass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

        }
    }
}
