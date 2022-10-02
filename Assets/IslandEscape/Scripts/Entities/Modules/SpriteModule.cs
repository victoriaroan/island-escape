using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandEscape.Entities.Modules
{
    public class SpriteModule : RenderModule
    {
        public SpriteRenderer spriteRenderer;
        public override Renderer Renderer { get { return spriteRenderer; } }

        public Sprite[] sprites;

        public override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (sprites.Length == 1)
            {
                spriteRenderer.sprite = sprites[0];
            }
            else if (sprites.Length > 1)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }

        // TODO: handle sprite order in sorting layer when player is near
        // TODO: handle collider(s) for things like trees (and the player, for that matter) where the base should be a collision but the top is just a trigger.
    }
}

