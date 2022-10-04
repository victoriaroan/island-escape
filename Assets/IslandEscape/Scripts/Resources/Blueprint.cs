using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Resources
{
    public abstract class Blueprint : ScriptableObject
    {
        public string displayName;
        public virtual string DisplayName { get { return displayName == "" ? name : displayName; } }

        public Sprite sprite;
        public Sprite Sprite { get { return sprite; } }

        public float rarity;

        public string description;

    }

}
