using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBook : MonoBehaviour
{
    [SerializeField] GameObject selectJobUI;

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
                if (hit.transform.name.Equals("book"))
                {
                    selectJobUI.SetActive(true);
                }
            }
        }
    }
}
