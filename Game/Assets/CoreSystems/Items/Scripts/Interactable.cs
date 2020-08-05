using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    private IEnumerable<Transform> interactionPoints;
    // public Vector3 InteractionPoint { get { return interactionPoint.position; } }

    public abstract void Interact(GameObject intiatingObject);

    protected virtual void Awake()
    {
        interactionPoints = GetComponentsInChildren<InteractionPoint>().Select(p => p.transform);
        if (interactionPoints.Count() == 0)
        {
            Debug.LogWarning($"'" + name + "' has no interaction points.");
        }
    }

    public Vector3 GetInteractionPoint(Transform interactingObject)
    {
        return GetClosestInteractinPoint(interactingObject).position;
    }

    private Transform GetClosestInteractinPoint(Transform interactingObject)
    {
        Transform closestTransform = null;
        float closestMagnitude = float.MaxValue;

        foreach(var interactionPoint in interactionPoints)
        {
            var magnitude = (interactingObject.position - interactionPoint.transform.position).magnitude;
            if (magnitude < closestMagnitude)
            {
                closestTransform = interactionPoint;
                closestMagnitude = magnitude;
            }
        }

        return closestTransform;
    }
}
