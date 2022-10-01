using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules
{
    public abstract class CommandInputModule : EntityModule
    {
        protected CommandModule commandModule;

        public virtual bool Active
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public virtual void Awake()
        {
            AddRequiredModule(typeof(CommandModule));
        }

        public override void Start()
        {
            base.Start();
            commandModule = GetComponent<CommandModule>();
        }
    }
}
