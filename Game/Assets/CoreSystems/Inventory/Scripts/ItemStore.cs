using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : Inventory, IInteractable
{
    public void Interact()
        => DisplayStore();

    private void DisplayStore()
    {
        InventorySystem.Instance.ShowOtherInventory(this);
    }
    
    public bool TakeItem(Item item)
    {
        return Remove(item);
    }

    public bool PlaceItem(Item item)
    {
        return Add(item);
    }
}
