using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button discardButton;

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
        discardButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        discardButton.interactable = false;
    }

    public void ClickItem()
    {
        if (item != null)
        {
            if (inventoryUi.Inventory is ItemStore)
            {
                InventorySystem.Instance.TransferToPlayer(item);
            }
            else
            {
                item.Use();
            }
        }
    }

    public void OnDiscardItem()
    {
        inventoryUi.Inventory.Remove(item);
    }
}
