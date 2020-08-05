using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class ItemStore : Interactable
{

    private Inventory inventory;

    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();

        inventory.InventoryClosed += CloseAnim;
    }

    public override void Interact(GameObject intiatingObject)
        => DisplayStore();

    private void DisplayStore()
    {
        ItemManagementUI.Instance.ShowItemStore(inventory);
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
        return inventory.Remove(item);
    }

    public bool PlaceItem(Item item)
    {
        return inventory.Add(item);
    }
}
