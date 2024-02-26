using UnityEngine;
using System;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public class Item
{
    public enum ItemType
    {
        // Items
        Sword,
        Pickaxe,
        Torch,
        HealthPotion,
        JumpPotion,
        GuardianPotion,
        Bomb,
        Key,       
        
        // Blocks
        Dirt,
        Stone,
    }

    // attributes
    public ItemType itemType;
    public int amount;
    public bool isConsumable;
    public bool isActive = false;
    public string itemInfo;

    public Sprite SetSprite()
    {
        switch(itemType)
        {
            default:
                // Items
                case ItemType.Sword:            return ItemAssets.Instance.swordSprite;
                case ItemType.Pickaxe:          return ItemAssets.Instance.pickaxeSprite;
                case ItemType.Torch:            return ItemAssets.Instance.torchSprite;
                case ItemType.HealthPotion:     return ItemAssets.Instance.healthPotionSprite;                
                case ItemType.JumpPotion:       return ItemAssets.Instance.jumpPotionSprite;
                case ItemType.GuardianPotion:   return ItemAssets.Instance.guardianPotionSprite;
                case ItemType.Bomb:             return ItemAssets.Instance.BombSprite;
                case ItemType.Key:              return ItemAssets.Instance.keySprite;
                // Blocks
                case ItemType.Dirt:             return ItemAssets.Instance.dirtSprite;
                case ItemType.Stone:            return ItemAssets.Instance.stoneSprite;
        }
    }

    public bool SetIsConsumable()
    {
        switch (itemType)
        {
            case ItemType.Sword:    return false;
            case ItemType.Pickaxe: return false;
            default:                return true;
        }
    }

    public string SetItemInfo()
    {
        switch(itemType)
        {          
            case ItemType.Sword: return "Sword: Melee weapon for close combat";
            case ItemType.Pickaxe: return "Pickaxe: Tool for mining and breaking blocks";
            case ItemType.Torch: return "Torch: Portable light source";
            case ItemType.HealthPotion: return "Health Potion: Restores player's health";
            case ItemType.JumpPotion: return "Jump Potion: Enhances jumping abilities permanently.";
            case ItemType.GuardianPotion: return "Guardian Potion: Grants temporary protection";
            case ItemType.Bomb: return "Bomb: Explosive device for breaking blocks";
            case ItemType.Key: return "Key: Unlocks doors or restricted areas";
            // Blocks
            case ItemType.Dirt: return "Dirt: Common surface terrain";
            case ItemType.Stone: return "Stone: Durable building block and crafting resource";

            default: return "no relevant item info";
        }
    }

    /// <summary>
    /// Do we need this function?
    /// </summary>
    public void UseItem()
    {
        switch(itemType)
        {            
            case ItemType.Sword:
                Debug.Log("Trying to use " + itemType);
                isActive = true;
                break;
            default:
                Debug.Log("Trying to use " + itemType);
                break;
        }
    }

    public void TurnOff()
    {
        isActive = false;
    }

    public Item CreateDuplicateItem(Item item)
    {
        return new Item { itemType = item.itemType, amount = item.amount, isConsumable = item.SetIsConsumable() , itemInfo = item.SetItemInfo()};
    }
}
