using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public Material GreenMat;
    public Material RedMat;
    private float TransitionPercent = 1.0f;
    private Renderer rend;

	void Start ()
    {
        rend = GetComponent<Renderer>();
        rend.material = GreenMat;
	}

	void Update ()
    {
        rend.material.Lerp(GreenMat, RedMat, (1.0f - TransitionPercent));
	}

    public void UpdateTransitionPercentage(float NewPercentage)
    {
        TransitionPercent = NewPercentage;
        Debug.Log("Percent: " + TransitionPercent);
    }
}
