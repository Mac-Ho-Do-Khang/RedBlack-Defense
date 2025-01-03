using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] RBTree tree;
    [SerializeField] GameObject node_object;
    
    Labeler labeler;

    void Awake()
    {
        tree= GetComponent<RBTree>();
        if (!tree) Debug.LogWarning("Cannot find the script RBTree");
        labeler=node_object.GetComponent<Labeler>();
        if (!labeler) Debug.LogWarning("Cannot find the labeler");
    }
    public void OnMouseDown()
    {
        int x = UnityEngine.Random.Range(0, 10);
        tree.Insert(x);
    }
}
