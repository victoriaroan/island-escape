using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules.Commands
{
    public interface IBlockCommand
    {
        public List<Command> Commands { get; }

        public void AddCommand(Command command);

        public void RemoveCommand(Command command);
    }
}

