using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCamera : MonoBehaviour
{
    [SerializeField]
    GameObject myCharacter;

    bool isFocusing = true;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    public float shakeRange = 0.05f;
    [SerializeField]
    [Range(0.1f, 1f)]
    public float duration = 0.5f;

    void Update()
    {
        if (isFocusing)
        {
            transform.position = myCharacter.transform.position - new Vector3(0, 0, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Shake();
        }
    }


    public void Shake()
    {
        isFocusing = false;
        InvokeRepeating("StartShake", 0, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = this.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        this.transform.position = cameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        isFocusing = true;
    }

    public void AAA()
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@@@");
    }
}
