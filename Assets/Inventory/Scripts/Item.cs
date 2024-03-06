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
        USword,
        Pickaxe,
        HealthPotion,
        JumpPotion,
        GuardianPotion,
        Bomb,
        Key,    
        Lantern,
        Star,
        
        // Blocks
        Dirt,
        Stone,
        Iron,
        Gold,

        // Spaceship
        Battery,
        Thruster,
        Control_Panel,
        Spaceship,

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
                case ItemType.USword:           return ItemAssets.Instance.UswordSprite;
                case ItemType.Pickaxe:          return ItemAssets.Instance.pickaxeSprite;
                case ItemType.HealthPotion:     return ItemAssets.Instance.healthPotionSprite;                
                case ItemType.JumpPotion:       return ItemAssets.Instance.jumpPotionSprite;
                case ItemType.GuardianPotion:   return ItemAssets.Instance.guardianPotionSprite;
                case ItemType.Bomb:             return ItemAssets.Instance.BombSprite;
                case ItemType.Key:              return ItemAssets.Instance.keySprite;
                case ItemType.Lantern:          return ItemAssets.Instance.lanternSprite;
                case ItemType.Star:             return ItemAssets.Instance.starSprite;
                // Blocks
                case ItemType.Dirt:             return ItemAssets.Instance.dirtSprite;
                case ItemType.Stone:            return ItemAssets.Instance.stoneSprite;
                case ItemType.Iron:            return ItemAssets.Instance.ironSprite;
                case ItemType.Gold:            return ItemAssets.Instance.goldSprite;
                // Spaceship
                case ItemType.Battery:             return ItemAssets.Instance.batterySprite;
                case ItemType.Thruster:            return ItemAssets.Instance.thrusterSprite;
                case ItemType.Control_Panel:       return ItemAssets.Instance.controlPanelSprite;
                case ItemType.Spaceship:           return ItemAssets.Instance.spaceshipSprite;
                
               
        }
    }

    public bool SetIsConsumable()
    {
        switch (itemType)
        {
            case ItemType.Sword:    return false;
            case ItemType.USword: return false;
            case ItemType.Pickaxe: return false;
            case ItemType.Lantern: return false;
            case ItemType.Battery: return false;
            case ItemType.Thruster: return false;
            case ItemType.Control_Panel: return false;
            case ItemType.Spaceship: return false;

            case ItemType.Dirt: return false;
            case ItemType.Stone: return false;
            case ItemType.Iron: return false;
            case ItemType.Gold: return false;
            default:                return true;
        }
    }

    public string SetItemInfo()
    {
        switch(itemType)
        {          
            case ItemType.Sword: return "Sword: Melee weapon for close combat";
            case ItemType.USword: return "Iron Sword: Faster and sharper melee weapon.";
            case ItemType.Pickaxe: return "Pickaxe: Tool for mining and breaking blocks";
            case ItemType.HealthPotion: return "Health Potion: Restores player's health";
            case ItemType.JumpPotion: return "Jump Potion: Enhances jumping abilities permanently.";
            case ItemType.GuardianPotion: return "Guardian Potion: Grants temporary protection";
            case ItemType.Bomb: return "Bomb: Explosive device for breaking blocks";
            case ItemType.Key: return "Key: Unlocks doors or restricted areas";
            case ItemType.Lantern: return "Lantern: Portable light source";
            case ItemType.Star: return "Star: Place in the air to light up the surrounding";
            // Blocks
            case ItemType.Dirt: return "Dirt: Common surface terrain";
            case ItemType.Stone: return "Stone: Durable building block and crafting resource";
            case ItemType.Iron: return "Iron: Durable building block and crafting resource";
            case ItemType.Gold: return "Gold: Durable building block and crafting resource";
            // Spaceship
            case ItemType.Battery: return "Battery: Power source for the spaceship's systems";
            case ItemType.Thruster: return "Thruster: Maneuvering engine for controlling the spaceship's movement";
            case ItemType.Control_Panel: return "Control Panel: Central interface for managing and operating the spaceship";
            case ItemType.Spaceship: return "Spaceship: An interstellar vessel";

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
            case ItemType.USword:
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
