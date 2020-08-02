using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    public GameObject cinemachine;

    private CinemachineFreeLook freeLook;
    // Start is called before the first frame update
    void Start()
    {
        freeLook = cinemachine.GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    public void Look(InputAction.CallbackContext context){
        if(Mouse.current.rightButton.isPressed){
            freeLook.m_XAxis.m_InputAxisValue = context.ReadValue<Vector2>().x;
            freeLook.m_YAxis.m_InputAxisValue = context.ReadValue<Vector2>().y;
        }
        else{
            freeLook.m_XAxis.m_InputAxisValue = 0;
            freeLook.m_YAxis.m_InputAxisValue = 0;
        }
    }
}
