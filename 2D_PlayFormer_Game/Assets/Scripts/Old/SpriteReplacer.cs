using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpriteReplacer : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;

    public void ReplaceSprite()
    {
        spriteRenderer.sprite = sprite;
    }
}
