using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private IList<InventorySlot> slots;
    public TMP_Text title;
    
    public Inventory Inventory { get; private set; }

    void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }

    void Start()
    {
        UpdateUI();
    }

    public void SetInventory(Inventory inventory)
    {
        if (Inventory != null)
        {
            Inventory.ItemsChanged -= UpdateUI;
        }

        Inventory = inventory;
        Inventory.ItemsChanged += UpdateUI;
        
        title.text = inventory.Name;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < Inventory.items.Count)
            {
                slots[i].AddItem(Inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void OnCloseInventory()
    {
        InventorySystem.Instance.CloseInventory();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
