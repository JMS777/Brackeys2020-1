using UnityEngine;

public interface IInteractable
{
    void Interact();

    Vector3 InteractionPoint { get; }
}