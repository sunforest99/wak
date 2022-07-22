using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================================
//                          보류
// =====================================================
[CreateAssetMenu(fileName ="New ItemData", menuName = "Custom/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ITEM itemType;
}
