using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    public EquipmentSlot equipmentSlot;

    public Image icon;
    public Image inactiveIcon;
    public Button slotButton;

    private Equipment item;

    private EquipmentUI equipmentUI;

    void Awake()
    {
        equipmentUI = GetComponentInParent<EquipmentUI>();
    }

    public void AddItem(Equipment item)
    {
        this.item = item;

        icon.sprite = item.Icon;
        icon.enabled = true;
        inactiveIcon.enabled = false;
        slotButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        inactiveIcon.enabled = true;

        slotButton.interactable = false;
    }

    public void ClickItem()
    {
        if (item != null)
        {
            equipmentUI.Context.Unequip(item);
        }
    }
}
