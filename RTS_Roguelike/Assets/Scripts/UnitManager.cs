using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    private List<GameObject> SelectedUnits;

	// Use this for initialization
	void Start ()
    {
        // Init values
        SelectedUnits = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left click
        {
            Debug.Log("Left Click");

            // Clear selection
            if (SelectedUnits.Count != 0)
            {
                SelectedUnits.Clear();
                Debug.Log("Selection Cleared");
            }

            // Check for unit hit
            RaycastHit2D Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        
            if(Hit.collider != null && Hit.collider.gameObject.tag == "Unit")
            {
                SelectedUnits.Add(Hit.transform.gameObject);
                Debug.Log(Hit.transform.gameObject.name + " selected");
            }            
            
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) // Right click
        {
            Debug.Log("Right Click");
            if (SelectedUnits.Count > 0)
            {
                // Move units
                foreach (GameObject Unit in SelectedUnits)
                {
                    Unit.GetComponent<Unit>().SetTargetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            else
            {
                Debug.Log("No units selected");
            }
        }

	}
}
