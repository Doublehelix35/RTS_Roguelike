using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    private List<GameObject> SelectedUnits;
    private int SelectedFormation = 0; // Formation of selected units


	void Start ()
    {
        // Init values
        SelectedUnits = new List<GameObject>();        
        
	}
	
	void Update ()
    {
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

    // Add unit to list
    public void AddUnitToSelection(GameObject UnitToAdd)
    {
        if (!SelectedUnits.Contains(UnitToAdd))
        {
            // Add unit to list
            SelectedUnits.Add(UnitToAdd);
        }
        
    }

    // Remove unit from list
    public void RemoveUnitFromSelection(GameObject UnitToRemove)
    {
        if (SelectedUnits.Contains(UnitToRemove))
        {
            SelectedUnits.Remove(UnitToRemove);
        }
    }
    
    // Return unit count
    public int SelectionCount()
    {
        return SelectedUnits.Count;
    }

    // Clear list
    public void ClearSelection()
    {
        SelectedUnits.Clear();
        Debug.Log("Selection Cleared");
        // Reset formation
        SelectedFormation = 0;
    }

    // Move units
    public void MoveSelection(Vector3 NewDestination)
    {
        switch (SelectedFormation)
        {
            // No formation selected
            case 0:
                // Move all units to single target
                foreach (GameObject Unit in SelectedUnits)
                {
                    Unit.GetComponent<Unit>().SetTargetDestination(NewDestination);
                }
                break;
            // Line formation
            case 1:
                Vector3 LinePos = new Vector3(-2f, 0f, 0f);
                for(int i = 0; i < SelectedUnits.Count; i++)
                {
                    LinePos.x += 2.5f;
                    SelectedUnits[i].GetComponent<Unit>().SetTargetDestination(NewDestination + LinePos);
                }
                break;
            // Square formation
            case 2:
                // Calculate width of formation
                int width = 1, minRange = 1, maxRange = 1, gap = 1;

                while(SelectedUnits.Count >= maxRange)
                {
                    if(SelectedUnits.Count >= minRange && SelectedUnits.Count <= maxRange)
                    {
                        break;
                    }
                    else
                    {
                        gap *= 2;
                        width++;
                        minRange = maxRange + 1;
                        maxRange = minRange + gap;
                    }
                }

                int unitNum = SelectedUnits.Count;
                for (int i = 0; i < SelectedUnits.Count; i++)
                {
                    for (int j = 0; j < width; j++)
                    {                        
                        Vector3 SquarePos = new Vector3(j * 2.5f, 0f, -i * 2.5f);
                        SelectedUnits[unitNum - 1].GetComponent<Unit>().SetTargetDestination(NewDestination + SquarePos);
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
