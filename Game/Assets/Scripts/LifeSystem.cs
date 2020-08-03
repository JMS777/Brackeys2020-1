using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EquipmentSystem))]
public class LifeSystem : MonoBehaviour
{
    public event Action<int> PlayerDamaged;
    public event Action PlayerDied;


    [Range(0, 1)]
    [Tooltip("The chance of armour reducing amount of damage taken. (Set to 1 to always remove armour rating from damage value)")]
    public float armourEffectiveness = 0.8f;

    [Range(0,1)]
    [Tooltip("The chance armour will completely block an attack.")]
    public float armourBlockChance = 0.1f;

    public int MaxHealth = 10;
    public int CurrentHealth { get; private set; }

    private EquipmentSystem equipment;
    
    void Awake()
    {
        equipment = GetComponent<EquipmentSystem>();
        CurrentHealth = MaxHealth;
    }

    public void SetHealth(int hp)
    {
        CurrentHealth = Mathf.Clamp(hp, 0, MaxHealth);
    }

    public void Damage(DamageInfo damage)
    {
        if (damage.Value < 0)
        {
            Debug.LogWarning("Damange less than 0.");
            return;
        }

        var value = ApplyArmourRating(damage.Value, damage.DamageType);

        CurrentHealth -= value;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        PlayerDamaged?.Invoke(CurrentHealth);

        if (CurrentHealth == 0)
        {
            PlayerDied?.Invoke();
        }
    }

    private int ApplyArmourRating(int damage, DamageType type)
    {
        var armourRatings = equipment.ArmourResistances;
        DamageInfo? relevantRating = armourRatings.FirstOrDefault(p => p.DamageType == type);

        if (relevantRating.HasValue)
        {
            var armourEffective = UnityEngine.Random.Range(0, 1) < armourEffectiveness;
            var armourBlocked = UnityEngine.Random.Range(0, 1) < armourBlockChance;

            if (armourBlocked)
            {
                damage = 0;
            }
            else if (armourEffective)
            {
                damage = Mathf.Max(0, damage - relevantRating.Value.Value);
            }
        }

        return damage;
    }
}
