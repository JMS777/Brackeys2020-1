using UnityEngine;

public interface IInteractable
{
    void Interact();

    GameObject gameObject { get; }

    Vector3 InteractionPoint { get; }
}