using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    /// <summary>
    /// player health
    /// </summary>
    public int health = 100;

    /// <summary>
    /// player max health
    /// </summary>
    public int maxHealth = 100;

    /// <summary>
    /// stamina of player
    /// </summary>
    public float stamina = 100f;

    /// <summary>
    /// speed of player
    /// </summary>
    public float speed = 20f;

    /// <summary>
    /// jump force of the player
    /// </summary>
    public float jumpforce = 400f;

    /// <summary>
    /// max speed of the player
    /// </summary>
    public float maxspeed = 100f;

    /// <summary>
    /// inventory of player
    /// </summary>
    public UnityEngine.Object[] inventory = new UnityEngine.Object[0];

    /// <summary>
    /// Rigidbody representing player's body
    /// </summary>
    private Rigidbody2D playerbody;

    /// <summary>
    /// SpriteRender representing player's look
    /// </summary>
    private SpriteRenderer playerlook;


    /// <summary>
    /// Keeps track of the direction player sprite is facing
    /// </summary>
    public bool facing_right;

    /// <summary>
    /// Keeps track of if the player is on the ground
    /// </summary>
    public bool onground;

    public bool timetojump;

    public float jumpFlexibility = 1;

    public HealthBar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        playerbody = GetComponent<Rigidbody2D>();
        playerlook = GetComponent<SpriteRenderer>();
        facing_right = true;
        tag = "Player";
        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // If player has less than or equal to 0 health, death occurs
        if (health <= 0) {
            Death();
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && onground && playerbody.velocity.y <= 0.01)
        {
            timetojump = true;
        }

        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Move();
        if (timetojump)
        {
            Jump();
            timetojump = false;
        }
    }

    /// <summary>
    /// Governs how the player moves
    /// </summary>
    private void Move()
    {

        // If space bar is pressed make the player jump

        // Player can only move in one direction at a time
        // If neither of the move keys is being pressed, the player should be still
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {

            // If player is facing right, flip sprite on x-axis and move left
            if (facing_right)
            {
                playerlook.flipX = true;
                facing_right = false;
            }

            playerbody.velocity = new Vector2(-speed * playerbody.mass, playerbody.velocity.y);

        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {

            // If player is facing left, flip sprite on x-axis and move right
            if (!facing_right)
            {
                playerlook.flipX = false;
                facing_right = true;
            }

            playerbody.velocity = new Vector2(speed * playerbody.mass, playerbody.velocity.y);

        }
        else
        {
            if (playerbody.velocity.x != 0)
            {
                playerbody.velocity = new Vector2(0, playerbody.velocity.y);
            }

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "blocc")
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint2D contact = collision.contacts[0];
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
                {
                    onground = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "blocc")
        {
            onground = false;
        }
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    private void Jump()
    {
        playerbody.AddForce(Vector2.up * jumpforce * playerbody.mass);
        onground = false;
    }

    /// <summary>
    /// Called when player dies
    /// </summary>
    private void Death()
    {
        //health = 0;
        //playerlook.enabled = false;
        //Destroy(gameObject);
        SceneManager.LoadScene("KyleTrashScene");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetHealth(health);
        Debug.Log("Took Damage!");
    }
}
