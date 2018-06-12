using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelector : MonoBehaviour {

    GameObject GameManagerRef;


	void Start ()
    {
        // Init Values
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
    }

    
    private void OnTriggerEnter(Collider col)
    {
        // Check if gameobject is a unit
        if (col.gameObject.tag == "Unit")
        {
            // Check if unit is friendly
            if(col.gameObject.GetComponent<Unit>().GetIsEnemy() == false)
            {
                // Tell GM to add unit to list
                GameManagerRef.GetComponent<UnitManager>().AddUnitToSelection(col.gameObject);
            }
            
        }
    }
}
