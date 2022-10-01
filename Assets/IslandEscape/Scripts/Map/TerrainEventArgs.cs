using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Map
{
    public class TerrainEventArgs
    {
        public MonoBehaviour publisher;
        public List<Vector3Int> position;

        public TerrainEventArgs(MonoBehaviour publisher, List<Vector3Int> position)
        {
            this.publisher = publisher;
            this.position = position;
        }
    }
}
