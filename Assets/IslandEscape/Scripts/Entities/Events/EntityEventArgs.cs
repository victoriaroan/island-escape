using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandEscape.Entities.Events
{
    [Serializable]
    public class EntityEventArgs
    {
        public MonoBehaviour publisher;

        public EntityEventArgs(MonoBehaviour publisher)
        {
            this.publisher = publisher;
        }
    }
}