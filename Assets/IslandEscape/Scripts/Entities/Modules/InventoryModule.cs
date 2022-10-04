using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Resources;

namespace IslandEscape.Entities.Modules
{
    public class InventoryModule : EntityModule
    {
        public int inventoryLimit;

        private Inventory inventory;
        public Inventory Inventory { get => inventory; }

        public void Awake()
        {
            inventory = new Inventory(inventoryLimit);
        }
    }
}

