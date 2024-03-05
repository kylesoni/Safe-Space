using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float aggroDistance; // Distance at which the enemy aggroes the player
    public float visionAngle; // Field of view angle in degrees
    public float visiondistance; // Distance at which enemy can see the player
    public float jumpForce; // Force enemy ai will jump with
    public LayerMask obstacleMask; // Layer mask for obstacles that block vision
    public bool onground;
    public bool stop_jump;

    private Transform player;
    private bool isAggressive = false;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private SpriteRenderer sprite;

    public bool Aggroed()
    {
        return isAggressive;
    }

    public bool Onground()
    {
        return onground;
    }

    void Start()
    {
        player = FindObjectOfType<Movement>().transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        onground = false;
        stop_jump = false;
    }

    void Update()
    {     
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is within aggro distance
        if (distanceToPlayer <= aggroDistance)
        {
            Debug.Log("Player is within aggro distance");
            // Check if the player is within the enemy's vision cone
            if (IsPlayerInVisionCone() && onground)
            {
                isAggressive = true;
            }
            else
            {
                isAggressive = false;
            }
        }
        else
        {
            Debug.Log("Player is not within aggro distance");
            isAggressive = false;
        }


    }

    private void FixedUpdate()
    {
        if (!stop_jump)
        {

            if ((int)rb.velocity.y < 0)
            {
                onground = false;
            }

            if (onground && ShouldJump())
            {
                Debug.Log("enemy just jumped");
                Jump();
            }

        }
    }

    bool IsPlayerInVisionCone()
    {      
        Vector3 transform_direction;

        if (transform.localScale.x > 0)
        {
            transform_direction = transform.right;
        } else
        {
            transform_direction = -transform.right;
        }

        // Calculate the position in front of the enemy to start the raycast
        Vector3 raycastOrigin = transform.position + transform_direction * 1;

        Vector3 directionToPlayer = player.position - raycastOrigin;
        float angleToPlayer = Vector2.Angle(transform_direction, directionToPlayer);

        // Check if the player is within the vision angle
        if (angleToPlayer <= visionAngle * 0.5f)
        {
            Debug.Log("It is possible to see the player");

            // Check if there are obstacles blocking the line of sight to the player
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, directionToPlayer, visiondistance, obstacleMask);


            if (hit.collider.gameObject == player.gameObject)
            {
                Debug.Log("We can see the player");
                return true;
            }
        }
        Debug.Log("We cannot see the player");
        return false;
    }

    private bool ShouldJump()
    {
        Vector2 ray_direction;

        if (transform.localScale.x > 0)
        {
            ray_direction = transform.right;
        }
        else
        {
            ray_direction = -transform.right;
        }


        // Calculate the position to start casting  ray
        Vector2 raycastStart1 = (Vector2)transform.position + new Vector2(0, -1f);
        Vector2 raycastStart2 = (Vector2)transform.position + new Vector2(0, 3.5f);

        col.enabled = false;

        // Cast ray 1 outward more near the ground to detect if there is an obstacle there
        RaycastHit2D hit1 = Physics2D.Raycast(raycastStart1, ray_direction, 1.5f, obstacleMask);

        if (hit1.collider != null)
        {
            Debug.Log("There is an obstacle in front of the enemy");
        }

        // Cast ray 2 outward upward to detect if there is space to jump
        RaycastHit2D hit2 = Physics2D.Raycast(raycastStart2, ray_direction, 1.5f, obstacleMask);

        col.enabled = true;

        if (hit2.collider == null)
        {
            Debug.Log("There is a place to jump to in front of the enemy");
        }

        if (hit1.collider != null && hit1.collider.gameObject != player.gameObject && hit2.collider == null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce * rb.mass));
        onground = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Dot(contact.normal, Vector2.up) > 0.9) // Adjust threshold as needed
            {
                onground = true;
                return; // Exit the loop early if ground is detected
            }
        }


        onground = false; // Set to false if no ground is detected
    }

}
