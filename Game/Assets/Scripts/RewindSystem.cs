using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ArrayExtensions
{
    public static List<T> CopyList<T>(this List<T> list)
    {
        var newList = new List<T>();

        foreach (var element in list)
        {
            newList.Add(element);
        }

        return newList;
    }

    public static T[] CopyArray<T>(this T[] array)
    {
        var newArray = new T[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        return newArray;
    }
}

[Serializable]
public struct CharacterSnapshot
{
    public int Turn;
    public int Health;
    public int MaxHealth;
    public List<Item> Inventory;
    public Equipment[] Equipment;
    public int Damage;
    public int Armour;

    public CharacterSnapshot(int turn, EquipmentSystem equipment, LifeSystem lifeSystem, Inventory inventory)
    {
        Turn = turn;
        Health = lifeSystem.CurrentHealth;
        MaxHealth = lifeSystem.MaxHealth;
        Inventory = inventory.items.CopyList();
        Equipment = equipment.EquipmentSlots.CopyArray();
        Damage = equipment.WeaponDamage?.Sum(p => p.Value) ?? 0;
        Armour = equipment.ArmourResistances?.Sum(p => p.Value) ?? 0;
    }
}

[RequireComponent(typeof(EquipmentSystem))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(LifeSystem))]
public class RewindSystem : MonoBehaviour
{

    public int cooldown = 3;
    private int currentCooldown = 0;

    public bool Ready
    {
        get
        {
            return currentCooldown == 0;
        }
    }

    private EquipmentSystem equipment;
    private Inventory inventory;
    private LifeSystem lifeSystem;

    private TurnSystem turnSystem;
    private RewindUI rewindUI;

    private List<CharacterSnapshot> snapshots = new List<CharacterSnapshot>();

    void Awake()
    {
        equipment = GetComponent<EquipmentSystem>();
        inventory = GetComponent<Inventory>();
        lifeSystem = GetComponent<LifeSystem>();
        rewindUI = FindObjectOfType<RewindUI>();
        turnSystem = FindObjectOfType<TurnSystem>();

        turnSystem.PlayerTurnStarted += TakeSnapshot;
        turnSystem.PlayerTurnStarted += ReduceCooldown;

        currentCooldown = 4;
    }

    public void TakeSnapshot(int turn)
    {
        snapshots.Add(new CharacterSnapshot(turn, equipment, lifeSystem, inventory));
    }

    public void ReduceCooldown(int turn)
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;

            rewindUI.UpdateButton(currentCooldown);
        }
    }

    public bool Activate()
    {
        if (!Ready)
        {
            return false;
        }

        var snapshots = GetSnapshots(turnSystem.CurrentTurn);

        rewindUI.Open(snapshots);
        return true;
    }

    public IEnumerable<CharacterSnapshot> GetSnapshots(int currentTurn)
    {
        var availableSnapshots = AvaliableSnapshots(currentTurn).Select(p => snapshots.Single(s => s.Turn == p));

        return availableSnapshots;
    }

    private IEnumerable<int> AvaliableSnapshots(int currentTurn)
    {
        if (currentTurn - 3 > 0)
            yield return currentTurn - 3;
        if (currentTurn - 7 > 0)
            yield return currentTurn - 7;
        if (currentTurn - 11 > 0)
            yield return currentTurn - 11;
    }

    public bool RestoreSnapshot(int turn)
    {
        CharacterSnapshot? snapshot = snapshots.SingleOrDefault(p => p.Turn == turn);

        if (snapshot == null)
        {
            return false;
        }

        currentCooldown = cooldown;

        lifeSystem.SetHealth(snapshot.Value.Health);
        equipment.RestoreEquipment(snapshot.Value.Equipment);
        inventory.RestoreInventory(snapshot.Value.Inventory);

        return true;
    }
}
