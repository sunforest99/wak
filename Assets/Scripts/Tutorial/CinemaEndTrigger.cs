using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CinemaEndTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vCamera;
    // 카메라 흔들림 ==================================================================================
    private CinemachineBasicMultiChannelPerlin cPerlin;
    private float shakeTimer, shakeTimerTotal, startingIntensity;

    void Start()
    {
        cPerlin = _vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shake(5, 1f);
        StartCoroutine(goToTemple());
    }
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
    }

    IEnumerator goToTemple() {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("TempleScene");
    }
}
