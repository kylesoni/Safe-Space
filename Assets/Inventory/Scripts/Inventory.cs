using System;
using System.Collections.Generic;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;

    // have dictionaries for both direction for faster lookup
    public Dictionary<Item.ItemType, int> itemTypeToSlotIndex;
    public Dictionary<int, Item.ItemType> slotIndexToItemType;

    public Inventory() { 
        itemList = new List<Item>();
        itemTypeToSlotIndex = new Dictionary<Item.ItemType, int>();
        slotIndexToItemType = new Dictionary<int, Item.ItemType>();     
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
            itemList.Remove(itemInInventory);
            // remove corresponding slot index
            int slotIndex = itemTypeToSlotIndex[item.itemType];
            itemTypeToSlotIndex.Remove(item.itemType);
            slotIndexToItemType.Remove(slotIndex);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList() {
        return itemList;
    }


}
