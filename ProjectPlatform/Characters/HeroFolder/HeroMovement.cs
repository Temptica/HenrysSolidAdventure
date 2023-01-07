using HenrySolidAdventure.Controller;
using HenrySolidAdventure.MapFolder;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters.HeroFolder
{
    internal static class HeroMovement
    {
        private static Hero Hero => Hero.Instance;
        public static void MoveUpdate(GameTime gameTime, float leftBound, float rightBound, float bottomBound) //main menu
        {
            GetVelocity(gameTime);

            float velocityXDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Hero.Velocity.X);

            var nextPosition = Hero.Position;
            if (velocityXDelta != 0)
            {
                nextPosition.X += velocityXDelta;
            }

            if (Hero.Velocity.Y != 0)
            {
                nextPosition.Y += (float)(Hero.Velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            var nextHitBox = Hero.GetHitBox(nextPosition);

            if (Hero.Velocity.X > 0 && nextHitBox.Right > rightBound)
            {
                nextHitBox.X = (int)(rightBound - nextHitBox.Width);
            }
            else if (Hero.Velocity.X < 0 && nextHitBox.Left < leftBound)
            {
                nextHitBox.X = (int)leftBound;
            }

            if (Hero.Velocity.Y > 0 && nextHitBox.Bottom > bottomBound)
            {
                nextHitBox.Y = (int)(bottomBound - nextHitBox.Height);
                Hero.Velocity.Y = 0;
            }
            else if (Hero.Velocity.Y < 0 && nextHitBox.Top < 0)
            {
                nextHitBox.Y = 0;
            }

            Hero.IsJumping = Hero.Velocity.Y != 0;
            Hero.IsWalking = Hero.Velocity.X != 0;
            Hero.Position = Hero.GetPosition(nextHitBox);
        }

        public static void MoveUpdate(GameTime gameTime, Map map) //during game
        {
            GetVelocity(gameTime);

            float velocityXDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Hero.Velocity.X);
            var nextHitBox =
                Hero.GetHitBox(new Vector2(Hero.Position.X + velocityXDelta, Hero.Position.Y + Hero.Velocity.Y));


            if (Hero.Velocity.Y < 0) //going up
            {
                Hero.IsJumping = true;
                Hero.Velocity.Y += (float)(Hero.Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                nextHitBox = new Rectangle(nextHitBox.X,
                    (int)(nextHitBox.Y + Hero.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds),
                    Hero.HitBox.Width, Hero.HitBox.Height);
                var tile = CollisionHelper.TopHit(nextHitBox, map.FrontMap, Hero.IsFacingLeft);
                if (tile != null)
                {
                    Hero.Velocity.Y = 0f;
                    Hero.IsJumping = false;
                    nextHitBox = new Rectangle(nextHitBox.X, tile.HitBox.Bottom + 3, Hero.HitBox.Width,
                        Hero.HitBox.Height);
                }
            }

            if (Hero.Velocity.X > 0) //to right
            {
                var tile = CollisionHelper.RightHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextHitBox = new Rectangle((int)tile.Position.X - Hero.HitBox.Width - 1, nextHitBox.Y,
                        Hero.HitBox.Width, Hero.HitBox.Height);
                    Hero.Velocity.X = 0;
                }
                else if (CollisionHelper.LeavingRightMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Right))
                {
                    MapLoader.LoadNextMap(Map.Instance.ScreenRectangle.Height);
                    Hero.Position = Map.Instance.Spawn;
                    Hero.Velocity = Vector2.Zero;
                    return;
                }
            }
            else if (Hero.Velocity.X < 0)
            {
                var tile = CollisionHelper.LeftHit(nextHitBox, map.FrontMap);
                if (tile is not null)
                {
                    nextHitBox = new Rectangle(tile.HitBox.Right + 1, nextHitBox.Y, Hero.HitBox.Width,
                        Hero.HitBox.Height);
                    Hero.Velocity.X = 0;
                }
                else if (CollisionHelper.LeavingLeftMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Left))
                {
                    Hero.Velocity.X = 0;
                    nextHitBox = new Rectangle(1, nextHitBox.Y, Hero.HitBox.Width, Hero.HitBox.Height);
                }
            }

            if (Hero.Velocity.Y >= 0) //falling mechanism
            {
                var result = OnGround(nextHitBox);
                if (result != Vector2.Zero)
                {
                    Hero.Velocity.Y = 0f;
                    Hero.IsFalling = false;
                    nextHitBox = new Rectangle((int)result.X, (int)result.Y, nextHitBox.Width, nextHitBox.Height);
                }
                else
                {
                    if (CollisionHelper.LeavingBottomMapBorder(nextHitBox, Map.Instance.ScreenRectangle.Height))
                    {
                        Hero.Health -= Hero.BaseHp / 4;
                        if (Hero.Health <= 0)
                        {
                            Game1.SetState(GameState.GameOver);
                            return;
                        }
                        Hero.Position = Map.Instance.Spawn;
                        
                        Hero.IsFalling = false;
                        return;
                    }

                    Hero.IsFalling = true;
                    var newY = Hero.Velocity.Y + (float)(Hero.Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                    Hero.Velocity.Y = newY > Hero.MaxYVelocity ? Hero.MaxYVelocity : newY;

                    nextHitBox = new Rectangle(nextHitBox.X,
                        (int)(nextHitBox.Y + Hero.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalMilliseconds),
                        Hero.HitBox.Width, Hero.HitBox.Height);
                }
            }


            Hero.IsJumping = Hero.Velocity.Y != 0;
            Hero.IsWalking = Hero.Velocity.X != 0;
            Hero.Position = Hero.GetPosition(nextHitBox);
        }
        public static void GetVelocity(GameTime gameTime)
        {
            if (!Hero.CanWalk)
            {
                Hero.Velocity.X = 0;
            }
            else if (Hero.State is State.Rolling)
            {
                if (Hero.IsFacingLeft)
                {
                    Hero.Velocity.X -= (float)(Hero.XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (Hero.Velocity.X < -Hero.RollingSpeed) Hero.Velocity.X = -Hero.RollingSpeed;
                }
                else
                {
                    Hero.Velocity.X += (float)(Hero.XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (Hero.Velocity.X > Hero.RollingSpeed) Hero.Velocity.X = Hero.RollingSpeed;
                }


            }
            else if (InputController.LeftInput) //left
            {
                Hero.Velocity.X -= (float)(Hero.XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (Hero.Velocity.X < -Hero.WalkSpeed) Hero.Velocity.X = -Hero.WalkSpeed;
                Hero.IsRolling = false;
                Hero.IsFacingLeft = true;
                Hero.IsWalking = true;
            }
            else if (InputController.RightInput) //right
            {
                Hero.Velocity.X += (float)(Hero.XAcceleration * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (Hero.Velocity.X > Hero.WalkSpeed) Hero.Velocity.X = Hero.WalkSpeed;
                Hero.IsRolling = false;
                Hero.IsWalking = true;
                Hero.IsFacingLeft = false;
            }
            else
            {
                Hero.Velocity.X = 0;
                Hero.IsWalking = false;
                Hero.IsRolling = false;
            }

            if (InputController.JumpInput && Hero.CanJump) //jump
            {
                Hero.Velocity.Y = -Hero.JumpForce;
                Hero.CanJump = false;
            }
            else if (!Hero.CanJump)
            {
                var newY = Hero.Velocity.Y + (float)(Hero.Gravity * gameTime.ElapsedGameTime.TotalMilliseconds);
                Hero.Velocity.Y = newY > Hero.MaxYVelocity ? Hero.MaxYVelocity : newY;
            }
        }
        private static Vector2 OnGround(Rectangle hitbox)
        {
            var groundBox = new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height + 5);
            //checks if any tile in map.currentMap has collision with the bottom of the otter if so set otter to the tile height            
            var tile = CollisionHelper.GroundHit(groundBox, Map.Instance.FrontMap, Hero.IsFacingLeft);
            if (tile == -1) return Vector2.Zero;
            //set the otter position so the otter is on the tile
            return new Vector2(hitbox.X, tile - hitbox.Height);
        }
    }
}
