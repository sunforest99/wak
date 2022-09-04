using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMapSize : MonoBehaviour
{
    [SerializeField] private Vector3 mapRightTop;
    
    [SerializeField] private Vector3 mapLeftBotton;

    void Start()
    {
        GameMng.I.mapRightTop = mapRightTop;
        GameMng.I.mapLeftBotton = mapLeftBotton;
        // GameMng.I.mapCenter = boxcol.bounds.center;
        
        Debug.Log("right top" + GameMng.I.mapRightTop);
        Debug.Log("left bottom" + GameMng.I.mapLeftBotton);
    }
}
