using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Resources
{
    public enum PartCategory
    {
        BindingAgent,
        Waterproofing,
        Propulsion,
        Steering,
        StarboardHull,
        PortHull,
        Anchor,
        Ballast,
        Shelter,
        Accessory,
        Optional,
        Utility,
    }

    public enum PartQuality
    {
        Poor,
        Normal,
        High,
        Exceptional,
    }

    [System.Serializable]
    public struct PartSlot
    {
        public PartCategory category;
        public PartQuality quality;
    }

    [CreateAssetMenu(fileName = "New Part Blueprint", menuName = "Island Escape/Part Blueprint")]
    public class PartBlueprint : Blueprint
    {
        public PartSlot[] slots;
        public PartSlot[] Slots { get => slots; }
    }
}

