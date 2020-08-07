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

    [Serializable]
    public class PhysicalSlot{
        public EquipmentSlot equipmentSlot;
        public Transform transform;
        public GameObject equippedItem;
    }

    public List<PhysicalSlot> PhysicalSlots;
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
            onUnequip(equipment);
            inventory.Add(EquipmentSlots[(int)equipment.equipmentSlot]);
        }
        onEquip(equipment);
        EquipmentSlots[(int)equipment.equipmentSlot] = equipment;
        Debug.Log("Equipment changed");
        ItemsChanged?.Invoke();
    }

    public void Unequip(Equipment equipment)
    {
        if (inventory.Add(equipment))
        {
            onUnequip(equipment);
            EquipmentSlots[(int)equipment.equipmentSlot] = null;
        }
        Debug.Log("Unequip: Equipment changed");

        ItemsChanged?.Invoke();

    }

    private void onEquip(Equipment equipment){
        var currentSlot = PhysicalSlots.SingleOrDefault(p => p.equipmentSlot == equipment.equipmentSlot);

        if (currentSlot != null)
        {
            currentSlot.equippedItem = Instantiate(equipment.gameObject, currentSlot.transform);
        }
    }

    private void onUnequip(Equipment equipment){
        var currentSlot = PhysicalSlots.SingleOrDefault(p => p.equipmentSlot == equipment.equipmentSlot);

        if (currentSlot != null)
        {
            var equippedItem = currentSlot.equippedItem;
            currentSlot.equippedItem = null;
            Destroy(equippedItem);
        }
    }
}
