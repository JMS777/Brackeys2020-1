using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LifeSystem))]
[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour
{
    public ItemStore temp;
    public IInteractable NextInteraction { get; private set; }
    private IInteractable lastInteraction;

    private LifeSystem lifeSystem;
    private CharacterMotor motor;
    private PlayerMovement playerInput;

    public void SetInteraction(IInteractable interactableObject)
    {
        NextInteraction = interactableObject;

        motor.setDestination(NextInteraction.GetInteractionPoint(transform));

        // Todo: Remove action points (or maybe keep that in the player movement script along with everything else).
    }

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        playerInput = GetComponent<PlayerMovement>();
        lifeSystem = GetComponent<LifeSystem>();

        lifeSystem.PlayerDied += OnPlayerDied;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            var interactable = other.transform.parent.GetComponent<IInteractable>();
            if (interactable != null && interactable == NextInteraction)
            {
                interactable.Interact(gameObject);


                StartCoroutine(LookAtInteraction(other.transform.parent));
                lastInteraction = NextInteraction;
                NextInteraction = null;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            var interactable = other.transform.parent.GetComponent<IInteractable>();
            if (interactable != null && interactable == lastInteraction && lastInteraction is ItemStore)
            {
                ((ItemStore)lastInteraction).Close();
            }
        }
    }

    IEnumerator LookAtInteraction(Transform interaction)
    {
        yield return new WaitForSeconds(1);

        transform.LookAt(interaction, Vector3.up);
    }

    private void OnPlayerDied()
    {
        playerInput.enabled = false;
    }
}
