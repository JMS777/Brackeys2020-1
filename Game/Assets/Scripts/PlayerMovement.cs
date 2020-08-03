using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;


[RequireComponent(typeof(CharacterMotor))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 mousePosition; 

    
    private CharacterMotor characterMotor;

    private BoxCollider collider;
    private Vector3 updatedPos;
    private bool isValid;
    private ParticleSystem particleSystem;
    
    public GameObject playerMarker;
    public GameObject tileChecker;
    public GameObject EnemyList;
    // Start is called before the first frame update
    private Action currentAction;

    enum Action{
        Move,
        Attack,
        Interact
    }



    void Start()
    {
        characterMotor = GetComponent<CharacterMotor>();
        playerMarker.SetActive(false);
        collider = tileChecker.GetComponent<BoxCollider>();
        particleSystem = playerMarker.GetComponentInChildren<ParticleSystem>();
        //tileChecker.SetActive(false);
    }
    

    void Update(){
        var main = particleSystem.main;
        // Gets list of current enemies
        CapsuleCollider[] list = EnemyList.gameObject.GetComponentsInChildren<CapsuleCollider>();
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if(!characterMotor.isMoving && Physics.Raycast(ray, out hitData, 1000)) {
            
            // Get tile position that cursor is over
            updatedPos = new Vector3(Mathf.Round(hitData.point.x/5)*5, 0.1f, Mathf.Round(hitData.point.z/5)*5);
            playerMarker.transform.position = updatedPos;
            playerMarker.SetActive(true);

            // Moves the tile checker to that tile
            tileChecker.transform.position = updatedPos;

            // Check if an enemy is on that tile
            foreach (CapsuleCollider enemy in list){
                if(collider.bounds.Intersects(enemy.bounds)){
                    currentAction = Action.Attack;
                    main.startColor = Color.red;                    
                }
                else{
                    currentAction = Action.Move;
                    main.startColor = Color.cyan;
                }
            }
            isValid = true;
        }
        else{
            playerMarker.SetActive(false);
        }
    }

    public void OnAction(InputAction.CallbackContext context){
        if(!characterMotor.isMoving){
            switch(currentAction){
                case Action.Attack:
                    // Call attack interaction
                    break;
                case Action.Move:
                    characterMotor.setDestination(updatedPos);
                    break;
                case Action.Interact:
                    break;
            }
        }
    }
}
