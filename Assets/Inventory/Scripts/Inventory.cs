using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    public Inventory() { 
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.Sword, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Ore, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Medkit, amount = 1 });
        Debug.Log(itemList.Count);
    }

    public void AddItem(Item item)
    {
        bool itemAlreadyInInventory = false;
        foreach (Item inventoryItem in itemList)
        {
            if(inventoryItem.itemType == item.itemType)
            {
                inventoryItem.amount += item.amount;
                itemAlreadyInInventory = true;
            }
        }
        if (!itemAlreadyInInventory) {
            itemList.Add(item);
        }        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList() {
        return itemList;
    }
}
