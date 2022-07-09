using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(timeEnd());
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime);
    }

    IEnumerator timeEnd()
    {
        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }
}
