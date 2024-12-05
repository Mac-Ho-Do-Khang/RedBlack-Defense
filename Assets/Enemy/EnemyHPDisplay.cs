using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class EnemyHPDisplay : MonoBehaviour
{
    TextMeshPro hp;
    EnemyHealth enemy;
    int n;

    void Awake()
    {
        hp = GetComponent<TextMeshPro>();
        enemy = GetComponentInParent<EnemyHealth>();
        hp.enabled = false;
    }

    void Update()
    {
        hp.text = enemy.currentHitPoints.ToString();
        if (Input.GetKeyDown(KeyCode.H)) hp.enabled = !hp.IsActive();
    }
}
