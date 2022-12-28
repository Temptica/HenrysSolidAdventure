using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.EnemyFolder
{
    internal abstract class TrackingEnemy : Enemy
    {
        public Vector2 SpawnPosition { get; }
        public float Range { get; } 
        public TrackingEnemy(Vector2 spawnPosition, float range)
        {
            SpawnPosition = spawnPosition;
            Range = range;
        }
        public override void Update(GameTime gameTime)
        {
            
            if (Vector2.Distance(Otter.Instance.Position, Position) <= Range)
            {
                Chase();
            }
            else if(Position != SpawnPosition)
            { 
                Return();
            }
            base.Update(gameTime);
        }

        internal abstract void Chase();

        internal abstract void Return();
    }
}
