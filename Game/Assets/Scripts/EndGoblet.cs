using System.Collections;
using System.Collections.Generic;
using CoreSystems.TransitionSystem;
using UnityEngine;

public class EndGoblet : Interactable
{
    public override void Interact(GameObject intiatingObject)
    {
        LevelLoader.Instance.LoadLevel(Level.Menu);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
