using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemy_count;
    int n = 0;
    void Start()
    {
        enemy_count=GetComponent<TextMeshProUGUI>();
        enemy_count.text = "0";
    }

    public void increase()
    {
        n++;
        enemy_count.text = n.ToString();
    }
}
