using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
   public enum ItemType
    {
        Sword,
        HealthPotion,
        Ore,
        Coin,
        Medkit,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
                case ItemType.Sword:            return ItemAssets.Instance.swordSprite;
                case ItemType.HealthPotion:     return ItemAssets.Instance.healthPotionSprite;
                case ItemType.Ore:              return ItemAssets.Instance.oreSprite;
                case ItemType.Coin:             return ItemAssets.Instance.coinSprite;
                case ItemType.Medkit:           return ItemAssets.Instance.medkitSprite;

        }
    }
}
