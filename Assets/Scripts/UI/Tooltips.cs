using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltips : MonoBehaviour
{
    [SerializeField] private GameObject tooltip = null;

    void OnMouseEnter() 
    {
        tooltip.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z);
        tooltip.SetActive(true);
    }

    void OnMouseExit() 
    {
        tooltip.SetActive(false);
    }
}
