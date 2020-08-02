using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class EquipmentSystem : MonoBehaviour, IItemSystem
{
    [SerializeField]
    private new string name = "Equipment";
    public string Name { get { return name; } }
    
    public Equipment[] EquipmentSlots { get; private set; }

    public event Action ItemsChanged;

    public IEnumerable<DamageInfo> WeaponDamage
    {
        get
        {
            return (EquipmentSlots[(int)EquipmentSlot.Weapon] as Weapon)
                ?.Damage
                .GroupBy(p => p.DamageType)
                .Select(g => new DamageInfo { DamageType = g.Key, Value = g.Sum(p => p.Value) });
        }
    }

    public IEnumerable<DamageInfo> ArmourResistances
    {
        get
        {
            return EquipmentSlots
                .OfType<Armour>()
                .SelectMany(p => p.Resistances)
                .GroupBy(p => p.DamageType)
                .Select(g => new DamageInfo { DamageType = g.Key, Value = g.Sum(p => p.Value) });
        }
    }

    private Inventory inventory;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        EquipmentSlots = new Equipment[Enum.GetNames(typeof(EquipmentSlot)).Length];
    }

    public void Equip(Equipment equipment)
    {
        inventory.Remove(equipment);

        if (EquipmentSlots[(int)equipment.equipmentSlot] != null)
        {
            inventory.Add(EquipmentSlots[(int)equipment.equipmentSlot]);
        }

        EquipmentSlots[(int)equipment.equipmentSlot] = equipment;

        ItemsChanged?.Invoke();
    }

    public void Unequip(Equipment equipment)
    {
        if (inventory.Add(equipment))
        {
            EquipmentSlots[(int)equipment.equipmentSlot] = null;
        }

        ItemsChanged?.Invoke();

    }
}
