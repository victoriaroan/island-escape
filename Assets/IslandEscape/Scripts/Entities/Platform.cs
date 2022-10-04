using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Modules;
using IslandEscape.Resources;

namespace IslandEscape.Entities
{
    public class Platform : Entity
    {
        // TODO set the inventory size to length of this plus number of optional automatically? meh
        public PartCategory[] requiredParts;
        public Inventory Inventory { get { return inventoryModule.Inventory; } }

        private InventoryModule inventoryModule;

        public void Awake()
        {
            inventoryModule = GetComponent<InventoryModule>();
        }


    }
}

