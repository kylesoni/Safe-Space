using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public float itemSlotCellSize = 30f;
    private Player player;


    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
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

        int x = 0;
        int y = 0;
        // float itemSlotCellSize = 5f;
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            // Use and Drop Item with Left and Right Click
            itemSlotRectTransform.GetComponentInChildren<Button_UI>().ClickFunc = () =>
            {
                // Use Item
                // TODO
            };
            itemSlotRectTransform.GetComponentInChildren<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop Item
                inventory.RemoveItem(item);
                ItemWorld.DropItem(player.transform.position, item);
            };


            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize); 
            
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

            x++;
            if (x >= 4)
            {
                x = 0;
                y--;
            }

        }
    }
}
