using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Shadow : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float posVal;

    Vector2 startPos;
    float newPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, posVal);
        transform.position = startPos + Vector2.right * newPos;
    }
}
