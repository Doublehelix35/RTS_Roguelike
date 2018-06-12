using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

    GameObject GameManagerRef; // Reference to game manager
    NavMeshAgent UnitAgent; 
    Animator UnitAnimator;
    public GameObject AffiliationCircle; // Shows player if unit friendly or enemy
    public Material AffiliationEnemy; // Enemy material
    public Material AffiliationFriendly; // Friendly material
    public GameObject HealthBarRef; // Reference to health bar object

    public bool IsEnemy = false; // Is the unit friendly or an enemy

    // Stats
    public int HealthMax = 5; // Maximum health
    private int Health; // Current health
    public int Defense = 0; // Damage mitigation
    public int AttackValue = 1; // Attack strength
    public float AttackRange = 1f; // Range of attack
    public float AttackSpeed = 1f; // Interval between attacks


    void Start()
    {
        // Init values
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
        UnitAgent = GetComponent<NavMeshAgent>();
        UnitAnimator = GetComponent<Animator>();
        Health = HealthMax;
        GameManagerRef.GetComponent<GameManager>().AddUnitToAllUnitsList(gameObject);

        // Set up affilation circle and enemy
        if (IsEnemy)
        {
            AffiliationCircle.GetComponent<MeshRenderer>().material = AffiliationEnemy;
            var EnemyAggro = new GameObject().AddComponent<EnemyHoming>();
            EnemyAggro.name = "EnemyAggro";
            EnemyAggro.transform.parent = gameObject.transform;
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
        // Make gameobject look at target
        transform.LookAt(ObjectToRotateTowards);
    }

    public void ClearTargetDestination()
    {
        // Clears unit agent of destination
        UnitAgent.SetDestination(gameObject.transform.position);
    }

    public void TakeDamage(int damageToTake)
    {
        // Defense check
        if(Defense >= damageToTake)
        {
            // Take no damage
        }
        else
        {
            // Lose health
            Health -= damageToTake + Defense;
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
                GameManagerRef.GetComponent<GameManager>().UpdateArmySizeText();
                gameObject.transform.Find("EnemyAggro").gameObject.SetActive(false);
                ClearTargetDestination();
                // Update health bar
                float HealthPercent = (float)Health / (float)HealthMax;
                HealthBarRef.GetComponent<HealthBar>().UpdateTransitionPercentage(HealthPercent);
            }
            else
            {
                // Unit dies
                GameManagerRef.GetComponent<UnitManager>().RemoveUnitFromSelection(gameObject);
                GameManagerRef.GetComponent<GameManager>().RemoveUnitFromAllUnitsList(gameObject);
                Destroy(gameObject);
            }
            
        }
    }

    

    public bool GetIsEnemy()
    {
        // Return is enemy boolean
        return IsEnemy;
    }

    public void AttackAnimPlay()
    {
        // Play animation and stop movement
        UnitAnimator.SetBool("IsAttacking", true);
        UnitAgent.isStopped = true;
    }

    public void AttackAnimStop()
    {
        // Stop animation and resume movement
        UnitAnimator.SetBool("IsAttacking", false);
        UnitAgent.isStopped = false;
    }

    public void IncreaseMaxHealth(int ValueToIncreaseBy)
    {
        // Increase current and max health
        Health += ValueToIncreaseBy;
        HealthMax += ValueToIncreaseBy;
    }

    public void IncreaseDefense(int ValueToIncreaseBy)
    {
        // Increase defense value
        Defense += ValueToIncreaseBy;
    }

    public void IncreaseAttack(int ValueToIncreaseBy)
    {
        // Increase attack value
        AttackValue += ValueToIncreaseBy;
    }

    public int GetCurrentHealth()
    {
        // Return current health
        return Health;
    }
}
