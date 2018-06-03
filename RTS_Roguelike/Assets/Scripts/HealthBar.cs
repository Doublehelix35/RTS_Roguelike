using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public Material GreenMat;
    public Material RedMat;
    private float TransitionPercent = 1.0f;
    private Renderer rend;
    public float WidthMax = 0.8f;

	void Start ()
    {
        // Init Values
        rend = GetComponent<Renderer>();
        rend.material = GreenMat;
	}

	void Update ()
    {
        // Update health bar
        rend.material.Lerp(GreenMat, RedMat, (1.0f - TransitionPercent)); // Transition from green to red based on health
        transform.localScale = new Vector3(WidthMax * TransitionPercent, 0.1f, 0.1f); // Size relative to health percentage
	}

    public void UpdateTransitionPercentage(float NewPercentage)
    {
        TransitionPercent = NewPercentage;
        //Debug.Log("Percent: " + TransitionPercent);
    }
}
