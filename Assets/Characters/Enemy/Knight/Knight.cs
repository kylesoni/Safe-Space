using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{

    public float aggroDistance = 15f; // Distance at which the enemy detects the player
    public float visionDistance = 100f; // Distance at which the enemy can see player
    public float visionAngle = 90f; // Field of view angle in degrees
    public float moveSpeed = 10f; // Movement speed of the enemy
    public float maxSpeed = 5f; // Max speed of knight
    public float jumpForce = 1000f; // Jump force of archer
    public float attackDistance = 5f; // Distance at which the enemy starts attacking
    public int attackDamage = 25;
    public float knockbackForce = 20f;
    public float attackCooldown = 1.8333f; // Cooldown between attacks
    public float aggressiveToPassiveDuration = 10f; // Duration to move towards player after losing aggro
    private float timeSinceAggroLost = 0f;
    public float switchInterval = 10f; // Adjust this value to change the time interval for switching direction
    private float timeSinceLastSwitch = 0.0f;

    public Animator animator;

    private DamageableCharacter enemy;
    private EnemyAI ai;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D col;

    private Transform player;
    private bool isAggressive;
    private bool canAttack = true;
    private Vector2 patrolDirection; // Direction of patrol movement
    private bool switched_behavior;
    private bool isAttacking = false;
    private bool dying;
    private Vector2 org_size;
    private bool which_swing;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<DamageableCharacter>();
        enemy.health = 150;
        enemy.maxHealth = 150;
        ai = GetComponent<EnemyAI>();
        ai.aggroDistance = aggroDistance;
        ai.visiondistance = visionDistance;
        ai.jumpForce = jumpForce;
        ai.visionAngle = visionAngle;
        isAggressive = ai.Aggroed();
        col = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Movement>().transform;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sprite = GetComponent<SpriteRenderer>();
        NewPatrolDirection();
        dying = false;

        animator = GetComponent<Animator>();
        org_size = col.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dying)
        {
            if (enemy.health <= 0)
            {
                rb.velocity = Vector2.zero;
                ai.stop_jump = true;
                StartCoroutine(Death());
                dying = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dying)
        {
            if (!isAttacking)
            {

                if ((int)rb.velocity.x > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if ((int)rb.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }

                isAggressive = ai.Aggroed();

                if (isAggressive)
                {

                    timeSinceAggroLost = 0f; // Reset timer for aggression lost

                    // Aggressive behavior when aggroed
                    AggressiveBehavior();
                }
                else
                {
                    // AgressievToPassive behavior when not aggroed
                    AggressiveToPassiveBehavior();
                }

                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);

                animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
                animator.SetInteger("vertical_vel", (int)rb.velocity.y);

            } else
            {
                col.size = sprite.bounds.size / 11;
            }

        }
        else
        {
            col.size = sprite.bounds.size / 7;
        }
    }

    private void AggressiveBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackDistance)
        {
            ai.stop_jump = false;
            Vector2 moveDirection = (player.position - transform.position).normalized;
            rb.AddForce(moveDirection * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ai.stop_jump = true;
        }


        // Check if the enemy can shoot and if the player is within shoot distance
        if (canAttack && distanceToPlayer <= attackDistance && ai.Onground())
        {
            // Shoot at the player
            SwordAttack();
        }
    }

    private void AggressiveToPassiveBehavior()
    {
        if (!isAggressive && timeSinceAggroLost < aggressiveToPassiveDuration)
        {
            // Continue moving towards player for a certain duration after losing aggro
            ai.stop_jump = false;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 moveDirection = (player.position - transform.position).normalized;
            rb.AddForce(moveDirection * moveSpeed);
            Debug.Log("Moving torward player cautiously");
            timeSinceAggroLost += Time.deltaTime;
            switched_behavior = true;
        }
        else
        {
            // If player is not re-aggroed within the specified duration, switch to passive behavior
            Debug.Log("Archer is now passive and patrolling");
            if (switched_behavior)
            {
                NewPatrolDirection();
                switched_behavior = false;
            }

            PassiveBehavior();
        }
    }

    private void PassiveBehavior()
    {
        // Move along the patrol direction
        rb.AddForce(patrolDirection * moveSpeed);

        // Update time since last direction switch
        timeSinceLastSwitch += Time.deltaTime;

        // Check if it's time to switch direction based on the interval
        if (timeSinceLastSwitch >= switchInterval)
        {
            // Reset timer
            timeSinceLastSwitch = 0.0f;

            // Switch direction
            Debug.Log("Archer switched patrolling direction");
            patrolDirection *= -1;
        }


    }

    private void SwordAttack()
    {
        isAttacking = true;
        // Set shoot cooldown
        canAttack = false;

        rb.isKinematic = true;

        if (Random.Range(0, 10) < 7)
        {
            animator.SetTrigger("oneswing");
            which_swing = true;
        } else
        {
            animator.SetTrigger("threeswing");
            which_swing = false;
        }

        StartCoroutine(Swing());
        

    }

    private IEnumerator Swing()
    {
        if (which_swing)
        {
            yield return new WaitForSeconds(0.833f);
        } else
        {
            yield return new WaitForSeconds(1.667f);
        }
        
        animator.SetTrigger("stop_attack");
        Invoke("ResetAttackCooldown", attackCooldown);
        col.size = org_size / 11;
        rb.isKinematic = false;
        isAttacking = false;
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("dead");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 1f);
        Destroy(gameObject);
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;
    }

    private void NewPatrolDirection()
    {
        if (Random.Range(0, 2) == 0)
        {
            patrolDirection = new Vector2(-1, 0);
        }
        else
        {
            patrolDirection = new Vector2(1, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking && collision.gameObject.GetComponent<DamageableCharacter>())
        {
            DamageableCharacter damagable_object = collision.gameObject.GetComponent<DamageableCharacter>();

            Vector2 direction = new Vector2(collision.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;

            damagable_object.GetComponent<DamageableCharacter>().OnHit(attackDamage, knockback);
        }
    }
}
