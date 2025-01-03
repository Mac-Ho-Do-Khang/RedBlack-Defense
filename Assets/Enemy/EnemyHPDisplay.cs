using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class EnemyHPDisplay : MonoBehaviour
{
    DisplayManager displayManager;
    TextMeshPro hp;
    EnemyHealth enemy;

    void Awake()
    {
        GameObject manager = GameObject.FindWithTag("Manager");
        displayManager = manager.GetComponent<DisplayManager>();
        hp = GetComponent<TextMeshPro>();
        enemy = GetComponentInParent<EnemyHealth>();
        hp.enabled = false;
    }

    void Update()
    {
        hp.text = enemy.currentHitPoints.ToString();
        hp.enabled = displayManager.display_enemy_health;
    }
}
