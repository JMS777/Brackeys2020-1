using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ActionPointSystem))]
[RequireComponent(typeof(LifeSystem))]
[RequireComponent(typeof(CharacterMotor))]
public class AISystem : MonoBehaviour
{
    public event Action AITurnFinished;

    private IEnumerable<AIAgent> agents;

    private List<AIAgent> processingQueue = new List<AIAgent>();

    void Awake()
    {
        agents = FindObjectsOfType<AIAgent>();

        foreach (var agent in agents)
        {
            agent.FinishedTurn += AgentFinished;
        }
    }

    private void AgentFinished(AIAgent agent)
    {
        processingQueue.Remove(agent);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartTurn()
    {
        Debug.Log("Processing AI");
        StartCoroutine(ProcessTurn());
    }

    private IEnumerator ProcessTurn()
    {
        foreach (var agent in agents)
        {
            processingQueue.Add(agent);
            agent.StartTurn();

        }
        
        yield return new WaitWhile(() => { return processingQueue.Count > 0; });

        AITurnFinished?.Invoke();
    }
}
