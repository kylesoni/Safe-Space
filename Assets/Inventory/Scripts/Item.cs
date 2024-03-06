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
        IronSword,
        Pickaxe,
        IronPickaxe,
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
        Sand,
        Redsand,
        Wood,
        Glass,
        Redstone,
        Redwood,
        Brick,

        // Materials
        Gold,
        Ruby,
        Iron,

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

    public static Sprite GetSprite(ItemType itemType)
    {
        switch(itemType)
        {
            // Items
            case ItemType.Sword: return ItemAssets.Instance.swordSprite;
            case ItemType.IronSword: return ItemAssets.Instance.UswordSprite;
            case ItemType.Pickaxe: return ItemAssets.Instance.pickaxeSprite;
            case ItemType.IronPickaxe: return ItemAssets.Instance.UpickaxeSprite;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.JumpPotion: return ItemAssets.Instance.jumpPotionSprite;
            case ItemType.GuardianPotion: return ItemAssets.Instance.guardianPotionSprite;
            case ItemType.Bomb: return ItemAssets.Instance.BombSprite;
            case ItemType.Key: return ItemAssets.Instance.keySprite;
            case ItemType.Lantern: return ItemAssets.Instance.lanternSprite;
            case ItemType.Star: return ItemAssets.Instance.starSprite;

            // Blocks
            case ItemType.Dirt: return ItemAssets.Instance.dirtSprite;
            case ItemType.Stone: return ItemAssets.Instance.stoneSprite;
            case ItemType.Sand: return ItemAssets.Instance.SandSprite;
            case ItemType.Redsand: return ItemAssets.Instance.RedsandSprite;
            case ItemType.Wood: return ItemAssets.Instance.WoodSprite;
            case ItemType.Glass: return ItemAssets.Instance.GlassSprite;
            case ItemType.Redstone: return ItemAssets.Instance.RedstoneSprite;
            case ItemType.Redwood: return ItemAssets.Instance.RedwoodSprite;
            case ItemType.Brick: return ItemAssets.Instance.BrickSprite;

            // Materials            
            case ItemType.Iron: return ItemAssets.Instance.ironSprite;
            case ItemType.Ruby: return ItemAssets.Instance.RubySprite;
            case ItemType.Gold: return ItemAssets.Instance.goldSprite;

            // Spaceship
            case ItemType.Battery: return ItemAssets.Instance.batterySprite;
            case ItemType.Thruster: return ItemAssets.Instance.thrusterSprite;
            case ItemType.Control_Panel: return ItemAssets.Instance.controlPanelSprite;
            case ItemType.Spaceship: return ItemAssets.Instance.spaceshipSprite;

            default: return ItemAssets.Instance.stoneSprite;
        }
    }

    public Sprite SetSprite()
    {
        return GetSprite(itemType);
    }

    public bool isBlock()
    {
        switch (itemType)
        {
            case ItemType.Dirt: return true;
            case ItemType.Stone: return true;             
            case ItemType.Sand: return true;
            case ItemType.Redsand: return true;
            case ItemType.Wood: return true;
            case ItemType.Glass: return true;
            case ItemType.Redstone: return true;
            case ItemType.Redwood: return true;
            case ItemType.Brick: return true;
            default: return false;
        }
    }

    public bool isPickaxe()
    {
        switch (itemType)
        {
            case ItemType.Pickaxe: return true;
            case ItemType.IronPickaxe: return true;
            default: return false;
        }
    }

    public bool SetIsConsumable()
    {
        switch (itemType)
        {
            case ItemType.Sword:    return false;
            case ItemType.IronSword: return false;
            case ItemType.Pickaxe: return false;
            case ItemType.IronPickaxe: return false;
            case ItemType.Lantern: return false;
            case ItemType.Battery: return false;
            case ItemType.Thruster: return false;
            case ItemType.Control_Panel: return false;
            case ItemType.Spaceship: return false;

            default: return !(isBlock());
        }
    }

    public string SetItemInfo()
    {
        switch(itemType)
        {
            case ItemType.Sword:
                return "Sword: Melee weapon for close combat";
            case ItemType.IronSword:
                return "Iron Sword: Faster and sharper melee weapon.";
            case ItemType.Pickaxe:
                return "Iron Pickaxe: Tool for mining and breaking blocks";
            case ItemType.IronPickaxe:
                return "Pickaxe: Improved tool for mining and breaking blocks";
            case ItemType.HealthPotion:
                return "Health Potion: Restores player's health";
            case ItemType.JumpPotion:
                return "Jump Potion: Enhances jumping abilities permanently.";
            case ItemType.GuardianPotion:
                return "Guardian Potion: Grants temporary protection";
            case ItemType.Bomb:
                return "Bomb: Explosive device for breaking blocks";
            case ItemType.Key:
                return "Key: Unlocks doors or restricted areas";
            case ItemType.Lantern:
                return "Lantern: Portable light source";
            case ItemType.Star:
                return "Star: Place in the air to light up the surrounding";

            // Blocks
            case ItemType.Dirt:
                return "Dirt: Common surface terrain";
            case ItemType.Stone:
                return "Stone: Durable building block and crafting resource";
            case ItemType.Sand:
                return "Sand: Loose material often found in deserts";
            case ItemType.Redsand:
                return "Red Sand: Similar to regular sand, but with a reddish tint";
            case ItemType.Wood:
                return "Wood: Basic building material obtained from trees";
            case ItemType.Glass:
                return "Glass: Transparent material used for windows and decoration";
            case ItemType.Redstone:
                return "Redstone: Resource used for creating electrical circuits";
            case ItemType.Redwood:
                return "Redwood: Sturdy building material obtained from redwood trees";
            case ItemType.Brick:
                return "A hard, solid block used for building houses and other structures.";

            // Materials
            case ItemType.Iron:
                return "Iron: Durable building block and crafting resource";
            case ItemType.Gold:
                return "Gold: Durable building block and crafting resource";
            case ItemType.Ruby:
                return "Ruby: Precious gemstone often used in crafting";

            // Spaceship
            case ItemType.Battery:
                return "Battery: Power source for the spaceship's systems";
            case ItemType.Thruster:
                return "Thruster: Maneuvering engine for controlling the spaceship's movement";
            case ItemType.Control_Panel:
                return "Control Panel: Central interface for managing and operating the spaceship";
            case ItemType.Spaceship:
                return "Spaceship: An interstellar vessel";

            default:
                return "No relevant item info";
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
            case ItemType.IronSword:
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
