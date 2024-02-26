using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    private PlayerInventory playerInventory;

    public Dictionary<Item.ItemType, Dictionary<Item.ItemType, int>> CraftingRecipes;

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();

        // Initialize crafting recipes
        InitializeCraftingRecipes();
    }

    private void Update()
    {
        print(CanCraft(Item.ItemType.Sword));

        if (CanCraft(Item.ItemType.Sword))
        {
            CraftItem(Item.ItemType.Sword);
        }
    }

    void InitializeCraftingRecipes()
    {
        CraftingRecipes = new Dictionary<Item.ItemType, Dictionary<Item.ItemType, int>>();
        CraftingRecipes[Item.ItemType.Sword] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Dirt, 2 },
            { Item.ItemType.Stone, 1 }
        };
    }

    public bool CanCraft(Item.ItemType itemToCraft)
    {
        if (!CraftingRecipes.ContainsKey(itemToCraft))
            return false;

        var inventory = playerInventory.inventory;
        foreach (var requirement in CraftingRecipes[itemToCraft])
        {
            if (!inventory.itemTypeToSlotIndex.ContainsKey(requirement.Key) ||
                inventory.GetItemList().Find(item => item.itemType == requirement.Key).amount < requirement.Value)
            {
                return false;
            }
        }

        return true;
    }

    public void CraftItem(Item.ItemType itemToCraft)
    {
        if (CanCraft(itemToCraft))
        {
            var inventory = playerInventory.inventory;
            foreach (var requirement in CraftingRecipes[itemToCraft])
            {
                Item itemToDeduct = new Item { itemType = requirement.Key, amount = requirement.Value };
                inventory.RemoveItem(itemToDeduct);
            }

            Item craftedItem = new Item { itemType = itemToCraft, amount = 1 };
            inventory.AddItem(craftedItem);
        }
        else
        {
            Debug.Log("Cannot craft " + itemToCraft.ToString() + ", missing required items.");
        }
    }
}
