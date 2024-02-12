using UnityEngine;
using System;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Sword,
        HealthPotion,
        Ore,
        Coin,
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
                case ItemType.Sword:            return ItemAssets.Instance.swordSprite;
                case ItemType.HealthPotion:     return ItemAssets.Instance.healthPotionSprite;
                case ItemType.Ore:              return ItemAssets.Instance.oreSprite;
                case ItemType.Coin:             return ItemAssets.Instance.coinSprite;
                case ItemType.Key:           return ItemAssets.Instance.keySprite;
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
            case ItemType.Ore:
                Debug.Log("Trying to use ore");
                break;
            case ItemType.Coin:
                Debug.Log("Trying to use coin");
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
