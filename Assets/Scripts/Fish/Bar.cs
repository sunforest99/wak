using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public FishingGame FishingScript;                                                 // 낚시 게임 스크립트
    public GameObject BarObj;                                                       // Bar 위치를 알아오기 위한 변수

    float barSpeed = 1000.0f;
    bool barMove = false;                                                           // Bar가 움직여도 되는지
    bool barUp = false;                                                             // Bar의 방향

    public float BarSpeed                                                           // Fishing에서 사용하기 위한 프로퍼티
    {
        get
        {
            return barSpeed;
        }
        set
        {
            barSpeed = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FishingScript.difficultSetting)                                         // 난이도 설정중이면 바가 움직여도 됨
        {
            barMove = true;
        }
        if (barMove)
            BarScroll();
    }

    void BarScroll()                                                                // 바 위치 관련 함수
    {
        if (BarObj.transform.localPosition.y >= 145.0f)                             // 145보다 클시 방향 전환
            barUp = false;
        else if (BarObj.transform.localPosition.y <= -145.0f)
            barUp = true;

        if (barUp)                                                                  // 바 움직임 제어
            BarObj.transform.Translate(Vector3.up * barSpeed * Time.deltaTime);
        else
            BarObj.transform.Translate(Vector3.down * barSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))                                        // space누를 시 바 멈춤
        {
            barMove = false;
            FishingScript.difficultSetting = false;                                 // 난이도 설정 종료

            if (BarObj.transform.localPosition.y > -130.0f && BarObj.transform.localPosition.y < -50.0f)                // 위치 기반 난이도 설정
                FishingScript.difficult = 1;
            else if (BarObj.transform.localPosition.y > -40.0f && BarObj.transform.localPosition.y < -10.0f)
                FishingScript.difficult = 3;
            else if (BarObj.transform.localPosition.y > 30.0f && BarObj.transform.localPosition.y < 85.0f)
                FishingScript.difficult = 2;
            else
                FishingScript.difficult = 0;

            FishingScript.RandomKey();                                                                                  // 랜덤 문자열 생성
            StartCoroutine(FishingScript.Timer());                                                                      // 타이머 시작
        }
    }
}
