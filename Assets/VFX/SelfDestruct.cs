using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float time_to_live = 1f;
    void Start()
    {
        Destroy(gameObject,time_to_live);
    }
}
