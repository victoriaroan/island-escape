using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules.Commands
{
    public class MoveCommand : Command
    {
        public new const string Name = "Move";
        public const float MIN_TOLERANCE = 0.00001f;

        private Vector2 movement = Vector2.zero;
        public Vector2 Movement { get => movement; set => movement = value; }

        private float tolerance = 0f;
        public float Tolerance { get => tolerance > 0f ? tolerance : MIN_TOLERANCE; set => tolerance = value; }

        public MoveCommand()
        {
        }

        public static new bool Available(Entity entity)
        {
            return entity.HasModule<MovementModule>();
        }

        public override bool Capable()
        {
            return Available(Module.Entity);
        }

        public override void Execute()
        {
            Module.GetComponent<MovementModule>().SetTargetVelocity(movement);
        }

        public override bool Check()
        {
            // TODO: new type of command/better way of requiring a certain condition remains true and interrupting command otherwise?
            if (movement.x == 0 && movement.y == 0)
            {
                return true;
            }

            Module.GetComponent<MovementModule>().SetTargetVelocity(movement);
            return false;
        }

        public override bool Finish()
        {
            Module.GetComponent<MovementModule>().SetTargetVelocity(Vector2.zero);
            return true;
        }
    }
}
