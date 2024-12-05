using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_score;
    [SerializeField] GameObject background_music;
    public void setup(int score)
    {
        gameObject.SetActive(true);
        background_music.GetComponent<AudioSource>().volume /= 3;
        text_score.text="Your score: "+score.ToString();
    }
    public void Exit()
    {
        SceneManager.LoadScene("Main1");
    }
    public void ReloadScene()
    {
        background_music.GetComponent<AudioSource>().volume *= 3;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}
