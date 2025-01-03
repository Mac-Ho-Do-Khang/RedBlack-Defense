using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Renderer : MonoBehaviour
{
    [SerializeField] public GameObject parent; // Parent to connect the line to
    private LineRenderer lineRenderer; // Changed to private, initialized in Start()

    private void Start()
    {
        if (parent == null)
        {
            Debug.LogError("Cannot find parent node for the line renderer");
        }

        lineRenderer = GetComponent<LineRenderer>(); // Get the LineRenderer component
        if (lineRenderer == null)
        {
            Debug.LogError("Cannot find the LineRenderer component");
        }
    }

    private void Update()
    {
        if (lineRenderer != null && parent != null)
        {
            // Set positions for the line; ensure it always points to the parent object's position
            lineRenderer.SetPosition(0, transform.position); // Starting point (self)
            lineRenderer.SetPosition(1, parent.transform.position); // End point (parent)
        }
    }
}
