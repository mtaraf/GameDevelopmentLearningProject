using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> inventoryItemDictionary;
    public List<InventoryItem> inventoryItemsList;

    void Awake()
    {
        inventoryItemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        inventoryItemsList = new List<InventoryItem>();
    }

    public InventoryItem GetItem(InventoryItemData data)
    {
        if (inventoryItemDictionary.TryGetValue(data, out InventoryItem item))
        {
            return item;
        }
        return null;
    }

    public void AddItem(InventoryItemData itemData)
    {
        if (inventoryItemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            // if item is present, increment amount
            item.AddToStack();
        }
        else
        {
            //if item is not in inventory, add new item
            InventoryItem inventoryItem = new InventoryItem(itemData);
            inventoryItemsList.Add(inventoryItem);
            inventoryItemDictionary.Add(itemData, inventoryItem);
        }
    }

    public void RemoveItem(InventoryItemData data)
    {
        if (inventoryItemDictionary.TryGetValue(data, out InventoryItem item))
        {
            item.RemoveFromStack();
            
            if (item.quantity == 0)
            {
                inventoryItemsList.Remove(item);
                inventoryItemDictionary.Remove(data);
            }
        }
    }
}
