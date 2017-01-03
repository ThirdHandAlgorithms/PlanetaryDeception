using System;
using System.Collections.Generic;

namespace PlanetaryDeception
{
    class PlayerInventory : ItemInventory
    {
        private static PlayerInventory ThisInstance = null;

        public static PlayerInventory Instance()
        {
            if (ThisInstance == null)
            {
                ThisInstance = new PlayerInventory();
            }

            return ThisInstance;
        }
    };
}
