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
    // Start is called before the first frame update
    void Start()
    {
        characterMotor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context){
        //mousePosition = Mouse.current.position;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;

        if(Physics.Raycast(ray, out hitData, 1000)) {
            var updatedPos = new Vector3(Mathf.Round(hitData.point.x/5)*5, hitData.point.y, Mathf.Round(hitData.point.z/5)*5);
            characterMotor.setDestination(updatedPos);
            
        }
    }
}
