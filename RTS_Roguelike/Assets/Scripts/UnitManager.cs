using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    private List<GameObject> SelectedUnits;
    public GameObject BoxSelectorPrefab;
    GameObject BoxSelector;
    private bool BoxSelectionStarted = false;

	// Use this for initialization
	void Start ()
    {
        // Init values
        SelectedUnits = new List<GameObject>();        
        BoxSelector = Instantiate(BoxSelectorPrefab, transform);       
        BoxSelector.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left click
        {
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
        else if (Input.GetKey(KeyCode.Mouse0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {

            // Box Selector
            BoxSelector.SetActive(true);

            if (BoxSelectionStarted)
            {
                // Edit box size
                Vector3 BoxSize = BoxSelector.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                BoxSize.z = 1f;
                BoxSelector.transform.GetChild(0).transform.localScale = BoxSize;
                
                // Edit box position
                Vector3 BoxChildPos = BoxSelector.transform.position - (BoxSelector.transform.GetChild(0).transform.localScale / 2);
                BoxChildPos.z = 0f;
                BoxSelector.transform.GetChild(0).position = BoxChildPos;
                    
            }
            else
            {
                // Clear selection
                if (SelectedUnits.Count != 0)
                {
                    SelectedUnits.Clear();
                    Debug.Log("Selection Cleared");
                }

                // Set box position
                Vector3 BoxPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                BoxSelector.transform.position = new Vector2(BoxPos.x, BoxPos.y);

                BoxSelectionStarted = true;
            }

        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) // Right click
        {
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

        // Reset box selection
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            BoxSelectionStarted = false;
            BoxSelector.transform.GetChild(0).transform.localScale = new Vector3(0.2f, 0.2f, 0f);
            BoxSelector.SetActive(false);
        }

	}

    public void AddUnitToSelection(GameObject UnitToAdd)
    {
        if (!SelectedUnits.Contains(UnitToAdd))
        {
            SelectedUnits.Add(UnitToAdd);
            Debug.Log("Unit Added!");
        }
        
    }
}
