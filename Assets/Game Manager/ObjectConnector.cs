using System.Collections.Generic;
using UnityEngine;

public class ObjectConnector : MonoBehaviour
{
    public  GameObject   linePrefab;
    private GameObject   currentLine;
    private LineRenderer lineRenderer;

    RBTreeLogic tree_manager;

    private Transform    startObject; // Object where the line starts (Left/Right)
    private Transform    endObject;   // Object where the line ends (BallistaBody)

    private bool         isDrawing = false;
    // Dictionaries to track active connections
    private Dictionary<Transform, GameObject> activeLines = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> connectedBodies = new Dictionary<Transform, GameObject>();

    private void Start()
    {
        GameObject manager = GameObject.FindWithTag("Manager");
        tree_manager = manager.GetComponent<RBTreeLogic>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("ConnectPoint"))
                {
                    startObject = hit.collider.transform;
                    HandleExistingLine(startObject); // Remove existing line if present
                    StartLine(hit.point);
                }
            }
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                UpdateLine(hit.point);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("BallistaBody")
                    && hit.collider.transform != startObject.parent.parent
                    && !connectedBodies.ContainsKey(hit.collider.transform))
                {
                    endObject = hit.collider.transform;
                    EndLine();
                }
                else
                {
                    CancelLine();
                }
            }
        }
    }

    void HandleExistingLine(Transform connectPoint)
    {
        if (activeLines.ContainsKey(connectPoint))
        {
            GameObject lineToRemove = activeLines[connectPoint];
            LineRenderer lr = lineToRemove.GetComponent<LineRenderer>();
            if (lr != null && lr.positionCount > 1)
            {
                Vector3 endPosition = lr.GetPosition(1);
                foreach (var entry in connectedBodies)
                {
                    if (entry.Key.position == endPosition)
                    {
                        // Update TowerLabeler for the old connection
                        UpdateTowerLabeler(entry.Key, connectPoint, null);
                        connectedBodies.Remove(entry.Key);
                        break;
                    }
                }
            }
            Destroy(lineToRemove);
            activeLines.Remove(connectPoint);
        }
    }

    void StartLine(Vector3 startPosition)
    {
        isDrawing = true;
        currentLine = Instantiate(linePrefab);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
    }

    void UpdateLine(Vector3 currentPosition)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, currentPosition);
        }
    }

    void EndLine()
    {
        isDrawing = false;
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, endObject.position);
        }

        activeLines[startObject] = currentLine;
        connectedBodies[endObject] = currentLine;

        // Update TowerLabeler for the new connection
        UpdateTowerLabeler(endObject, startObject, endObject.gameObject);
    }

    void CancelLine()
    {
        isDrawing = false;
        if (currentLine != null)
        {
            Destroy(currentLine);
        }
    }

    void UpdateTowerLabeler(Transform ballistaBody, Transform connectPoint, GameObject targetBallista)
    {
        // Traverse to the parent Ballista from connectPoint
        Transform parentBallista = connectPoint.parent.parent;
        bool removing_parent = false;

        if (parentBallista != null && parentBallista.TryGetComponent<TowerLabeler>(out TowerLabeler parentLabeler))
        {
            // Determine if it's Left or Right and update the corresponding child
            if (connectPoint.name == "Left")
            {
                // Clear the parent of the previous child if there was one
                if (parentLabeler.left_child != null && parentLabeler.left_child.TryGetComponent<TowerLabeler>(out TowerLabeler previousChildLabeler))
                {
                    //Debug.Log($"Clearing parent of: {previousChildLabeler.gameObject.name}");
                    previousChildLabeler.parent = null;
                    removing_parent = true;
                    //Debug.Log($"Parent of {previousChildLabeler.gameObject.name} after clearing: {previousChildLabeler.parent}");
                }

                // Update left_child
                parentLabeler.left_child = targetBallista;
            }
            else if (connectPoint.name == "Right")
            {
                // Clear the parent of the previous child if there was one
                if (parentLabeler.right_child != null && parentLabeler.right_child.TryGetComponent<TowerLabeler>(out TowerLabeler previousChildLabeler))
                {
                    //Debug.Log($"Clearing parent of: {previousChildLabeler.gameObject.name}");
                    previousChildLabeler.parent = null;
                    removing_parent = true;
                    //Debug.Log($"Parent of {previousChildLabeler.gameObject.name} after clearing: {previousChildLabeler.parent}");
                }

                // Update right_child
                parentLabeler.right_child = targetBallista;
            }
        }

        if (ballistaBody != null && ballistaBody.TryGetComponent<TowerLabeler>(out TowerLabeler childLabeler))
        {
            if (!removing_parent) childLabeler.parent = parentBallista?.gameObject;
        }

        tree_manager.check(); // Retrieve all existing roots and check for valid RBTrees
    }

}
