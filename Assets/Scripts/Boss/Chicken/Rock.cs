using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Rigidbody rigd = null;
    void Start()
    {
        rigd = GetComponent<Rigidbody>();
        rigd.velocity = transform.TransformDirection(Vector3.down * 3f);
    }
}
