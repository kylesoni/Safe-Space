using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int AttackSpeed;
    public int AttackDamage;

    public float speed;

    public float knockbackForce = 100f;

    private DamageableCharacter enemy;
    private bool move = false;
    Animator anim;
    GameObject player;
    Rigidbody2D body;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<DamageableCharacter>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Movement>().gameObject;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health < enemy.maxHealth)
        {
            move = true;
            anim.SetBool("isMoving", true);
        }
        if (body.velocity.y < -3)
        {
            body.velocity = new Vector2(body.velocity.x, -3);
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
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Movement>())
        {
            Collider2D collider = other.collider;
            Vector2 direction = new Vector2(collider.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;
            other.gameObject.GetComponent<DamageableCharacter>().OnHit(AttackDamage, knockback);
        }
    }
}
