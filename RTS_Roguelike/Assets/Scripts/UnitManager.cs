using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    private List<GameObject> SelectedUnits;
    private int SelectedFormation = 0; // Formation of selected units
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
        // Left click
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            // Clear selection
            if (SelectedUnits.Count != 0)
            {
                SelectedUnits.Clear();
                Debug.Log("Selection Cleared");
                // Reset formation
                SelectedFormation = 0;
            }

            // Check for unit hit
            RaycastHit2D Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        
            if(Hit.collider != null && Hit.collider.gameObject.tag == "Unit")
            {
                SelectedUnits.Add(Hit.transform.gameObject);
                Debug.Log(Hit.transform.gameObject.name + " selected");
            }                       
        }
        // Left click & control
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
        // Right click
        else if (Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            if (SelectedUnits.Count > 0)
            {
                MoveSelection();
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

        // 1 button
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedFormation = 1;
            Debug.Log("Line formation!");
        }
        // 2 button
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedFormation = 2;
            Debug.Log("Square formation!");
        }

	}

    public void AddUnitToSelection(GameObject UnitToAdd)
    {
        if (!SelectedUnits.Contains(UnitToAdd))
        {
            // Add unit to list
            SelectedUnits.Add(UnitToAdd);
        }
        
    }

    public void RemoveUnitFromSelection(GameObject UnitToRemove)
    {
        if (SelectedUnits.Contains(UnitToRemove))
        {
            SelectedUnits.Remove(UnitToRemove);
        }
    }

    void MoveSelection()
    {
        switch (SelectedFormation)
        {
            // No formation selected
            case 0:
                // Move all units to single target
                foreach (GameObject Unit in SelectedUnits)
                {
                    Unit.GetComponent<Unit>().SetTargetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
                break;
            // Line formation
            case 1:
                Vector3 LinePos = new Vector3(-2f, 0f, 0f);
                for(int i = 0; i < SelectedUnits.Count; i++)
                {
                    LinePos.x += 2f;
                    SelectedUnits[i].GetComponent<Unit>().SetTargetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition) + LinePos);
                }
                break;
            // Square formation
            case 2:
                int width = 3; // replace with formula based on count
                int unitNum = SelectedUnits.Count;
                for (int i = 0; i < SelectedUnits.Count; i++)
                {
                    for (int j = 0; j < width; j++)
                    {                        
                        Vector3 SquarePos = new Vector3(j * 2, -i * 2, 0f);
                        SelectedUnits[unitNum - 1].GetComponent<Unit>().SetTargetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition) + SquarePos);
                        unitNum--;
                        Debug.Log(unitNum);
                        if(unitNum == 0)
                        {
                            return;
                        }
                    }                
                }
                break;
            default:
                break;
        }
        
    }
}
