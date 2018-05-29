using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject BoxSelectorPrefab;
    GameObject BoxSelector;
    private bool BoxSelectionStarted = false;
    float PanPercentageLowMin = 0.08f;
    float PanPercentageLowMax = 0.15f;
    float PanPercentageHighMin = 0.85f;
    float PanPercentageHighMax = 0.92f;
    float PanSpeed = 2.2f;

    float ScrollSpeed = 15f;
    float ScrollClampMin = 4f;
    float ScrollClampMax = 25f;

    void Start ()
    {
        //Init values
        BoxSelector = Instantiate(BoxSelectorPrefab, transform);
        BoxSelector.SetActive(false);
    }
	

	void Update ()
    {
        // Panning camera
        PanCamera();

        // Scrolling
        Scroll();

        // Clicks
        ClickChecks();
        
    }

    void PanCamera()
    {
        // Left pan
        if (Input.mousePosition.x < Screen.width * PanPercentageLowMax) 
        {
            if (Input.mousePosition.x < Screen.width * PanPercentageLowMin) // Fast pan
            {
                Camera.main.transform.position += Vector3.left * PanSpeed * Time.deltaTime * 3;
            }
            else // Normal speed
            {
                Camera.main.transform.position += Vector3.left * PanSpeed * Time.deltaTime;
            }

        }
        // Right pan
        else if (Input.mousePosition.x > Screen.width * PanPercentageHighMin)
        {
            if (Input.mousePosition.x > Screen.width * PanPercentageHighMax) // Fast pan
            {
                Camera.main.transform.position += Vector3.right * PanSpeed * Time.deltaTime * 3;
            }
            else // Normal speed
            {
                Camera.main.transform.position += Vector3.right * PanSpeed * Time.deltaTime;
            }

        }

        // Down pan
        if (Input.mousePosition.y < Screen.height * PanPercentageLowMax)
        {
            if (Input.mousePosition.y < Screen.height * PanPercentageLowMin) // Fast pan
            {
                Camera.main.transform.position += Vector3.back * PanSpeed * Time.deltaTime * 3;
            }
            else // Normal speed
            {
                Camera.main.transform.position += Vector3.back * PanSpeed * Time.deltaTime;
            }

        }
        // Up pan
        else if (Input.mousePosition.y > Screen.height * PanPercentageHighMin)
        {
            if (Input.mousePosition.y > Screen.height * PanPercentageHighMax) // Fast pan
            {
                Camera.main.transform.position += Vector3.forward * PanSpeed * Time.deltaTime * 3;
            }
            else // Normal speed
            {
                Camera.main.transform.position += Vector3.forward * PanSpeed * Time.deltaTime;
            }

        }

    }

    void Scroll()
    {
        // Scroll in
        if (Input.GetAxis("Mouse ScrollWheel") > 0) 
        {
            Camera.main.transform.position += Vector3.down * ScrollSpeed * Time.deltaTime;
        }
        // Scroll out
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.transform.position += Vector3.up * ScrollSpeed * Time.deltaTime;
        }

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
                                                     Mathf.Clamp(Camera.main.transform.position.y, ScrollClampMin, ScrollClampMax), 
                                                     Camera.main.transform.position.z);
    }

    void ClickChecks()
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
                    // Check if unit is friendly
                    if (hit.transform.gameObject.GetComponent<Unit>().GetIsEnemy() == false)
                    {
                        GetComponent<UnitManager>().AddUnitToSelection(hit.transform.gameObject);
                        Debug.Log(hit.transform.gameObject.name + " selected");
                    }
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
                        Vector3 BoxSize = BoxSelector.transform.position - hit.point;
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
