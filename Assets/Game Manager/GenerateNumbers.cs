using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GenerateNumbers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI display_next_number;
    public int[] numbers;
    public int index = 0;
    void Start()
    {
        numbers = Enumerable.Range(0, 100).ToArray();
        ShuffleArray(numbers);
        display_next_number.text = numbers[index].ToString();
    }
    private void ShuffleArray(int[] array)
    {
        System.Random rng = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = rng.Next(i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    public void update_number()
    {
        display_next_number.text = numbers[index + 1].ToString();
    }
}
