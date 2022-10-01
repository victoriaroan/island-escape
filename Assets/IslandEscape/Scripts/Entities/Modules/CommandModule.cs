using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

using IslandEscape.Entities.Modules.Commands;

namespace IslandEscape.Entities.Modules
{
    // TODO: I feel like there's a better place for tracking Entity status and/or a better way of doing it...
    // TODO: clean this up for island escape instead of golemancer
    public enum CommandStatus
    {
        Active, // TODO: used "Scripted" instead of "Active"?
        Idle,
        Paused,
        Stopping,
        Controlled,
    }

    public class CommandModule : EntityModule
    {
        public static IEnumerable<Type> AllCommands = null;

        // TODO: how to deal with commands failing?
        public UnityEvent<CommandEventArgs> CommandQueued { get; } = new UnityEvent<CommandEventArgs>();
        public UnityEvent<CommandEventArgs> CommandStarted { get; } = new UnityEvent<CommandEventArgs>();
        public UnityEvent<CommandEventArgs> CommandInterrupted { get; } = new UnityEvent<CommandEventArgs>();
        public UnityEvent<CommandEventArgs> CommandCompleted { get; } = new UnityEvent<CommandEventArgs>();
        public UnityEvent<CommandEventArgs> CommandFailed { get; } = new UnityEvent<CommandEventArgs>();

        private Queue<Command> commandQueue;

        private Command currentCommand;
        public Command CurrentCommand { get { return currentCommand; } }

        public CommandStatus CurrentState { get; set; }

        public IEnumerable<CommandInputModule> inputModules;

        public void Awake()
        {
            commandQueue = new Queue<Command>();
            GetAllCommands();
            inputModules = GetComponents<CommandInputModule>();
            bool hasActiveModule = false;
            foreach (CommandInputModule inputModule in inputModules)
            {
                if (inputModule.Active && hasActiveModule)
                {
                    Debug.Log("Warning: more than one active CommandInputModule. Disabling " + inputModule.ToString());
                    inputModule.Active = false;
                }
                else if (inputModule.Active)
                {
                    hasActiveModule = true;
                }
            }
            if (!hasActiveModule)
            {
                Debug.Log("Warning: no active CommandInputModule", gameObject);
            }
            Debug.Log(inputModules);
        }

        public override void Start()
        {
            base.Start();

        }

        public void Update()
        {

            // if a command is currently in progress, check completion
            if (currentCommand != null && currentCommand.Check())
            {
                CompleteCurrentCommand();
            }

            // if no command in progress, check if queue has any commands
            if (currentCommand == null && commandQueue.Count > 0)
            {
                // if at least one command in queue, start execution of that command
                ExecuteNextCommand();
            }
            else if (currentCommand == null)
            {
                // set golem idle only if not controlled
                if (IsState(CommandStatus.Active))
                {
                    Debug.Log("No more script commands to execute, going idle.");
                    CurrentState = CommandStatus.Idle;
                }
            }
        }

        private void GetAllCommands()
        {
            // only ever do this once
            if (AllCommands == null)
            {
                Debug.Log("Finding all command classes");
                AllCommands = Assembly.GetAssembly(typeof(Command)).GetTypes().Where(
                    type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Command))
                );
            }
            foreach (Type t in AllCommands)
            {
                Debug.Log(t.ToString());
            }
        }

        public bool IsState(CommandStatus state)
        {
            return CurrentState == state;
        }

        public bool IsState(CommandStatus[] states)
        {
            return states.Contains(CurrentState);
        }

        public Command InitCommand<T>() where T : Command, new()
        {
            Command command = new T();
            command.Module = this;
            return command;
        }

        public Command InitCommand<T>(Dictionary<string, dynamic> properties) where T : Command, new()
        {
            Command command = InitCommand<T>();
            SetCommandProperties(command, properties);
            return command;
        }

        public Command InitCommand(Type commandType)
        {
            // TODO: make sure commandType is a Command class (although the cast below will prob fail if it's not)
            Command command = (Command)Activator.CreateInstance(commandType);
            command.Module = this;
            return command;
        }

        public Command InitCommand(Type commandType, Dictionary<string, dynamic> properties)
        {
            Command command = InitCommand(commandType);
            SetCommandProperties(command, properties);
            return command;
        }

        public void SetCommandProperties(Command command, Dictionary<string, dynamic> properties)
        {
            // TODO: does the command need to be passed by ref for this to take?
            foreach (KeyValuePair<string, dynamic> property in properties)
            {
                command[property.Key] = property.Value;
            }
        }

        // TODO: interrupt commands

        public void QueueCommand(Command command)
        {
            commandQueue.Enqueue(command);
            CommandQueued?.Invoke(new CommandEventArgs(Entity, command));
        }

        public void QueueCommands(IEnumerable<Command> commands)
        {
            foreach (Command command in commands)
                QueueCommand(command);
        }

        public List<Command> GetQueuedCommands()
        {
            return commandQueue.ToList<Command>();
        }

        public void ClearQueue()
        {
            commandQueue.Clear();
        }

        private void ExecuteNextCommand()
        {
            // TODO: interrupt commands
            if (currentCommand != null)
                return;

            if (commandQueue.Count > 0)
            {
                currentCommand = commandQueue.Dequeue();
                currentCommand.Execute();
                CommandStarted?.Invoke(new CommandEventArgs(Entity, currentCommand));
            }
        }

        private void CompleteCurrentCommand()
        {
            if (currentCommand != null)
            {
                // run finishing steps
                bool invokeEvent = currentCommand.Finish();

                // invoke CommandCompleted event if desired
                if (invokeEvent)
                    CommandCompleted?.Invoke(new CommandEventArgs(Entity, currentCommand));

                // clear current command so next one can run
                currentCommand = null;
            }
        }

        public CommandInputModule GetCurrentInputModule()
        {
            return inputModules.Where(module => module.Active).First();
        }

        public void SetInputModule(Type moduleType)
        {
            bool moduleActivated = false;
            foreach (CommandInputModule inputModule in inputModules)
            {
                if (inputModule.GetType() == moduleType && moduleActivated)
                {
                    inputModule.Active = false;
                    Debug.Log("Warning: multiple matching modules found", gameObject);
                }
                else if (inputModule.GetType() == moduleType)
                {
                    moduleActivated = true;
                    inputModule.Active = true;
                }
                else
                {
                    inputModule.Active = false;
                }
            }
        }
    }
}
