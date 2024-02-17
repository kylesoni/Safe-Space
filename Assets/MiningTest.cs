using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMining : MonoBehaviour
{
    public Tilemap map;
    public Slider slider;
    public Tile placeTile;

    private PlayerInventory player;
    private Inventory inventory;

    private Vector3Int currentlyMining;

    void Start()
    {
        // Hide the slider
        slider.gameObject.SetActive(false);

        // initialize inventory
        player = FindObjectOfType<PlayerInventory>().GetComponent<PlayerInventory>();
        inventory = player.inventory;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = map.WorldToCell(worldPoint);

            if (map.GetTile(cellPosition) == null)
                map.SetTile(cellPosition, placeTile);
        }

        // Check for left mouse click when equip the pickaxe      
        if (inventory.EquippedItem != null && inventory.EquippedItem.itemType == Item.ItemType.Pickaxe && Input.GetMouseButton(0))
        { 
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = map.WorldToCell(worldPoint);

            if (cellPosition != currentlyMining)
            {
                currentlyMining = cellPosition;
                slider.value = 0;
                slider.gameObject.SetActive(false);
            }

            // Check if the clicked position has a tile
            if (map.GetTile(cellPosition) != null)
            {
                // Show the slider
                slider.gameObject.SetActive(true);
                slider.transform.position = Camera.main.WorldToScreenPoint(cellPosition + new Vector3(0.5f, 0.5f, 0));

                // Update the slider value
                slider.value += Time.deltaTime;
                if (slider.value >= slider.maxValue)
                {
                    // Remove the tile at the clicked position
                    map.SetTile(cellPosition, null);
                    slider.value = 0;
                    slider.gameObject.SetActive(false);
                }
            } else
            {
                slider.value = 0;
                slider.gameObject.SetActive(false);
            }

            //map.SetTile(cellPosition, null); // Remove the tile at the clicked position
            
        } else
        {
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }
    }
}