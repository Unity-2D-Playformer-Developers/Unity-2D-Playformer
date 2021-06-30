using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReplacer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;

    public void ReplaceSprite()
    {
        spriteRenderer.sprite = sprite;
    }
}
