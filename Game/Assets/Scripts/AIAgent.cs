using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ActionPointSystem))]
[RequireComponent(typeof(LifeSystem))]
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(VisionSystem))]
[RequireComponent(typeof(CombatSystem))]
public class AIAgent : MonoBehaviour
{
    public delegate void AgentFinished(AIAgent agent);
    public IInteractable NextInteraction { get; private set; }

    public const float TILE_SIZE = 5;
    public event AgentFinished FinishedTurn;

    private delegate bool AIAction();

    private ActionPointSystem actionPoints;
    private LifeSystem lifeSystem;
    private CharacterMotor motor;
    private VisionSystem vision;
    private CombatSystem combat;

    private bool ExecutingAction
    {
        get
        {
            return isMoving | isAttacking;
        }
    }

    private bool isMoving = false;
    private bool isAttacking = false;

    private Vector3? previousTile = null;

    void Awake()
    {
        actionPoints = GetComponent<ActionPointSystem>();
        lifeSystem = GetComponent<LifeSystem>();
        motor = GetComponent<CharacterMotor>();
        vision = GetComponentInChildren<VisionSystem>();
        combat = GetComponentInChildren<CombatSystem>();

        motor.FinishedMoving += OnFinishedMOving;
        combat.CombatFinished += OnCombatFinished;
    }

    private void OnFinishedMOving()
    {
        // Debug.Log("Agent finished moving");
        isMoving = false;
    }

    private void OnCombatFinished()
    {
        // Debug.Log("Agent finished combat");
        isAttacking = false;
    }

    private void SetInteraction(IInteractable interactableObject)
    {
        if (interactableObject == null)
        {
            return;
        }
        if (interactableObject.gameObject.transform == transform)
        {
            Debug.LogWarning("Cannot interact with self.");
            return;
        }

        NextInteraction = interactableObject;

        motor.setDestination(NextInteraction.GetInteractionPoint(transform));

        // Todo: Remove action points (or maybe keep that in the player movement script along with everything else).
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            var interactable = other.transform.parent.GetComponent<IInteractable>();
            if (interactable != null && interactable == NextInteraction)
            {
                interactable.Interact(gameObject);


                StartCoroutine(LookAtInteraction(other.transform.parent));
                NextInteraction = null;
            }
        }
    }

    IEnumerator LookAtInteraction(Transform interaction)
    {
        yield return new WaitForSeconds(1);

        transform.LookAt(interaction, Vector3.up);
    }

    public void StartTurn()
    {
        // Debug.Log("Processing AI Agent " + gameObject.name);
        StartCoroutine(ProcessTurn());
    }

    private IEnumerator ProcessTurn()
    {
        while (actionPoints.CurrentActionPoints > 0)
        {
            // Debug.Log("Processing AI - " + name);
            if (lifeSystem.IsDead)
            {
                break;
            }

            bool hasAction = StartBestAction();

            if (hasAction)
            {
                yield return new WaitWhile(() => { return ExecutingAction; });
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
            // Debug.Log(actions.Current());
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
        if (vision.Player != null)
        {
            var apRequired = motor.ValidPath(vision.Player.transform.position);
            
            if (apRequired >= 0 && actionPoints.CheckValidAction(apRequired))
            {
                // Debug.Log("Attacking player");
                actionPoints.ExecuteAction(apRequired);
                SetInteraction(vision.Player.GetComponent<Interactable>());
                isAttacking = true;
                StartCoroutine(EndAttack());
                return true;
                // TODO: Set interaction point, (SetInteraction code from player controller).
            }
        }

        return false;
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(7);
        isAttacking = false;
    }

    private bool MoveTowardsPlayer()
    {
        if (vision.PlayerInSight)
        {
            var targetTile = GridHelper.GetNearestTile(transform.position + ((vision.Player.position - transform.position).normalized * TILE_SIZE));

            // Debug.Log("Moving to player");
            previousTile = transform.position;
            isMoving = true;
            motor.setDestination(targetTile);
            actionPoints.ExecuteAction(1);
            return true;
        }

        return false;
    }

    private bool Patrol()
    {
        // Debug.Log("Checking patrol");
        var targetTile = GetNextPatrolTile();

        if (targetTile.HasValue)
        {
            // Debug.Log("Patrolling");
            previousTile = transform.position;
            isMoving = true;
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
            position = GridHelper.GetNearestTile(position);

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
            // Debug.Log("Moving randomly");
            var targetTile = availableTiles[UnityEngine.Random.Range(0, availableTiles.Count)];
            previousTile = transform.position;
            isMoving = true;
            motor.setDestination(targetTile.position);
            actionPoints.ExecuteAction(targetTile.cost);
            return true;
        }

        return false;
    }

    private IEnumerable<PossibleTile> GetAvailableTiles()
    {
        var directions = GetAllDirections().ToList();

        return directions
            .Select(p => new PossibleTile { position = GridHelper.GetNearestTile(transform.position + (p * TILE_SIZE)), cost = motor.ValidPath(GridHelper.GetNearestTile(transform.position + (p * TILE_SIZE)))})
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
