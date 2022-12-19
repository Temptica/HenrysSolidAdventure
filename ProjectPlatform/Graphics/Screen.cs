using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform.Graphics
{
    public sealed class Screen : IDisposable
    {//from https://www.youtube.com/watch?v=yUSB_wAVtE8 and own previous project

        private bool isDisposed;
        private Game game;
        private RenderTarget2D target;
        private bool isSet;

        public int Width
        {
            get { return this.target.Width; }
        }

        public int Height
        {
            get { return this.target.Height; }
        }

        public Screen(Game game, int width, int height)
        {

            this.game = game ?? throw new ArgumentNullException("game");

            this.target = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            this.isSet = false;
        }

        public void Dispose()
        {
            if(this.isDisposed)
            {
                return;
            }

            this.target?.Dispose();
            this.isDisposed = true;
        }

        public void Set()
        {
            if (this.isSet)
            {
                throw new Exception("Render target is already set.");
            }

            this.game.GraphicsDevice.SetRenderTarget(this.target);
            this.isSet = true;
        }

        public void UnSet()
        {
            if (!this.isSet)
            {
                throw new Exception("Render target is not set.");
            }

            this.game.GraphicsDevice.SetRenderTarget(null);
            this.isSet = false;
        }

        public void Present(Sprites sprites, bool textureFiltering = true)
        {
            if(sprites is null)
            {
                throw new ArgumentNullException("sprites");
            }
            
            this.game.GraphicsDevice.Clear(Color.Black);

            Rectangle destinationRectangle = this.CalculateDestinationRectangle();

            sprites.Begin(null,textureFiltering);
            sprites.Draw(this.target, null, destinationRectangle, Color.White);
            sprites.End();
        }
        
        internal Rectangle CalculateDestinationRectangle()
        {
            Rectangle backbufferBounds = this.game.GraphicsDevice.PresentationParameters.Bounds;
            int backbufferAspectRatio = backbufferBounds.Width / backbufferBounds.Height;
            int screenAspectRatio = this.Width / this.Height;

            float rx = 0f;
            float ry = 0f;
            float rw = backbufferBounds.Width;
            float rh = backbufferBounds.Height;

            if(backbufferAspectRatio > screenAspectRatio)
            {
                rw = rh * screenAspectRatio;
                rx = (backbufferBounds.Width - rw) / 2f;
            }
            else if(backbufferAspectRatio < screenAspectRatio)
            {
                rh = rw / screenAspectRatio;
                ry = (backbufferBounds.Height - rh) / 2f;
            }
            Rectangle result = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
            return result;
        }

        
    }
}
