using UnityEngine;
using UnityEngine.UI;
using static Item;

public class PlayerInventory : MonoBehaviour
{
    // public float speed = 5f;  
    public Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private Image equippedItemImage;
    private Transform playerTransform;

    public StarSpawner starSpawner;
    public Lantern lantern;
    public Tooltip tooltip;
    public TileMining tileMining;
    public Attack attack;

    private void Start() // change from Awake to Start or have null reference error
    {
        inventory = new Inventory(this);        
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        playerTransform = transform;
        starSpawner.SetInventory(inventory);
        lantern.SetInventory(inventory);
        tooltip.SetInventory(inventory);
        tileMining.SetInventory(inventory); 
        attack.SetInventory(inventory);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ItemWorld itemWorld = collision.gameObject.GetComponent<ItemWorld>();
        if(itemWorld != null && inventory.GetItemList().Count < UI_Inventory.slotCount)
        {
            // Add item to inventory
            Item item = itemWorld.GetItem();
            inventory.AddItem(item);
            AudioManager.instance.ItemPickupSound();

            // Equip item if it's put in the selected slot
            if (inventory.itemTypeToSlotIndex[item.itemType] == uiInventory.selectedSlotIndex)
            {
                inventory.EquipItem(item);
                uiInventory.SetItemSlotHighlight();
                Debug.Log("Equipped " + item.itemType.ToString());
            }
            itemWorld.DestroySelf();
        }
    }

    /// <summary>
    /// Display or hide image associated with equippedItem(Input)
    /// </summary>
    /// <param name="equippedItem"> Display this or hide this</param>
    public void SetEquipItemOnPlayer(Item equippedItem) { 
        // display the image
        if(equippedItem!= null && inventory.EquippedItem != null && equippedItem.itemType != Item.ItemType.Sword)
        {
            // TODO: show number? need? should?
            equippedItemImage.sprite = equippedItem.SetSprite();
            Vector3 itemScreenPos = Camera.main.WorldToScreenPoint(playerTransform.position + playerTransform.right * 0.5f);
            equippedItemImage.rectTransform.position = itemScreenPos;
            equippedItemImage.gameObject.SetActive(true);            
        }
        // hide the image
        else
        {
            equippedItemImage.gameObject.SetActive(false);
        }
    }
   

}
