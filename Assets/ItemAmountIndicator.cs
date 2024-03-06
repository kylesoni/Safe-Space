using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAmountIndicator : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    private Item.ItemType item;

    public Sprite SetSprite(Item.ItemType itemType)
    {
        switch (itemType)
        {
            default:
            // Items
            case Item.ItemType.Sword: return ItemAssets.Instance.swordSprite;
            case Item.ItemType.USword: return ItemAssets.Instance.UswordSprite;
            case Item.ItemType.Pickaxe: return ItemAssets.Instance.pickaxeSprite;
            case Item.ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case Item.ItemType.JumpPotion: return ItemAssets.Instance.jumpPotionSprite;
            case Item.ItemType.GuardianPotion: return ItemAssets.Instance.guardianPotionSprite;
            case Item.ItemType.Bomb: return ItemAssets.Instance.BombSprite;
            case Item.ItemType.Key: return ItemAssets.Instance.keySprite;
            case Item.ItemType.Lantern: return ItemAssets.Instance.lanternSprite;
            case Item.ItemType.Star: return ItemAssets.Instance.starSprite;
            case Item.ItemType.Sand: return ItemAssets.Instance.SandSprite;
            case Item.ItemType.Redsand: return ItemAssets.Instance.RedsandSprite;
            case Item.ItemType.Wood: return ItemAssets.Instance.WoodSprite;
            case Item.ItemType.Glass: return ItemAssets.Instance.GlassSprite;
            case Item.ItemType.Redstone: return ItemAssets.Instance.RedstoneSprite;
            case Item.ItemType.Redwood: return ItemAssets.Instance.RedwoodSprite;

            // Blocks
            case Item.ItemType.Dirt: return ItemAssets.Instance.dirtSprite;
            case Item.ItemType.Stone: return ItemAssets.Instance.stoneSprite;
            case Item.ItemType.Iron: return ItemAssets.Instance.ironSprite;
            case Item.ItemType.Ruby: return ItemAssets.Instance.RubySprite;
            case Item.ItemType.Gold: return ItemAssets.Instance.goldSprite;
            // Spaceship
            case Item.ItemType.Battery: return ItemAssets.Instance.batterySprite;
            case Item.ItemType.Thruster: return ItemAssets.Instance.thrusterSprite;
            case Item.ItemType.Control_Panel: return ItemAssets.Instance.controlPanelSprite;
            case Item.ItemType.Spaceship: return ItemAssets.Instance.spaceshipSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemType(Item.ItemType itemType, int amt)
    {
        image.sprite = SetSprite(itemType);
        text.text = "x" + amt;
    }
}
