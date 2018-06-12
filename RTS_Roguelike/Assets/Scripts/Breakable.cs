using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    public GameObject ObjectToSpawn; // Object to spawn once broken
    bool SpawnedOnce = false; // Only spawn an object once
	
    public void Break()
    {
        // Check if there is an prefab to spawn and if has been spawned already
        if(ObjectToSpawn != null && !SpawnedOnce)
        {
            // Spawn object at this objects location
            GameObject GO = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            // Move object up on the y axis
            GO.transform.position = new Vector3(GO.transform.position.x, GO.transform.position.y + 0.8f, GO.transform.position.z);
            SpawnedOnce = true;
        }
        // Destroy this object
        Destroy(gameObject, 0.5f);
    }
}
