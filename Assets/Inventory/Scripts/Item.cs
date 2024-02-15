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

    public ItemType itemType;
    public int amount;
    public bool isConsumable;
    public bool isActive = false;

    public Sprite GetSprite()
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

    public bool GetIsConsumable()
    {
        switch (itemType)
        {
            case ItemType.Sword:    return false;
            default:                return true;
        }
    }


    public void UseItem()
    {
        switch(itemType)
        {
            default:
            case ItemType.Sword:
                isActive = true;
                break;
            case ItemType.HealthPotion:
                Debug.Log("Trying to use potion");           
                break;
            case ItemType.Key:
                Debug.Log("Trying to use key");
                break;
            case ItemType.Dirt:
                Debug.Log("Trying to use dirt");
                break;
            case ItemType.Stone:
                Debug.Log("Trying to use stone");
                break;
        }
    }

    public void TurnOff()
    {
        isActive = false;
    }
}
