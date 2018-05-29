using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadScene(string SceneToLoad)
    {
        // Opens game scene
        SceneManager.LoadScene(SceneToLoad);
    }
    
    public void ExitGame()
    {
        // Closes game
        Application.Quit();
    }

}
