using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoming : MonoBehaviour {

    float EnemyAggroRange = 6;


    // Use this for initialization
    void Start ()
    {
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().radius = EnemyAggroRange;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gameObject.layer = 2;
        gameObject.transform.position = gameObject.transform.parent.transform.position;
    }


    void OnTriggerStay(Collider other)
    {
        if (gameObject.transform.parent.GetComponent<Unit>().GetIsEnemy() && other.gameObject.tag == "Unit")
        {
            if (!other.gameObject.GetComponent<Unit>().GetIsEnemy())
            {
                gameObject.transform.parent.GetComponent<Unit>().SetTargetDestination(other.gameObject.transform.position);
            }
        }
    }
}
