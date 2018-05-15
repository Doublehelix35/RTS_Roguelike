using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    GameObject GameManagerRef;
    Vector3 TargetDestination; // Destination to move to

    // Stats
    public int Health = 5;
    public int Defense = 0;
    public int AttackValue = 1;
    public float AttackRange = 1.5f;
    public float AttackSpeed = 1f;
    float LastAttackTime;

    float Speed = 1.0f; // Unit movement speed

	void Start ()
    {
        // Init values
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
        TargetDestination = transform.position;
        //GetComponent<CircleCollider2D>().radius = AttackRange;
        LastAttackTime = Time.time;
	}
	
	void Update ()
    {
        // Move
        if (transform.position != TargetDestination)
        {
            // Calculate direction to head
            Vector3 Dir = TargetDestination - transform.position;

            // Move in direction
            transform.Translate(Dir * Speed * Time.deltaTime);
        }
              
	}

    private void OnTriggerStay(Collider col)
    {
        //Debug.Log("Imma kill you " + col.gameObject.name);

        // Check for a unit
        if(col.gameObject.tag == "Unit")
        {
            if (LastAttackTime < Time.time - AttackSpeed)
            {
                // Attack enemy
                col.gameObject.GetComponent<Unit>().TakeDamage(AttackValue);
                // Update last attack time
                LastAttackTime = Time.time;
            }
            
        }
    }

    public void SetTargetDestination(Vector3 NewDestination)
    {
        // Set new target
        TargetDestination = NewDestination;
    }

    public void TakeDamage(int damageToTake)
    {
        // Defense check
        if(Defense >= damageToTake)
        {
            // Take no damage
            //Debug.Log("Ha! Pathetic attack");
        }
        else
        {
            // Lose health
            Health -= damageToTake + Defense;
            Debug.Log("Ouch said " + gameObject.name);
        }        

        //Check health
        if (Health <= 0)
        {
            // Unit dies
            GameManagerRef.GetComponent<UnitManager>().RemoveUnitFromSelection(gameObject);
            Destroy(gameObject);
        }
    }

    
}
