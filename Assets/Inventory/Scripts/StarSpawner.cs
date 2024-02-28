using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    private Inventory inventory;
    public Item item;
    public TileMining tileMining;
    public Transform playerTransform;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void PutStar()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(playerTransform.position, position) < 10)
        {
            // Check if mouse over game objects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            bool isOverGameObject = hit.collider != null;

            // check if mouse over tiles       
            bool isOverTile = tileMining.isMouseOverTile();

            if (isOverGameObject || isOverTile)
            {
                Debug.Log("Can't place star over objects");
            }
            else
            {
                position.z = 0;
                Item starItem = item.CreateDuplicateItem(inventory.EquippedItem);
                starItem.amount = 1;
                ItemWorld.SpawnItemWorld(position, starItem);
                inventory.UseItem(starItem);
            }
        }
        else
        {
            Debug.Log("Star placement out of reach. Try a shorter distance");
        }         
    }
}
