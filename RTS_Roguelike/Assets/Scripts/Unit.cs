using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    Vector3 TargetDestination;
    float Speed = 1.0f;

	// Use this for initialization
	void Start () {
        TargetDestination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        // Move
        if (transform.position != TargetDestination)
        {
            // Calculate direction to head
            Vector3 Dir = TargetDestination - transform.position;
            Dir.z = 0;

            // Move in direction
            transform.Translate(Dir * Speed * Time.deltaTime);
        }
	}
    
    // Set new target to move this unit to
    void SetTargetDestination(Vector3 NewDestination)
    {
        TargetDestination = NewDestination;
    }
}
