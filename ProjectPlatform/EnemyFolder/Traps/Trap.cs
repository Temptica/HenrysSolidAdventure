using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ProjectPlatform.Animations;
using ProjectPlatform.Interface;

namespace ProjectPlatform.EnemyFolder.Traps
{
    internal abstract class Trap: IAnimatable
    {
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation { get; }
        public Rectangle HitBox { get; set; }
        public Vector2 Position { get; set; }
    }
}
