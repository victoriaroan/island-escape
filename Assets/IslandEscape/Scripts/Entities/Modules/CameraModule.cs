using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules
{
    public class CameraModule : EntityModule
    {
        public const float MIN_TOLERANCE = 0.00001f;

        public float moveSpeed = 4f;

        protected float smoothSpeed = 0.05f;
        protected Vector3 velocity = Vector3.zero;

        public void FixedUpdate()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            var currentPosition = Camera.main.transform.position;
            var targetPosition = new Vector3(Entity.transform.position.x, Entity.transform.position.y, currentPosition.z);
            if ((targetPosition - currentPosition).sqrMagnitude > MIN_TOLERANCE)
            {
                Camera.main.transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothSpeed, moveSpeed);
            }
        }
    }
}

