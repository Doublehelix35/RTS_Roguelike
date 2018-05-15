using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject BoxSelectorPrefab;
    GameObject BoxSelector;
    private bool BoxSelectionStarted = false;

    void Start ()
    {
        //Init values
        BoxSelector = Instantiate(BoxSelectorPrefab, transform);
        BoxSelector.SetActive(false);
    }
	

	void Update ()
    {
        // Left click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Clear selection
            if (GetComponent<UnitManager>().SelectionCount() != 0)
            {
                GetComponent<UnitManager>().ClearSelection();
            }

            // Check for unit hit
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "Unit")
                {
                    GetComponent<UnitManager>().AddUnitToSelection(hit.transform.gameObject);
                    Debug.Log(hit.transform.gameObject.name + " selected");
                }
            }            
        }
        // Left click & control
        else if (Input.GetKey(KeyCode.Mouse0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {

            // Box Selector
            BoxSelector.SetActive(true);

            if (BoxSelectionStarted)
            { 
                // Check for floor               
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
                    {
                        // Edit box size
                        Vector3 BoxSize = BoxSelector.transform.position - hit.point ;
                        BoxSelector.transform.GetChild(0).transform.localScale = BoxSize;

                        // Edit box position
                        Vector3 BoxChildPos = BoxSelector.transform.position - (BoxSelector.transform.GetChild(0).transform.localScale / 2);
                        BoxSelector.transform.GetChild(0).position = BoxChildPos;
                    }
                }
            }
            else
            {
                // Clear selection
                if (GetComponent<UnitManager>().SelectionCount() != 0)
                {
                    GetComponent<UnitManager>().ClearSelection();
                }

                // Check for floor
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
                    {
                        // Set box position
                        BoxSelector.transform.position = hit.point + new Vector3(0f, 1f, 0f);
                        BoxSelectionStarted = true;
                    }
                }                
            }

        }
        // Right click
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (GetComponent<UnitManager>().SelectionCount() > 0)
            {
                // Move
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
                    {
                        GetComponent<UnitManager>().MoveSelection(hit.point);
                    }
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
            BoxSelector.transform.GetChild(0).transform.localScale = new Vector3(1f, 1f, 1f);
            BoxSelector.SetActive(false);
        }
    }
}
