using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PromptScreen : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] GameObject background_music;
    public void setup()
    {
        Time.timeScale = 0f; // Pause the game
        background_music.GetComponent<AudioSource>().volume /= 2;
        gameObject.SetActive(true);
    }
    public void yes()
    {
        Destroy(background_music);
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    public void no() {
        background_music.GetComponent<AudioSource>().volume *= 2;
        Time.timeScale = 1f; // Resume the game
        gameObject.SetActive(false);
    }
}
