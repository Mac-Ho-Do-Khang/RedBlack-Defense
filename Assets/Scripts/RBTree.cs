using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RBTree : MonoBehaviour
{
    // ------------ Configuration ------------
    [SerializeField] int grid_size = 12;

    [SerializeField] GameObject nodePrefab;
    [SerializeField] float transition_delay = 0.5f;
    [SerializeField] float max_transition_delay = 3.01f;
    [SerializeField] string child_object = "Cylinder";

    [SerializeField] GameObject glow_light;
    [SerializeField] float glow_delay = 0.5f;
    [SerializeField] float max_glow_delay = 3.01f;

    [SerializeField] GameObject main_camera;
    [SerializeField] float camera_sensitivity = 1f;
    Vector3 previousMousePosition;
    public float panSpeed = 0.1f;

    [SerializeField] public TMP_Text message;
    [SerializeField] public TMP_Text log;

    public bool is_moving = false;
    GameObject lighter; // point to the searched node (if found)

    // ------------ Interaction --------------
    public TMP_InputField search_input;
    public TMP_InputField insert_input;
    public TMP_InputField delete_input;
    public Button search_button;
    public Button insert_button;
    public Button delete_button;
    public Button insert_random_button;
    public Button delete_random_button;

    public Slider search_speed;
    public TMP_Text search_text;
    Vector3 original_search_speed_pos;
    public Slider transition_speed;
    public TMP_Text transition_text;
    Vector3 original_transition_speed_pos;
    public Slider camera_speed;
    public TMP_Text camera_text;
    Vector3 original_camera_speed_pos;
    float y_distance_text_slider = 26;
    float slider_length = 150;
    float max_value_on_search_slider;
    float max_value_on_transition_slider;
    float max_value_on_camera_slider;
    public void SearchNode()
    {
        if (search_input.text == "") return;
        int x = int.Parse(search_input.text);
        if (root == NIL || root == null)
        {
            message.text = $"Cannot find {x}";
            message.color = Color.red;
            return;
        }
        SearchTree(x, true, false, false);
        reset_state();
        message.text = $"Search {x}";
        message.color = Color.yellow;
    }
    public void InsertNode()
    {
        if (insert_input.text == "") return;
        int x = int.Parse(insert_input.text);
        SearchTree(x, false, true, false);
        Insert(x);
        reset_state();
        message.text = $"Insert {x}";
        message.color = Color.green;
    }
    public void DeleteNode()
    {
        if (delete_input.text == "") return;
        int x = int.Parse(delete_input.text);
        if (root == NIL || root == null)
        {
            message.text = $"Cannot find {x}";
            message.color = Color.red;
            return;
        }
        SearchTree(x, false, false, true);
        Delete(x);
        reset_state(); 
        message.text = $"Delete {x}";
        message.color = Color.blue;
    }
    public void SearchSpeed(float x)
    {
        glow_delay = max_glow_delay - x;
        Vector3 search_speed_text_pos = original_search_speed_pos;
        search_text.text = x.ToString("F2");
        search_speed_text_pos.x += x * slider_length / max_value_on_search_slider;
        search_text.GetComponent<RectTransform>().anchoredPosition = search_speed_text_pos;
    }
    public void TransitionSpeed(float x)
    {
        transition_delay = max_transition_delay - x;
        Vector3 transition_speed_text_pos = original_transition_speed_pos;
        transition_text.text = x.ToString("F2");
        transition_speed_text_pos.x += x * slider_length / max_value_on_transition_slider;
        transition_text.GetComponent<RectTransform>().anchoredPosition = transition_speed_text_pos;
    }
    public void CameraSpeed(float x)
    {
        camera_sensitivity = x;
        Vector3 camera_speed_text_pos = original_camera_speed_pos;
        camera_text.text = camera_sensitivity.ToString("F2");
        camera_speed_text_pos.x += x * slider_length / max_value_on_camera_slider;
        camera_text.GetComponent<RectTransform>().anchoredPosition = camera_speed_text_pos;
    }
    public void RandomInsert()
    {
        //Debug.Log("//////////////////////////////////////////////");
        reset_state();
        x = UnityEngine.Random.Range(0, 100);
        SearchTree(x, false, true, false);
        Insert(x);
        message.text = $"Insert {x}";
        message.color = Color.green;
    }
    public void RandomDelete()
    {
        //Debug.Log("//////////////////////////////////////////////");
        reset_state();
        x = UnityEngine.Random.Range(0, 100);
        SearchTree(x, false, false, true);
        Delete(x);
        message.text = $"Delete {x}";
        message.color = Color.blue;
    }
    public void Awake()
    {
        // Properly place the search speed slider
        original_search_speed_pos = search_speed.GetComponent<RectTransform>().anchoredPosition;
        original_search_speed_pos.y -= y_distance_text_slider;
        original_search_speed_pos.x -= slider_length / 2;
        search_text.GetComponent<RectTransform>().anchoredPosition = original_search_speed_pos;
        // Properly place the transition speed slider
        original_transition_speed_pos = transition_speed.GetComponent<RectTransform>().anchoredPosition;
        original_transition_speed_pos.y -= y_distance_text_slider;
        original_transition_speed_pos.x -= slider_length / 2;
        transition_text.GetComponent<RectTransform>().anchoredPosition = original_transition_speed_pos;
        // Properly place the camera speed slider
        original_camera_speed_pos = camera_speed.GetComponent<RectTransform>().anchoredPosition;
        original_camera_speed_pos.y -= y_distance_text_slider;
        original_camera_speed_pos.x -= slider_length / 2;
        camera_text.GetComponent<RectTransform>().anchoredPosition = original_camera_speed_pos;
    }
    public void Start() 
    {
        max_value_on_search_slider = search_speed.maxValue;
        max_value_on_transition_slider = transition_speed.maxValue;
        max_value_on_camera_slider = camera_speed.maxValue;
        SearchSpeed(2.6f);
        TransitionSpeed(2.6f);
        CameraSpeed(2f);

        NIL = new Node();
        NIL.color = 0; // Black
        NIL.height = -1;
        root = NIL;
    }
    public void Update()
    {
        if (is_moving) // When the animation is happening, no interaction on UI
        {
            GetComponent<BoxCollider>().enabled = false;
            search_button.interactable = false;
            insert_button.interactable = false;
            delete_button.interactable = false;
            insert_random_button.interactable = false;
            delete_random_button.interactable = false;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
            search_button.interactable = true;
            insert_button.interactable = true;
            delete_button.interactable = true;
            insert_random_button.interactable = true;
            delete_random_button.interactable = true;
        }

        // Zooming
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // Scrolled up
        {
            Vector3 temp = main_camera.transform.position;
            temp.y += camera_sensitivity;
            main_camera.transform.position = temp;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Scrolled down
        {
            Vector3 temp = main_camera.transform.position;
            temp.y -= camera_sensitivity;
            main_camera.transform.position = temp;
        }

        // Moving
        if (Input.GetMouseButton(2))
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            Vector3 movement = new Vector3(-mouseDelta.x * panSpeed, -mouseDelta.y * panSpeed, 0);
            main_camera.transform.Translate(movement, Space.Self);
        }
        previousMousePosition = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.Space)) // delete randomly
        {
            x = UnityEngine.Random.Range(0, 100);
            SearchTree(x, false, false, true);
            Delete(x);
        }
    }
    public void OnMouseDown()
    {
        Debug.Log("//////////////////////////////////////////////");
        x = UnityEngine.Random.Range(0, 100);
        SearchTree(x, false, true, false);
        Insert(x);
    }
    public void reset_state() // reset everything neccesary before each action
    {
        message.text = "";
        delete_input.text = "";
        insert_input.text = "";
        search_input.text = "";
        Destroy(lighter);
    }

    // ------------ Node structure --------------
    [SerializeField] int x = 0;
    int i = 0; // the index used sequentially for arr
    int j = 1; // the index used randomly for arr
    public class Node
    {
        public int data;
        public Node parent;
        public Node left;
        public Node right;
        public int color;   // 0 -> Black, 1 -> Red
        public int height;  // Height of the subtree rooted at this node
        public int left_decsendants;
        public int right_decsendants;
        public int pos;     // -1 for left child, 1 for right child
        public int depth;
        public GameObject nodeObject;
        public Labeler labeler;

        public Node(int val = 0)
        {
            data = val;
            parent = null;
            left = null;
            right = null;
            color = 1;  // Red
            height = 0;
            left_decsendants = 0;
            right_decsendants = 0;
            pos = 0;
            nodeObject = null;
            labeler = null;
        }
    }

    public Node root;
    public Node NIL;

    int[] arr = { 76, 83, 6, 13, 77, 21, 38, 34, 98, 53, 21, 84, 30, 68, 35, 22 };
    List<int> array = new List<int>();
    

    // ---------------------- Coroutines ----------------------
    public Dictionary<int, Queue<IEnumerator>> coroutineQueues = new Dictionary<int, Queue<IEnumerator>>();
    public int currentId = 0;
    public int idx = 0;

    public void AddToQueue(int id, IEnumerator coroutine)
    {
        if (!coroutineQueues.ContainsKey(id))
        {
            coroutineQueues[id] = new Queue<IEnumerator>();
        }
        coroutineQueues[id].Enqueue(coroutine);
        //Debug.Log($"---ADD [{idx}] to queue");
        // Start processing if no coroutine is currently running
        if (!is_moving)
        {
            StartCoroutine(ProcessQueue());
        }
    }
    public IEnumerator ProcessQueue()
    {
        is_moving = true;

        while (true)
        {
            // Get the next ID to process
            int nextId = -1;
            foreach (var id in coroutineQueues.Keys)
                if (nextId == -1 || id < nextId) nextId = id;

            if (nextId == -1) break; // No more coroutines to process

            // Process the next coroutine of the nextId
            if (coroutineQueues[nextId].Count > 0)
            {
                //Debug.Log($"<{nextId}>");
                //Debug.Log($"Queue {nextId} - {coroutineQueues[nextId].Count} elements");
                IEnumerator currentCoroutine = coroutineQueues[nextId].Peek();
                if (coroutineQueues[nextId].Count == 1) yield return StartCoroutine(currentCoroutine);
                else                                    StartCoroutine(currentCoroutine);
                coroutineQueues[nextId].Dequeue();
            }

            // Remove the queue if it's empty
            if (coroutineQueues[nextId].Count == 0)
                coroutineQueues.Remove(nextId);
        }

        BFS_Color();
        is_moving = false;
    }
    public IEnumerator SmoothMove(
    GameObject nodeObject,  // Cannot get from node.nodeObject in the cylinder (mesh) case
    Vector3 targetPosition, float duration, Node node)
    {
        //Debug.Log($"{node.data}| x = {node.nodeObject.transform.position.x}, z = {node.nodeObject.transform.position.z}" +
        //    $" -> x = {targetPosition.x}, z = {targetPosition.z}");

        Vector3 startPosition = nodeObject.transform.position;

        // Enable the connecting line
        node.labeler.lineRenderer.enabled = true;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            nodeObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            //Debug.Log($"{elapsedTime}/{duration}");
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log($"[{node.data}]: finished");

        nodeObject.transform.position = targetPosition; // Ensure the final position is set

        // Set parent to establish the line renderer
        if (node.parent != null)
            node.labeler.parent_node = node.parent.nodeObject;
        else node.labeler.parent_node = null;

        node.labeler.color = node.color;
    }

    // ---------------- Traversing ----------------
    public void BFS(bool deletion = false)
    {
        //Debug.Log(str);
        //if(is_moving) return;
        if (root == NIL) return;
        
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            UpdateNode(current, deletion);
            // Enqueue the left child if it exists
            if (current.left != NIL)
            {
                queue.Enqueue(current.left);
            }

            // Enqueue the right child if it exists
            if (current.right != NIL)
            {
                queue.Enqueue(current.right);
            }
        }
    }
    public void BFS_Color()
    {
        //Debug.Log("BFSColor");
        //if(is_moving) return;
        if (root == null || root == NIL) return;

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            current.labeler.color=current.color;
            // Enqueue the left child if it exists
            if (current.left != NIL)
            {
                queue.Enqueue(current.left);
            }

            // Enqueue the right child if it exists
            if (current.right != NIL)
            {
                queue.Enqueue(current.right);
            }
        }
    }
    public void SearchTree(int k,
                           bool search_action = false, // nothing yet
                           bool insert_action = false, // prompt when not found, maintain spot light when found
                           bool delete_action = false  // prompt when not found
                           )
    {
        Debug.Log($"[{idx}] # SEARCH for {k}");

        SearchTreeWithGlow(root, k, glow_delay, search_action, insert_action, delete_action);
    }
    void SearchTreeWithGlow(Node node, int key, float duration,
                            bool search_action, // nothing yet
                            bool insert_action, // prompt when not found, maintain spot light when found
                            bool delete_action  // prompt when not found
                                                        )
    {
        idx += 1;
        Debug.Log($"[{idx}] # Search {node.data}");
        if (node == null || node == NIL)
        {
            if (search_action || delete_action)
                AddToQueue(idx, not_found_delay(key));
            return;
        }

        // Pass the node.nodeObject.transform.position for the spot light before it changes (which is before the instantiation of the light)
        AddToQueue(idx, Glow(node, duration, node.nodeObject.transform.position));

        if (key == node.data)
        {
            AddToQueue(idx, Glow(node, duration, node.nodeObject.transform.position, search_action));
            return;
        }
        if (key < node.data)
        {
            SearchTreeWithGlow(node.left, key, duration, search_action, insert_action, delete_action);
        }
        else
        {
            SearchTreeWithGlow(node.right, key, duration, search_action, insert_action, delete_action);
        }
    }
    public IEnumerator Glow(Node node, float duration, Vector3 position,
                            bool search = false // in case searching a node, remain the spot light for the target node (if found)
        )   
    {
        if (node.nodeObject != null)
        {
            // Cannot instantiate at node.nodeObject.transform.position because by the time placing the spot light, the position
            // of the nodeObject has already been different
            
            if (search) lighter = Instantiate(glow_light, /*node.nodeObject.transform.position*/ position, Quaternion.identity);
            int original_color = node.labeler.color;
            node.labeler.color = 2;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                //Debug.Log($"{elapsedTime}/{duration}");
                yield return new WaitForEndOfFrame();
            }

            node.labeler.color = original_color;
            //Destroy(lighter);
        }
    }
    public IEnumerator not_found_delay(int x) // make sure the message "cannot find x" comes after the search animation
    {
        message.text = $"Cannot find {x}";
        message.color = Color.red;
        yield return null;
    }

    // ---------------- Updating --------------------
    public void UpdateNode(Node node,
        bool update_parent = false // During deletion, we want to update parent even when the position doesn't change (exit right after that)
                                   // During insertion, we want to postpone the update until the smooth transition is finished
        )
    {
        //if(is_moving) return;
        if (node.nodeObject != null)
        {
            // Calculating the target postition
            Vector3 position = new Vector3();
            //int breadth = (node.parent != null) ?
            //    node.pos * (int)Math.Pow(2, node.height + 1) / 2 + (int)node.parent.nodeObject.transform.position.x / grid_size : 0;

            if (node.parent == null) position.x = 0; // node is root
            else
            {
                if (node.pos == -1) // node is a left child
                    position.x = node.pos * (node.right_decsendants +1) * grid_size + node.parent.nodeObject.transform.position.x;
                else                // node is a right child
                    position.x = node.pos * (node.left_decsendants +1) * grid_size + node.parent.nodeObject.transform.position.x;
            }
            position.z = -node.depth * grid_size;

            // If the node's position doesn't change
            if (node.nodeObject.transform.position == position)
            {
                // During deletion, update the parent before exit
                if (update_parent)
                {
                    if (node.parent != null)
                        node.labeler.parent_node = node.parent.nodeObject;
                    else node.labeler.parent_node = null;
                }
                return;
            }

            GameObject cylinder = node.nodeObject.transform.Find(child_object)?.gameObject;
            if (cylinder != null)
            {
                // Detach the "mesh" from the object
                cylinder.transform.parent = null;
                node.nodeObject.transform.position = position;
                // Re-attach the "mesh"
                cylinder.transform.parent = node.nodeObject.transform;

                AddToQueue(idx, SmoothMove(cylinder, position, transition_delay, node));
            }

            //if (node.parent != null) Debug.Log($"---[{node.parent.data}]---");
            //Debug.Log($"{node.data} | x = {node.nodeObject.transform.position.x}, z = {node.nodeObject.transform.position.z}");
            //Debug.Log($"{node.data} | L = {node.left_decsendants}, R = {node.right_decsendants} " +
            //    (node.parent != null ? $"| P_x = {node.parent.nodeObject.transform.position.x} " : $"") +
            //    $"-> P = {node.nodeObject.transform.position.x} ");
        }

    }

    //public void UpdateHeight(Node node, bool update_position = false)
    //{
    //    //if(is_moving) return;
    //    if (node == null)
    //    {
    //        return;
    //    }

    //    int leftHeight = node.left != NIL ? node.left.height : -1;
    //    int rightHeight = node.right != NIL ? node.right.height : -1;
    //    node.height = 1 + Math.Max(leftHeight, rightHeight);

    //    //if (update_position) UpdateNode(node, false);

    //    // Recursively update parent height
    //    if (node.parent != null)
    //    {
    //        UpdateHeight(node.parent);
    //    }
    //}
    public void UpdateDepths(Node node)
    {
        //if(is_moving) return;
        if (node == null)
        {
            return;
        }

        // Update the depth based on the parent
        if (node.parent != null)
        {
            node.depth = node.parent.depth + 1;
        }
        else
        {
            node.depth = 0; // Root node
        }

        // Recursively update left and right children
        UpdateDepths(node.left);
        UpdateDepths(node.right);
    }
    public int Count(Node node)
    {
        if (node == NIL)
            return 0;

        int leftCount = Count(node.left);
        node.left_decsendants = leftCount;
        int rightCount = Count(node.right);
        node.right_decsendants = rightCount;

        return 1 + leftCount + rightCount;
    }
    public void LeftRotate(Node x)
    {
        idx += 1;
        //if(is_moving) return;
        //Debug.Log($"Left rotate: {x.data} [{((x.parent != null) ? x.parent.data : "root")}]");
        Debug.Log($"[{idx}] # Left rotate {x.data}");
        Node y = x.right;
        x.right = y.left;
        if (y.left != NIL)
        {
            y.left.parent = x;
            y.left.pos = 1; // Adjust position after rotation
        }
        y.parent = x.parent;
        if (x.parent == null)
        {
            root = y;
        }
        else if (x == x.parent.left)
        {
            x.parent.left = y;
            y.pos = -1;
        }
        else
        {
            x.parent.right = y;
            y.pos = 1;
        }
        y.left = x;
        x.parent = y;
        x.pos = -1;

        // Update heights
        //UpdateHeight(x);
        //UpdateHeight(y);
        UpdateDepths(root);
        Count(root);
        BFS();
    }
    public void RightRotate(Node x)
    {
        idx += 1;
        //if(is_moving) return;
        //Debug.Log($"Right rotate: {x.data} [{((x.parent != null) ? x.parent.data : "root")}]");
        Debug.Log($"[{idx}] # Right rotate {x.data}");
        Node y = x.left;
        x.left = y.right;
        if (y.right != NIL)
        {
            y.right.parent = x;
            y.right.pos = -1; // Adjust position after rotation
        }
        y.parent = x.parent;
        if (x.parent == null)
        {
            root = y;
        }
        else if (x == x.parent.right)
        {
            x.parent.right = y;
            y.pos = 1;
        }
        else
        {
            x.parent.left = y;
            y.pos = -1;
        }
        y.right = x;
        x.parent = y;
        x.pos = 1;

        // Update heights
        //UpdateHeight(x);
        //UpdateHeight(y);
        UpdateDepths(root);
        Count(root);
        BFS();
    }

    // ---------------- Insertion --------------------
    public void InstantiateNode(Node newNode)
    {
        //if(is_moving) return;
        Vector3 vt = new Vector3(0, 0, 30);
        newNode.nodeObject = Instantiate(nodePrefab, vt, Quaternion.identity);
        newNode.labeler = newNode.nodeObject.GetComponent<Labeler>();
        if (newNode.labeler != null)
        {
            newNode.labeler.enabled = true;
            newNode.labeler.id = newNode.data;
            newNode.labeler.value = newNode.data;
            newNode.labeler.color = 1;
        }
        else Debug.LogError("Labeler component not found on the instantiated node object.");
    }
    public void Insert(int key)
    {
        log.text+= $" {key} ";
        idx += 1;
        Debug.Log($"[{idx}] # Insert {key}");
        Node node = new Node(key)
        {
            left = NIL,
            right = NIL,
            color = 1,
            height = 0
        };

        Node y = null;
        Node x = root;

        while (x != NIL)
        {
            y = x;
            if (node.data < x.data)
            {
                x = x.left;
            }
            else
            {
                x = x.right;
            }
        }

        node.parent = y;
        if (y == null)
        {
            root = node;
        }
        else if (node.data < y.data)
        {
            y.left = node;
            node.pos = -1; // Set position for left child
        }
        else
        {
            y.right = node;
            node.pos = 1; // Set position for right child
        }


        Count(root);

        if (node.parent == null)
        {
            node.color = 0; // Root is always black
            InstantiateNode(node);
            UpdateNode(node, false);
            return;
        }

        node.depth = node.parent.depth + 1;
        InstantiateNode(node);
        UpdateNode(node, false);
        BFS();
        if (node.parent.parent == null)
        {
            //UpdateHeight(y);
            UpdateDepths(root);
            return;
        }

        InsertFix(node);
        //UpdateHeight(y);
        UpdateDepths(root);
    }
    public void InsertFix(Node k)
    {
        //if(is_moving) return;
        Node u;
        while (k.parent.color == 1)
        {
            if (k.parent == k.parent.parent.right)
            {
                u = k.parent.parent.left;
                if (u != null && u.color == 1)
                {
                    u.color = 0;
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    k = k.parent.parent;
                }
                else
                {
                    if (k == k.parent.left)
                    {
                        k = k.parent;
                        RightRotate(k);
                    }
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    LeftRotate(k.parent.parent);
                }
            }
            else
            {
                u = k.parent.parent.right;

                if (u != null && u.color == 1)
                {
                    u.color = 0;
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    k = k.parent.parent;
                }
                else
                {
                    if (k == k.parent.right)
                    {
                        k = k.parent;
                        LeftRotate(k);
                    }
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    RightRotate(k.parent.parent);
                }
            }
            if (k == root)
            {
                break;
            }
        }
        root.color = 0;
    }

    // ---------------- Deletion --------------------
    public void Delete(int data)
    {
        idx++;
        Debug.Log($"Delete [{data}]");
        DeleteHelper(root, data);
    }
    public void DeleteHelper(Node node, int key)
    {
        //if(is_moving) return;
        Node z = NIL;
        Node x, y;
        while (node != NIL)
        {
            if (node.data == key)
            {
                z = node;
            }

            if (node.data <= key)
            {
                node = node.right;
            }
            else
            {
                node = node.left;
            }
        }

        if (z == NIL)
        {
            //Debug.Log("Key not found in the tree");
            //message.text = $"Cannot find {key}";
            //message.color = Color.red;
            return;
        }

        log.text += $" -{key} ";
        y = z;
        int yOriginalColor = y.color;
        if (z.left == NIL)
        {
            x = z.right;
            Transplant(z, z.right);
        }
        else if (z.right == NIL)
        {
            x = z.left;
            Transplant(z, z.left);
        }
        else
        {
            y = Minimum(z.right);
            yOriginalColor = y.color;
            x = y.right;
            if (y.parent == z)
            {
                x.parent = y;
            }
            else
            {
                Transplant(y, y.right);
                y.right = z.right;
                y.right.parent = y;
            }
            Transplant(z, y);
            y.left = z.left;
            y.left.parent = y;
            y.color = z.color;
        }



        idx++;
        AddToQueue(idx, DeleteDelay(z.nodeObject));


        UpdateDepths(root);
        Count(root);
        BFS(true);
        if (yOriginalColor == 0)
        {
            DeleteFix(x);
        }
    }
    public void Transplant(Node u, Node v)
    {
        //if(is_moving) return;
        if (u.parent == null)
        {
            root = v;
        }
        else if (u == u.parent.left)
        {
            u.parent.left = v;
            v.pos = -1; // Adjust position after transplant
        }
        else
        {
            u.parent.right = v;
            v.pos = 1;
        }
        v.parent = u.parent;
        UpdateDepths(root);
        Count(root);
        BFS(true);
        //UpdateHeight(v.parent);
    }
    public void DeleteFix(Node x)
    {
        //if(is_moving) return;
        Node w; // x's sibling
        while (x != root && x.color == 0)
        {
            if (x == x.parent.left)
            {
                w = x.parent.right;
                if (w.color == 1) // If w is red
                {
                    w.color = 0;
                    x.parent.color = 1;
                    LeftRotate(x.parent);
                    w = x.parent.right;
                }

                if (w.left.color == 0 && w.right.color == 0) // If w is black, and w.left & w.right is black
                {
                    w.color = 1;
                    x = x.parent;
                }
                else
                {
                    if (w.right.color == 0) // If w is black, and w.left is red & w.right is black
                    {
                        w.left.color = 0;
                        w.color = 1;
                        RightRotate(w);
                        w = x.parent.right;
                    }
                    // If w is black, w.right is red
                    w.color = x.parent.color;
                    x.parent.color = 0;
                    w.right.color = 0;
                    LeftRotate(x.parent);
                    x = root;
                }
            }
            else
            {
                w = x.parent.left;
                if (w.color == 1) // If w is red
                {
                    w.color = 0;
                    x.parent.color = 1;
                    RightRotate(x.parent);
                    w = x.parent.left;
                }

                if (w.right.color == 0 && w.left.color == 0) // If w is black, and w.left & w.right is black
                {
                    w.color = 1;
                    x = x.parent;
                }
                else
                {
                    if (w.left.color == 0) // If w is black, and w.left is black & w.right is red
                    {
                        w.right.color = 0;
                        w.color = 1;
                        LeftRotate(w);
                        w = x.parent.left;
                    }
                    // If w is black, and w.left is red
                    w.color = x.parent.color;
                    x.parent.color = 0;
                    w.left.color = 0;
                    RightRotate(x.parent);
                    x = root;
                }
            }
        }
        x.color = 0;
        //UpdateHeight(x); // Ensure height is updated after fixes
    }
    public IEnumerator DeleteDelay(GameObject o) // make sure an object is deleted after the search tree animation
    {
        if (o != null)
        {
            Destroy(o);
        }
        yield return null;
    }

    // ---------------- Traversal ----------------
    public void Preorder()
    {
        //if(is_moving) return;
        PreOrderHelper(root);
    }
    public void Inorder()
    {
        //if(is_moving) return;
        InOrderHelper(root);
    }
    public void Postorder()
    {
        //if(is_moving) return;
        PostOrderHelper(root);
    }
    public void PreOrderHelper(Node node)
    {
        //if(is_moving) return;
        if (node != NIL)
        {
            Debug.Log($"{node.data}(H:{node.height}, P:{node.pos}) ");
            PreOrderHelper(node.left);
            PreOrderHelper(node.right);
        }
    }
    public void InOrderHelper(Node node)
    {
        //if(is_moving) return;
        if (node != NIL)
        {
            InOrderHelper(node.left);
            // TODO
            InOrderHelper(node.right);
        }
    }
    public void PostOrderHelper(Node node)
    {
        //if(is_moving) return;
        if (node != NIL)
        {
            PostOrderHelper(node.left);
            PostOrderHelper(node.right);
            Debug.Log($"{node.data}(H:{node.height}, P:{node.pos}) ");
        }
    }

    // ---------------- Display tree ----------------
    public void PrintTree()
    {
        //if(is_moving) return;
        if (root != NIL)
        {
            PrintHelper(root, "", true);
        }
    }
    public void PrintHelper(Node root, string indent, bool last)
    {
        //if(is_moving) return;
        if (root != NIL)
        {
            //Debug.Log(indent);
            //if (last)
            //{
            //    Debug.Log("R----");
            //    indent += "   ";
            //}
            //else
            //{
            //    Debug.Log("L----");
            //    indent += "|  ";
            //}

            string sColor = root.color == 1 ? "RED" : "BLACK";
            //Debug.Log($"{root.data}({sColor}, H:{root.height}, P:{root.pos})");
            Debug.Log($"{root.data}({sColor}, depth:{root.depth}, pos:{root.nodeObject.transform.position.x})");
            PrintHelper(root.left, indent, false);
            PrintHelper(root.right, indent, true);
        }
    }

    // ---------------- Redundants ----------------
    public void InitializeNULLNode(Node node, Node parent)
    {
        //if(is_moving) return;
        node.data = 0;
        node.parent = parent;
        node.left = null;
        node.right = null;
        node.color = 0; // Black
        node.height = -1;
        node.pos = 0;
    }
    //public Node SearchTree(int k)
    //{
    //    return SearchTreeHelper(root, k);
    //}
    //public Node SearchTreeHelper(Node node, int key)
    //{
    //    if (node == NIL || key == node.data)
    //    {
    //        return node;
    //    }
    //    if (key < node.data)
    //    {
    //        return SearchTreeHelper(node.left, key);
    //    }
    //    return SearchTreeHelper(node.right, key);
    //}
    public Node Minimum(Node node)
    {
        while (node.left != NIL)
        {
            node = node.left;
        }
        return node;
    } // used when delete an intermediate node
    public Node Maximum(Node node)
    {
        while (node.right != NIL)
        {
            node = node.right;
        }
        return node;
    }
    public Node Successor(Node x)
    {
        if (x.right != NIL)
        {
            return Minimum(x.right);
        }

        Node y = x.parent;
        while (y != NIL && x == y.right)
        {
            x = y;
            y = y.parent;
        }
        return y;
    }
    public Node Predecessor(Node x)
    {
        if (x.left != NIL)
        {
            return Maximum(x.left);
        }

        Node y = x.parent;
        while (y != NIL && x == y.left)
        {
            x = y;
            y = y.parent;
        }

        return y;
    }
    public int max_height()
    {
        return max_height(root);
    }
    public int max_height(Node node)
    {
        if (node == null)
            return -1; // Base case: height of an empty tree is -1

        int leftHeight = max_height(node.left);
        int rightHeight = max_height(node.right);

        return Math.Max(leftHeight, rightHeight) + 1; // Add 1 for the current node
    }
}