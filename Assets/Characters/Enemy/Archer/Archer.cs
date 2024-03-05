using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.U2D;

public class Archer : MonoBehaviour
{

    public float aggroDistance = 25f; // Distance at which the enemy detects the player
    public float visionDistance = 100f; // Distance at which the enemy can see player
    public float visionAngle = 90f; // Field of view angle in degrees
    public float shootDistance = 20f; // Distance at which the enemy starts shooting
    public float moveSpeed = 15f; // Movement speed of the enemy
    public float maxSpeed = 10f; // Max speed of archer
    public float jumpForce = 1000f; // Jump force of archer
    public float shootCooldown = 1.8333f; // Cooldown between shots
    public float aggressiveToPassiveDuration = 10f; // Duration to move towards player after losing aggro
    private float timeSinceAggroLost = 0f;
    public float switchInterval = 7f; // Adjust this value to change the time interval for switching direction
    private float timeSinceLastSwitch = 0.0f;


    public GameObject arrowPrefab; // Prefab of the arrow to be shot
    public Animator animator;
    public Vector2 shootPoint; // Point from where the arrow is shot

    private DamageableCharacter enemy;
    private EnemyAI ai;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D col;

    private Transform player;
    private bool isAggressive;
    private bool canShoot = true;
    private Vector2 patrolDirection; // Direction of patrol movement
    private bool switched_behavior;
    private bool isShooting = false;
    private bool dying;
    

    void Start()
    {
        enemy = GetComponent<DamageableCharacter>();
        enemy.health = 50;
        enemy.maxHealth = 50;
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

        
    }

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
            if (!isShooting)
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

            }

        } else
        {
            col.size = sprite.bounds.size / 7;
        }
        
    }

    private void AggressiveBehavior()
    {   
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > shootDistance) {
            ai.stop_jump = false;
            Vector2 moveDirection = (player.position - transform.position).normalized;
            rb.AddForce(moveDirection * moveSpeed);
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ai.stop_jump = true;
        }

        
        // Check if the enemy can shoot and if the player is within shoot distance
        if (canShoot && distanceToPlayer <= shootDistance && ai.Onground())
        {
            // Shoot at the player
            Shoot();
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

    private void Shoot()
    {
        isShooting = true;
        // Set shoot cooldown
        canShoot = false;


        // Disable movement along the X axis
        // rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        // Disable movement along the Y axis
        // rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        Debug.Log("Commence Shot");
        rb.isKinematic = true;
        animator.SetTrigger("shoot");
        StartCoroutine(Shoot_Helper());
    }

    private IEnumerator Shoot_Helper()
    {
        Vector2 shootPoint;

        if (transform.localScale.x > 0)
        {
            shootPoint = transform.position + transform.right.normalized;
            shootPoint += new Vector2(-0.5f, 0.4f);
        }
        else
        {
            shootPoint = transform.position - transform.right.normalized;
            shootPoint += new Vector2(0.5f, 0.4f);
        }

        

        // Calculate the direction towards the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Calculate the rotation to face the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        yield return new WaitForSeconds(1f);

        // Instantiate arrow at the shoot point
        GameObject arrow = Instantiate(arrowPrefab, shootPoint, Quaternion.identity);
        arrow.GetComponent<Arrow>().archer = gameObject;

        // Apply force to the arrow to make it move towards the player
        Rigidbody2D arrowRigidbody = arrow.GetComponent<Rigidbody2D>();
   
        arrow.transform.rotation = rotation;

        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(0.3f);

        arrow.GetComponent<Rigidbody2D>().velocity = directionToPlayer.normalized * 20;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - 1f - 0.3f);

        // Disable movement along the X axis
        // rb.constraints = ~RigidbodyConstraints2D.FreezePositionX;

        // Disable movement along the Y axis
        // rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;

        animator.SetTrigger("stop_shoot");
        Invoke("ResetShootCooldown", shootCooldown);
        rb.isKinematic = false;
        isShooting = false;

    }

    private IEnumerator Death()
    {
        animator.SetTrigger("dead");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 1f);
        Destroy(gameObject);
    }

    private void ResetShootCooldown()
    {
        canShoot = true;
    }

    private void NewPatrolDirection()
    {
        if (Random.Range(0,2) == 0)
        {
            patrolDirection = new Vector2(-1, 0);
        } else
        {
            patrolDirection = new Vector2(1, 0);
        }
    }

}



