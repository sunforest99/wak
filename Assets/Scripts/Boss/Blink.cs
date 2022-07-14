using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> render = new List<SpriteRenderer>();

    [SerializeField] private Material[] materials = new Material[2];

    SpriteRenderer temp;
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            temp = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (temp)
            {
                render.Add(temp);
            }
        }
    }

    /**
     * @bref 맞을때 호출해주기
     */
    IEnumerator HitBlink()
    {
        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = materials[1];
        }

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = materials[0];
        }
    }
}
