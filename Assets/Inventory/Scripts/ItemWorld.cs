using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = GetRandomDirUpperHalf();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 2f, item);
        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }
    public void SetItem(Item item)
    {
        this.item = item;

        // Set Sprite
        spriteRenderer.sprite = item.GetSprite();

        // Set Amount
        if (item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());
        } else
        {
            textMeshPro.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


    public static Vector3 GetRandomDirUpperHalf()
    {
        // Generate a random angle in degrees limited to the upper half (0 to 180 degrees)
        float randomAngle = UnityEngine.Random.Range(0f, 180f);

        // Convert the angle to a 3D direction
        float radianAngle = randomAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radianAngle);
        float y = Mathf.Sin(radianAngle);

        // Create a normalized vector representing the direction
        Vector3 randomDirection = new Vector3(x, y, 0f).normalized;

        return randomDirection;
    }

}
