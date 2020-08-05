using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 mousePosition;


    private CharacterMotor characterMotor;

    private new BoxCollider collider;
    private Vector3 updatedPos;
    private bool isValid;
    private new ParticleSystem particleSystem;

    public GameObject playerMarker;
    public GameObject tileChecker;
    public GameObject MoveCost;
    //public GameObject EnemyList;
    // Start is called before the first frame update
    private Action currentAction;
    private IEnumerable<IInteractable> interactableObjects;
    private IInteractable currentTarget;
    private PlayerController playerController;
    public Vector3 CurrentLocation;

    private ActionPointSystem actionPointSystem;
    private int tileToMove;
    private NavMeshPath path;
    private TextMesh textMesh;
    private bool isClickable;
    enum Action
    {
        Move,
        Interact
    }



    void Awake()
    {
        characterMotor = GetComponent<CharacterMotor>();
        playerMarker.SetActive(false);
        collider = tileChecker.GetComponent<BoxCollider>();
        particleSystem = playerMarker.GetComponentInChildren<ParticleSystem>();
        interactableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();
        playerController = GetComponent<PlayerController>();
        actionPointSystem = GetComponent<ActionPointSystem>();
        textMesh = MoveCost.GetComponent<TextMesh>();
        path = new NavMeshPath();
        //tileChecker.SetActive(false);
    }


    void Update()
    {
        if (characterMotor.isMoving)
        {
            playerMarker.transform.position = characterMotor.NextPosition;
        }
        MoveCost.transform.position = updatedPos + new Vector3(0, 1,0);
        MoveCost.transform.LookAt(FindObjectOfType<Cinemachine.CinemachineFreeLook>().transform);
        var main = particleSystem.main;
        // Gets list of current enemies
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if (!Physics.Raycast(ray)){
            isClickable = false;
        }
        if (!characterMotor.isMoving && Physics.Raycast(ray, out hitData, 1000))
        {
            isClickable = true;
            // Get tile position that cursor is over
            updatedPos = new Vector3(Mathf.Round(hitData.point.x / 5) * 5, 0.1f, Mathf.Round(hitData.point.z / 5) * 5);
            playerMarker.transform.position = updatedPos;
            playerMarker.SetActive(true);

            // Moves the tile checker to that tile
            tileChecker.transform.position = updatedPos;
            currentAction = Action.Move;
            main.startColor = Color.cyan;
            textMesh.color = Color.cyan;
            

            // Check if an enemy is on that tile
            foreach (IInteractable interactable in interactableObjects)
            {
                if (collider.bounds.Contains(interactable.gameObject.transform.position))
                {
                    //interactable.gameObject.SetActive(false);
                    if (interactable.gameObject.tag.Equals("Enemy"))
                    {
                        main.startColor = Color.red;
                        textMesh.color = Color.red;
                        MoveCost.transform.position += new Vector3(0,3,0);
                    }
                    else
                    {
                        main.startColor = Color.yellow;
                        textMesh.color = Color.yellow;
                        MoveCost.transform.position += new Vector3(0,1,0);
                    }
                    currentAction = Action.Interact;
                    currentTarget = interactable;
                }
            }
            isValid = true;
        }
        
        tileToMove = remainingDistance(updatedPos);
        textMesh.text = tileToMove.ToString();
        
        //Debug.Log(remainingDistance(updatedPos));
        if (!actionPointSystem.CheckValidAction(tileToMove) || tileToMove.Equals(0))
        {
            main.startColor = Color.gray;
            textMesh.color = Color.gray;
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (!EventSystem.current.IsPointerOverGameObject(0) || !isClickable)
        {
            CurrentLocation = transform.position;
            //Debug.Log(remainingDistance());
            //CalculateDistance();
            if (context.performed)
            {
                if (!characterMotor.isMoving)
                {
                    if (actionPointSystem.ExecuteAction(tileToMove) || tileToMove.Equals(0))
                    {
                        switch (currentAction)
                        {
                            case Action.Move:
                                //Debug.Log(remainingDistance(updatedPos));
                                characterMotor.SetPath(path);
                                break;
                            case Action.Interact:
                                playerController.SetInteraction(currentTarget);
                                //characterMotor.setDestination(currentTarget.InteractionPoint);
                                break;
                        }
                    }
                }
            }
        }
    }

    private Vector3[] UpdateVectors(Vector3[] corners)
    {
        Vector3[] updatedCorners = new Vector3[corners.Length];
        for (int i = 0; i < corners.Length - 1; ++i)
        {
            updatedCorners[i] = new Vector3(Mathf.Round(corners[i].x / 5) * 5, 0.1f, Mathf.Round(corners[i].z / 5) * 5);
        }
        return updatedCorners;
    }

    private int remainingDistance(Vector3 position)
    {
        
        NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);


        Vector3[] points = path.corners;
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; ++i)
        {
            distance += Vector3.Distance(points[i], points[i + 1]);
        }

        return (int)((0.95f * distance) / 5) + 1;
    }
}
