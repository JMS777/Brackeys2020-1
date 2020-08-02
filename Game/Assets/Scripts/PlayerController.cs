using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public ItemStore temp;
    public IInteractable NextInteraction { get; private set; }
    
    private CharacterMotor motor;

    public void SetInteraction(IInteractable interactableObject)
    {
        NextInteraction = interactableObject;

        motor.setDestination(NextInteraction.InteractionPoint);

        // Todo: Remove action points (or maybe keep that in the player movement script along with everything else).
    }

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SetInteraction(temp);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Player is colliding with the interaction point, so get the script from the parent object.
        var interactable = other.transform.parent.GetComponent<IInteractable>();
        if (interactable != null && interactable == NextInteraction)
        {
            interactable.Interact();
            NextInteraction = null;
        }
    }
}
