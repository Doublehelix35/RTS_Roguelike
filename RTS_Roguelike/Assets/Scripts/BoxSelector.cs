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
        Debug.Log("Triggered!");
        if (col.gameObject.tag == "Unit")
        {
            GameManagerRef.GetComponent<UnitManager>().AddUnitToSelection(col.gameObject);
            Debug.Log("Unit Added?");
        }
    }
}
