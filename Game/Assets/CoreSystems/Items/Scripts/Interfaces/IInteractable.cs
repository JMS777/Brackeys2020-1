using UnityEngine;

public interface IInteractable
{
    void Interact();

    Transform InteractionTransform { get; }
}