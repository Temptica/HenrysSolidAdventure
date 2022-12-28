using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Mapfolder;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.EnemyFolder
{
    internal abstract class RoamingEnemy:Enemy
    {
        private float _maxLeftPosition;
        private float _maxRightPosition;

        protected RoamingEnemy()
        {
            
        }
        
        public abstract bool CheckAttack();

        internal void DefineWalkablePath()
        {
            //get the tile the enemy is on
            //find how far left you can go without having a gap or a wall. Then same for right
            Rectangle hitbox = new Rectangle(0, HitBox.Y, Map.Instance.ScreenRectangle.Right, 200);
            
            var walkableMap = Map.Instance.FrontMap.FindAll(tile => tile.Tile.Type != TileType.Air && tile.HitBox.Bottom>= hitbox.Top && hitbox.Intersects(tile.HitBox));
            var sortedList = walkableMap.OrderBy(tile => Vector2.Distance(tile.Position, Position)).ToList();
            var bottomTile = sortedList.First();


            float x = _maxLeftPosition = _maxRightPosition = bottomTile.Position.X;
            float y = bottomTile.Position.Y;

            while (x >= Tile.TileSize)
            {
                x -= Tile.TileSize;
                var tile = walkableMap.Find(tile => tile.Position.X == x && tile.Position.Y == y) ;
                if (tile == null) break;//gap
                tile = walkableMap.Find(tile => tile.Position.X == x && tile.Position.Y == y - Tile.TileSize);
                if (tile != null) break;//wall
                tile = walkableMap.Find(tile => tile.Position.X == x && tile.Position.Y == y - Tile.TileSize * 2);
                if (tile != null) break;//wall
                _maxLeftPosition = x;
            }
            //same for the right side
            x = bottomTile.Position.X;
            while (x <= Map.Instance.ScreenRectangle.Right - Tile.TileSize)
            {
                x += Tile.TileSize;
                var tile = walkableMap.Find(tile => tile.Position.X == x && tile.Position.Y == y);
                if (tile == null) break;//gap
                tile = walkableMap.Find(tile => tile.Position.X == x && tile.Position.Y == y - Tile.TileSize);
                if (tile != null) break;//wall
                tile = walkableMap.Find(tile => tile.Position.X == x && tile.Position.Y == y - Tile.TileSize * 2);
                if (tile != null) break;//wall
                _maxRightPosition = x;
            }

            Debug.WriteLine(_maxLeftPosition);
            Debug.WriteLine(_maxRightPosition);


        }
        public virtual void Move(GameTime gameTime)
        {
            var x = Position.X;
            Rectangle otterHb = Otter.Instance.HitBox;
            if (this is not Slime && otterHb.Top <= HitBox.Bottom && otterHb.Bottom >= HitBox.Top && otterHb.Right > _maxLeftPosition && otterHb.Left < _maxRightPosition )
            {
                IsFacingLeft = otterHb.Center.X < HitBox.Center.X;
            }
            if (IsFacingLeft)
            {
                if (x <= _maxLeftPosition)
                {
                    IsFacingLeft = false;
                }
                else
                {
                    x -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                if (x >= _maxRightPosition)
                {
                    IsFacingLeft = true;
                }
                else
                {
                    x += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            Position = new Vector2(x, Position.Y );
        }
        public override void Update(GameTime gameTime)
        {
            Animations.Update(State, gameTime);
            if (State is State.Walking)
            {

                Move(gameTime);

            }
            base.Update(gameTime);
        }

        internal virtual void SetState()
        {
            if (State is State.Dead && CurrentAnimation.IsFinished)
            {
                Remove = true;
                return;
            }
            if (IsDead) State = State.Dead;
            else if (IsHit) State = State.Hit;
            if (State is State.Dead || IsDead) return;
            
            if ((State is State.Hit && !CurrentAnimation.IsFinished) || (!CurrentAnimation.IsFinished && State == State.Attacking))
            {
                CanAttack = false;
                return;
            }
            if (State is State.Hit or State.Attacking && CurrentAnimation.IsFinished)
            {
                IsHit = false;
                CanAttack = true;
                State = State.Walking;
            }
            IsAttacking = CheckAttack();

            if (IsAttacking&& State is not State.Attacking)
            {
                State = State.Attacking;
                CanDamage = true;
                return;
            }
            State = IsWalking ? State.Walking : State.Idle;
        }
        public override void Draw(Sprites spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
        }

    }
}
