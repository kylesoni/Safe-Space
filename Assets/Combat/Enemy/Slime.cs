using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float speed;

    private Enemy enemy;
    private bool move = false;
    Animator anim;
    GameObject player;
    Rigidbody2D body;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<player>().gameObject;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.Health < enemy.MaxHealth)
        {
            move = true;
            anim.SetBool("isMoving", true);
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            Move();
        }
    }

    private void Move()
    {
        float playerDirection = Mathf.Sign(player.transform.position.x - this.transform.position.x);
        body.velocity = new Vector2(speed * playerDirection, body.velocity.y);
        if (body.velocity.x > 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
}
