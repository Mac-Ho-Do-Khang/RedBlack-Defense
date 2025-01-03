using System.Collections;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class Labeler : MonoBehaviour
{
    public int id;
    public int value;
    public int color;
    public int pos_x;
    public int pos_y;

    [SerializeField] Material red;
    [SerializeField] Material black;
    [SerializeField] Material glow;
    [SerializeField] string child_object = "Cylinder";

    [SerializeField] public GameObject parent_node;
    [SerializeField] public LineRenderer lineRenderer;

    TextMeshPro[] labels;
    GameObject cylinder;
    Renderer node_color;

    void Awake()
    {
        labels = GetComponentsInChildren<TextMeshPro>();
        node_color = GetComponentInChildren<Renderer>();

        cylinder = transform.Find(child_object)?.gameObject;
        if (cylinder != null) node_color = cylinder.GetComponent<MeshRenderer>();
        else                  Debug.Log($"Cannot find the children object <{child_object}>");

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            Debug.LogError("Cannot find the LineRenderer component");
        lineRenderer.enabled= false; // The nodeObject will stay for some time before moving. During that, there should be no connecting line
    }

    void Update()
    {
        if (labels.Length >= 2) labels[1].text = value.ToString();
        else                    Debug.Log($"Cannot find TextMeshPro key-value for the node [{id}] -> {value}");

        if (node_color != null)
        {
            if      (color == 1) node_color.material = red;
            else if (color == 0) node_color.material = black;
            else    node_color.material = glow;
        }
        else Debug.Log($"Cannot find Material color for the node [{id}] -> {value}");

        transform.name = $"({value})";

        // Connecting the line renderer
        if (parent_node != null)
        {
            lineRenderer.SetPosition(0, transform.Find(child_object).gameObject.transform.position);
            lineRenderer.SetPosition(1, parent_node.transform.Find(child_object).gameObject.transform.position);
            //Debug.Log($"Node {value} is set to parent {parent_node.GetComponentsInChildren<TextMeshPro>()[1].text}");
        }
        else // if no parent, both ends of the line point to the object itself
        {
            lineRenderer.SetPosition(0, transform.Find(child_object).gameObject.transform.position);
            lineRenderer.SetPosition(1, transform.Find(child_object).gameObject.transform.position);
            //Debug.Log($"Node {value} is set to parent NULL");
        }
            
    }
}
