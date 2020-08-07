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
    private PlayerController player;

    void Awake()
    {
        aiSystem = FindObjectOfType<AISystem>();
        aiSystem.AITurnFinished += OnAiTurnFinished;

        player = FindObjectOfType<PlayerController>();
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
        player.OnEndTurn();
        aiSystem.StartTurn();
    }

    private void OnAiTurnFinished()
    {
        NextTurn();
    }

    public void NextTurn()
    {
        CurrentTurn++;
        player.OnTurnStarted();
        PlayerTurnStarted?.Invoke(CurrentTurn);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
