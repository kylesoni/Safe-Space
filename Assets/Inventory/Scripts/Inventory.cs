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
    private PlayerInventory player;
    
    public Inventory(PlayerInventory player) { 
        itemList = new List<Item>();
        itemTypeToSlotIndex = new Dictionary<Item.ItemType, int>();
        slotIndexToItemType = new Dictionary<int, Item.ItemType>();  
        this.player = player;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotIndex"> optional value</param>
    public void AddItem(Item item, int slotIndex =-1)
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
        if (!itemAlreadyInInventory)
        {
            itemList.Add(item);
            // store corresponding slot index
            if (slotIndex != -1)
            {
                if (!slotIndexToItemType.ContainsKey(slotIndex))
                {
                    slotIndexToItemType[slotIndex] = item.itemType;
                    itemTypeToSlotIndex[item.itemType] = slotIndex;
                }
               else
                {
                    Debug.Log("Can't add to this slot. It contains another item.");
                }
            }
            else { 
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
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Remove item from Inventory (can specify the amount to remove)
    /// </summary>
    /// <param name="item"></param>
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
            ClearEquip();
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
            ClearEquip();
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
