using UnityEngine;

[CreateAssetMenu(fileName="New Potion", menuName = "Inventory/Potion")]
public class Potion : Item
{
    public PotionEffect Effect;
    public int Value;
}