using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // public int AttackSpeed;
    // public int AttackDamage;

    public float speed;
    public float materialize_dist;
    // private bool is_invisible = true;
    private bool is_overlapping = false;
    private bool is_dead;
    private bool is_invisible;
    private bool shape_shifting;
    private int health_before_invis;

    public float knockbackForce = 100f;
    public float attack_range;

    private DamageableCharacter enemy;
    // private bool move = false;
    // Animator anim;
    GameObject player;
    Rigidbody2D body;
    BoxCollider2D col;
    SpriteRenderer sprite;
    Animator animator;

    private int collision_count = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<DamageableCharacter>();
        health_before_invis = enemy.health;
        // anim = GetComponent<Animator>();
        player = FindObjectOfType<Movement>().gameObject;
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        is_dead = false;
        attack_range = materialize_dist * 0.75f;
        shape_shifting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!is_dead)
        {
            if (enemy.health > 0)
            {
                Debug.Log("Ghost Health (Neutral):");
                Debug.Log(enemy.health);

                Move();

                if (IsInvisible() && !is_invisible)
                {
                    TurnInvisble();
                }
                else if (!IsInvisible() && is_invisible)
                {
                    Rematerialize();
                }

                if (Vector2.Distance(transform.position, player.transform.position) < attack_range)
                {
                    animator.SetBool("attack", true);
                } else
                {
                    animator.SetBool("attack", false);
                }
            }
            else
            {
                is_dead = true;
            }
        } else
        {
            animator.SetTrigger("death");
            StartCoroutine(Death());
        }
    }

    private void Move()
    {
        Vector2 direct_to_player = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y + 2f, 0) - GetComponent<Transform>().position;

        float yOffset = Mathf.Sin(Time.time * speed * 2) * 0.5f;

        if (Vector2.Distance(transform.position, player.transform.position) < materialize_dist + 10)
        {
            body.velocity = (direct_to_player.normalized * speed * 2) + new Vector2(0, yOffset);
        } else
        {
            body.velocity = (direct_to_player.normalized * speed * 4) + new Vector2(0, yOffset);
        }
        

        if (body.velocity.x > 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }

        animator.SetBool("Moving", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision_count++;
        if (collision_count > 1)
        {
            is_overlapping = true;

        } else
        {
            is_overlapping = !(collision.gameObject == player);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision_count > 1)
        {
            is_overlapping = true;

        }
        else
        {
            is_overlapping = !(collision.gameObject == player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        collision_count--;

        if (collision_count == 0)
        {
            is_overlapping = false;
        }
    }

    private bool IsInvisible()
    {
        if (Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position) <= materialize_dist + 10)
        {

            if (is_overlapping)
            {
                return true;
                
            } else
            {
                return false;
            }

        } else
        {
            return true;
        }
    }

    private void TurnInvisble()
    {
        if (!shape_shifting)
        {
            is_invisible = true;
            shape_shifting = true;
            animator.SetBool("done_deMaterialize", false);
            animator.SetTrigger("deMaterialize");
            StartCoroutine(deMaterialize());
        }
    }

    private void Rematerialize()
    {
        if (!shape_shifting)
        {
            is_invisible = false;
            animator.SetBool("done_deMaterialize", false);
            animator.SetTrigger("deMaterialize");
            StartCoroutine(reMaterialize());
        }
    }

    private IEnumerator deMaterialize()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1.833f);
        enemy.health = int.MaxValue;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.05f);
        col.isTrigger = true;
        animator.SetBool("done_deMaterialize", true);
        shape_shifting = false;

    }

    private IEnumerator reMaterialize()
    {
        yield return new WaitForSeconds(0.833f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b);
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1.833f - 0.833f);
        enemy.health = health_before_invis;
        col.isTrigger = false;
        animator.SetBool("done_deMaterialize", true);
        shape_shifting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision_count++;
        if (collision.collider.gameObject == player && collision_count == 1)
        {
            Vector2 direction = new Vector2(collision.collider.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;

            player.GetComponent<DamageableCharacter>().OnHit((int) Mathf.Round(player.GetComponent<DamageableCharacter>().maxHealth * 0.5f), knockback);
        } else
        {
            is_overlapping = true;
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject == player && collision_count == 1)
        {
            Vector2 direction = new Vector2(collision.collider.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;

            player.GetComponent<DamageableCharacter>().OnHit((int)Mathf.Round(player.GetComponent<DamageableCharacter>().maxHealth * 0.5f), knockback);
        }
 

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision_count--;
    }

    private IEnumerator Death()
    {
        body.velocity = Vector2.zero;
        body.isKinematic = true;
        col.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }



}
