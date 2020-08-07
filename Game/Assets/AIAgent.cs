using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ActionPointSystem))]
[RequireComponent(typeof(LifeSystem))]
[RequireComponent(typeof(CharacterMotor))]
public class AIAgent : MonoBehaviour
{
    public delegate void AgentFinished(AIAgent agent);

    public const float TILE_SIZE = 5;
    public event AgentFinished FinishedTurn;

    private delegate bool AIAction();

    private ActionPointSystem actionPoints;
    private LifeSystem lifeSystem;
    private CharacterMotor motor;

    private bool executingAction = false;

    private Vector3? previousTile = null;

    void Awake()
    {
        actionPoints = GetComponent<ActionPointSystem>();
        lifeSystem = GetComponent<LifeSystem>();
        motor = GetComponent<CharacterMotor>();
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
        Debug.Log("Processing AI Agent " + gameObject.name);
        StartCoroutine(ProcessTurn());
    }

    private IEnumerator ProcessTurn()
    {
        while (actionPoints.CurrentActionPoints > 0)
        {
            bool hasAction = StartBestAction();

            if (hasAction)
            {
                executingAction = true;
                yield return new WaitWhile(() => { return executingAction; });
            }
            else
            {
                break;
            }
        }

        FinishedTurn?.Invoke(this);
    }

    private bool StartBestAction()
    {
        bool hasAction = false;

        var actions = GetAIActions();

        while (!hasAction && actions.MoveNext())
        {
            hasAction = actions.Current();
        }

        return hasAction;
    }

    private IEnumerator<AIAction> GetAIActions()
    {
        yield return AttackPlayer;
        yield return MoveTowardsPlayer;
        yield return Patrol;
        yield return RandomMove;
    }

    private bool AttackPlayer()
    {
        // Debug.Log("Checking for player visibility");
        return false;
    }

    private bool MoveTowardsPlayer()
    {
        // Debug.Log("Checking move to player");
        return false;
    }

    private bool Patrol()
    {
        // Debug.Log("Checking patrol");
        var targetTile = GetNextPatrolTile();

        if (targetTile.HasValue)
        {
            Debug.Log("Patrolling");
            motor.FinishedMoving += ActionFinished;
            previousTile = transform.position;
            motor.setDestination(targetTile.Value.position);
            actionPoints.ExecuteAction(targetTile.Value.cost);
            return true;
        }

        return false;
    }

    private PossibleTile? GetNextPatrolTile()
    {
        if (previousTile.HasValue)
        {
            var position = transform.position + (transform.position - previousTile.Value);
            var target = new PossibleTile
            {
                position = position,
                cost = motor.ValidPath(position)
            };

            if (target.cost > 0)
            {
                return target;
            }
        }

        return null;
    }

    private bool RandomMove()
    {
        // Debug.Log("Checking random move");
        var availableTiles = GetAvailableTiles().ToList();

        if (availableTiles.Count > 0)
        {
            Debug.Log("Moving randomly");
            var targetTile = availableTiles[UnityEngine.Random.Range(0, availableTiles.Count)];
            previousTile = transform.position;
            motor.FinishedMoving += ActionFinished;
            motor.setDestination(targetTile.position);
            actionPoints.ExecuteAction(targetTile.cost);
            return true;
        }

        return false;
    }

    private void ActionFinished()
    {
        executingAction = false;
        motor.FinishedMoving -= ActionFinished;
    }

    private IEnumerable<PossibleTile> GetAvailableTiles()
    {
        var directions = GetAllDirections().ToList();

        return directions
            .Select(p => new PossibleTile { position = transform.position + (p * TILE_SIZE), cost = motor.ValidPath(transform.position + (p * TILE_SIZE))})
            .Where(p => p.cost > 0);
    }

    struct PossibleTile
    {
        public Vector3 position;
        public int cost;
    }

    private IEnumerable<Vector3> GetAllDirections()
    {
        yield return Vector3.left;
        yield return Vector3.right;
        yield return Vector3.forward;
        yield return Vector3.back;
    }

    IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(3);
        FinishedTurn?.Invoke(this);
    }
    
    private int NumberOfTiles(Vector3 position)
    {
        return motor.ValidPath(position);
    }
}
