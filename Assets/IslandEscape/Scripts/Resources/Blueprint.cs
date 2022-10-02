using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Resources
{
    public abstract class Blueprint : ScriptableObject
    {
        public string displayName;
        public virtual string DisplayName { get { return displayName == "" ? name : displayName; } }

        public Sprite icon;
        public Sprite Icon { get { return icon; } }

        public int rarity;
    }

}
