using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScene : MonoBehaviour
{
    private bool isSecondSceneLoaded = false; // Track if the second scene is loaded

    void Awake()
    {
        // Make this GameObject persistent across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        // Check for the 'S' key to load the second scene
        if (Input.GetKeyDown(KeyCode.S) && !isSecondSceneLoaded)
        {
            LoadSecondScene();
        }

        // Check for the 'D' key to return to the first scene
        if (Input.GetKeyDown(KeyCode.D) && isSecondSceneLoaded)
        {
            ReturnToFirstScene();
        }
    }

    // Load the second scene additively without refreshing the first scene
    public void LoadSecondScene()
    {
        isSecondSceneLoaded = true;

        // Load the "Simulation" scene additively
        SceneManager.LoadScene("Simulation", LoadSceneMode.Additive);
    }

    // Return to the first scene without refreshing it
    public void ReturnToFirstScene()
    {
        if (isSecondSceneLoaded)
        {
            isSecondSceneLoaded = false;

            // Unload the "Simulation" scene
            SceneManager.UnloadSceneAsync("Simulation");
        }
    }
}
