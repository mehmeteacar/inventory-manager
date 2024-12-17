using UnityEngine;
using UnityEngine.UI;

public class BackpackSlot : MonoBehaviour
{
    private Item storedItem; // The item in this slot

    public bool HasItem()
    {
        return storedItem != null;
    }

    public Item GetItem()
    {
        return storedItem;
    }

    public void SetItem(Item item)
    {
        storedItem = item;
        UpdateSlotVisual();
    }

    public void ClearSlot()
    {
        storedItem = null;
        UpdateSlotVisual();
    }

    public void EnableIcon()
    {
        Transform icon = transform.GetChild(0); // Get the first child (the icon)
        if (icon != null && storedItem != null)
        {
            icon.gameObject.SetActive(true); // Ensure the icon is active
        }
    }

    public void DisableIcon()
    {
        Transform icon = transform.GetChild(0); // Get the first child (the icon)
        if (icon != null)
        {
            icon.gameObject.SetActive(false); // Deactivate the icon
        }
    }

    private void UpdateSlotVisual()
    {
        Transform icon = transform.GetChild(0); // Get the first child (the icon)
        if (icon != null)
        {
            Image iconImage = icon.GetComponent<Image>();
            iconImage.sprite = storedItem != null ? storedItem.icon : null;
            icon.gameObject.SetActive(storedItem != null); // Show or hide the icon based on whether the item exists
        }
    }
}