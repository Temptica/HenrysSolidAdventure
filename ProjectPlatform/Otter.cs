using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform
{
    enum state { Idle, Walking, Running, Jumping, Attacking, Sleeping, Dead }
    internal class Otter : IMovable
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Rectangle { get; set; }
        public int MoveSpeed { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public state State { get; set; }
        int fps = 4;
        int height = 200;
        int width = 200;
        int currentx;
        double time = 0d;

        public Otter(Texture2D otter, Vector2 position)
        {
            Texture = otter;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            if (State == state.Idle)
            {
                //do idle stuff
                //make idle animation
                time += gameTime.ElapsedGameTime.TotalMilliseconds; 

                if (time > 1000/fps)
                {
                    currentx += width;
                    if (currentx >= Texture.Width)
                        currentx = 0;
                    time = 0;
                }

            }
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,new Rectangle(currentx,0,height,width),Color.White,0,Vector2.Zero,0.5f,SpriteEffects.None,0f);
        }
        
        
    }
}
