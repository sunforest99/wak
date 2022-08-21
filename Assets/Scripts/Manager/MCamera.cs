using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCamera : MonoBehaviour
{
    [Range(0.01f, 0.1f)] public float shakeRange = 0.05f;
    [Range(0.1f, 1f)] public float duration = 0.5f;

    // 카메라 안에 들어왔을때 상호작용하는 오브젝트들을 위한 콜리더
    [SerializeField] BoxCollider2D cameraBox;

    void Start()
    {
        // 상호작용하는 오브젝트가 없는 맵은 처리할 필요가 없기 때문에 관둠
        // if (cameraBox)
        //     setCameraCollider();
    }

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

    // void setCameraCollider()
    // {
    //     float sizeY = Camera.main.orthographicSize * 2;
    //     float ratio = (float)Screen.width/(float)Screen.height;
    //     cameraBox.size = new Vector2(sizeY * ratio, sizeY);
    // }
}
