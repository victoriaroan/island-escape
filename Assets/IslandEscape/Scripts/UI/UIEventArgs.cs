using System;
using UnityEngine;

namespace IslandEscape.UI
{
    [Serializable]
    public class UIEventArgs
    {
        public MonoBehaviour publisher;

        public UIEventArgs(MonoBehaviour publisher)
        {
            this.publisher = publisher;
        }
    }
}

