using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Mapfolder;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.Characters
{
    internal class Boss : Enemy
    {//https://luizmelo.itch.io/evil-wizard-2
        public static Texture2D Texture { get; set; }
        private const float Gravity = 0.0001f;
        public Boss(Vector2 position)
        {
            Position = position;
            var width = Texture.Width / 8;
            var height = Texture.Height / 8;
            Animations = new AnimationList<Animation>()
            {
                new(Texture, State.Attacking, 16, width, height, 0, 0, 8),
                new(Texture, State.Dead, 7, width, height, height * 2, 0, 5),
                new(Texture, State.Idle, 8, width, height, height * 4, 0, 8),
                new(Texture, State.Walking, 8, width, height, height * 6, 0, 10),
                new(Texture, State.Hit, 3, width, height, Texture.Height - height, 0, 4)
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //put boss on the ground with Hitbox
            var result = OnGround(HitBox);
            //look at otter
            IsFacingLeft = Otter.Instance.HitBox.Center.X < HitBox.Center.X;
                




            Move(gameTime);
        }

        public override void Draw(Sprites spriteBatch)
        {
            
            base.Draw(spriteBatch);
           
            //todo
        }
        public Vector2 OnGround(Rectangle hitbox)
        {
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = CollisionHelper.OtterGroundHit(groundBox, Map.Instance.FrontMap);
            if (tile == -1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile - hitbox.Height - CurrentAnimation.CurrentFrame.HitBox.Y);
        }
        public void Move(GameTime gameTime)
        {
            var nextHitBox = HitBox;
            var NextPosition = Position;
            //gravity
            var result = OnGround(nextHitBox);
            result = result;
            if (result != Vector2.Zero)
            {
                Velocity.Y = 0f;
                var dif = result.Y - nextHitBox.Y;
                nextHitBox = new Rectangle((int)result.X, (int)result.Y, nextHitBox.Width, nextHitBox.Height);
                NextPosition = new Vector2(NextPosition.X, NextPosition.Y+dif);
            }
            else
            {
                var newY = Velocity.Y + (float)(Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                Velocity = new Vector2(Velocity.X, newY);
                var dif = Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                NextPosition = new Vector2(NextPosition.X, NextPosition.Y + dif);
            }

            //Position = NextPosition;

        }

        

        public override bool CheckDamage()
        {
            return true;
        }
    }
}
