using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IslandEscape.Entities.Modules;

namespace IslandEscape.Entities
{
    public class Entity : MonoBehaviour
    {
        /// <summary>
        /// Check to see if the Entity has the given module.
        /// </summary>
        /// <typeparam name="T">The type of EntityModule to look for.</typeparam>
        /// <returns></returns>
        public bool HasModule<T>() where T : EntityModule
        {
            return (T)GetComponent<T>() != null;
        }

        /// <summary>
        /// Check to see if the Entity has the given module.
        /// </summary>
        /// <param name="module">The type of EntityModule to look for.</param>
        /// <returns>Whether or not the module exists on the Entity</returns>
        // public bool HasModule(Type query)
        // {

        // }
    }
}
