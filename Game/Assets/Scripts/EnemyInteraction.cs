﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LifeSystem), typeof (CombatSystem))]
public class EnemyInteraction : MonoBehaviour, IInteractable
{
    public Transform interactionPoint;
    public Vector3 InteractionPoint { get { return interactionPoint.position; } }
    private CombatSystem combatSystem;
    private LifeSystem lifeSystem;
    private CharacterMotor characterMotor;

    void Awake(){
        lifeSystem = GetComponent<LifeSystem>();
        combatSystem = GetComponent<CombatSystem>();
        characterMotor = GetComponent<CharacterMotor>();
    }

    public void Interact(GameObject intiatingObject)
    {
        Debug.Log("Attacking Enemy");
        var intiatingLifeSystem = intiatingObject.GetComponent<LifeSystem>();
        var intiatingCombatSystem = intiatingObject.GetComponent<CombatSystem>();
        intiatingObject.GetComponent<CharacterMotor>().OnAttack();
        intiatingCombatSystem.Attack(lifeSystem);
        characterMotor.OnHit();
        StartCoroutine(Retaliate(intiatingLifeSystem));

    }

    private IEnumerator Retaliate(LifeSystem targetLifeSystem){
        
        yield return new WaitForSeconds(2);
        transform.LookAt(targetLifeSystem.transform,Vector3.up);
        characterMotor.OnAttack();
        combatSystem.Attack(targetLifeSystem);
        targetLifeSystem.GetComponent<CharacterMotor>().OnHit();
    }

}