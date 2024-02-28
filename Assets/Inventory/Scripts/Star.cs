using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class Star : MonoBehaviour
{
    private Inventory inventory;    
    public Item item;
    private TileMining tileMining;
    public GameObject playerPrefab;

    private void Start()
    {
        tileMining = playerPrefab.GetComponent<TileMining>();         
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }
    public void PutStar()
    {
        // check if position is valid
        // if valid        
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.down);
        if(playerPrefab == null)
        {
            Debug.Log("put star: playerInventory is null");
        }
        if (tileMining == null)
        {
            Debug.Log("tilemining is null");
            return;
        }
        tileMining.DetectTileatMousePosition();

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // If it hit something, it's an invalid position
            Debug.Log("Invalid position.");
            Debug.Log("Hit collider: " + hit.collider.name);
            return;
        }

        Debug.Log("valid position");

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Item starItem = item.CreateDuplicateItem(inventory.EquippedItem);
        starItem.amount = 1;
        // ItemWorld.SpawnItemWorld(position, starItem); // TODO: change position to mouse position
        inventory.UseItem(starItem);
    }
}
