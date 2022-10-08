using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MCamera : MonoBehaviour
{
    // [Range(0.01f, 0.1f)] public float shakeRange = 0.05f;
    // [Range(0.1f, 1f)] public float duration = 0.5f;

    public static MCamera I { get; private set; }
    public CinemachineVirtualCamera _vCamera;

    // 카메라 흔들림 ==================================================================================
    private CinemachineBasicMultiChannelPerlin cPerlin;
    private float shakeTimer, shakeTimerTotal, startingIntensity;

    // 카메라 줌인/아웃
    private float zoomTimer, zoomTimerTotal;
    private float zoomFrom, zoomTo;

    private void Awake() {
        I = this;
        _vCamera = GetComponent<CinemachineVirtualCamera>();
        cPerlin = _vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start() {
        StartCoroutine(foundFollowTarget());
    }

    IEnumerator foundFollowTarget()
    {
        yield return null;
        if (GameMng.I.character)
            _vCamera.Follow = GameMng.I.character.transform;
        else
            StartCoroutine(foundFollowTarget());
    }

    /*
     * @breif 화면 흔들림
     * @param intensity 정도, 5가 평균
     * @param time 지속시간, 0.2가 평균
     */
    public void shake(float intensity, float time)
    {
        cPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update() {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            cPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
        if (zoomTimer > 0)
        {
            zoomTimer -= Time.deltaTime;
            _vCamera.m_Lens.FieldOfView = Mathf.Lerp(zoomFrom, zoomTo, 1 - (zoomTimer / zoomTimerTotal));
        }
    }

    public void setTargetChange(Transform target) {
        _vCamera.Follow = target;
    }

    /*
     * @breif 화면 줌인. NPC 선택 모드일때 사용 (적은 줌인)
     * @param time 줌인 속도
     */
    public void zoomIn(float time = 0.4f)
    {
        zoomTimer = time;
        zoomTimerTotal = time;
        // zoomFrom = 30;
        zoomFrom = _vCamera.m_Lens.FieldOfView;
        zoomTo = 26;
    }
    /*
     * @breif 화면 줌인2. NPC 대화 모드일때 사용 (큰 줌인)
     * @param time 줌인 속도
     */
    public void zoomIn2(float time = 0.4f)
    {
        zoomTimer = time;
        zoomTimerTotal = time;
        // zoomFrom = 26;
        zoomFrom = _vCamera.m_Lens.FieldOfView;
        zoomTo = 20;
    }
    /*
     * @breif 화면 줌아웃. NPC 선택 모드일때 사용 (적은 줌아웃)
     * @param time 줌아웃 속도
     */
    public void zoomOut(float time = 0.4f)
    {
        zoomTimer = time;
        zoomTimerTotal = time;
        // zoomFrom = 26;
        zoomFrom = _vCamera.m_Lens.FieldOfView;
        zoomTo = 30;
    }
    /*
     * @breif 화면 줌아웃2. NPC 대화 모드일때 사용 (큰 줌아웃)
     * @param time 줌아웃 속도
     */
    public void zoomOut2(float time = 0.4f)
    {
        zoomTimer = time;
        zoomTimerTotal = time;
        // zoomFrom = 20;
        zoomFrom = _vCamera.m_Lens.FieldOfView;
        zoomTo = 30;
    }

    // void Start()
    // {
    //     // 상호작용하는 오브젝트가 없는 맵은 처리할 필요가 없기 때문에 관둠
    //     // if (cameraBox)
    //     //     setCameraCollider();
    // }

    // void Update()
    // {
    //     if (GameMng.I.isFocusing && GameMng.I.character)
    //     {
    //         transform.position = GameMng.I.character.transform.position - new Vector3(0, 0, 10);
    //     }
    // }

    // public void shake(float shakeRange = 0.05f)
    // {
    //     this.shakeRange = shakeRange;

    //     GameMng.I.isFocusing = false;
    //     InvokeRepeating("startShake", 0, 0.005f);
    //     Invoke("stopShake", duration);
    // }

    // void startShake()
    // {
    //     float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
    //     float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
    //     Vector3 cameraPos = this.transform.position;
    //     cameraPos.x += cameraPosX;
    //     cameraPos.y += cameraPosY;
    //     this.transform.position = cameraPos;
    // }

    // void stopShake()
    // {
    //     CancelInvoke("startShake");
    //     GameMng.I.isFocusing = true;
    // }

    // // void setCameraCollider()
    // // {
    // //     float sizeY = Camera.main.orthographicSize * 2;
    // //     float ratio = (float)Screen.width/(float)Screen.height;
    // //     cameraBox.size = new Vector2(sizeY * ratio, sizeY);
    // // }
}
