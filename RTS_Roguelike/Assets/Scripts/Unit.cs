using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

    GameObject GameManagerRef;
    NavMeshAgent UnitAgent;
    Animator UnitAnimator;
    public GameObject AffiliationCircle; // Shows player if unit friendly or enemy
    public Material AffiliationEnemy;
    public Material AffiliationFriendly;
    public GameObject HealthBarRef;

    public bool IsEnemy = false;


    // Stats
    public int HealthMax = 5;
    private int Health;
    public int Defense = 0;
    public int AttackValue = 1;
    public float AttackRange = 1f;
    public float AttackSpeed = 1f;


    void Start()
    {
        // Init values
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
        UnitAgent = GetComponent<NavMeshAgent>();
        UnitAnimator = GetComponent<Animator>();
        Health = HealthMax;

        if (IsEnemy)
        {
            AffiliationCircle.GetComponent<MeshRenderer>().material = AffiliationEnemy;
        }
        else
        {
            AffiliationCircle.GetComponent<MeshRenderer>().material = AffiliationFriendly;
        }
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
            //Debug.Log("Ouch said " + gameObject.name);
            // Update health bar
            float HealthPercent = (float)Health / (float)HealthMax;
            HealthBarRef.GetComponent<HealthBar>().UpdateTransitionPercentage(HealthPercent);
        }        

        //Check health
        if (Health <= 0)
        {
            if (IsEnemy)
            {
                // Make friendly and reset health
                IsEnemy = false;
                Health = HealthMax;
                AffiliationCircle.GetComponent<MeshRenderer>().material = AffiliationFriendly;
                // Update health bar
                float HealthPercent = (float)Health / (float)HealthMax;
                HealthBarRef.GetComponent<HealthBar>().UpdateTransitionPercentage(HealthPercent);
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

    public void IncreaseMaxHealth(int ValueToIncreaseBy)
    {
        Health += ValueToIncreaseBy;
        HealthMax += ValueToIncreaseBy;
    }

    public void IncreaseDefense(int ValueToIncreaseBy)
    {
        Defense += ValueToIncreaseBy;
    }

    public void IncreaseAttack(int ValueToIncreaseBy)
    {
        AttackValue += ValueToIncreaseBy;
    }
}
