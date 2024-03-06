using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    public Button CraftButton;
    public TextMeshProUGUI ItemNameText;
    public Image ItemImage;
    public GameObject GridLayout;

    private Crafting craftingSystem; // Reference to the Crafting system

    public Item.ItemType selectedItemType; // Currently selected item type for crafting

    public GameObject ItemAmountIndicatorPrefab;

    void Start()
    {
        craftingSystem = FindObjectOfType<Crafting>();
        // Bind the Craft button to the crafting method
        CraftButton.onClick.AddListener(CraftSelectedItem);
    }

    private void Update()
    {
        CraftButton.interactable = craftingSystem.CanCraft(selectedItemType);
        print(craftingSystem.CanCraft(selectedItemType));
        UpdateRecipeDisplay(selectedItemType);
    }

    public void UpdateRecipeDisplay(Item.ItemType itemType)
    {
        // Clear existing indicators
        foreach (Transform child in GridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        selectedItemType = itemType;
        if (craftingSystem.CraftingRecipes.TryGetValue(itemType, out var recipe))
        {
            ItemNameText.text = $"{itemType}:";

            // Assuming ItemAssets.Instance provides direct access to sprites based on ItemType
            // Set the main item image
            Sprite itemSprite = ItemAssets.Instance.swordSprite; // Simplified approach
            if (itemSprite != null)
            {
                ItemImage.sprite = itemSprite;
            }

            // Instantiate an ItemAmountIndicatorPrefab for each item in the recipe
            foreach (var item in recipe)
            {
                GameObject indicator = Instantiate(ItemAmountIndicatorPrefab, GridLayout.transform);
                // Assuming your prefab has a script attached that contains the SetItemType method
                indicator.GetComponent<ItemAmountIndicator>().SetItemType(item.Key, item.Value); // Replace YourIndicatorComponent with the actual component name

                // Optionally, set the amount or other properties of the indicator here
            }

            CraftButton.interactable = craftingSystem.CanCraft(itemType);
        }
        else
        {
            ItemNameText.text = "Unavailable";
            ItemImage.sprite = null; // Clear the item image
        }
    }

    void CraftSelectedItem()
    {
        craftingSystem.CraftItem(selectedItemType);
        // Update the recipe display to reflect any changes in inventory
        UpdateRecipeDisplay(selectedItemType);
    }

    public void ShowCraftingForItem(Item.ItemType itemType)
    {
        gameObject.SetActive(true);
        UpdateRecipeDisplay(itemType);
    }
}
