using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : MonoBehaviour
{
    private Light2D spotlight;
    private PlayerInventory player;

    void Start()
    {
        spotlight = GetComponent<Light2D>();
        spotlight.enabled = false;
        player = FindObjectOfType<PlayerInventory>().GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if(player.inventory.EquippedItem == null || player.inventory.EquippedItem.itemType != Item.ItemType.Lantern)
        {
            spotlight.enabled = false;
        }
    }

    public void TurnOn()
    {
        spotlight.enabled = true;
    }
}
