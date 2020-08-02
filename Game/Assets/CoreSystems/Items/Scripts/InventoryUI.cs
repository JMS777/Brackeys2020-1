using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InventoryUI : ItemUIPanel<Inventory>
{
    private IList<InventorySlot> slots;

    void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }

    protected override void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < Context.items.Count)
            {
                slots[i].AddItem(Context.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
