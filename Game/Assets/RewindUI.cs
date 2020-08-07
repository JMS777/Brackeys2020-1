using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindUI : MonoBehaviour
{
    public Transform states;
    public GameObject statePrefab;
    public GameObject mainPanel;

    private RewindSystem rewindSystem;

    public void Open(IEnumerable<CharacterSnapshot> snaphots)
    {
        ClearChildren();

        foreach(var snapshot in snaphots)
        {
            var stateUI = Instantiate(statePrefab, states).GetComponent<CharacterStateUI>();
            stateUI.SetUI(snapshot, this);
        }

        mainPanel.SetActive(true);
    }

    private void ClearChildren()
    {
        while (states.childCount > 0)
        {
            Destroy(states.GetChild(0));
        }
    }

    public void Close()
    {
        mainPanel.SetActive(false);
    }

    public void SetState(CharacterSnapshot snapshot)
    {
        rewindSystem.RestoreSnapshot(snapshot.Turn);
        Close();
    }
}
