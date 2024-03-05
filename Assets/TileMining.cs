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

        // Left click to place tile
        if (Input.GetMouseButtonDown(0))
        {
            // Can't place tile on other gameobjects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider == null)
            {
                if (map.GetTile(cellPosition) == null)
                {
                    if (inventory.EquippedItem != null && inventory.EquippedItem.itemType == Item.ItemType.Stone)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Stone, amount = 1 });
                        map.SetTile(cellPosition, placeStone);
                        AudioManager.instance.PlaceBlockSound();
                    }
                    else if (inventory.EquippedItem != null && inventory.EquippedItem.itemType == Item.ItemType.Dirt)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Dirt, amount = 1 });
                        map.SetTile(cellPosition, placeDirt);
                        AudioManager.instance.PlaceBlockSound();
                    }
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

                // Play Mining Sound
                AudioManager.instance.PlayMiningSound();

                // Update the slider value
                slider.value += Time.deltaTime;
                if (slider.value >= slider.maxValue)
                {
                    Item newItem;
                    switch (tile)
                    {                        
                        case Tile _ when tile == stoneTile:
                            newItem = new Item { itemType = Item.ItemType.Stone, amount = 1 };                            
                            break;
                        case Tile _ when tile == dirtTile:
                            newItem = new Item { itemType = Item.ItemType.Dirt, amount = 1 };                            
                            break;
                        case Tile _ when tile == grassTile:
                            newItem = new Item { itemType = Item.ItemType.Dirt, amount = 1 };                            
                            break;
                        default:
                            newItem = new Item { itemType = Item.ItemType.Stone, amount = 1 };                                              
                            break;
                    }
                    newItem.isConsumable = newItem.SetIsConsumable();
                    newItem.itemInfo = newItem.SetItemInfo();
                    inventory.AddItem(newItem);
                    AudioManager.instance.ItemPickupSound();

                    // Remove the tile at the clicked position
                    map.SetTile(cellPosition, null);
                    slider.value = 0;
                    slider.gameObject.SetActive(false);

                    // Stop Mining Sound
                    AudioManager.instance.StopMiningSound();

                }
            }
            else if (background.GetTile(cellPosition) != null)
            {
                // Similar implementation for background tiles if needed
                slider.gameObject.SetActive(true);
                slider.transform.position = Camera.main.WorldToScreenPoint(cellPosition + new Vector3(0.5f, 0.5f, 0));
                AudioManager.instance.PlayMiningSound();

                slider.value += Time.deltaTime;
                if (slider.value >= slider.maxValue)
                {
                    background.SetTile(cellPosition, null);
                    slider.value = 0;
                    slider.gameObject.SetActive(false);

                    AudioManager.instance.StopMiningSound();
                }
            }
            else
            {
                slider.value = 0;
                slider.gameObject.SetActive(false);
                AudioManager.instance.StopMiningSound(); // debug
            }
        }
        else
        {
            slider.value = 0;
            slider.gameObject.SetActive(false);
            AudioManager.instance.StopMiningSound(); // debug
        }
    }
}
