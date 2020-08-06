using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : Interactable
{
    private Animator animator;
    private NavMeshObstacle obstacle; 


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        obstacle = GetComponentInChildren<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject intiatingObject)
    {
        animator.SetTrigger("Open");
        obstacle.enabled = false;
        GetComponentInParent<NavMeshObstacle>().enabled = false;
        var targetDestination = transform.position + (intiatingObject.transform.position - transform.position).normalized * -2.5f;
        var updatedPos = new Vector3(Mathf.Round(targetDestination.x / 5) * 5, 0.1f, Mathf.Round(targetDestination.z / 5) * 5);
        intiatingObject.GetComponent<CharacterMotor>().setDestination(updatedPos);
    }
}
