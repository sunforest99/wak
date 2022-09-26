using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackInk : MonoBehaviour
{
    private Vector3 pA;
    private Vector3 pB;
    private Vector3 pC;
    private Vector3[] curvePoints;
    [SerializeField] GameObject ink_eff;
    const int LENGTH = 70;

    IEnumerator throwing()
    {
        for (int i = 0; i < LENGTH + 1; i++)
        {
            yield return new  WaitForSeconds(0.01f);
            transform.position = curvePoints[i];
            transform.Rotate(Vector3.forward * 720 * Time.deltaTime);
        }
        Instantiate(ink_eff, new Vector3(curvePoints[LENGTH].x, -0.197f, curvePoints[LENGTH].z), Quaternion.Euler(90, 0, 0));
        Destroy(this.gameObject);
    }


    private void CalculateCurvePoints(int count)
    {
        curvePoints = new Vector3[count + 1];
        float unit = 1.0f / count;

        int i = 0; float t = 0f;
        for (; i < count + 1; i++, t += unit)
        {
            float u = (1 - t);
            float t2 = t * t;
            float u2 = u * u;

            curvePoints[i] = 
                pA *       u2      + 
                pB * (t  * u  * 2) + 
                pC * t2
            ;
        }
    }

    public void startThrow()
    {
        pA = transform.position;
        pC = GameMng.I.character.transform.parent.position;
        pC.y = 0;
        pB = (pA + pC) * 0.5f;
        pB.y = pA.y + 3;

        CalculateCurvePoints(LENGTH);
        StartCoroutine(throwing());
    }
}
