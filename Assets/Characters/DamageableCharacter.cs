using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class DamageableCharacter : MonoBehaviour
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

 
    public float iFrames = 1f;
    public float iTimer = 0f;
    public bool canTurnInvincible = true;
    private bool Invincible = false;

    public float movementCooldown = 0.5f;
    public float movementTimer = 0f;
    private bool canMove = true;

    public HealthBar healthbar;

    private Animator player_anim;

    public bool isPlayer = false;

    private Rigidbody2D rb;

    private Invincible InvincibleAbility;


    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<Movement>())
        {
            isPlayer = true;
            healthbar.SetMaxHealth(maxHealth);
            player_anim = GetComponent<Animator>();
        }
        InvincibleAbility = FindObjectOfType<PlayerInventory>().GetComponent<Invincible>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player has less than or equal to 0 health, death occurs
        if (health <= 0)
        {
            Death();
        }

        if (Invincible)
        {
            iTimer += Time.deltaTime;
        }
        if (iTimer > iFrames)
        {
            Invincible = false;
            iTimer = 0f;
        }

        if (!canMove)
        {
            movementTimer += Time.deltaTime;
        }

        if (movementTimer > movementCooldown)
        {
            canMove = true;
            movementTimer = 0;
            if (isPlayer)
            {
                GetComponent<Movement>().enabled = true;
            }
            else
            {
                if (GetComponent<Slime>())
                {
                    GetComponent<Slime>().enabled = true;
                }
                if (GetComponent<Wisp>())
                {
                    GetComponent<Wisp>().enabled = true;
                }
            }
        }
    }

    private void Death()
    {
        if (isPlayer)
        {
            SceneManager.LoadScene("WorldPrototype");
            AudioManager.instance.GameOverSound();
        }
        else if (GetComponent<Wisp>() || GetComponent<Slime>()) 
        {
            Destroy(gameObject);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (!Invincible && !InvincibleAbility.isPotionInvincible)
        {
            health -= damage;
            if (isPlayer)
            {
                healthbar.SetHealth(health);
                Debug.Log("Took Damage!");
                player_anim.SetTrigger("Hit");
                //GetComponent<Movement>().enabled = false;
                AudioManager.instance.TakeDamageSound();
            }

            rb.AddForce(knockback, ForceMode2D.Impulse);
            canMove = false;
            if (!isPlayer)
            {
                if (GetComponent<Slime>())
                {
                    GetComponent<Slime>().enabled = false;
                }
                if (GetComponent<Wisp>())
                {
                    GetComponent<Wisp>().enabled = false;
                }
                AudioManager.instance.HitEnemySound();
            }

            if (canTurnInvincible)
            {
                Invincible = true;
            }
            
        }
        
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (isPlayer)
        {
            healthbar.SetHealth(health);
        }
    }
}
