using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.Shop
{
    internal class Store
    {
        private int FoodCount;
        private StatsBuff StatsBuff;
        private Random rng;
        private bool interacting = false;

        public Store()
        {
            rng = new Random();
            FoodCount = rng.Next(3, 6);
            StatsBuff = new StatsBuff();
        }
        public void GetMenu()
        {

        }
        
    }
}
