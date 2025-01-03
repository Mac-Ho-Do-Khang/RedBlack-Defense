using System.Collections.Generic;
using UnityEngine;

public class RBTreeLogic : MonoBehaviour
{
    public List<GameObject> roots;
    public List<GameObject> GetRootBallista()
    {
        List<GameObject> rootBallista = new List<GameObject>();

        // Find all TowerLabeler components in the scene
        TowerLabeler[] allBallistas = FindObjectsOfType<TowerLabeler>();

        foreach (TowerLabeler ballista in allBallistas)
        {
            if (ballista.parent == null) // Check if the Ballista has no parent
            {
                rootBallista.Add(ballista.gameObject);
            }
        }

        return rootBallista;
    }

    public void check()
    {
        roots = GetRootBallista();
        foreach (GameObject root in roots) { check_each_root(root); }
    }

    public static bool check_each_root(GameObject root)
    {
        if (root == null) return false;

        TowerLabeler rootLabeler = root.GetComponent<TowerLabeler>();
        if (rootLabeler == null || rootLabeler.red)
        {
            // Root must be black for a valid Red-Black Tree
            InvalidateTree(root);
            return false;
        }

        bool isValid = true;

        // Recursive function to validate and calculate the total count of nodes
        int ValidateAndCount(GameObject node, bool parentIsRed, int? minValue, int? maxValue)
        {
            if (node == null) return 1; // Null nodes are black, contribute 1 to black height

            TowerLabeler labeler = node.GetComponent<TowerLabeler>();
            if (labeler == null)
            {
                isValid = false;
                return -1; // Indicate error
            }

            // Check the BST property
            if ((minValue.HasValue && labeler.value <= minValue.Value) ||
                (maxValue.HasValue && labeler.value >= maxValue.Value))
            {
                isValid = false; // Violates the BST property
                return -1; // Indicate error
            }

            // Check Red-Black Property
            if (parentIsRed && labeler.red)
            {
                isValid = false; // No two consecutive red nodes allowed
                return -1; // Indicate error
            }

            // Recursively validate and count black height in left and right subtrees
            int leftBlackHeight = ValidateAndCount(labeler.left_child, labeler.red, minValue, labeler.value);
            int rightBlackHeight = ValidateAndCount(labeler.right_child, labeler.red, labeler.value, maxValue);

            // If either side is invalid, propagate failure
            if (leftBlackHeight == -1 || rightBlackHeight == -1 || leftBlackHeight != rightBlackHeight)
            {
                isValid = false; // Black height must be the same on both sides
                return -1; // Indicate error
            }

            // Increment black height if the current node is black
            int currentBlackHeight = labeler.red ? 0 : 1;

            return currentBlackHeight + leftBlackHeight;
        }


        // Recursive function to update count_nodes for all nodes
        void UpdateNodeCounts(GameObject node, int count)
        {
            if (node == null) return;

            TowerLabeler labeler = node.GetComponent<TowerLabeler>();
            TargetLocator detail = node.GetComponent<TargetLocator>();
            if (labeler != null && detail != null) 
            {
                labeler.count_nodes = count;
                detail.buff_factor = Mathf.Pow((float)1.2, count - 2); // FORMULA
                if (count >= 3) detail.buffed = true;
                else            detail.buffed = false;

                UpdateNodeCounts(labeler.left_child, count);
                UpdateNodeCounts(labeler.right_child, count);
            }
        }

        // Recursive function to set count_nodes to 0 for all nodes
        void InvalidateTree(GameObject node)
        {
            if (node == null) return;

            TowerLabeler labeler = node.GetComponent<TowerLabeler>();
            TargetLocator detail = node.GetComponent<TargetLocator>();
            if (labeler != null && detail != null)
            {
                labeler.count_nodes = 0;
                detail.buffed = false;

                InvalidateTree(labeler.left_child);
                InvalidateTree(labeler.right_child);
            }
        }

        int GetTotalNodeCount(GameObject root_node)
        {
            if (root_node == null) return 0; // No nodes in a null tree

            TowerLabeler labeler = root_node.GetComponent<TowerLabeler>();
            if (labeler == null) return 0; // If no labeler is attached, treat as no nodes

            // Recursively calculate the number of nodes in the left and right subtrees
            int leftCount = GetTotalNodeCount(labeler.left_child);
            int rightCount = GetTotalNodeCount(labeler.right_child);

            // Total nodes = 1 (current node) + left subtree nodes + right subtree nodes
            return 1 + leftCount + rightCount;
        }


        ValidateAndCount(root, false, null, null);
        int totalNodeCount = GetTotalNodeCount(root);
        if (isValid)
        {
            // Update count_nodes for every node in the tree
            UpdateNodeCounts(root, totalNodeCount);
        }
        else
        {
            // If invalid, invalidate the entire tree
            InvalidateTree(root);
        }

        return isValid;
    }
}
