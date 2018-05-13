using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelector : MonoBehaviour {

    GameObject GameManagerRef;


	void Start ()
    {
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Unit")
        {
            // Tell GM to add unit to list
            GameManagerRef.GetComponent<UnitManager>().AddUnitToSelection(col.gameObject);
        }
    }
}
