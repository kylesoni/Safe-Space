using UnityEngine;
using System;

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
    }

    public ItemType itemType;
    public int amount;
    public bool isConsumable = true;

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

        }
    }

    public void UseItem()
    {
        switch(itemType)
        {
            default:
            case ItemType.Sword: 
                Debug.Log("Trying to use sword");
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
        }
    }
}
