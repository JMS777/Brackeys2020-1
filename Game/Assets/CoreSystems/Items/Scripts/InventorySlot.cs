using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button discardButton;
    public Button slotButton;

    private InventoryUI inventoryUi;

    private Item item;

    void Awake()
    {
        inventoryUi = GetComponentInParent<InventoryUI>();
    }

    public void AddItem(Item item)
    {
        this.item = item;

        icon.sprite = item.Icon;
        icon.enabled = true;
        slotButton.interactable = true;

        if (!(inventoryUi.Context is ItemStore))
        {
            discardButton.interactable = true;
        }
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        slotButton.interactable = false;

        if (!(inventoryUi.Context is ItemStore))
        {
            discardButton.interactable = false;
        }
    }

    public void ClickItem()
    {
        if (item != null)
        {
            if (inventoryUi.Context is ItemStore)
            {
                ItemManagementUI.Instance.TransferToInventory(item);
            }
            else
            {
                item.Use(inventoryUi.Context.gameObject);
            }
        }
    }

    public void OnDiscardItem()
    {
        inventoryUi.Context.Remove(item);
    }
}
