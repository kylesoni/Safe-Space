using UnityEngine;

public class Player : MonoBehaviour
{
    // public float speed = 5f;  
    public Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    private void Start() // change from Awake to Start or have null reference error
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        uiInventory.SetPlayer(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            // Touching Item
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }

    }

}
