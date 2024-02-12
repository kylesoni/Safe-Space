using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // public float speed = 5f;  
    public Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private Image equippedItemImage;
    private Transform playerTransform;

    private void Start() // change from Awake to Start or have null reference error
    {
        inventory = new Inventory(this);
        uiInventory.SetInventory(inventory);
        uiInventory.SetPlayer(this);
        playerTransform = transform;       
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ItemWorld itemWorld = collision.gameObject.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            // Touching Item
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    /// <summary>
    /// Display or hide image associated with equippedItem(Input)
    /// </summary>
    /// <param name="equippedItem"> Display this or hide this</param>
    public void SetEquipItemOnPlayer(Item equippedItem) { 
        // display the image
        if(equippedItem!= null && inventory.EquippedItem != null)
        {
            // TODO: show number? need? should?
            equippedItemImage.sprite = equippedItem.GetSprite();
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
