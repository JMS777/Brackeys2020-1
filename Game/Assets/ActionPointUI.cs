using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionPointUI : MonoBehaviour
{
    public GameObject ActionPointPrefab;

    private ICollection<GameObject> actionPointObjects;

    private ActionPointSystem actionPointSystem;

    void Awake()
    {
        actionPointSystem = FindObjectOfType<PlayerController>().GetComponent<ActionPointSystem>();
        actionPointSystem.ActionPointsUpdated += UpdateUI;

        actionPointObjects = new List<GameObject>();
    }

    private void UpdateUI(int actionPoints)
    {
        Debug.Log(actionPoints + ", " + actionPointObjects.Count);
        while (actionPointObjects.Count < actionPoints)
        {
            actionPointObjects.Add(Instantiate(ActionPointPrefab, transform));
        }

        while (actionPointObjects.Count > actionPoints)
        {
            var actionPoint = actionPointObjects.Last();
            actionPointObjects.Remove(actionPoint);
            Destroy(actionPoint.gameObject);
        }
    }
}
