using System.Collections;
using System.Collections.Generic;
using CoreSystems;
using UnityEngine;

public class ItemManagementUI : Singleton<ItemManagementUI>
{
    [SerializeField]
    public ItemUIPanel<Inventory> inventoryUi;
    public ItemUIPanel<Inventory> itemStoreUi;

    public ItemUIPanel<EquipmentSystem> equipmentUI;

    public RewindUI rewindUI;

    void Awake()
    {
        var player = FindObjectOfType<PlayerController>();

        inventoryUi.Context = player.GetComponent<Inventory>();
        inventoryUi.Context.SetPrimary();
        equipmentUI.Context = player.GetComponent<EquipmentSystem>();

        inventoryUi.PanelClosed += InventoryClosed;

        equipmentUI.PanelOpened += ShowInventory;
        itemStoreUi.PanelOpened += ShowInventory;
        itemStoreUi.PanelClosed += ItemStoreClosed;

        rewindUI.PanelOpened += RewindUIOpened;
    }

    public void ShowInventory()
    {
        inventoryUi.Open();
    }

    public void ShowItemStore(Inventory itemStore)
    {
        Debug.Log(itemStore.Name);
        itemStoreUi.Context = itemStore;
        itemStoreUi.Open();
    }

    public void ShowEquipment()
    {
        equipmentUI.Open();
    }

    public void ToggleInventory() => TogglePanel(inventoryUi);
    public void ToggleEquipment() => TogglePanel(equipmentUI);

    public void CloseItemStore()
    {
        itemStoreUi.Close();
    }

    private void InventoryClosed()
    {
        itemStoreUi.Close();
        equipmentUI.Close();
    }

    private void ItemStoreClosed()
    {
        itemStoreUi.Context?.CloseInventory();
    }

    private void TogglePanel<T>(IItemUIPanel<T> panel)
    {
        if (panel.IsOpen)
        {
            panel.Close();
        }
        else
        {
            panel.Open();
        }
    }

    public void TransferToInventory(Item item)
    {
        if (inventoryUi.Context.Add(item))
        {
            itemStoreUi.Context.Remove(item);
        }
    }

    private void RewindUIOpened()
    {
        inventoryUi.Close();
    }
}
