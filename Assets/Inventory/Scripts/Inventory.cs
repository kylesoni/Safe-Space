using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;

    // have dictionaries for both direction for faster lookup
    public Dictionary<Item.ItemType, int> itemTypeToSlotIndex;
    public Dictionary<int, Item.ItemType> slotIndexToItemType;

    public Item EquippedItem = null;
    public int EquippedIndex = -1;
    private Player player;
    
    public Inventory(Player player) { 
        itemList = new List<Item>();
        itemTypeToSlotIndex = new Dictionary<Item.ItemType, int>();
        slotIndexToItemType = new Dictionary<int, Item.ItemType>();  
        this.player = player;
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
            // store corresponding slot index
            for (int i = 0; i < UI_Inventory.slotCount; i++)
            {
                if (!slotIndexToItemType.ContainsKey(i))
                {
                    slotIndexToItemType[i] = item.itemType;
                    itemTypeToSlotIndex[item.itemType] = i;
                    break;
                }
            }
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {       
        Item itemInInventory = null;
        foreach (Item inventoryItem in itemList)
        {           
            if (inventoryItem.itemType == item.itemType)
            {
                inventoryItem.amount -= item.amount;
                itemInInventory = inventoryItem;
            }
        }
        if (itemInInventory != null && itemInInventory.amount <= 0)
        {
            if (itemInInventory == EquippedItem)
            {
                EquippedItem = null;
                EquippedIndex = -1;
                player.SetEquipItemOnPlayer(EquippedItem);
            }
            itemList.Remove(itemInInventory);
            // remove corresponding slot index
            int slotIndex = itemTypeToSlotIndex[item.itemType];
            itemTypeToSlotIndex.Remove(item.itemType);
            slotIndexToItemType.Remove(slotIndex);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void EquipItem(Item item)
    {
        Item itemInInventory = null;
        int i = 1;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                itemInInventory = inventoryItem;
                EquippedItem = itemInInventory;
                EquippedIndex = i;                
                player.SetEquipItemOnPlayer(EquippedItem);
            }
            i++;
        }
        if (itemInInventory == null)
        {
            Debug.Log("No item found to equip");
        }
    }
    public void EquipItem(Item.ItemType itemType)
    {
        Item itemInInventory = null;
        int i = 1;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == itemType)
            {
                itemInInventory = inventoryItem;
                EquippedItem = itemInInventory;
                EquippedIndex = i;
                player.SetEquipItemOnPlayer(EquippedItem);
            }
            i++;
        }
        if (itemInInventory == null)
        {
            Debug.Log("No item found to equip");
        }
    }

    public void ClearEquip()
    {
        EquippedItem = null;
        EquippedIndex = -1;
        player.SetEquipItemOnPlayer(null);
    }

    public void UseItem(Item item)
    {
        Item itemInInventory = null;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {                
                if (inventoryItem.isConsumable)
                {
                    inventoryItem.amount -= 1;
                }
                itemInInventory = inventoryItem;
            }
        }
        itemInInventory.UseItem();
        if (itemInInventory.amount <= 0)
        {
            itemList.Remove(itemInInventory);
            // remove corresponding slot index
            int slotIndex = itemTypeToSlotIndex[item.itemType];
            itemTypeToSlotIndex.Remove(item.itemType);
            slotIndexToItemType.Remove(slotIndex);
            EquippedItem = null;
            EquippedIndex = -1;
            player.SetEquipItemOnPlayer(EquippedItem);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList() {
        return itemList;
    }


}
