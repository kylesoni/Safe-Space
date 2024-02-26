using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float meleeSpeed;
    [SerializeField] private int damage;
    private PlayerInventory player;
    private Movement player_move;
    private Inventory inventory;
    private DamageableCharacter character;
    public float knockbackForce = 25f;
    [SerializeField] private GameObject sword;

    float timeUntilMelee = 0;

    void Start()
    {
        player = FindObjectOfType<PlayerInventory>().GetComponent<PlayerInventory>();
        player_move = FindObjectOfType<PlayerInventory>().GetComponent<Movement>();
        character = FindObjectOfType<PlayerInventory>().GetComponent<DamageableCharacter>();
    }

    private void Update()
    {
       if (sword.activeInHierarchy)
       {
           anim.SetBool("isAttacking", true);
       }
       else
       {
            anim.SetBool("isAttacking", false);
        }
    }

    private void FixedUpdate()
    {
        Item item = player.inventory.EquippedItem;
        if (item != null && item.itemType == Item.ItemType.Sword)
        {
            if (timeUntilMelee <= 0f)
            {
                if (item.isActive)
                {
                    if (player_move.facing_right)
                    {
                        anim.SetTrigger("RightAttack");
                    }
                    else
                    {
                        anim.SetTrigger("LeftAttack");
                    }

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
