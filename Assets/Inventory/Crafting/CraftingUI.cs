using UnityEngine;
using UnityEngine.UI; // Required for UI components like Text and Button
using System.Linq; // Used for LINQ queries

public class CraftingUI : MonoBehaviour
{
    public Crafting craftingSystem; // Reference to the Crafting system
    public Text recipeDisplayText; // UI Text to display recipes
    public Button craftButton; // Button to initiate crafting

    public GameObject craftingRecipeUIPrefab;

    void Start()
    {
        // Initially hide the crafting UI or select an item type to display
        // gameObject.SetActive(true); // or UpdateRecipeDisplay(Item.ItemType.Sword);

        // Add a listener to the craft button onClick event
        // craftButton.onClick.AddListener(CraftSelectedItem);

        // Ensure you have a reference to your scene's Canvas.
        //GameObject canvas = GameObject.Find("Canvas"); // Make sure the Canvas is named "Canvas" or adjust as needed.
        // Select canvas as parent
        //GameObject canvas = GetComponentInParent<Canvas>().gameObject;

        // Instantiate the Panel prefab as a child of the Canvas.
        //GameObject panelInstance = Instantiate(craftingRecipeUIPrefab, canvas.transform);
        //panelInstance.GetComponent<CraftingPanel>().UpdateRecipeDisplay(Item.ItemType.Sword);
        //panelInstance.transform.SetParent(this.transform, false);

        // Optionally, adjust the instantiated Panel's RectTransform or other properties.
    }

    void Update()
    {
        // UpdateRecipeDisplay(Item.ItemType.Sword);
    }
}
