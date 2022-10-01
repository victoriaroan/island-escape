using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules
{
    public class SpriteModule : EntityModule
    {
        public Rigidbody2D rb;
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetAnimatorParams(Vector2 movement)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
        }
    }
}

