using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EquipmentSystem))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(DamageSystem))]
public class RewindSystem : MonoBehaviour
{
    public struct CharacterSnapshot
    {
        public int Turn;
        public int Health;
        public List<Item> Inventory;
        public Armour Armour;
        public Weapon[] Weapons;

        public CharacterSnapshot(int turn, EquipmentSystem equipment, DamageSystem damageSystem, Inventory inventory)
        {
            Turn = turn;
            Health = damageSystem.CurrentHealth;
            Inventory = inventory.items;
            Armour = equipment.Armour;
            Weapons = equipment.WeaponSlots;
        }
    }

    private EquipmentSystem equipment;
    private Inventory inventory;
    private DamageSystem damageSystem;


    private IList<CharacterSnapshot> snapshots = new List<CharacterSnapshot>();

    void Awake()
    {
        equipment = GetComponent<EquipmentSystem>();
        inventory = GetComponent<Inventory>();
        damageSystem = GetComponent<DamageSystem>();
    }

    public void TakeSnapshot(int turn)
    {
        snapshots.Add(new CharacterSnapshot(turn, equipment, damageSystem, inventory));
    }

    public bool RestoreSnapshot(int turn)
    {
        CharacterSnapshot? snapshot = snapshots.SingleOrDefault(p => p.Turn == turn);

        if (snapshot == null)
        {
            return false;
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
