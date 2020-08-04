using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    public TMP_Text text;

    private TurnSystem turnSystem;
    private Button endTurn;

    void Awake()
    {
        endTurn = GetComponentInChildren<Button>();
        turnSystem = FindObjectOfType<TurnSystem>();
        turnSystem.PlayerTurnStarted += UpdateUI;
        turnSystem.PlayerTurnStarted += EnableUI;
        turnSystem.PlayerTurnEnded += DisableUI;
    }
    
    private void UpdateUI(int turn)
    {
        text.text = $"Turn {turn}";
    }

    private void EnableUI(int _)
    {
        endTurn.interactable = true;
    }

    private void DisableUI()
    {
        endTurn.interactable = false;
    }

    public void EndTurn()
    {
        turnSystem.EndTurn();
    }
}
