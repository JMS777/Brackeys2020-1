using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ItemManagementUI.Instance.ToggleEquipment();
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ItemManagementUI.Instance.ToggleInventory();
        }
    }
}
