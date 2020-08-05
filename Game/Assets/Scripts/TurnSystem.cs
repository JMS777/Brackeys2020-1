using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public event Action<int> PlayerTurnStarted;
    public event Action PlayerTurnEnded;

    public int CurrentTurn { get; set; }

    private AISystem aiSystem;

    void Awake()
    {
        aiSystem = FindObjectOfType<AISystem>();
        aiSystem.FinishedTurn += OnAiTurnFinished;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentTurn = 1;
        PlayerTurnStarted?.Invoke(CurrentTurn);
    }

    public void EndTurn()
    {
        PlayerTurnEnded?.Invoke();
        aiSystem.StartTurn();
    }

    private void OnAiTurnFinished()
    {
        NextTurn();
    }

    public void NextTurn()
    {
        CurrentTurn++;
        PlayerTurnStarted?.Invoke(CurrentTurn);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
