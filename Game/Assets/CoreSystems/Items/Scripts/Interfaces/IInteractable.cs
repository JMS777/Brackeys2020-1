using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject intiatingObject);

    GameObject gameObject { get; }

    // Vector3 InteractionPoint { get; }
    Vector3 GetInteractionPoint(Transform interactingObject);
}