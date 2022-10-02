using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules.Commands
{
    public abstract class Command
    {
        public static string Name;

        public CommandModule Module { get; set; }

        private Command parent;
        public Command Parent
        {
            get { return this.parent; }
            set
            {
                if (value != null && !(value is IBlockCommand))
                    throw new System.Exception("Invalid parent command. Must be IBlockCommand.");

                if (parent != null)
                {
                    // Debug.Log("old parent" + parent.ToString());
                    ((IBlockCommand)parent).RemoveCommand(this);
                }

                parent = value;

                if (parent != null)
                {
                    // Debug.Log("new parent" + parent.ToString());
                    ((IBlockCommand)parent).AddCommand(this);
                }

            }
        }

        /// <summary>
        /// Checks whether or not the command is available to the Entity. 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        // TODO: I really would prefer this be abstract
        public static bool Available(Entity entity) { return true; }

        /// <summary>
        /// Check whether or not the Entity is capable of executing the command.
        /// </summary>
        /// <returns>true if the Entity is capable, false otherwise</returns>
        public abstract bool Capable();

        /// <summary>
        /// Initiate the execution of the command.
        /// </summary>
        /// <returns></returns>
        public abstract void Execute();

        /// <summary>
        /// Check if the command has been completed.
        /// </summary>
        /// <returns>true if the command has been completed, false otherwise</returns>
        public abstract bool Check();

        /// <summary>
        /// Execute any finishing steps for the command after it has been completed and determine
        /// if the CommandCompleted event should be invoked. For example, if a loop command should
        /// continue, no CommandCompleted event should be invoked for the loop command
        /// </summary>
        /// <returns>true if the CommandCompleted event should be invoked, false otherwise</returns>
        public abstract bool Finish();

        public object this[string propertyName]
        {
            get
            {
                // TODO throw/handle exception if property doesn't exist
                // from https://stackoverflow.com/a/10283288 : probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName]
                return this.GetType().GetProperty(propertyName).GetValue(this, null);
            }
            set
            {
                // TODO throw/handle exception if property doesn't exist
                this.GetType().GetProperty(propertyName).SetValue(this, value, null);
            }
        }
    }
}
