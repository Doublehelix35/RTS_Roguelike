using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

    public int NumOfUses = 1; // Number of units that can use this potion
    public int StatModifier = 1; // How strong this potion is
    private List<GameObject> PrevUnitList; // List of units that have already used the potion

    public enum PotionType {Health, Attack, Defense}; // Types of potions
    public PotionType PotionSelected;

    private void Start()
    {
        // Init values
        PrevUnitList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if gameobject is a unit and whether its recieved the stat change yet
        if(other.gameObject.tag == "Unit" && PrevUnitList.Contains(other.gameObject) == false)
        {
            // Apply effects of selected potion
            switch (PotionSelected)
            {
                case PotionType.Health:
                    other.GetComponent<Unit>().IncreaseMaxHealth(StatModifier);
                    break;
                case PotionType.Attack:
                    other.GetComponent<Unit>().IncreaseAttack(StatModifier);
                    break;
                case PotionType.Defense:
                    other.GetComponent<Unit>().IncreaseDefense(StatModifier);
                    break;
                default:
                    break;
            }
            // Add unit to prevunit list
            PrevUnitList.Add(other.gameObject);
            // Reduce num of uses
            NumOfUses--;
        }

        // Destroy if out of uses
        if (NumOfUses <= 0)
        {
            Destroy(gameObject);
        }
    }
}
