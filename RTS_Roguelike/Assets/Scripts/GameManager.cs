using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject BoxSelectorPrefab; // Box selector prefab to spawn and scale
    GameObject BoxSelector; // Gameobject that will add units to selection on trigger
    bool BoxSelectionStarted = false; // Tracks whether box selection is active
    float PanScreenWidthMin = 0.08f; // Percentage of screen for fast pan
    float PanScreenWidthMax = 0.15f; // Percentage of screen for normal pan
    float PanSpeed = 2.2f; // Speed of camera panning
    float PanFaster = 3f;

    public GameObject PauseMenu; // Pause menu ref
    public Text ArmySizeText; // UI text for army size

    List<GameObject> AllUnits; // List of all units

    float ScrollSpeed = 15f; // Speed of scrolling
    float ScrollClampMin = 4f; // Closest the camera can get to the floor
    float ScrollClampMax = 25f; // Furthest the camera can get from the floor

    void Awake()
    {
        // Init values
        AllUnits = new List<GameObject>();
        ArmySizeText.text = "" + AllUnits.Count;
    }

    void Start ()
    {
        //Init values
        BoxSelector = Instantiate(BoxSelectorPrefab, transform);
        BoxSelector.SetActive(false);
        PauseMenu.SetActive(false);        
    }
	

	void Update ()
    {
        // Panning camera
        PanCamera();

        // Scrolling
        Scroll();

        // Clicks
        ClickChecks();

        // Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
        }
        
    }

    void PanCamera()
    {
        // Left pan
        if (Input.mousePosition.x < Screen.width * PanScreenWidthMax) 
        {
            // Fast speed
            if (Input.mousePosition.x < Screen.width * PanScreenWidthMin) 
            {
                // Move camera left
                Camera.main.transform.position += Vector3.left * PanSpeed * Time.deltaTime * PanFaster;
            }
            // Normal speed
            else
            {
                // Move camera left
                Camera.main.transform.position += Vector3.left * PanSpeed * Time.deltaTime;
            }

        }
        // Right pan
        else if (Input.mousePosition.x > Screen.width * (1f - PanScreenWidthMax))
        {
            // Fast Speed
            if (Input.mousePosition.x > Screen.width * (1f -PanScreenWidthMin)) 
            {
                // Move camera right
                Camera.main.transform.position += Vector3.right * PanSpeed * Time.deltaTime * PanFaster;
            }
            // Normal speed
            else
            {
                // Move camera right
                Camera.main.transform.position += Vector3.right * PanSpeed * Time.deltaTime;
            }

        }

        // Down pan
        if (Input.mousePosition.y < Screen.height * PanScreenWidthMax)
        {
            // Fast speed
            if (Input.mousePosition.y < Screen.height * PanScreenWidthMin) 
            {
                // Move camera down
                Camera.main.transform.position += Vector3.back * PanSpeed * Time.deltaTime * PanFaster;
            }
            // Normal speed
            else
            {
                // Move camera down
                Camera.main.transform.position += Vector3.back * PanSpeed * Time.deltaTime;
            }

        }
        // Up pan
        else if (Input.mousePosition.y > Screen.height * (1f - PanScreenWidthMax))
        {
            // Fast speed
            if (Input.mousePosition.y > Screen.height * (1f - PanScreenWidthMin)) 
            {
                // Move camera up
                Camera.main.transform.position += Vector3.forward * PanSpeed * Time.deltaTime * PanFaster;
            }
            // Normal speed
            else
            {
                // Move camera up
                Camera.main.transform.position += Vector3.forward * PanSpeed * Time.deltaTime;
            }

        }

    }

    void Scroll()
    {
        // Scroll in
        if (Input.GetAxis("Mouse ScrollWheel") > 0) 
        {
            // Move camera closer to floor
            Camera.main.transform.position += Vector3.down * ScrollSpeed * Time.deltaTime;
        }
        // Scroll out
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            // Move camera away from floor
            Camera.main.transform.position += Vector3.up * ScrollSpeed * Time.deltaTime;
        }
        // Clamp the camera's positon based on the min and max values
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
                // Clear list of units
                GetComponent<UnitManager>().ClearSelection();
            }

            // Check for unit hit via raycast
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Check if raycast hit anything and if it did was that gameobject a unit
                if (hit.collider != null && hit.collider.gameObject.tag == "Unit")
                {
                    // Check if unit is friendly
                    if (hit.transform.gameObject.GetComponent<Unit>().GetIsEnemy() == false)
                    {
                        // Add that unit to the selection
                        GetComponent<UnitManager>().AddUnitToSelection(hit.transform.gameObject);
                        //Debug.Log(hit.transform.gameObject.name + " selected");
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
                    // Check if raycast hit something and whether the floor was it
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
                    // Check if raycast hit something and whether the floor was it
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

    public void AddUnitToAllUnitsList(GameObject UnitToAdd)
    {
        // Check if unit isn't already in list
        if (!AllUnits.Contains(UnitToAdd))
        {
            // Add the unit to the list
            AllUnits.Add(UnitToAdd);

            // Calculate the friendly unit count
            UpdateArmySizeText();
        }
        else
        {
            Debug.Log("ERROR Unit already in list");
        }
    }

    public void RemoveUnitFromAllUnitsList(GameObject UnitToRemove)
    {
        // Check if list contains the unit
        if (AllUnits.Contains(UnitToRemove))
        {
            // Remove the unit
            AllUnits.Remove(UnitToRemove);

            // Recalculate the friendly unit count
            UpdateArmySizeText();
        }
        else
        {
            Debug.Log("ERROR Unit not in list");
        }
    } 

    public void UpdateArmySizeText()
    {
        int FriendlyUnitCount = 0;
        foreach (var Unit in AllUnits)
        {
            // Check if unit is friendly
            if (!Unit.GetComponent<Unit>().IsEnemy)
            {
                FriendlyUnitCount++;
            }
        }
        ArmySizeText.text = "" + FriendlyUnitCount;
    } 
}
