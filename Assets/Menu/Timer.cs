using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Assign a UI Text component in the Inspector
    private float elapsedTime;
    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Format the time as minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // Display the time
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
