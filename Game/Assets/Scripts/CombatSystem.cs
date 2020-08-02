using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipmentSystem))]
public class CombatSystem : MonoBehaviour
{
    private EquipmentSystem equipment;

    public Weapon Unarmed;

    void Awake()
    {
        equipment = GetComponent<EquipmentSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(LifeSystem enemy)
    {
        var weaponDamage = equipment.WeaponDamage ?? Unarmed.Damage.ToArray();

        foreach (var damage in weaponDamage)
        {
            enemy.Damage(damage);
        }
    }
}
