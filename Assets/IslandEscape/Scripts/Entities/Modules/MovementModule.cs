using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules
{
    public class MovementModule : EntityModule
    {
        /// <summary>
        /// Allows the Entity to move.
        /// </summary>

        // TODO collision/blocking layer?
        public LayerMask blockingLayer;

        public float walkSpeed = 3f;
        public float runSpeed = 5f;

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.005f;
        protected const float smoothSpeed = 0.05f;
        protected Vector2 velocity = Vector2.zero;
        protected Vector2 targetVelocity;

        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

        protected SpriteModule spriteModule;

        public virtual void Awake()
        {
            AddRequiredModule(typeof(SpriteModule));
        }

        public override void Start()
        {
            base.Start();
            spriteModule = GetComponent<SpriteModule>();

            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(blockingLayer);
            contactFilter.useLayerMask = true;
        }

        public void FixedUpdate()
        {
            Move();
        }

        public void Update()
        {
            spriteModule.SetAnimatorParams(targetVelocity);
        }

        public void SetTargetVelocity(Vector2 target)
        {
            targetVelocity = target;
        }

        private void Move()
        {
            if (targetVelocity == Vector2.zero)
                return;

            Vector2 delta = targetVelocity * walkSpeed * Time.fixedDeltaTime;
            float magnitude = delta.magnitude;

            if (magnitude > minMoveDistance)
            {
                RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
                int count = spriteModule.rb.Cast(delta, contactFilter, hitBuffer, magnitude + shellRadius);

                // TODO: let the player get a bit closer to the water/put their feet in the water
                for (int i = 0; i < count; i++)
                {
                    float modifiedMagnitude = hitBuffer[i].distance - shellRadius;
                    magnitude = modifiedMagnitude < magnitude ? modifiedMagnitude : magnitude;
                }
            }

            spriteModule.rb.MovePosition(spriteModule.rb.position + targetVelocity.normalized * magnitude);
        }
    }
}
