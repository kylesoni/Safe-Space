using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;

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
            case ItemType.Sword: return ItemAssets.Instance.swordSprite;
            case ItemType.USword: return ItemAssets.Instance.UswordSprite;
            case ItemType.Pickaxe: return ItemAssets.Instance.pickaxeSprite;
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

            // Materials            
            case ItemType.Iron: return ItemAssets.Instance.ironSprite;
            case ItemType.Ruby: return ItemAssets.Instance.RubySprite;
            case ItemType.Gold: return ItemAssets.Instance.goldSprite;

            // Spaceship
            case ItemType.Battery: return ItemAssets.Instance.batterySprite;
            case ItemType.Thruster: return ItemAssets.Instance.thrusterSprite;
            case ItemType.Control_Panel: return ItemAssets.Instance.controlPanelSprite;
            case ItemType.Spaceship: return ItemAssets.Instance.spaceshipSprite;
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
