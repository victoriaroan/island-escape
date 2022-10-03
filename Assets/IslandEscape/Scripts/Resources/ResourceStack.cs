using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Resources
{
    // I did have this as a struct but man, c#'s handling of structs is confusing (coming from a
    // python perspective at least.) I also thought about playing with records cause I think that
    // they're a cool concept, but then I saw "they're primarily intended for supporting immutable
    // data models" in the docs, so class it is.
    [System.Serializable]
    public class ResourceStack
    {
        public Blueprint blueprint;
        public int count;

        public bool IsSet { get => blueprint != null; }

        public ResourceStack() { }

        public ResourceStack(Blueprint blueprint, int count)
        {
            this.blueprint = blueprint;
            this.count = count;
        }
    }
}
