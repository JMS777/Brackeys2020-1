using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeSystem), typeof(CombatSystem))]
public class AttackInteraction : Interactable
{
    private CombatSystem combatSystem;
    private LifeSystem lifeSystem;
    private CharacterMotor characterMotor;


    protected override void Awake()
    {
        base.Awake();
        lifeSystem = GetComponent<LifeSystem>();
        combatSystem = GetComponent<CombatSystem>();
        characterMotor = GetComponent<CharacterMotor>();
    }

    public override void Interact(GameObject initiatingObject)
    {
        Debug.Log(initiatingObject.name + " attacking " + gameObject.name);

        var intiatingLifeSystem = initiatingObject.GetComponent<LifeSystem>();
        var intiatingCombatSystem = initiatingObject.GetComponent<CombatSystem>();
        
        initiatingObject.GetComponent<CharacterMotor>().OnAttack();

        intiatingCombatSystem.Attack(lifeSystem);
        characterMotor.OnHit();
        
        StartCoroutine(Retaliate(intiatingLifeSystem, intiatingCombatSystem));
    }

    private IEnumerator Retaliate(LifeSystem targetLifeSystem, CombatSystem targetCombatSystem)
    {
        yield return new WaitForSeconds(2);

        if (!lifeSystem.IsDead)
        {
            transform.LookAt(targetLifeSystem.transform, Vector3.up);
            characterMotor.OnAttack();
            combatSystem.Attack(targetLifeSystem);
            targetLifeSystem.GetComponent<CharacterMotor>().OnHit();
            yield return new WaitForSeconds(2);

        }
        
        if (!targetLifeSystem.IsDead)
        {
            var targetDestination = GridHelper.GetNearestTile(transform.position + (targetLifeSystem.transform.position - transform.position).normalized * 5);
            targetLifeSystem.GetComponent<CharacterMotor>().setDestination(targetDestination);
        }

        targetCombatSystem.OnConbatFinished();
    }

}
