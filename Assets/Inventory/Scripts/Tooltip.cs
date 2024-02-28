using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject tooltipBox;
    private RectTransform tooltipBoxRender;
    public TextMeshProUGUI tooltipText;   
    public UI_Inventory ui_inventory;
    private Inventory inventory;
    

    private static Tooltip _instance; // Singleton pattern
    public static Tooltip Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Tooltip>();
                if (_instance == null)
                {
                    Debug.LogError("Tooltip instance not found in the scene.");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        gameObject.SetActive(false);
        tooltipBoxRender = GetComponent<RectTransform>();
    }


    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void DisplayTooltip(int slotIndex)
    {       
        if (inventory == null)
        {
            Debug.LogError("No inventory in tooltip");
        }

        if (inventory.slotIndexToItemType.ContainsKey(slotIndex))
        {   
            // Update tooltipBox position
            if (ui_inventory.isInventoryMode)
            {                
                tooltipBoxRender.anchoredPosition = new Vector2(-90f, -190f);
            } else if (!ui_inventory.isInventoryMode)
            {
                tooltipBoxRender.anchoredPosition = new Vector2(-90f, -50f);
            }

            tooltipBox.SetActive(true);

            Item.ItemType itemType = inventory.slotIndexToItemType[slotIndex];
            foreach (Item item in inventory.GetItemList())
            {
                if (item.itemType == itemType)
                {
                    tooltipText.SetText(item.itemInfo);

                    // Set tooltipBox size based on tooltipText preferred height
                    RectTransform tooltipBoxRect = tooltipBox.GetComponent<RectTransform>();
                    tooltipBoxRect.sizeDelta = new Vector2(tooltipBoxRect.sizeDelta.x, tooltipText.preferredHeight);
                    
                    break;
                }
            }
        }
    }

    public void HideTooltip()
    {
        tooltipBox.SetActive(false);
    }
}
