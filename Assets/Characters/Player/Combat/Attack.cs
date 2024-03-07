using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float meleeSpeed;
    [SerializeField] private int damage;

    [SerializeField] private float UmeleeSpeed;
    [SerializeField] private int Udamage;

    public bool isUpgraded;

    public bool canAttack = true;

    private Inventory inventory;
    
    public float knockbackForce = 25f;
    [SerializeField] private GameObject sword;

    public float OmeleeSpeed;
    public int Odamage;

    float timeUntilMelee = 0;

    private AudioManager audioManager;

    private void Start()
    {
        OmeleeSpeed = meleeSpeed;
        Odamage = damage;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        Item item = inventory.EquippedItem;
        if (item != null && item.itemType == Item.ItemType.IronSword)
        {
            isUpgraded = true;
            sword.transform.localScale = new Vector2(0.1f, 0.1f);
        }
        else
        {
            isUpgraded = false;
            sword.transform.localScale = new Vector2(0.06f, 0.06f);
        }
        if (sword.activeInHierarchy)
        {
           anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
        if (isUpgraded && meleeSpeed != UmeleeSpeed)
        {
            meleeSpeed = UmeleeSpeed;
            damage = Udamage;
        }
        else if (!isUpgraded && meleeSpeed != OmeleeSpeed)
        {
            meleeSpeed = OmeleeSpeed;
            damage = Odamage;
        }
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    private void FixedUpdate()
    {
        if (timeUntilMelee <= 0f)
        {
            canAttack = true;
        }
        timeUntilMelee -= Time.fixedDeltaTime;
    }

    public void RightAttack()
    {
        if (canAttack)
        {
            anim.SetTrigger("RightAttack");
            canAttack = false;
            timeUntilMelee = meleeSpeed;
            audioManager.SwingSwordSound();
        }
    }

    public void LeftAttack()
    {
        if (canAttack)
        {
            anim.SetTrigger("LeftAttack");
            canAttack = false;
            timeUntilMelee = meleeSpeed;
            audioManager.SwingSwordSound();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageableCharacter>())
        {
            Collider2D collider = other.GetComponent<Collider2D>();
            Vector2 direction = new Vector2(collider.transform.position.x - transform.position.x, collider.transform.position.y - transform.position.y).normalized;
            Vector2 knockback = direction * knockbackForce;
            other.gameObject.GetComponent<DamageableCharacter>().OnHit(damage, knockback);
            Debug.Log("Enemy Hit!");            
        }
    }
}
