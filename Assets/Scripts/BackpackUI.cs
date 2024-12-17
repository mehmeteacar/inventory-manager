using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackpackUI : MonoBehaviour
{
    public GameObject backpackPanel; // The panel canvas object
    public Sprite xIconSprite;       // The "X" sprite for removal indication

    private bool isHovering = false; // Tracks if the mouse is over the backpack
    private bool isHolding = false;  // Tracks if LBM is being held
    private Image hoveredSlotImage;  // Tracks the current hovered slot image
    private Sprite originalSlotSprite; // To restore the original sprite after X icon

    private Item hoveredItem; // Tracks the item being hovered
    private bool isRemovingItem = false;

    void Start()
    {
        if (backpackPanel != null)
            backpackPanel.SetActive(false); // Ensure the panel is hidden initially
    }

    void Update()
    {
        // Handle showing backpack UI
        if (Input.GetMouseButton(0))
        {
            if (isHovering && !isHolding)
            {
                isHolding = true;
                ShowBackpackUI();
            }
            HandleSlotHover(); // Detect slot hover and change sprite
        }
        else if (isHolding)
        {
            isHolding = false;
            HandleSlotRelease(); // Check if LMB release should remove the item
            HideBackpackUI();
        }
    }

    void ShowBackpackUI()
    {
        if (backpackPanel != null)
            backpackPanel.SetActive(true);
    }

    void HideBackpackUI()
    {
        if (backpackPanel != null)
            backpackPanel.SetActive(false);

        RestoreSlotSprite(); // Ensure original sprite is restored
    }

    void HandleSlotHover()
    {
        // Use EventSystem to detect UI elements under the mouse
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        
        Debug.Log($"Raycast results count: {results.Count}");

        // Look for a slot with an Image component
        foreach (RaycastResult result in results)
        {
            BackpackSlot slot = result.gameObject.GetComponent<BackpackSlot>();
            if (slot != null && slot.HasItem())
            {
                if (hoveredSlotImage == null) // Save the original sprite
                {
                    // Directly access the first child's Image component
                    hoveredSlotImage = slot.transform.GetChild(0).GetComponent<Image>();

                    originalSlotSprite = hoveredSlotImage.sprite;
                    hoveredItem = slot.GetItem();

                    // Change to X icon
                    hoveredSlotImage.sprite = xIconSprite;
                    isRemovingItem = true;
                }
                return;
            }
        }

        // If not hovering over a valid slot, restore sprite
        if (hoveredSlotImage != null)
        {
            RestoreSlotSprite();
        }
    }


    void HandleSlotRelease()
    {
        if (isRemovingItem && hoveredItem != null)
        {
            TakeOutItem(hoveredItem);
        }
        RestoreSlotSprite();
    }

    void RestoreSlotSprite()
    {
        if (hoveredSlotImage != null)
        {
            hoveredSlotImage.sprite = originalSlotSprite;
            hoveredSlotImage = null;
            hoveredItem = null;
            isRemovingItem = false;
        }
    }

    void TakeOutItem(Item item)
    {
        // Logic to "take out" the item and return it to its original position
        InventoryManager.Instance.RemoveItem(item);

        Debug.Log($"Item '{item.itemName}' taken out of the backpack!");
    }

    // Mouse enters the backpack collider
    void OnMouseEnter()
    {
        isHovering = true;
    }

    // Mouse exits the backpack collider
    void OnMouseExit()
    {
        isHovering = false;
    }
}
