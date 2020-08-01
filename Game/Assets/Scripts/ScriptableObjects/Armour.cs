using System;
using UnityEngine;

[CreateAssetMenu(fileName="New Armour", menuName = "Inventory/Armour")]
public class Armour : Item
{
    public ArmourRating[] Ratings;

    [Serializable]
    public struct ArmourRating
    {
        public DamageType Type;
        public int Value;
    }
}