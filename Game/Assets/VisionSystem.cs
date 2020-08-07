using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VisionSystem : MonoBehaviour
{
    private IList<GameObject> visibleObjects = new List<GameObject>();

    private GameObject player;

    public bool PlayerInSight { get; private set; } = false;
    public Transform Player { get; private set; }

    [Range(0, 50)]
    public float range = 10;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        PlayerInSight = HasLineOfSight(player);

        if (PlayerInSight)
        {
            Player = player.transform;
        }
        else
        {
            Player = null;
        }
    }

    // void FixedUpdate()
    // {
    //     var objectsToRemove = new List<GameObject>();
    //     foreach (var visibleObject in visibleObjects)
    //     {
    //         if (!)
    //     }
    // }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (HasLineOfSight(other))
    //     {
    //         visibleObjects.Add(other.gameObject);
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     visibleObjects.Remove(other.gameObject);
    // }

    private bool HasLineOfSight(GameObject player)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, range))
        {
            return hit.transform.gameObject == player.gameObject;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
