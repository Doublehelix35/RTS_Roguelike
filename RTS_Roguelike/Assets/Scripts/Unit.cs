using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    Vector3 TargetDestination; // Destination to move to
    float Speed = 1.0f; // Unit movement speed

	void Start ()
    {
        // Init values
        TargetDestination = transform.position;
	}
	
	void Update ()
    {
		
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
    
    
    public void SetTargetDestination(Vector3 NewDestination)
    {
        // Set new target
        TargetDestination = NewDestination;
    }
}
