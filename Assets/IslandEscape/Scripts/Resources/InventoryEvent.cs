using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandEscape.Resources
{
    [Serializable]
    public class InventoryEvent : UnityEvent<InventoryEventArgs> { }
}