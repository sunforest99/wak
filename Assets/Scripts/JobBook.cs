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
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit)
            if (!hit.collider.Equals(null))
            {
                if (hit.collider.name.Equals("book"))
                {
                    selectJobUI.SetActive(true);
                }
            }
        }
    }
}
