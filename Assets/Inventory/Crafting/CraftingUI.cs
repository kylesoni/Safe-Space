using UnityEngine;
using UnityEngine.UI; // Required for UI components like Text and Button
using System.Linq; // Used for LINQ queries

public class CraftingUI : MonoBehaviour
{
    public Crafting craftingSystem; // Reference to the Crafting system
    public Text recipeDisplayText; // UI Text to display recipes
    public Button craftButton; // Button to initiate crafting
    private Item.ItemType selectedItemType; // Currently selected item type for crafting

    void Start()
    {
        // Initially hide the crafting UI or select an item type to display
        gameObject.SetActive(true); // or UpdateRecipeDisplay(Item.ItemType.Sword);

        // Add a listener to the craft button onClick event
        craftButton.onClick.AddListener(CraftSelectedItem);
    }

    void Update()
    {
        UpdateRecipeDisplay(Item.ItemType.Sword);
    }

    public void UpdateRecipeDisplay(Item.ItemType itemType)
    {
        selectedItemType = itemType;
        if (craftingSystem.CraftingRecipes.TryGetValue(itemType, out var recipe))
        {
            // Build the recipe text
            string recipeText = $"{itemType}:\n";
            recipeText += string.Join("\n", recipe.Select(r => $"{r.Key}: {r.Value}"));

            // Update the UI
            recipeDisplayText.text = recipeText;

            // Check if the item can be crafted and enable/disable the craft button accordingly
            craftButton.interactable = craftingSystem.CanCraft(itemType);
        }
        else
        {
            recipeDisplayText.text = "No recipe available.";
        }
    }

    void CraftSelectedItem()
    {
        craftingSystem.CraftItem(selectedItemType);
        // Optionally, update inventory display or provide feedback to the player
    }

    // This method can be called to show the UI for a specific item's recipe
    public void ShowCraftingForItem(Item.ItemType itemType)
    {
        gameObject.SetActive(true);
        UpdateRecipeDisplay(itemType);
    }
}
