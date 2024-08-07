using System.Collections.Generic;
using UnityEngine;
using static Item;

public class Crafting : MonoBehaviour
{
    private PlayerInventory playerInventory;

    public Dictionary<Item.ItemType, Dictionary<Item.ItemType, int>> CraftingRecipes;

    void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();

        // Initialize crafting recipes
        InitializeCraftingRecipes();
    }

    void InitializeCraftingRecipes()
    {
        CraftingRecipes = new Dictionary<Item.ItemType, Dictionary<Item.ItemType, int>>();
        CraftingRecipes[Item.ItemType.Sword] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Wood, 2 },
            { Item.ItemType.Stone, 3 }
        };

        CraftingRecipes[Item.ItemType.IronPickaxe] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Wood, 3 },
            { Item.ItemType.Iron, 5 }
        };

        CraftingRecipes[Item.ItemType.IronSword] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Wood, 8 },
            { Item.ItemType.Gold, 5 },
            { Item.ItemType.Iron, 5 }
        };

        CraftingRecipes[Item.ItemType.Battery] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Gold, 30 },
            { Item.ItemType.Ruby, 10 }
        };

        CraftingRecipes[Item.ItemType.HealthPotion] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Stone, 5 },
            { Item.ItemType.Ruby, 1 }
        };

        CraftingRecipes[Item.ItemType.Thruster] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Wood, 10 },
            { Item.ItemType.Iron, 15 },
            { Item.ItemType.Ruby, 15 },
            { Item.ItemType.Gold, 15 }
        };

        CraftingRecipes[Item.ItemType.Spaceship] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Control_Panel, 1 },
            { Item.ItemType.Thruster, 1 },
            { Item.ItemType.Battery, 3 },
            { Item.ItemType.Iron, 10 },
            { Item.ItemType.Ruby, 5 }
        };
    }

    public bool CanCraft(Item.ItemType itemToCraft)
    {      
        if (!CraftingRecipes.ContainsKey(itemToCraft))
            return false;

        var inventory = playerInventory.inventory;
        if (inventory == null) return false;
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
            // Set other item attributes
            craftedItem.isConsumable = craftedItem.SetIsConsumable();
            craftedItem.itemInfo = craftedItem.SetItemInfo();
            inventory.AddItem(craftedItem);
            AudioManager.instance.ItemPickupSound();
        }
        else
        {
            Debug.Log("Cannot craft " + itemToCraft.ToString() + ", missing required items.");
        }
    }
}
