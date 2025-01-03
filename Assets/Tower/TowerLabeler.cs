using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerLabeler : MonoBehaviour
{
    TextMeshPro label;
    TextMeshPro left;
    TextMeshPro right;

    DisplayManager display_manager;
    RBTreeLogic    tree_manager;


    [SerializeField] public int        value;
    [SerializeField] public int        count_nodes; // number of nodes of the tree in which this node belongs to
    [SerializeField] public bool       red;         // true if node is red, false if node is black
    [SerializeField] public GameObject parent;
    [SerializeField] public GameObject left_child;
    [SerializeField] public GameObject right_child;

    // Dectect double click
    private float last_click = 0f;
    private float double_click_threshold = 0.3f; // Time in seconds between clicks to register a double click.

    void Awake()
    {
        GameObject manager = GameObject.FindWithTag("Manager");
        display_manager=manager.GetComponent<DisplayManager>();
        tree_manager= manager.GetComponent<RBTreeLogic>();

        label = transform.Find("Detail/Label").GetComponent<TextMeshPro>();
        left  = transform.Find("Detail/Left").GetComponent<TextMeshPro>();
        right = transform.Find("Detail/Right").GetComponent<TextMeshPro>();

        label.enabled = false;
    }
    private void Start()
    {
        red = true;
    }
    void Update()
    {
        // Toggle visibility
        left.enabled = display_manager.display_connection;
        right.enabled = display_manager.display_connection;
        label.enabled = display_manager.display_tower_label;
        label.text=value.ToString();

        // Red or black
        if (red)
        {
            label.color = Color.red;
            left.color = Color.red;
            right.color = Color.red;
        }
        else
        {
            label.color = Color.black;
            left.color = Color.black;
            right.color = Color.black;
        }
    }

    void OnMouseDown()
    {
        if (Time.time - last_click <= double_click_threshold)
        {
            red = !red;
            tree_manager.check();
        }
        last_click = Time.time;
    }
}
