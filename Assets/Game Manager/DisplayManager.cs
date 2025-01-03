using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] public bool display_enemy_health = false;
    [SerializeField] public bool display_tower_label = false;
    [SerializeField] public bool display_connection = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) display_enemy_health = !display_enemy_health;
        if (Input.GetKeyDown(KeyCode.T)) display_tower_label = !display_tower_label;
        if (Input.GetKeyDown(KeyCode.G)) display_connection = !display_connection;
    }
    public void toggle_enemy_health()
    {
        display_enemy_health = !display_enemy_health;
    }
    public void toggle_tower_label()
    {
        display_tower_label = !display_tower_label;
    }
    public void toggle_connection()
    {
        display_connection = !display_connection;
    }
}
