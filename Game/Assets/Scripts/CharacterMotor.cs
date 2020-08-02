﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterMotor : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    public bool isMoving;
    private bool isAttacking;

    public GameObject playerMarker;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        playerMarker.SetActive(false);
    }

    // Update is called once per frame
    public void setDestination(Vector3 pos)
    {
        agent.destination = pos;
    }

    void FixedUpdate(){
        var horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        isMoving = horizontalVelocity.magnitude > 0.5;
        Debug.Log(agent.velocity);
        
        
            if(isMoving){
                playerMarker.transform.position = agent.destination;
                playerMarker.SetActive(true);
            
                if((transform.position - agent.destination).magnitude > 6){
                    animator.SetBool("Running", true);
                    animator.SetBool("Moving", false);
                    agent.speed = 5;
                }
                else {
                    animator.SetBool("Moving",true);
                    animator.SetBool("Running", false);
                    agent.speed = 3;
                }   
            }
            else{
                    animator.SetBool("Moving", false);
                    animator.SetBool("Running", false);
                    
                playerMarker.SetActive(false);
            }
        
    }

    public void OnAttack(){
        animator.SetTrigger("Attack");
        isAttacking = true;
    }

    
}