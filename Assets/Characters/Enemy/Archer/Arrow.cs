using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public float knockbackForce = 10f;
    private Rigidbody2D rb;
    private bool been_shot;
    private BoxCollider2D col;
    public GameObject archer;

    private void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            been_shot = true;
            
        }
    }

    private void Start()
    {
        Debug.Log("Arrow Shot");
        
        damage = 20;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        gameObject.layer = 21;

        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    private void OnBecameInvisible()
    {
        if (been_shot)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != archer && collision.gameObject.GetComponent<Movement>() && been_shot)
        {
            DamageableCharacter damagable_object = collision.gameObject.GetComponent<DamageableCharacter>();

            Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;

            damagable_object.GetComponent<DamageableCharacter>().OnHit(damage, knockback);

            Destroy(gameObject);
        }
        else if (collision.gameObject != archer && been_shot)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != archer && collision.gameObject.GetComponent<Movement>() && been_shot)
        {
            DamageableCharacter damagable_object = collision.gameObject.GetComponent<DamageableCharacter>();

            Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;

            damagable_object.GetComponent<DamageableCharacter>().OnHit(damage, knockback);

            Destroy(gameObject);
        }
    }

}
