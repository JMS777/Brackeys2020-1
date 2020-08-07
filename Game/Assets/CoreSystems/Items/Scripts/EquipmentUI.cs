using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreSystems;
using UnityEngine;

public class EquipmentUI : ItemUIPanel<EquipmentSystem>
{
    private IEnumerable<EquipmentSlotUI> slots;

    void Awake()
    {
        slots = GetComponentsInChildren<EquipmentSlotUI>();
    }

    protected override void OnContextChanged()
    {
        Context.ItemsChanged += Open;
    }

    protected override void UpdateUI()
    {
        // Open();

        if (slots == null)
        {
            return;
        }

        foreach (var slot in slots)
        {
            if (Context.EquipmentSlots[(int)slot.equipmentSlot] != null)
            {
                slot.AddItem(Context.EquipmentSlots[(int)slot.equipmentSlot]);
            }
            else
            {
                slot.ClearSlot();
            }
        }
    }
}
