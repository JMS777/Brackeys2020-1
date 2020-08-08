using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewindUI : MonoBehaviour
{
    public Transform states;
    public GameObject statePrefab;
    public GameObject mainPanel;

    private RewindSystem rewindSystem;

    public Button activateButton;
    public TMP_Text activateText;

    public event Action PanelOpened;
    public event Action PanelClosed;

    private ICollection<CharacterStateUI> stateUIs = new List<CharacterStateUI>();

    void Awake()
    {
        rewindSystem = FindObjectOfType<RewindSystem>();
    }

    public void Open(IEnumerable<CharacterSnapshot> snaphots)
    {
        ClearChildren();

        foreach(var snapshot in snaphots)
        {
            var stateUI = Instantiate(statePrefab, states).GetComponent<CharacterStateUI>();
            stateUI.SetUI(snapshot, this);
            stateUIs.Add(stateUI);
        }

        mainPanel.SetActive(true);
        PanelOpened?.Invoke();
    }

    private void ClearChildren()
    {
        foreach(var stateui in stateUIs)
        {
            Destroy(stateui.gameObject);
        }

        stateUIs.Clear();
        // while (states.childCount > 0)
        // {
        //     Destroy(states.GetChild(0));
        // }
    }

    public void Close()
    {
        mainPanel.SetActive(false);
        PanelClosed?.Invoke();
    }

    public void SetState(CharacterSnapshot snapshot)
    {
        rewindSystem.RestoreSnapshot(snapshot.Turn);
        activateButton.interactable = false;
        Close();
    }

    public void UpdateButton(int cooldown)
    {
        if (cooldown > 0)
        {
            activateButton.interactable = false;
            activateText.text = $"{cooldown} turns";
        }
        else
        {
            activateText.text = "Rewind";
            activateButton.interactable = true;
        }
    }

    public void Activate()
    {
        rewindSystem.Activate();
    }
}
