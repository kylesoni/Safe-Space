using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
// using UnityEngine.UIElements;

public class UI_Inventory : MonoBehaviour
{    

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private int itemSlotCellSize = 70;
    private PlayerInventory player;
    
    public Image slotPrefab;
    private Transform hotbarSlots;
    public TextMeshProUGUI keyTextPrefab;
    [HideInInspector]
    public int selectedSlotIndex = -1;
    
    public static int slotCount = 30;  // total slot count: hotbar + inventory
    public static int hotbarSlotCount = 10;
    private int numSlotPerRow = 10;
    public GameObject inventorySlots;

    private Item draggedItem = null;
    public bool isInventoryMode = false;
    public Image dimmingOverlay;
    public GameObject draggedItemUI;
    public Image draggedItemImage;
    public GameObject craftingPanel;
    public GameObject menuButton;
    private SortedList<int, RectTransform> itemRectTransforms = new SortedList<int, RectTransform>();

    private Movement Movement;
    private DamageableCharacter DamageableCharacter;
    private Invincible InvincibleAbility;

    public Lantern lantern;
    public StarSpawner starSpawner;

    private static bool isMouseOverHotbar = false;

    public Spaceship spaceship;
    public GameObject SpaceshipUI;


    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        hotbarSlots = transform.Find("hotbarSlots");       
    }

    private void Start()
    {
        // ! make sure position of slots and itemSlotContainer are the same         
        SetHotbarSlots(hotbarSlotCount);
        inventorySlots.SetActive(false);

        Movement = FindObjectOfType<PlayerInventory>().GetComponent<Movement>();
        DamageableCharacter = FindObjectOfType<PlayerInventory>().GetComponent<DamageableCharacter>();
        InvincibleAbility = FindObjectOfType<PlayerInventory>().GetComponent<Invincible>();

        dimmingOverlay.gameObject.SetActive(false);
        draggedItemUI.SetActive(false);
        craftingPanel.SetActive(false);
        menuButton.SetActive(false);
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
        itemRectTransforms.Clear();

        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
   
        foreach (Item item in inventory.GetItemList())
        {            
            int slotIndex = inventory.itemTypeToSlotIndex[item.itemType];     
            
            RectTransform itemRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemRectTransform.gameObject.SetActive(true);

            // Set Position
            int xPosition = (slotIndex % numSlotPerRow) * itemSlotCellSize;
            int yPosition = -(slotIndex / numSlotPerRow) * itemSlotCellSize;
            itemRectTransform.anchoredPosition = new Vector2(xPosition, yPosition); 
            
            // Set Image
            Image image = itemRectTransform.Find("image").GetComponent<Image>();           
            image.sprite = item.SetSprite();
            // make block sprite smaller
            if (item.isBlock())
            {   
                image.rectTransform.localScale = new Vector3(0.6f, 0.6f, 1f);
            }

            // Set Amount Text
            TextMeshProUGUI uiText = itemRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if(item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }

            // Hide item with slot index >= 10 if not in inventoryMode
            if (slotIndex >= hotbarSlotCount && !isInventoryMode)
            {
                itemRectTransform.gameObject.SetActive(false);
                Debug.Log("Hide item " + item.itemType);
            }

            itemRectTransforms.Add(slotIndex, itemRectTransform);
        }
    }

    private void DisplayInactiveItem()
    {        
        for (int i = itemRectTransforms.Count - 1; i >= 0; i--)
        {
            var kvp = itemRectTransforms.ElementAt(i);
            int slotIndex = kvp.Key;
            Debug.Log("slot index: " + slotIndex);
            if (slotIndex < 10) // item with slot index < 10 should already be active
            {                
                break;
            }
            kvp.Value.gameObject.SetActive(true);         
        }
    }

    private void HideInventoryItem()
    {
        for (int i = itemRectTransforms.Count - 1; i >= 0; i--)
        {
            var kvp = itemRectTransforms.ElementAt(i);
            int slotIndex = kvp.Key;
            if (slotIndex < 10)
            {
                break;
            }
            kvp.Value.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) {
            ToggleInventoryMode();
        }

        // Disable other actions in InventoryMode
        if (!isInventoryMode)
        {
            NumberKeySelect();
            MouseScrollSelect();            
            LeftClickUse();
        } else if (isInventoryMode)
        {
            RightClickDrop();
            UpdateDraggedItemPosition();
        }        
    }

    public static void SetIsMouseOverHotbar(bool isMouseOver)
    {
        isMouseOverHotbar = isMouseOver;
    }

    private void UpdateDraggedItemPosition()
    {
        if (isInventoryMode && draggedItem != null)
        {
            draggedItemUI.transform.position = Input.mousePosition;
        }
    }

    private void NumberKeySelect()
    {
        if (!isInventoryMode)
        {
            // equip items corresponding to the numbers
            for (int i = 0; i < hotbarSlotCount; i++)
            {
                if ((i == 9 && Input.GetKeyDown(KeyCode.Alpha0)) || Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    inventory.ClearEquip();
                    selectedSlotIndex = i;
                    SetItemSlotHighlight();
                    if (inventory.slotIndexToItemType.ContainsKey(i))
                    {
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
    }

    public void LeftClickSelect(int slotIndex)
    {
        if (!isInventoryMode)
        {
            selectedSlotIndex = slotIndex; 
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
    }

    private void RightClickDrop()
    {
        // drop equipped item when right click
        if (isInventoryMode && Input.GetMouseButtonDown(1))
        {
            if (draggedItem != null)
            {                
                Item duplicateItem = draggedItem.CreateDuplicateItem(draggedItem);
                inventory.RemoveItem(draggedItem);                
                ItemWorld.DropItem(player.transform.position, duplicateItem);

                draggedItem = null;
                draggedItemUI.SetActive(false);
            }
            else
            {
                Debug.Log("No item to drop");
            }
        }
    }

    private void LeftClickUse()
    {
        // use equipped item when left click and mouse not over hotbar
        if (!isInventoryMode && Input.GetMouseButtonDown(0) && !isMouseOverHotbar)
        {
            if (inventory.EquippedItem != null)
            {
                Debug.Log(inventory.EquippedItem.SetItemInfo());
                switch (inventory.EquippedItem.itemType)
                {
                    default: 
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.Sword:
                        if (Input.mousePosition.x > Screen.width / 2)
                        {
                            player.GetComponent<Attack>().RightAttack();
                        }
                        else
                        {
                            player.GetComponent<Attack>().LeftAttack();
                        }
                        break;
                    case Item.ItemType.IronSword:
                        if (Input.mousePosition.x > Screen.width / 2)
                        {
                            player.GetComponent<Attack>().RightAttack();
                        }
                        else
                        {
                            player.GetComponent<Attack>().LeftAttack();
                        }
                        break;
                    case Item.ItemType.HealthPotion:
                        DamageableCharacter.Heal(100);
                        AudioManager.instance.DrinkPotionSound();
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.JumpPotion:
                        Movement.jumpforce = 1500f;
                        AudioManager.instance.DrinkPotionSound();
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.GuardianPotion:
                        if(!InvincibleAbility.isPotionInvincible){
                            AudioManager.instance.DrinkPotionSound();
                            InvincibleAbility.PlayerInvincibleForSeconds(5);
                            inventory.UseItem(inventory.EquippedItem);
                        }
                        break;
                    case Item.ItemType.Lantern:
                        lantern.TurnOn();
                        AudioManager.instance.SwitchOnSound();
                        inventory.UseItem(inventory.EquippedItem);
                        break;
                    case Item.ItemType.Star:
                        starSpawner.PutStar();
                        AudioManager.instance.PlaceStarSound();
                        // handle inventory.UseItem(inventory.EquippedItem) in PutStar()
                        break;
                    case Item.ItemType.Spaceship:
                        if (!SpaceshipUI.activeSelf)
                        {
                            spaceship.ShowSpaceshipUI();
                            AudioManager.instance.GameWinSound();
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
        if (!isInventoryMode)
        {
            // mouse scroll to equip item
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            // scroll up
            if (scrollInput > 0f) ScrollSelectSlot(-1);
            // scroll down
            else if (scrollInput < 0f) ScrollSelectSlot(1);
        }              
    }

    /// <summary>
    /// Helper function used in MouseScrollSelect()
    /// </summary>
    /// <param name="direction"></param>
    private void ScrollSelectSlot(int direction)
    {        
        int newIndex = selectedSlotIndex + direction;   
        if (newIndex >= hotbarSlotCount)
        {
            newIndex = 0;
        }
        else if (newIndex < 0)
        {
            newIndex = hotbarSlotCount - 1;
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

    private void SetHotbarSlots(int hotbarSlotCount)
    {
        int x = 0;
        int y = 0;
        
        for (int i = 0; i < hotbarSlotCount; i++)
        {
            // Set image sprite
            Image slot = Instantiate(slotPrefab, hotbarSlots);
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
            buttonComponent.onClick.AddListener(() => LeftClickSelect(slotIndex));               

            // Set the name of slot to contain the corresponding slotIndex
            slot.gameObject.name = "slot_" + i.ToString();
        }
    }

    public void SetItemSlotHighlight()
    {
        Color defaultColor = new Color(0f, 0f, 0f, 150f / 255f);
        Color highlightColor = new Color(255f, 0f, 0f, 30f / 255f);

        // Reset color for all slots
        for (int i = 0; i < hotbarSlots.childCount; i++) // bug? 
        {
            Image slotImage = hotbarSlots.GetChild(i).GetComponent<Image>();
            slotImage.color = defaultColor; 
        }

        // Update the color of selected slot
        if (selectedSlotIndex != -1)
        {
            Image selectedSlotImage = hotbarSlots.GetChild(selectedSlotIndex + 1).GetComponent<Image>();
            selectedSlotImage.color = highlightColor;
        }       
        
    }

    private void ToggleInventoryMode()
    {
        isInventoryMode = !isInventoryMode;
        
        if (isInventoryMode)
        {
            // remove the slot highlight when enter
            selectedSlotIndex = -1;
            SetItemSlotHighlight();

            dimmingOverlay.gameObject.SetActive(true);
            inventorySlots.SetActive(true);
            craftingPanel.SetActive(true);
            menuButton.SetActive(true);
            DisplayInactiveItem();
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
            dimmingOverlay.gameObject.SetActive(false);
            draggedItemUI.SetActive(false);
            inventorySlots.SetActive(false);
            craftingPanel.SetActive(false);
            menuButton.SetActive(false);
            HideInventoryItem();
            SetIsMouseOverHotbar(false);
        }
    }

    public void LeftClickMoveItem(int clickedSlotIndex)
    {
        if (isInventoryMode)
        {
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
                            draggedItemImage.sprite = draggedItem.SetSprite();
                            break;
                        }
                    }
                }
                else
                {
                    // Add Item to the clickedSlotIndex slot
                    inventory.AddItem(draggedItem, clickedSlotIndex);
                    if (clickedSlotIndex > 9)
                    {
                        inventory.ClearEquip();
                    }
                    Debug.Log("Put " + draggedItem.itemType + " in new slot");
                    draggedItem = null;
                    draggedItemUI.SetActive(false);
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
                            draggedItemUI.SetActive(true);
                            draggedItemImage.sprite = draggedItem.SetSprite();
                            break;
                        }
                    }
                }
            }
        }      
    }
}
