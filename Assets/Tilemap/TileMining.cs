using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class TileMining : MonoBehaviour
{
    public Tilemap map;
    public Tilemap overlay_map;
    public Tilemap background;
    public Slider slider;
    public Tile placeTile;

    public Tile stoneTile; // Reference to the stone tile
    public Tile dirtTile; // Reference to the dirt tile
    public Tile grassTile; // Reference to the grass tile

    public Tile placeStone;
    public Tile placeDirt;

    public Tile selectedTileOverlay;
    public Tile placeTileOverlay;
    public Tile overlay_break_1;
    public Tile overlay_break_2;
    public Tile overlay_break_3;
    public Tile overlay_break_4;
    public Tile overlay_break_5;

    public Tile[] gold_tiles;
    public Tile[] greystone_tiles;
    public Tile[] silver_tiles;
    public Tile sand;
    public Tile redsand;
    public Tile[] wood;
    public Tile glass;
    public Tile redstone;
    public Tile redwood;
    public Tile brick;

    private Inventory inventory;

    private Vector3Int currentlyMining;

    void Start()
    {
        // Hide the slider
        slider.gameObject.SetActive(false);
    }

    public Item.ItemType TileToItem(TileBase tile)
    {
        // God forgive me for I have sinned.

        if (tile == sand)
        {
            return Item.ItemType.Sand;
        }
        else if (tile == redsand)
        {
            return Item.ItemType.Redsand;
        }
        else if (gold_tiles.Contains(tile))
        {
            return Item.ItemType.Gold;
        }
        else if (greystone_tiles.Contains(tile))
        {
            return Item.ItemType.Ruby;
        }
        else if (silver_tiles.Contains(tile))
        {
            return Item.ItemType.Iron;
        }
        else if (wood.Contains(tile))
        {
            return Item.ItemType.Wood;
        }
        else if (tile == glass)
        {
            return Item.ItemType.Glass;
        }
        else if (tile == redstone)
        {
            return Item.ItemType.Redstone;
        }
        else if (tile == redwood)
        {
            return Item.ItemType.Redwood;
        }
        else if (tile == dirtTile || tile == grassTile)
        {
            return Item.ItemType.Dirt;
        }
        else if (tile == brick)
        {
            return Item.ItemType.Brick;
        }
        // Add more conditions as needed for other tile types
        else
        {
            // Return a default item type if none of the conditions above are met
            return Item.ItemType.Stone; // Consider adding an Unknown or similar default type if not already present
        }
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
        Vector3Int overlayCellPosition = overlay_map.WorldToCell(worldPoint);

        overlay_map.ClearAllTiles();

        if (Vector3.Distance(transform.position, worldPoint) > 12)
        {
            return;
        }

        if (inventory.EquippedItem != null && inventory.EquippedItem.isBlock() && map.GetTile(cellPosition) == null && background.GetTile(cellPosition) == null)
        {
            overlay_map.SetTile(overlayCellPosition, placeTileOverlay);
        } else if (map.GetTile(cellPosition) != null && inventory.EquippedItem != null && inventory.EquippedItem.isPickaxe())
        {
            overlay_map.SetTile(overlayCellPosition, selectedTileOverlay);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Can't place tile on other gameobjects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider == null)
            {
                if (map.GetTile(cellPosition) == null)
                {
                    Tile placeTile = null;
                    if (inventory.EquippedItem != null)
                    {
                        switch (inventory.EquippedItem.itemType)
                        {
                            case Item.ItemType.Stone: placeTile = stoneTile; break;
                            case Item.ItemType.Dirt: placeTile = dirtTile; break;
                            case Item.ItemType.Sand: placeTile = sand; break;
                            case Item.ItemType.Redsand: placeTile = redsand; break;
                            case Item.ItemType.Wood: placeTile = wood[0]; break;
                            case Item.ItemType.Glass: placeTile = glass; break;
                            case Item.ItemType.Redstone: placeTile = redstone; break;
                            case Item.ItemType.Redwood: placeTile = redwood; break;
                            case Item.ItemType.Brick: placeTile = brick; break;
                            default: placeTile = null; break;
                        }
                    }
                    if (placeTile != null)
                    {
                        inventory.RemoveItem(new Item { itemType = inventory.EquippedItem.itemType, amount = 1 });
                        map.SetTile(cellPosition, placeTile);
                        AudioManager.instance.PlaceBlockSound();
                    }
                }
            }                      
        }

        // Check for left mouse click when equipped with the pickaxe      
        if (Input.GetMouseButton(0) && inventory.EquippedItem != null && inventory.EquippedItem.isPickaxe())
        {
            if (cellPosition != currentlyMining)
            {
                currentlyMining = cellPosition;
                slider.value = 0;
                slider.gameObject.SetActive(false);

                switch (inventory.EquippedItem.itemType)
                {
                    case Item.ItemType.Pickaxe:
                        {
                            if (map.GetTile(cellPosition) == brick || greystone_tiles.Contains(map.GetTile(cellPosition)))
                            {
                                slider.maxValue = 99999f;
                            } else
                            {
                                slider.maxValue = 0.3f;
                            }

                            break;
                        }
                    case Item.ItemType.IronPickaxe: slider.maxValue = 0.15f; break;
                    default: slider.maxValue = 0.3f; break;
                }
            }

            // Check if the clicked position has a tile and it's a stone tile
            TileBase tile = map.GetTile(cellPosition);
            bool isBackground = false;

            if (tile == null)
            {
                tile = background.GetTile(cellPosition);

                // Check if tile is in the list Tile[] wood
                if (wood.Contains(tile))
                {
                    isBackground = true;
                } else
                {
                    tile = null;
                }
            }

            if (tile != null)
            {
                // Show the slider
                // slider.gameObject.SetActive(true);
                slider.transform.position = Camera.main.WorldToScreenPoint(cellPosition + new Vector3(0.5f, 0.5f, 0));

                // Play Mining Sound
                AudioManager.instance.PlayMiningSound();

                // Update the slider value
                slider.value += Time.deltaTime;

                // Update break overlay
                double breakPercent = slider.value / slider.maxValue;

                if (breakPercent < 0.2)
                {
                    overlay_map.SetTile(overlayCellPosition, overlay_break_1);
                }
                else if (breakPercent < 0.4)
                {
                    overlay_map.SetTile(overlayCellPosition, overlay_break_2);
                }
                else if (breakPercent < 0.6)
                {
                    overlay_map.SetTile(overlayCellPosition, overlay_break_3);
                }
                else if (breakPercent < 0.8)
                {
                    overlay_map.SetTile(overlayCellPosition, overlay_break_4);
                }
                else
                {
                    overlay_map.SetTile(overlayCellPosition, overlay_break_5);
                }

                if (slider.value >= slider.maxValue)
                {
                    Item newItem = new Item { itemType = TileToItem( tile ), amount = 1 };                            

                    newItem.isConsumable = newItem.SetIsConsumable();
                    newItem.itemInfo = newItem.SetItemInfo();
                    inventory.AddItem(newItem);
                    AudioManager.instance.ItemPickupSound();

                    // Remove the tile at the clicked position
                    if (isBackground)
                    {
                        background.SetTile(cellPosition, null);
                    }
                    else
                    {
                        map.SetTile(cellPosition, null);
                    }
                    slider.value = 0;
                    slider.gameObject.SetActive(false);

                    // Stop Mining Sound
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
