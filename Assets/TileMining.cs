using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMining : MonoBehaviour
{
    public Tilemap map;
    public Tilemap background;
    public Slider slider;
    public Tile placeTile;

    public Tile stoneTile; // Reference to the stone tile
    public Tile dirtTile; // Reference to the dirt tile
    public Tile grassTile; // Reference to the grass tile

    public Tile placeStone;
    public Tile placeDirt;

    private Inventory inventory;

    private Vector3Int currentlyMining;

    void Start()
    {
        // Hide the slider
        slider.gameObject.SetActive(false);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public bool isMouseOverTile()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = map.WorldToCell(worldPoint);
        if (map.GetTile(cellPosition) == null && background.GetTile(cellPosition) == null)
        {
            // Debug.Log("not over tile");
            return false;
        }
        // Debug.Log("over tile");
        return true;
    }

    void Update()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = map.WorldToCell(worldPoint);

        if (Vector3.Distance(transform.position, worldPoint) > 12)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (map.GetTile(cellPosition) == null)
            {
                if (inventory.EquippedItem != null && inventory.EquippedItem.itemType == Item.ItemType.Stone)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.Stone, amount = 1 });
                    map.SetTile(cellPosition, placeStone);
                } else if (inventory.EquippedItem != null && inventory.EquippedItem.itemType == Item.ItemType.Dirt)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.Stone, amount = 1 });
                    map.SetTile(cellPosition, placeDirt);
                }
            }
        }

        // Check for left mouse click when equipped with the pickaxe      
        if (inventory.EquippedItem != null && inventory.EquippedItem.itemType == Item.ItemType.Pickaxe && Input.GetMouseButton(0))
        {
            if (cellPosition != currentlyMining)
            {
                currentlyMining = cellPosition;
                slider.value = 0;
                slider.gameObject.SetActive(false);
            }

            // Check if the clicked position has a tile and it's a stone tile
            TileBase tile = map.GetTile(cellPosition);
            if (tile != null)
            {
                // Show the slider
                slider.gameObject.SetActive(true);
                slider.transform.position = Camera.main.WorldToScreenPoint(cellPosition + new Vector3(0.5f, 0.5f, 0));

                // Update the slider value
                slider.value += Time.deltaTime;
                if (slider.value >= slider.maxValue)
                {
                    switch (tile)
                    {
                        case Tile _ when tile == stoneTile:
                            inventory.AddItem(new Item { itemType = Item.ItemType.Stone, amount = 1 });
                            break;
                        case Tile _ when tile == dirtTile:
                            inventory.AddItem(new Item { itemType = Item.ItemType.Dirt, amount = 1 });
                            break;
                        case Tile _ when tile == grassTile:
                            inventory.AddItem(new Item { itemType = Item.ItemType.Dirt, amount = 1 });
                            break;
                        default:
                            inventory.AddItem(new Item { itemType = Item.ItemType.Stone, amount = 1 });
                            break;
                    }

                    // Remove the tile at the clicked position
                    map.SetTile(cellPosition, null);
                    slider.value = 0;
                    slider.gameObject.SetActive(false);
                }
            }
            else if (background.GetTile(cellPosition) != null)
            {
                // Similar implementation for background tiles if needed
                slider.gameObject.SetActive(true);
                slider.transform.position = Camera.main.WorldToScreenPoint(cellPosition + new Vector3(0.5f, 0.5f, 0));

                slider.value += Time.deltaTime;
                if (slider.value >= slider.maxValue)
                {
                    background.SetTile(cellPosition, null);
                    slider.value = 0;
                    slider.gameObject.SetActive(false);
                }
            }
            else
            {
                slider.value = 0;
                slider.gameObject.SetActive(false);
            }
        }
        else
        {
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }
    }
}
