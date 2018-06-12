using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoming : MonoBehaviour {

    float EnemyAggroRange = 6; // Collider size


    // Use this for initialization
    void Start ()
    {
        // Add sphere collider to gameobject and set properties
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = EnemyAggroRange;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gameObject.layer = 2;
        // Set position equal to parents
        gameObject.transform.position = gameObject.transform.parent.transform.position;
    }


    void OnTriggerStay(Collider other)
    {
        // Check if the parent is an enemy unit and the other gameobject is an unit
        if (gameObject.transform.parent.GetComponent<Unit>().GetIsEnemy() && other.gameObject.tag == "Unit")
        {
            // Check if the other game object is a friendly unit
            if (!other.gameObject.GetComponent<Unit>().GetIsEnemy())
            {
                // Set movement target for that unit
                gameObject.transform.parent.GetComponent<Unit>().SetTargetDestination(other.gameObject.transform.position);
            }
        }
    }
}
