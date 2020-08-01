using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipmentSystem))]
public class CombatSystem : MonoBehaviour
{
    private EquipmentSystem equipment;

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

    public void Attack(DamageSystem enemy)
    {
        var weapon = equipment.CurrentWeapon;
        enemy.Damage(weapon.BaseDamage, equipment.CurrentWeapon.BaseDamageType);
    }
}
