using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static Item;
using static UnityEditor.Progress;

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
        //print(CanCraft(Item.ItemType.Sword));

        //if (CanCraft(Item.ItemType.Sword))
        //{
        //    CraftItem(Item.ItemType.Sword);
        //}
    }

    void InitializeCraftingRecipes()
    {
        CraftingRecipes = new Dictionary<Item.ItemType, Dictionary<Item.ItemType, int>>();
        CraftingRecipes[Item.ItemType.Sword] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Wood, 2 },
            { Item.ItemType.Stone, 3 }
        };

        CraftingRecipes[Item.ItemType.USword] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Wood, 8 },
            { Item.ItemType.Stone, 5 },
            { Item.ItemType.Iron, 5 }
        };

        CraftingRecipes[Item.ItemType.Battery] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Gold, 1 },
            { Item.ItemType.Iron, 5 }
        };

        CraftingRecipes[Item.ItemType.Spaceship] = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Control_Panel, 1 },
            { Item.ItemType.Thruster, 1 },
            { Item.ItemType.Battery, 2 },
            { Item.ItemType.Iron, 10 },
            { Item.ItemType.Ruby, 5 }
        };
    }

    public bool CanCraft(Item.ItemType itemToCraft)
    {
        print("CanCraft");

        if (!CraftingRecipes.ContainsKey(itemToCraft))
            return false;

        print("CanCraft1");

        var inventory = playerInventory.inventory;
        foreach (var requirement in CraftingRecipes[itemToCraft])
        {
            print("CanCraft 2");
            if (!inventory.itemTypeToSlotIndex.ContainsKey(requirement.Key) ||
                inventory.GetItemList().Find(item => item.itemType == requirement.Key).amount < requirement.Value)
            {
                return false;
            }
        }

        print("CanCraft 3");

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
