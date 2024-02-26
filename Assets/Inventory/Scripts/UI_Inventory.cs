using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class UI_Inventory : MonoBehaviour
{    

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public float itemSlotCellSize = 30f;
    private PlayerInventory player;
    
    public Image slotPrefab;
    private Transform slots;
    public TextMeshProUGUI keyTextPrefab;
    public int selectedSlotIndex = -1;
    
    public static int slotCount = 10;  // can modify the slot count here

    private Item draggedItem = null;
    private bool isInventoryMode = false;

    private Movement Movement;
    private DamageableCharacter DamageableCharacter;
    private Invincible InvincibleAbility;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");      
        slots = transform.Find("slots");       
    }

    public void SetPlayer(PlayerInventory player)
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

            // Set position
            int slotIndex = inventory.itemTypeToSlotIndex[item.itemType];
            itemSlotRectTransform.anchoredPosition = new Vector2(slotIndex * itemSlotCellSize, 0); 
            
            // Set Image
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();           
            image.sprite = item.SetSprite();

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

    /*
        private void DropItemFromSlot(Item item)
        {
            Item duplicateItem = item.CreateDuplicateItem(item);
            inventory.RemoveItem(item);
            ItemWorld.DropItem(player.transform.position, duplicateItem);        
        }
    */

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) {
            ToggleMovingItemMode();
        }

        // Disable other actions in MovingItemMode
        if (!isInventoryMode)
        {
            NumberKeySelect();
            MouseScrollSelect();            
            LeftClickUse();
        }

        FDrop();

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
        if (isInventoryMode && Input.GetKeyDown(KeyCode.F))
        // if Input.GetMouseButtonDown(1)
        {
            if (draggedItem != null)
            {
                // DropItemFromSlot(draggedItem);
                Item duplicateItem = draggedItem.CreateDuplicateItem(draggedItem);
                inventory.RemoveItem(draggedItem);
                ItemWorld.DropItem(player.transform.position, duplicateItem);

                draggedItem = null;
            }
            else
            {
                Debug.Log("No item to drop");
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
                        DamageableCharacter.Heal(100);
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.JumpPotion:
                        Movement.jumpforce = 1500f;
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.GuardianPotion:
                        if(!InvincibleAbility.isPotionInvincible){
                            InvincibleAbility.PlayerInvincibleForSeconds(5);
                            inventory.UseItem(inventory.EquippedItem);
                        }
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

        Movement = FindObjectOfType<PlayerInventory>().GetComponent<Movement>();
        DamageableCharacter = FindObjectOfType<PlayerInventory>().GetComponent<DamageableCharacter>();
        InvincibleAbility = FindObjectOfType<PlayerInventory>().GetComponent<Invincible>();

    }
    private void SetSlots(int slotCount)
    {
        int x = 0;
        int y = 0;
        
        for (int i = 0; i < slotCount; i++)
        {
            // Set image sprite
            Image slot = Instantiate(slotPrefab, slots);
            slot.rectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y);
            slot.gameObject.SetActive(true);        
            x++;

            // Set the pressKey text
            TextMeshProUGUI keyText = slot.transform.Find("keyText").GetComponent<TextMeshProUGUI>();
            keyText.SetText((i + 1).ToString());            
            if (i == 9)
            {
                keyText.SetText("0");
            }

            // Add OnClick function dynamically
            int slotIndex = i; // ensure different value is assigned to different slots
            Button buttonComponent = slot.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => LeftClickMoveItem(slotIndex));            

            // Set the name of slot to contain the corresponding slotIndex
            slot.gameObject.name = "slot_" + i.ToString();
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
        if (selectedSlotIndex != -1)
        {
            Image selectedSlotImage = slots.GetChild(selectedSlotIndex + 1).GetComponent<Image>();
            selectedSlotImage.color = highlightColor;
        }       
        
    }

    private void ToggleMovingItemMode()
    {
        isInventoryMode = !isInventoryMode;
        
        if (isInventoryMode)
        {
            // remove the slot highlight when enter
            selectedSlotIndex = -1;
            SetItemSlotHighlight();
        }
        
        if (!isInventoryMode)
        {
            // handle exit without putting back dragged item
            if (draggedItem != null)
            {
                inventory.AddItem(draggedItem);
                draggedItem = null;
            }

            // select the first slot when exit
            selectedSlotIndex = 0;
            SetItemSlotHighlight();
            if (inventory.slotIndexToItemType.ContainsKey(0))
            {
                inventory.EquipItem(inventory.slotIndexToItemType[0]);
            }
        }
    }

    public void LeftClickMoveItem(int clickedSlotIndex)
    {
        if (!isInventoryMode)
        {
            Debug.Log("Not in moving item Mode");
            return;
        }

        /*if (!IsPointerOverItemSlot())
        {
            Debug.Log("Not over item slot");
        }*/

        Debug.Log("Slot " + clickedSlotIndex + " pressed");

        // is dragging item
        if (draggedItem != null)
        {
            // if new slot has item, remove it from inventory and start dragging it
            if (inventory.slotIndexToItemType.ContainsKey(clickedSlotIndex))
            {
                Item.ItemType newItemType = inventory.slotIndexToItemType[clickedSlotIndex];
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemType == newItemType)
                    {
                        Item duplicateItem = item.CreateDuplicateItem(item);
                        inventory.RemoveItem(item);
                        // Add current item to inventory
                        inventory.AddItem(draggedItem, clickedSlotIndex);
                        // update draggedItem to new item
                        draggedItem = duplicateItem;
                        // TODO: update the item image near the cursor
                        break;
                    }
                }
            }
            else
            { 
                // Add Item to the clickedSlotIndex slot
                inventory.AddItem(draggedItem, clickedSlotIndex);
                Debug.Log("Put " + draggedItem.itemType + " in new slot");
                draggedItem = null;
                // TODO: delete the item image near the cursor
            }
        }
        else
        {
            // click on a slot that contains item
            if (inventory.slotIndexToItemType.ContainsKey(clickedSlotIndex))
            {
                Item.ItemType clickedItemType = inventory.slotIndexToItemType[clickedSlotIndex];
                foreach (Item item in inventory.GetItemList())
                {
                    if (item.itemType == clickedItemType)
                    {
                        Item duplicateItem = item.CreateDuplicateItem(item);
                        draggedItem = duplicateItem;                        
                        inventory.RemoveItem(item);
                        Debug.Log("Pick up " + draggedItem.itemType);
                        break;
                    }
                }

                // TODO: visually show the item image near the cursor
            }
        }
    }

    // don't work don't know why
    /*
        private bool IsPointerOverItemSlot()
        {
            // Cast a ray from the mouse position
            Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 0f, LayerMask.GetMask("UI")); // Ensure the LayerMask is correct

            if (hit.collider != null && hit.collider.CompareTag("itemSlot"))
            {
                // The pointer is over an item slot
                return true;
            }

            // The pointer is not over an item slot        
            return false;
        }*/
}
