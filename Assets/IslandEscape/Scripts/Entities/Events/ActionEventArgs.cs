using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Modules;

namespace IslandEscape.Entities.Events
{
    [Serializable]
    public class ActionEventArgs : EntityEventArgs
    {
        public AgentModule agent;

        public ActionEventArgs(MonoBehaviour publisher, AgentModule agent) : base(publisher)
        {
            this.agent = agent;
        }
    }
}
