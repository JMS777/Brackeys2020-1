using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class EquipmentSystem : MonoBehaviour
{
    private Inventory inventory;
    private WeaponSystem weapons;
    
    public Armour Armour;

    void Awake()
    {
        weapons = GetComponent<WeaponSystem>();
        inventory = GetComponent<Inventory>();
    }
    
    public void EquipArmour(Armour armour)
    {
        inventory.Remove(armour);
        inventory.Add(Armour);

        Armour = armour;
    }

    public void EquipWeapon(Weapon weapon, int slot)
    {
        weapons.Equip(weapon, slot);
    }
}
