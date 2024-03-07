using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class CraftingPanel : MonoBehaviour
{
    public Button CraftButton;
    public TextMeshProUGUI ItemNameText;
    public Image ItemImage;
    public GameObject GridLayout;

    private Crafting craftingSystem;

    public Item.ItemType selectedItemType;

    public GameObject ItemAmountIndicatorPrefab;

    void Start()
    {
        craftingSystem = FindObjectOfType<Crafting>();

        CraftButton.onClick.AddListener(CraftSelectedItem);
        UpdateRecipeDisplay(selectedItemType);
    }

    private void Update()
    {
        CraftButton.interactable = craftingSystem.CanCraft(selectedItemType);
        UpdateRecipeDisplay(selectedItemType);
    }

    public void UpdateRecipeDisplay(Item.ItemType itemType)
    {
        foreach (Transform child in GridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        selectedItemType = itemType;
        if (craftingSystem.CraftingRecipes.TryGetValue(itemType, out var recipe))
        {
            ItemNameText.text = $"{itemType}:";

            ItemImage.sprite = Item.GetSprite( itemType );

            foreach (var item in recipe)
            {
                GameObject indicator = Instantiate(ItemAmountIndicatorPrefab, GridLayout.transform);
                indicator.GetComponent<ItemAmountIndicator>().SetItemType(item.Key, item.Value);
            }

            CraftButton.interactable = craftingSystem.CanCraft(itemType);
        }
        else
        {
            ItemNameText.text = "Unavailable";
            ItemImage.sprite = null;
        }
    }

    void CraftSelectedItem()
    {
        craftingSystem.CraftItem(selectedItemType);

        UpdateRecipeDisplay(selectedItemType);
    }

    public void ShowCraftingForItem(Item.ItemType itemType)
    {
        gameObject.SetActive(true);
        UpdateRecipeDisplay(itemType);
    }
}
