using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : Inventory, IInteractable
{
    public Transform InteractionTransform { get; private set; }

    public void Interact()
        => DisplayStore();

    private void DisplayStore()
    {
        ItemManagementUI.Instance.ShowItemStore(this);
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
