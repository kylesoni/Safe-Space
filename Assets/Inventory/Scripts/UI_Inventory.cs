using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using static UnityEditor.Progress;


public class UI_Inventory : MonoBehaviour
{    

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public float itemSlotCellSize = 30f;
    private Player player;
    
    public Image slotImagePrefab;
    private Transform slots;
    public TextMeshProUGUI keyTextPrefab;
    
    public static int slotCount = 5;  // can modify the slot count here


    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");      
        slots = transform.Find("slots");       
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        // initialize the ui_inventory
        RefreshInventoryItems(); 
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
   
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();           
            itemSlotRectTransform.gameObject.SetActive(true);

            // Use and Drop Item with Left and Right Click
            itemSlotRectTransform.GetComponentInChildren<Button_UI>().ClickFunc = () =>
            {
                // equip item
                // TODO
            };
            itemSlotRectTransform.GetComponentInChildren<Button_UI>().MouseRightClickFunc = () =>
            {          
                // drop item in slot when right click
                DropItemFromSlot(item);
            };

            // Set position
            int slotIndex = inventory.itemTypeToSlotIndex[item.itemType];
            itemSlotRectTransform.anchoredPosition = new Vector2(slotIndex * itemSlotCellSize, 0); 
            
            // Set Image
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();           
            image.sprite = item.GetSprite();

            // Set Amount Text
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if(item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }            
          
        }        
    }

    private void DropItemFromSlot(Item item)
    {
        Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount, isConsumable = item.GetIsConsumable()};
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.transform.position, duplicateItem);        
    }


    private void Update()
    {
        NumberKeyEquip();
        MouseScrollEquip();
        RightClickDrop();
        LeftClickUse();
        UpdateEquippedItemSlotHighlight();
        
    }
    
    private void NumberKeyEquip()
    {
        // equip items corresponding to the numbers
        for (int i = 0; i < slotCount; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                try
                {                    
                    Item.ItemType itemType = inventory.slotIndexToItemType[i];                    
                    inventory.EquipItem(itemType);
                    Debug.Log("Equipped " + itemType.ToString());
                }
                catch
                {                    
                    Debug.Log("Empty slot");
                }
            }
        }              
    }

    private void RightClickDrop()
    {
        // drop equipped item when right click
        if (Input.GetMouseButtonDown(1))
        {
            if (inventory.EquippedItem != null)
            {
                DropItemFromSlot(inventory.EquippedItem);
            }
            else
            {
                Debug.Log("No equipped item");
            }
        }
    }

    private void LeftClickUse()
    {
        // use equipped item when left click
        if (Input.GetMouseButtonDown(0))
        {
            if (inventory.EquippedItem != null)
            {
                inventory.UseItem(inventory.EquippedItem);
            }
            else
            {
                Debug.Log("No equipped item");
            }
        }
    }

    private void MouseScrollEquip()
    {
        // mouse scroll to equip item
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // scroll up
        if (scrollInput > 0f)       ScrollEquippedItem(-1);
        // scroll down
        else if (scrollInput < 0f)  ScrollEquippedItem(1);        
    }

    /// <summary>
    /// Helper function used in MouseScrollEquip()
    /// </summary>
    /// <param name="direction"></param>
    private void ScrollEquippedItem(int direction)
    {
        int currentIndex = inventory.EquippedIndex;
        int newIndex = currentIndex + direction;
        var inventoryList = inventory.GetItemList();
        Item itemToEquip;

        // Ensure the index is within valid bounds            
        try
        {
            // if no equipped item, equip the first item in the slot
            if (inventory.EquippedItem == null && inventoryList.Count > 0)
            {
                itemToEquip = inventoryList[0];
                inventory.EquippedItem = itemToEquip;
                inventory.EquippedIndex = 0;
                player.SetEquipItemOnPlayer(itemToEquip);
            }
            else
            {                
                if (newIndex >= inventoryList.Count)
                {
                    newIndex = 0;
                }
                else if (newIndex < 0)
                {
                    newIndex = inventoryList.Count - 1;
                }                
                itemToEquip = inventoryList[newIndex];
                inventory.EquippedItem = itemToEquip;
                inventory.EquippedIndex = newIndex;
                player.SetEquipItemOnPlayer(itemToEquip);
                Debug.Log("Equipped " + itemToEquip.itemType.ToString());
            }
        }
        catch
        {
            // Debug.Log("Scrolling Out of Range");
        }
    }

    private void Start()
    {
        // ! make sure position of slots and itemSlotContainer are the same         
        SetSlots(slotCount);
    }
    private void SetSlots(int slotCount)
    {
        int x = 0;
        int y = 0;
        
        for (int i = 0; i < slotCount; i++)
        {
            // set image sprite
            Image slotImage = Instantiate(slotImagePrefab, slots);            
            slotImage.rectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y);
            slotImage.gameObject.SetActive(true);        
            x++;

            // set the pressKey text
            TextMeshProUGUI keyText = slotImage.transform.Find("keyText").GetComponent<TextMeshProUGUI>();            
            keyText.SetText((i + 1).ToString());
        }
    }

    private void UpdateEquippedItemSlotHighlight()
    {
        Color defaultColor = new Color(0f, 0f, 0f, 150f / 255f);
        Color highlightColor = new Color(255f, 0f, 0f, 30f / 255f);

        // Reset color for all slots
        for (int i = 0; i < slots.childCount; i++)
        {
            Image slotImage = slots.GetChild(i).GetComponent<Image>();
            slotImage.color = defaultColor; 
        }

        // Change the color of the slot of equipped item
        Item equippedItem = inventory.EquippedItem;
        if (equippedItem != null)
        {
            int slotIndex = inventory.itemTypeToSlotIndex[equippedItem.itemType] + 1;
            Image selectedSlotImage = slots.GetChild(slotIndex).GetComponent<Image>();
            selectedSlotImage.color = highlightColor;
        }    
    }

}
