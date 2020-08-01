using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class EquipmentSystem : MonoBehaviour
{
    public event Action WeaponsChanged;

    public int CurrentSlot { get; private set; } = 0;
    public Weapon[] WeaponSlots;
    public Armour Armour;
    public Weapon Unarmed;

    public Weapon CurrentWeapon
    {
        get
        {
            if (WeaponSlots[CurrentSlot] != null)
            {
                return WeaponSlots[CurrentSlot];
            }
            else
            {
                return Unarmed;
            }
        }
    }

    private Inventory inventory;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
    
    public void EquipArmour(Armour armour)
    {
        inventory.Remove(armour);
        inventory.Add(Armour);

        Armour = armour;
    }

    public void SwitchWeapon(int slot)
    {
        CurrentSlot = slot;
        WeaponsChanged?.Invoke();
    }

    public void EquipWeapon(Weapon weapon, int slot)
    {
        if (slot > WeaponSlots.Length - 1)
        {
            Debug.LogWarning("Weapon slot out of range.");
        }

        inventory.Remove(weapon);

        if (WeaponSlots[slot] != null)
        {
            inventory.Add(WeaponSlots[slot]);
        }
        
        WeaponSlots[slot] = weapon;

        WeaponsChanged?.Invoke();
    }
}
