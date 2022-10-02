using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandEscape.Entities.Events
{
    [Serializable]
    public class ActionEvent : UnityEvent<ActionEventArgs> { }
}

