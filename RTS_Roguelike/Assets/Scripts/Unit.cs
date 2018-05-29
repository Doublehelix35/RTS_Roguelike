using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

    GameObject GameManagerRef;
    NavMeshAgent UnitAgent;
    Animator UnitAnimator;

    public bool IsEnemy = false;


    // Stats
    public int HealthMax = 5;
    private int Health;
    public int Defense = 0;
    public int AttackValue = 1;
    internal float AttackRange = 2f;
    public float AttackSpeed = 1f;


    void Start()
    {
        // Init values
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
        //GetComponent<CircleCollider2D>().radius = AttackRange;
        UnitAgent = GetComponent<NavMeshAgent>();
        UnitAnimator = GetComponent<Animator>();
        Health = HealthMax;
    }

    void Update()
    {
        // If unit is moving on x or z axes then transition to walk anim
        if (UnitAgent.velocity.x >= 0.1f || UnitAgent.velocity.x <= -0.1f ||
            UnitAgent.velocity.z >= 0.1f || UnitAgent.velocity.z <= -0.1f)
        {
            UnitAnimator.SetBool("IsWalking", true);
        }
        // If unit isn't moving transition to idle anim
        else
        {
            UnitAnimator.SetBool("IsWalking", false);
        }
    } 

    public void SetTargetDestination(Vector3 NewDestination)
    {
        // Set new target
        UnitAgent.SetDestination(NewDestination);
    }

    public void SetTargetRotation(Vector3 ObjectToRotateTowards)
    {
        transform.LookAt(ObjectToRotateTowards);
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
            if (IsEnemy)
            {
                // Make friendly and reset health
                IsEnemy = false;
                Health = HealthMax;
            }
            else
            {
                // Unit dies
                GameManagerRef.GetComponent<UnitManager>().RemoveUnitFromSelection(gameObject);
                Destroy(gameObject);
            }
            
        }
    }

    public bool GetIsEnemy()
    {
        return IsEnemy;
    }

    public void AttackAnimPlay()
    {
        UnitAnimator.SetBool("IsAttacking", true);
        UnitAgent.isStopped = true;
    }

    public void AttackAnimStop()
    {
        UnitAnimator.SetBool("IsAttacking", false);
        UnitAgent.isStopped = false;
    }
}
