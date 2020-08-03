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
            switch(hitData.transform.gameObject.tag){
            case "Enemy":
            //Debug.Log("Enemy");
                main.startColor = Color.red;
                break;
            case "Chest":
                break;
            default:
            //Debug.Log("Tile");
                main.startColor = Color.cyan;
                tileChecker.transform.position = updatedPos;
                foreach (CapsuleCollider enemy in list){
                    if(collider.bounds.Intersects(enemy.bounds)){
                        Debug.Log("Enemy on tile");
                        main.startColor = Color.red;
                    }
                }
                updatedPos = new Vector3(Mathf.Round(hitData.point.x/5)*5, hitData.point.y, Mathf.Round(hitData.point.z/5)*5);
                playerMarker.transform.position = updatedPos;
                playerMarker.SetActive(true);
                break;

        }
            
            isValid = true;
        }
        else{
            playerMarker.SetActive(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context){
        if(!characterMotor.isMoving){
            //mousePosition = Mouse.current.position;
            if(isValid){
                characterMotor.setDestination(updatedPos);
            }
        }
    }
}
