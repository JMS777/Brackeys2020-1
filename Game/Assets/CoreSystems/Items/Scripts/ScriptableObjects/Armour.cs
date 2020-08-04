using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Armour", menuName = "Inventory/Equipment/Armour")]
public class Armour : Equipment
{
    public List<DamageInfo> Resistances;
    public IEnumerable<StatusEffect> Effects;
}

[Serializable]
public struct DamageInfo
{
    public DamageType DamageType;
    public int Value;
}