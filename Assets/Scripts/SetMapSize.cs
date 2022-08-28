using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMapSize : MonoBehaviour
{
    [SerializeField] private SpriteRenderer map;
    [SerializeField] BoxCollider boxcol;

    void Start()
    {
        GameMng.I.mapRightTop = boxcol.bounds.center + boxcol.bounds.extents - new Vector3(3, 2.5f, 1.0f);
        GameMng.I.mapLeftBotton = boxcol.bounds.center - boxcol.bounds.extents + new Vector3(3, 3f, 1.0f);
        GameMng.I.mapCenter = boxcol.bounds.center;
        
        Debug.Log("right top" + GameMng.I.mapRightTop);
        Debug.Log("left bottom" + GameMng.I.mapLeftBotton);
    }
}
