using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float meleeSpeed;
    [SerializeField] private int damage;
    private Player player;
    private Movement player_move;
    private Inventory inventory;
    public float knockbackForce = 25f;

    float timeUntilMelee = 0;

    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        player_move = FindObjectOfType<Player>().GetComponent<Movement>();
    }

    private void Update()
    {
        
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
            Vector2 direction = new Vector2(collider.transform.position.x - transform.position.x, 0.1f).normalized;
            Vector2 knockback = direction * knockbackForce;
            other.gameObject.GetComponent<DamageableCharacter>().OnHit(damage, knockback);
            Debug.Log("Enemy Hit!");
        }
    }
}
