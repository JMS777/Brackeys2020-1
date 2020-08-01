using System.Collections;
using System.Collections.Generic;
using CoreSystems;
using UnityEngine;

public class InventorySystem : Singleton<InventorySystem>
{
    public InventoryUI playerInventoryUi;
    public InventoryUI otherInventoryUi;

    void Start()
    {
        var player = FindObjectOfType<PlayerController>();
        var playerInventory = player.GetComponent<Inventory>();
        SetPlayerInventory(playerInventory);
    }

    public void SetPlayerInventory(Inventory inventory)
    {
        playerInventoryUi.SetInventory(inventory);
    }

    public void ShowPlayerInventory()
    {
        playerInventoryUi.Open();
    }

    public void ShowOtherInventory(Inventory inventory)
    {
        otherInventoryUi.SetInventory(inventory);
        otherInventoryUi.Open();

        ShowPlayerInventory();
    }

    public void CloseInventory()
    {
        playerInventoryUi.Close();
        otherInventoryUi?.Close();
    }

    public void TransferToPlayer(Item item)
    {
        if (playerInventoryUi.Inventory.Add(item))
        {
            otherInventoryUi.Inventory.Remove(item);
        }
    }
}
