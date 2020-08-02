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
    private Vector3 updatedPos;
    private bool isValid;

    public GameObject playerMarker;
    // Start is called before the first frame update
    void Start()
    {
        characterMotor = GetComponent<CharacterMotor>();
        playerMarker.SetActive(false);
    }
    

    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if(!characterMotor.isMoving && Physics.Raycast(ray, out hitData, 1000)) {
            updatedPos = new Vector3(Mathf.Round(hitData.point.x/5)*5, hitData.point.y, Mathf.Round(hitData.point.z/5)*5);
            playerMarker.transform.position = updatedPos;
            playerMarker.SetActive(true);
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
