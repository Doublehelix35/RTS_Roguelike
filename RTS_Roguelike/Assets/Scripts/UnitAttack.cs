using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour {

    float LastAttackTime; // Time stamp of last attack
    GameObject UnitRef; // Ref to parent unit

    void Start ()
    {
        // Init values
        LastAttackTime = Time.time;
        UnitRef = transform.parent.gameObject;
        GetComponent<SphereCollider>().radius = UnitRef.GetComponent<Unit>().AttackRange;
    }

    private void OnTriggerStay(Collider col)
    {
        // Check for a unit
        if (col.gameObject.tag == "Unit")
        {
            // Check if can attack
            if (LastAttackTime < Time.time - UnitRef.GetComponent<Unit>().AttackSpeed)
            {
                // Check if unit is of opposite affiliation
                if (UnitRef.GetComponent<Unit>().GetIsEnemy() != col.gameObject.GetComponent<Unit>().GetIsEnemy())
                {
                    // Attack other unit
                    StartCoroutine(AttackWaitThenStop());
                    UnitRef.GetComponent<Unit>().SetTargetRotation(col.gameObject.transform.position);
                    col.gameObject.GetComponent<Unit>().TakeDamage(UnitRef.GetComponent<Unit>().AttackValue);
                    // Update last attack time
                    LastAttackTime = Time.time;
                }
                
            }
        }
        // Check if attacking unit is friendly and object is breakable
        else if (!UnitRef.GetComponent<Unit>().GetIsEnemy() && col.gameObject.tag == "Breakable") 
        {
            // Face object, attack and then tell it to break
            StartCoroutine(AttackWaitThenStop());
            UnitRef.GetComponent<Unit>().SetTargetRotation(col.gameObject.transform.position);
            col.gameObject.GetComponent<Breakable>().Break();
        }
    }

    IEnumerator AttackWaitThenStop()
    {
        // Attack then stop
        UnitRef.GetComponent<Unit>().AttackAnimPlay();
        yield return new WaitForSeconds(0.2f);
        UnitRef.GetComponent<Unit>().AttackAnimStop();
    }
}
