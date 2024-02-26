using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    public int AttackSpeed;
    public int AttackDamage;

    public float speed;

    public float aggroRange = 10f;

    public float knockbackForce = 100f;

    private float distanceToPlayer;

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
        distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);
        if (distanceToPlayer < aggroRange || enemy.health < enemy.maxHealth)
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
        Vector2 playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
        body.velocity = speed * playerDirection;
        if (body.velocity.x > 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Movement>())
        {
            Collider2D collider = other.collider;
            Vector2 direction = new Vector2(collider.transform.position.x - transform.position.x, collider.transform.position.y - transform.position.y).normalized;
            Vector2 knockback = direction * knockbackForce;
            other.gameObject.GetComponent<DamageableCharacter>().OnHit(AttackDamage, knockback);
        }
    }
}
