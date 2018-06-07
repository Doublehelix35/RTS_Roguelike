using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    public GameObject ObjectToSpawn;
	
    public void Break()
    {
        if(ObjectToSpawn != null)
        {
            Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 0.5f);
    }
}
