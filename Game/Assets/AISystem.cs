using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    public event Action FinishedTurn;

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
        StartCoroutine(WaitAndFinish());
    }

    IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(3);
        FinishedTurn?.Invoke();
    }
}
