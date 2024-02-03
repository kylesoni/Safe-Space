using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;  
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        ItemWorld.SpawnItemWorld(new Vector3(2, 2), new Item { itemType = Item.ItemType.Sword, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(4, 5), new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(3, 6), new Item { itemType = Item.ItemType.Coin, amount = 1 });
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




    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");        
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);       
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
