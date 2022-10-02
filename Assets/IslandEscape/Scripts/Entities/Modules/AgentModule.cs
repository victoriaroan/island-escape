using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules
{
    public class AgentModule : EntityModule
    {
        public ActionModule availableAction;

        protected RenderModule renderModule;

        public virtual void Awake()
        {
            AddRequiredModule(typeof(RenderModule));
        }

        public override void Start()
        {
            base.Start();
            renderModule = GetComponent<RenderModule>();
        }

        // TODO: is this module event necessary? what does it do if the command module with an interact command does all the work?
    }
}
