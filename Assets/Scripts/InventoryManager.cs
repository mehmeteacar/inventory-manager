using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<Item> items = new List<Item>();

    public delegate void InventoryEvent(Item item);
    public event InventoryEvent OnItemAdded;
    public event InventoryEvent OnItemRemoved;

    // Inventory Slots to map identifiers to slots (assumes these are BackpackSlot components)
    public BackpackSlot[] inventorySlots;
    
    private NetworkManager networkManager; // Reference to the NetworkManager

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        networkManager = FindObjectOfType<NetworkManager>(); // Find the NetworkManager in the scene
    }

    public void AddItem(Item item)
    {
        // Ensure the item identifier is within the range of available slots
        if (item.identifier >= 0 && item.identifier < inventorySlots.Length)
        {
            items.Add(item);
            inventorySlots[item.identifier].SetItem(item); // Assign item to the corresponding slot
            OnItemAdded?.Invoke(item);
            Debug.Log($"Item Added: {item.itemName}");

            // Send the item added request to the server
            StartCoroutine(networkManager.SendInventoryStatusRequest(item.identifier, "retrieve"));
        }
        else
        {
            Debug.LogWarning($"Item identifier {item.identifier} is out of range for inventory slots!");
        }
    }

    public void RemoveItem(Item item)
    {
        // Search for the item in the inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].HasItem() && inventorySlots[i].GetItem() == item)
            {
                items.Remove(item);
                inventorySlots[i].ClearSlot(); // Clear the item from the slot
                OnItemRemoved?.Invoke(item);
                Debug.Log($"Item Removed: {item.itemName}");

                // Send the item removed request to the server
                StartCoroutine(networkManager.SendInventoryStatusRequest(item.identifier, "fold"));

                if (item != null && item.itemObject != null)
                {
                    item.itemObject.transform.position = item.originalPosition;
                    item.itemObject.transform.localScale *= 5f; // Scale it by 5
                }
                return;
            }
        }

        Debug.LogWarning($"Item '{item.itemName}' is not in the inventory!");
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void EnableIcon(int itemId)
    {
        if (itemId >= 0 && itemId < inventorySlots.Length)
        {
            BackpackSlot slot = inventorySlots[itemId];
            if (slot.HasItem()) 
            {
                slot.EnableIcon(); // Enable the icon for the slot
            }
        }
    }

    public void DisableIcon(int itemId)
    {
        if (itemId >= 0 && itemId < inventorySlots.Length)
        {
            BackpackSlot slot = inventorySlots[itemId];
            if (slot.HasItem()) 
            {
                slot.DisableIcon(); // Disable the icon for the slot
            }
        }
    }
}
