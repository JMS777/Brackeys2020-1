using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointSystem : MonoBehaviour
{
    public event Action<int> ActionPointsUpdated;

    public int actionPoints = 3;

    public int CurrentActionPoints { get; private set; }

    private TurnSystem turnSystem;

    void Awake()
    {
        turnSystem = FindObjectOfType<TurnSystem>();
        turnSystem.PlayerTurnStarted += OnTurnStarted;
    }

    public bool CheckValidAction(int tiles)
    {
        return CurrentActionPoints - tiles >= 0;
    }

    public bool ExecuteAction(int tiles)
    {
        if (!CheckValidAction(tiles))
        {
            return false;
        }

        CurrentActionPoints -= tiles;
        ActionPointsUpdated?.Invoke(CurrentActionPoints);

        return true;
    }

    private void OnTurnStarted(int turn)
    {
        CurrentActionPoints = actionPoints;
        ActionPointsUpdated?.Invoke(CurrentActionPoints);
    }
}
