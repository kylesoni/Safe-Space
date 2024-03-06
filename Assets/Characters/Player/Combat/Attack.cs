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

    private Inventory inventory;
    
    public float knockbackForce = 25f;
    [SerializeField] private GameObject sword;

    private float OmeleeSpeed;
    private int Odamage;

    float timeUntilMelee = 0;

    private void Start()
    {
        OmeleeSpeed = meleeSpeed;
        Odamage = damage;
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
        Item item = inventory.EquippedItem;
        if (item != null && (item.itemType == Item.ItemType.Sword || item.itemType == Item.ItemType.IronSword))
        {
            if (timeUntilMelee <= 0f)
            {
                if (item.isActive)
                {
                    if (Input.mousePosition.x > Screen.width / 2)
                    {
                        anim.SetTrigger("RightAttack");                        
                    }
                    else
                    {
                        anim.SetTrigger("LeftAttack");
                    }

                    AudioManager.instance.SwingSwordSound();
                    timeUntilMelee = meleeSpeed;
                    item.TurnOff();
                }
            }
            else
            {
                timeUntilMelee -= Time.fixedDeltaTime;
                item.TurnOff();
            }
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
