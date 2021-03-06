﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    private List<GameObject> SelectedUnits; // List of units selected
    private int SelectedFormation = 0; // Formation of selected units
    
    public enum FormationTypes { Health, Defense, Attack }; // Types of formations
    public FormationTypes Formation;


	void Start ()
    {
        // Init values
        SelectedUnits = new List<GameObject>();
        Formation = FormationTypes.Health;
	}
	
	void Update ()
    {
        // 1 = Line Formation
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedFormation = 1;

            // Sort selection list by formation type
            switch (Formation)
            {
                // Sort based on health
                case FormationTypes.Health:
                    SelectedUnits.Sort(delegate (GameObject unitA, GameObject unitB) 
                    {
                        return (unitA.GetComponent<Unit>().GetCurrentHealth()).CompareTo(unitB.GetComponent<Unit>().GetCurrentHealth());
                    });
                    break;
                // Sort based on defense
                case FormationTypes.Defense:
                    SelectedUnits.Sort(delegate (GameObject unitA, GameObject unitB)
                    {
                        return (unitA.GetComponent<Unit>().Defense).CompareTo(unitB.GetComponent<Unit>().Defense);
                    });
                    break;
                // Sort based on attack
                case FormationTypes.Attack:
                    SelectedUnits.Sort(delegate (GameObject unitA, GameObject unitB)
                    {
                        return (unitA.GetComponent<Unit>().AttackValue).CompareTo(unitB.GetComponent<Unit>().AttackValue);
                    });
                    break;
                default:
                    break;
            }
        }
        // 2 = Square formation
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedFormation = 2;

            // Sort selection list by formation type
            switch (Formation)
            {
                // Sort based on health
                case FormationTypes.Health:
                    SelectedUnits.Sort(delegate (GameObject unitA, GameObject unitB)
                    {
                        return (unitA.GetComponent<Unit>().GetCurrentHealth()).CompareTo(unitB.GetComponent<Unit>().GetCurrentHealth());
                    });
                    break;
                // Sort based on defense
                case FormationTypes.Defense:
                    SelectedUnits.Sort(delegate (GameObject unitA, GameObject unitB)
                    {
                        return (unitA.GetComponent<Unit>().Defense).CompareTo(unitB.GetComponent<Unit>().Defense);
                    });
                    break;
                // Sort based on attack
                case FormationTypes.Attack:
                    SelectedUnits.Sort(delegate (GameObject unitA, GameObject unitB)
                    {
                        return (unitA.GetComponent<Unit>().AttackValue).CompareTo(unitB.GetComponent<Unit>().AttackValue);
                    });
                    break;
                default:
                    break;
            }
        }
    }

    // Add unit to list
    public void AddUnitToSelection(GameObject UnitToAdd)
    {
        // Check unit isnt already in list
        if (!SelectedUnits.Contains(UnitToAdd))
        {
            // Add unit to list
            SelectedUnits.Add(UnitToAdd);
        }
        
    }

    // Remove unit from list
    public void RemoveUnitFromSelection(GameObject UnitToRemove)
    {
        // Check unit is in list
        if (SelectedUnits.Contains(UnitToRemove))
        {
            // Remove unit from list
            SelectedUnits.Remove(UnitToRemove);
        }
    }
    
    // Return unit count
    public int SelectionCount()
    {
        // Return selection count
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
                // Move all units in a line
                Vector3 LinePos = new Vector3(-2f, 0f, 0f);
                for(int i = 0; i < SelectedUnits.Count; i++)
                {
                    LinePos.x += 2.5f;
                    SelectedUnits[i].GetComponent<Unit>().SetTargetDestination(NewDestination + LinePos);
                }
                break;
            // Square formation
            case 2:
                // Move all units in a square

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
                        // Increase to next range
                        gap *= 2;
                        width++;
                        minRange = maxRange + 1;
                        maxRange = minRange + gap;
                    }
                }

                // Calculate postion for each unit in selection
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

    public void SetFormationType(int formationTypeNum)
    {
        Formation = (FormationTypes)formationTypeNum;
    }
}
