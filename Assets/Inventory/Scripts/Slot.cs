using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string slotName;
    private int slotIndex; // don't set to static

    private void Start()
    {
        // set slotIndex
        slotName = gameObject.name;
        string indexString = slotName.Substring(slotName.LastIndexOf('_') + 1);
        if (int.TryParse(indexString, out int parsedIndex))
        {
            slotIndex = parsedIndex;
        }        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.DisplayTooltip(slotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.HideTooltip();
    }
}
