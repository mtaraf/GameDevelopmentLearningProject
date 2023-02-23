using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public InventoryItemData data { get; private set; }
    public int quantity { get; private set; }

    public InventoryItem(InventoryItemData data)
    {
        this.data = data;
        quantity = 1;
    }

    public void AddToStack()
    {
        quantity++;
    }

    public void RemoveFromStack()
    {
        quantity--;
    }
}
