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
    }
    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");        
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);       
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
