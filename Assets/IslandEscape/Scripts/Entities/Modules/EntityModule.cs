using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add tests
namespace IslandEscape.Entities.Modules
{
    public abstract class EntityModule : MonoBehaviour
    {
        // TODO: cache this? is that what delegates are for?
        public Entity Entity { get { return GetComponent<Entity>(); } }

        private List<Type> requiredModules = new List<Type>();
        /// <summary>
        /// A list of all EntityModule classes required by the current EntityModule.
        /// </summary>
        public List<Type> RequiredModules { get { return requiredModules; } }

        // TODO: cache required components?

        public virtual void Start()
        {
            if (Entity == null)
                throw new System.Exception("EntityModules can only be attached to GameObjects with an Entity component.");

            foreach (Type type in RequiredModules)
            {
                if (GetComponent(type) == null)
                    throw new System.Exception(type.ToString() + " is required by " + this.GetType().ToString());
            }
        }

        /// <summary>
        /// Adds an EntityModule class to the list of required modules. Must be called in `Awake` to have any effect.
        /// </summary>
        /// <typeparam name="T">The type of the required EntityModule class.</typeparam>
        public void AddRequiredModule<T>() where T : EntityModule
        {
            requiredModules.Add(typeof(T));
        }

        /// <summary>
        /// Adds an EntityModule class to the list of required modules. Must be called in `Awake` to have any effect.
        /// </summary>
        /// <param name="requiredModule">The type of the required EntityModule class.</param>
        public void AddRequiredModule(Type requiredModule)
        {
            requiredModules.Add(requiredModule);
        }
    }
}
