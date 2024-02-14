using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int AttackSpeed;
    public int AttackDamage;

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<player>())
        {
            other.gameObject.GetComponent<player>().TakeDamage(AttackDamage);
        }
    }
}
