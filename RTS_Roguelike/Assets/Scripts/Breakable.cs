using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    public GameObject ObjectToSpawn;
    bool SpawnedOnce = false;
	
    public void Break()
    {
        if(ObjectToSpawn != null && !SpawnedOnce)
        {
            GameObject GO = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            GO.transform.position = new Vector3(GO.transform.position.x, GO.transform.position.y + 0.8f, GO.transform.position.z);
            SpawnedOnce = true;
        }
        Destroy(gameObject, 0.5f);
    }
}
