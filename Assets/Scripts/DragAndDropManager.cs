using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDragging = false;
    private bool isRaised = false;

    // Reference to the backpack slot where the object will be placed
    public Transform backpackSlot;
    public Transform backpack;

    // Adjustable raise distance during drag
    public float raiseHeight = 5f;

    private Rigidbody rb;
    private Collider itemCollider;
    private Collider backpackCollider;
    
    private Item item;
    
    private Vector3 originalScale;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        item = GetComponent<Item>();
        itemCollider = GetComponent<Collider>();
        backpackCollider = backpack.GetComponent<Collider>();
        
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Calculate the offset from the object to the mouse position
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;

        // Make the object kinematic when drag starts
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Raise the object when dragging starts, only once
        if (!isRaised)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + raiseHeight, transform.position.z);
            isRaised = true; // Ensure it's raised only once
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Get the mouse position in world space
            Vector3 newMousePosition = GetMouseWorldPos() + offset;

            // Move the object in X and Z only (Y is fixed after the initial raise)
            transform.position = new Vector3(newMousePosition.x, transform.position.y, newMousePosition.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;

            // Check if the object is dropped inside the backpack
            if (IsDroppedOnBackpack())
            {
                // Place the object in the backpack slot
                transform.position = backpackSlot.position;
                
                transform.localScale = originalScale * 0.2f;

                InventoryManager.Instance.AddItem(item);

                // Disable physics while the object is in the backpack
                if (rb != null)
                {
                    rb.isKinematic = true; // Make object unaffected by physics
                }
            }
            else
            {
                // Allow the object to fall to the ground by enabling physics
                if (rb != null)
                {
                    rb.isKinematic = false; // Reactivate physics after dropping outside the backpack
                }
                
                transform.localScale = originalScale;
            }

            // Reset isRaised flag to allow the object to be raised again in the next drag
            isRaised = false;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convert mouse position from screen space to world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;  // Use object's z-position
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private bool IsDroppedOnBackpack()
    {
        // Check if the object is inside the backpack's collider (can be expanded for more precise checks)
        if (backpackCollider != null && backpackCollider.bounds.Contains(transform.position))
        {
            return true;
        }
        return false;
    }
}
