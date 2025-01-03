using TMPro;
using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField] public GameObject associatedObject; // Reference to the Ballista
    TextMeshPro label;
    Color original_color;
    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        original_color= label.color;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            label.color= Color.blue;
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            label.color= original_color;
        }
    }
}
