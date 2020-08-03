using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileCheck : MonoBehaviour
{
    // Start is called before the first frame update

    public bool EnemyInBounds;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision){
        Debug.Log("Collision");
        if(collision.rigidbody.gameObject.tag.Equals("Enemy")){
            EnemyInBounds = true;
        }
        else{
            EnemyInBounds = false;
        }
    }
}
