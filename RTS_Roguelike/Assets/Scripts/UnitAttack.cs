using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour {

    float LastAttackTime;
    GameObject UnitRef;

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
            if (LastAttackTime < Time.time - UnitRef.GetComponent<Unit>().AttackSpeed)
            {
                // Attack enemy
                if (UnitRef.GetComponent<Unit>().GetIsEnemy() != col.gameObject.GetComponent<Unit>().GetIsEnemy())
                {
                    StartCoroutine(AttackWaitThenStop());
                    UnitRef.GetComponent<Unit>().SetTargetRotation(col.gameObject.transform.position);
                    col.gameObject.GetComponent<Unit>().TakeDamage(UnitRef.GetComponent<Unit>().AttackValue);
                    // Update last attack time
                    LastAttackTime = Time.time;
                }
                
            }
        }
        else if (col.gameObject.tag == "Breakable")
        {
            StartCoroutine(AttackWaitThenStop());
            UnitRef.GetComponent<Unit>().SetTargetRotation(col.gameObject.transform.position);
            col.gameObject.GetComponent<Breakable>().Break();
            

        }
    }

    IEnumerator AttackWaitThenStop()
    {
        UnitRef.GetComponent<Unit>().AttackAnimPlay();
        yield return new WaitForSeconds(0.2f);
        UnitRef.GetComponent<Unit>().AttackAnimStop();
    }
}
