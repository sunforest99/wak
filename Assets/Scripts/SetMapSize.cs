using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMapSize : MonoBehaviour
{
    [SerializeField] private SpriteRenderer map;

    void Start()
    {
        GameMng.I.mapRightTop = map.bounds.center + map.bounds.extents - new Vector3(3, 2.5f);
        GameMng.I.mapLeftBotton = map.bounds.center - map.bounds.extents + new Vector3(3, 3f);
        GameMng.I.mapCenter = map.bounds.center;
        
        Debug.Log("right top" + GameMng.I.mapRightTop);
        Debug.Log("left bottom" + GameMng.I.mapLeftBotton);
    }
}
