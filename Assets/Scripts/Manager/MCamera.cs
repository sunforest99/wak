using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCamera : MonoBehaviour
{
    [Range(0.01f, 0.1f)] public float shakeRange = 0.05f;
    [Range(0.1f, 1f)] public float duration = 0.5f;

    void Update()
    {
        if (GameMng.I.isFocusing && GameMng.I.character)
        {
            transform.position = GameMng.I.character.transform.position - new Vector3(0, 0, 10);
        }
    }

    public void shake(float shakeRange = 0.05f)
    {
        this.shakeRange = shakeRange;

        GameMng.I.isFocusing = false;
        InvokeRepeating("startShake", 0, 0.005f);
        Invoke("stopShake", duration);
    }

    void startShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = this.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        this.transform.position = cameraPos;
    }

    void stopShake()
    {
        CancelInvoke("startShake");
        GameMng.I.isFocusing = true;
    }
}
