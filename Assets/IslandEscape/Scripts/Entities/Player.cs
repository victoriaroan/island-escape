using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Modules;
using IslandEscape.UI;

namespace IslandEscape.Entities
{
    public class Player : Entity
    {
        public InventoryGrid inventoryGrid;
        InventoryModule inventoryModule;


        public void Awake()
        {
            inventoryModule = GetComponent<InventoryModule>();
        }

        public void Start()
        {
            inventoryGrid.Set(inventoryModule.inventory);
        }
    }
}

