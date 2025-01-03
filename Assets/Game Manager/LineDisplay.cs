using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDisplay : MonoBehaviour
{
    DisplayManager display_manager;
    void Awake()
    {
        GameObject manager = GameObject.FindWithTag("Manager");
        display_manager = manager.GetComponent<DisplayManager>();
    }
    private void Update()
    {
        gameObject.GetComponent<LineRenderer>().enabled = display_manager.display_connection;
    }
}
