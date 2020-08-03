using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : Inventory, IInteractable
{
    public Transform interactionPoint;
    public Vector3 InteractionPoint { get { return interactionPoint.position; } }

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
        => DisplayStore();

    private void DisplayStore()
    {
        ItemManagementUI.Instance.ShowItemStore(this);
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        ItemManagementUI.Instance.CloseItemStore();
        CloseAnim();
    }

    public void CloseAnim()
    {
        animator.SetTrigger("Close");
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
