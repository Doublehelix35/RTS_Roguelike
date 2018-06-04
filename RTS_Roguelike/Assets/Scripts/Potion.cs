using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

    public int NumOfUses = 1;
    public int StatModifier = 1;
    private List<GameObject> PrevUnitList; // List of units that have already used the potion

    public enum PotionType {Health, Attack, Defense};
    public PotionType PotionSelected;

    private void Start()
    {
        PrevUnitList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Unit" && PrevUnitList.Contains(other.gameObject) == false)
        {
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
            PrevUnitList.Add(other.gameObject);
            NumOfUses--;
        }

        if (NumOfUses <= 0)
        {
            Destroy(gameObject);
        }
    }
}
