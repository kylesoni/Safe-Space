using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{    
    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    private static Movement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerInventory>().GetComponent<Movement>();
    }

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {        
        Vector3 faceDirection = playerMovement.facing_right ? Vector3.right : Vector3.left;
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + faceDirection * 2f, item);
        return itemWorld;
    }

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// Set item attributes
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        this.item = item;

        // Set Sprite
        spriteRenderer.sprite = item.SetSprite();

        // Set Amount
        if (item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());
        } else
        {
            textMeshPro.SetText("");
        }

        // Set isConsumable
        item.isConsumable = item.SetIsConsumable();

        // Set itemInfo
        item.itemInfo = item.SetItemInfo();
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
