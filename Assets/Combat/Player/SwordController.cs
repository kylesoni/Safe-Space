using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class SwordController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private player player;
    public bool facing_right;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>().GetComponent<player>();
    }

    void Update()
    {
        facing_right = player.facing_right;
        if (facing_right)
        {
            sprite.flipX = false;
        }
        if (!facing_right)
        {
            sprite.flipX = true;
        }
    }
}
