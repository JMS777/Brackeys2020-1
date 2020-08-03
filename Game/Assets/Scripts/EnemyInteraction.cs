using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LifeSystem), typeof (CombatSystem))]
public class EnemyInteraction : MonoBehaviour, IInteractable
{
    public Transform interactionPoint;
    public Vector3 InteractionPoint { get { return interactionPoint.position; } }
    private CombatSystem combatSystem;
    private LifeSystem lifeSystem;

    void Awake(){
        lifeSystem = GetComponent<LifeSystem>();
        combatSystem = GetComponent<CombatSystem>();
    }

    public void Interact(GameObject intiatingObject)
    {
        var intiatingLifeSystem = intiatingObject.GetComponent<LifeSystem>();
        var intiatingCombatSystem = intiatingObject.GetComponent<CombatSystem>();
        intiatingObject.GetComponent<CharacterMotor>().OnAttack();
        intiatingCombatSystem.Attack(lifeSystem);
        StartCoroutine(Retaliate(intiatingLifeSystem));

    }

    private IEnumerator Retaliate(LifeSystem targetLifeSystem){
        yield return new WaitForSeconds(2);
        combatSystem.Attack(targetLifeSystem);
    }

}
