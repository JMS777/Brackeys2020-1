using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : Inventory, IInteractable
{
    public Transform interactionPoint;
    public Vector3 InteractionPoint { get { return interactionPoint.position; } }

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
