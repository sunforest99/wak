using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ihae : MonoBehaviour
{
    [SerializeField] GameObject eff;
    // [SerializeField] GameObject par
    Vector3 createPos;

    void Start()
    {
        createPos = transform.position;
        createPos.y = 2;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            string[] layers = { "Default" }; 
            int layerMask = LayerMask.GetMask(layers);
            // int layerMask = 1 << LayerMask.NameToLayer("Character");

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hit.transform.name.Equals("Ihae"))
                {
                    Instantiate(eff, createPos, Quaternion.Euler(270, 0, 0), transform.parent);
                }
            }
        }
    }
}
