using UnityEngine;

[CreateAssetMenu(fileName="New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    public int BaseDamage;
    public DamageType BaseDamageType;
}