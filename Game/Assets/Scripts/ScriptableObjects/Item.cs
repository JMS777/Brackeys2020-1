using System;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Icon;

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
}