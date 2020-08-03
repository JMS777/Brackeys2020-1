using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 mousePosition; 

    
    private CharacterMotor characterMotor;

    private new BoxCollider collider;
    private Vector3 updatedPos;
    private bool isValid;
    private new ParticleSystem particleSystem;
    
    public GameObject playerMarker;
    public GameObject tileChecker;
    //public GameObject EnemyList;
    // Start is called before the first frame update
    private Action currentAction;
    private IEnumerable<IInteractable> interactableObjects;
    private IInteractable currentTarget;
    private PlayerController playerController;

    enum Action{
        Move,
        Interact
    }



    void Start()
    {
        characterMotor = GetComponent<CharacterMotor>();
        playerMarker.SetActive(false);
        collider = tileChecker.GetComponent<BoxCollider>();
        particleSystem = playerMarker.GetComponentInChildren<ParticleSystem>();
        interactableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();
        playerController = GetComponent<PlayerController>();
        //tileChecker.SetActive(false);
    }
    

    void Update(){
        var main = particleSystem.main;
        // Gets list of current enemies
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
            foreach (IInteractable interactable in interactableObjects){
                if(collider.bounds.Intersects(interactable.gameObject.GetComponent<Collider>().bounds)){
                    main.startColor = Color.red;   
                    currentAction = Action.Interact;           
                    currentTarget = interactable;      
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
        if(context.performed){
            if(!characterMotor.isMoving){
                switch(currentAction){
                    case Action.Move:
                        characterMotor.setDestination(updatedPos);
                        break;
                    case Action.Interact:
                        playerController.SetInteraction(currentTarget);
                        //characterMotor.setDestination(currentTarget.InteractionPoint);
                        break;
                }
            }
        }
    }
}
