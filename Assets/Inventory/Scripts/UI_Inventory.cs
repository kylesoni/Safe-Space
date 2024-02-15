using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using static UnityEditor.Progress;
using UnityEditor.Experimental.GraphView;

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
    public int selectedSlotIndex;
    
    public static int slotCount = 10;  // can modify the slot count here


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
                Debug.Log("left click " + item);
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
        NumberKeySelect();
        MouseScrollSelect();
        FDrop();
        LeftClickUse();
    }
    
    private void NumberKeySelect()
    {
        // equip items corresponding to the numbers
        for (int i = 0; i < slotCount; i++)
        {
            if ((i == 9 && Input.GetKeyDown(KeyCode.Alpha0)) || Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlotIndex = i;
                SetItemSlotHighlight();
                if (inventory.slotIndexToItemType.ContainsKey(i)){             
                    Item.ItemType itemType = inventory.slotIndexToItemType[i];                    
                    inventory.EquipItem(itemType);
                    Debug.Log("Equipped " + itemType.ToString());
                }                
                else
                {
                    inventory.ClearEquip();
                    Debug.Log("Empty slot");
                }
            }
        }          
    }

    private void FDrop()
    {
        // drop equipped item when right click
        if (Input.GetKeyDown(KeyCode.F))
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
                switch (inventory.EquippedItem.itemType)
                {
                    default: 
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.HealthPotion:
                        FindObjectOfType<Player>().GetComponent<DamageableCharacter>().Heal(100);
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                }
            }
            else
            {
                Debug.Log("No equipped item");
            }
        }
    }

    private void MouseScrollSelect()
    {
        // mouse scroll to equip item
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // scroll up
        if (scrollInput > 0f)       ScrollSelectSlot(-1);
        // scroll down
        else if (scrollInput < 0f)  ScrollSelectSlot(1);        
    }

    /// <summary>
    /// Helper function used in MouseScrollSelect()
    /// </summary>
    /// <param name="direction"></param>
    private void ScrollSelectSlot(int direction)
    {        
        int newIndex = selectedSlotIndex + direction;
        if (newIndex >= slotCount)
        {
            newIndex = 0;
        }
        else if (newIndex < 0)
        {
            newIndex = slotCount - 1;
        }

        // select the new slot and equip item if not empty
        selectedSlotIndex = newIndex;
        SetItemSlotHighlight();
        if (inventory.slotIndexToItemType.ContainsKey(selectedSlotIndex))
        {
            Item.ItemType itemType = inventory.slotIndexToItemType[selectedSlotIndex];
            inventory.EquipItem(itemType);
            Debug.Log("Equipped " + itemType.ToString());
        }
        else
        {
            inventory.ClearEquip();
            Debug.Log("Empty slot");
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
            if (i == 9)
            {
                keyText.SetText("0");
            }
        }
    }

    private void SetItemSlotHighlight()
    {
        Color defaultColor = new Color(0f, 0f, 0f, 150f / 255f);
        Color highlightColor = new Color(255f, 0f, 0f, 30f / 255f);

        // Reset color for all slots
        for (int i = 0; i < slots.childCount; i++)
        {
            Image slotImage = slots.GetChild(i).GetComponent<Image>();
            slotImage.color = defaultColor; 
        }

        // Update the color of selected slot       
        Image selectedSlotImage = slots.GetChild(selectedSlotIndex + 1).GetComponent<Image>();
        selectedSlotImage.color = highlightColor;
    }

}
