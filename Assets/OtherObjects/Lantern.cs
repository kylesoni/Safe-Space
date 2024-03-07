using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : MonoBehaviour
{
    private Light2D spotlight;
    private Inventory inventory;

    void Start()
    {
        spotlight = GetComponent<Light2D>();
        spotlight.enabled = false;        
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    private void Update()
    {
        if(inventory.EquippedItem == null || inventory.EquippedItem.itemType != Item.ItemType.Lantern)
        {
            spotlight.enabled = false;
        }
    }

    public void TurnOn()
    {
        spotlight.enabled = true;
    }
}
