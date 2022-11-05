using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
    게임 성공 실패 때 Success / Fail 호출
*/
public class FishingGame : MonoBehaviour
{
    public Bar BarScript;                                       // 게이지 바 초기화를 위한 Bar클래스
    public TextMeshProUGUI stringText;                          // 랜덤 문자열을 적용시킬 텍스트
    public Image TimerImg;                                      // 시간 시각화를 위한 이미지
    public List<char> randomString = new List<char>();          // 랜덤 문자열을 저장할 리스트 (가변적인 길이를 위해 리스트로 구현)

    int stringSize = 10;                                        // 랜덤 문자열 길이
    public int keyCount = 0;                                    // 현재 몇번째 문자열을 입력하는지
    public int difficult = 0;                                   // 난이도
    public float TimeCount = 5;                                 // 시간 카운트

    float percentage = 50.0f;                                   // 낚시 성공률

    public bool isGaming = false;                               // 게임중인지 확인하는 변수
    public bool difficultSetting = false;                       // 난이도 설정 전에 키 입력을 불가능하게 하기 위한 변수


    [SerializeField] Sprite fishWeapon;                     // 낚시대
    [SerializeField] ItemData[] awardListFV;                     // 보상 리스트
    [SerializeField] ItemData[] awardListEQ;                     // 보상 리스트
    [SerializeField] ItemData[] awardListETC;                    // 보상 리스트
    
    Sprite beforeWeapon;
    [SerializeField] GameObject fishTargetObj;
    float countTingTime = 0;
    bool upAndDown = false;

    private void OnEnable()
    {   
        beforeWeapon = GameMng.I.character._weapon.sprite;
        
        countTingTime = 0;
        fishTargetObj.transform.position = GameMng.I.character.transform.parent.position;
        fishTargetObj.transform.position += new Vector3(0, -2, -4);
        fishTargetObj.SetActive(true);
        MCamera.I.setTargetChange(fishTargetObj.transform);
        GameMng.I.character._weapon.sprite = fishWeapon;
        GameMng.I.character._anim.SetBool("Fishing", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))                        // p키 누를 시 게임 시작 (임시)
        {
            if (!isGaming)                                      // 게임 중이지 않을때 p를 누르면 게임 셋팅 시작
            {
                isGaming = true;
                difficultSetting = true;
                BarScript.BarObj.transform.localPosition = new Vector3(0.0f, 150.0f, 0.0f);             // 현재 바의 위치를 이용하여 상하로 이동하는지 확인 (바 이동 범위 -145~140)
                BarScript.BarSpeed = Random.Range(1000f, 2000f);                                        // 바 이동 속도
            }
            else
            {
                isGaming = false;
            }
        }

        if (isGaming)
        {
            if (!difficultSetting)                                                                      // Bar 스크립트에서 난이도 셋팅이 끝나면 키 입력을 받을 수 있음
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    InputKey('W');
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    InputKey('A');
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    InputKey('S');
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    InputKey('D');
                }
            }
        }

        countTingTime += Time.deltaTime;
        if (upAndDown)
            fishTargetObj.transform.position += new Vector3(0, Time.deltaTime / 2, 0);
        else
            fishTargetObj.transform.position -= new Vector3(0, Time.deltaTime / 2, 0);
        if (countTingTime > 1) {
            countTingTime = 0;
            upAndDown = !upAndDown;
        }
    }
    void InputKey(char key)                                                                             // 입력 키를 매개변수로 받아옴
    {
        if (randomString[keyCount] == key)                                                              // 현재 keyCount와 입력값이 같을 때
        {
            for (int i = keyCount + 1; i < randomString.Count; i++)                                     // 텍스트 갱신
            {
                if (i == keyCount + 1)
                    stringText.text = randomString[i] + " ";
                else
                    stringText.text += randomString[i] + " ";
            }
            percentage += 5.0f;                                                                         // 낚시 성공 혹률을 높혀줌
            keyCount++;
        }
        else                                                                                            // 틀리면 게임 종료
        {
            Debug.Log("실패");
            isGaming = false;
        }
        
        if (keyCount == stringSize || !isGaming || TimeCount <= 0)                                      // 키 입력이 종료되는 조건
        {
            TimeCount = 0;
            isGaming = false;
            stringText.text = "확률 : " + percentage + "%";
        }
    }

    public void RandomKey()                                                                             // 랜덤 문자열을 만들어주는 함수
    {
        InitFisingGame();                                                                               // 게임 시작 전 변수 셋팅
        switch (difficult)                                                                              // Bar에서 받아온 난이도로 문자열 길이가 정해짐
        {
            case 0:
                stringSize = 4;
                break;
            case 1:
                stringSize = 6;
                break;
            case 2:
                stringSize = 8;
                break;
            case 3:
                stringSize = 10;
                break;
        }

        for (int i = 0; i < stringSize; i++)                                                            // 문자열 구성
        {
            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    randomString.Add('W');
                    break;
                case 1:
                    randomString.Add('A');
                    break;
                case 2:
                    randomString.Add('S');
                    break;
                case 3:
                    randomString.Add('D');
                    break;
            }

            if (i == 0)
                stringText.text = randomString[0] + " ";
            else
                stringText.text += randomString[i] + " ";
        }
    }

    void InitFisingGame()                                                                               // 변수 셋팅 함수
    {
        randomString.Clear();
        keyCount = 0;
        percentage = 50.0f;
        TimeCount = 5;
    }

    public IEnumerator Timer()                                                                          // 타이머
    {
        TimeCount -= Time.deltaTime;
        if (TimeCount < 0)
            TimeCount = 0;
        TimerImg.fillAmount = TimeCount / 5.0f;
        yield return null;
        if (TimeCount > 0)
            StartCoroutine(Timer());
    }


    void Success()
    {
        // 어려운 난이도 마다 아래 '개당 확률'에 n/2 을 곱함
        // - awardListFV 에서 나올 확률 개당 0.3%     -> 총 개수가 22개이기 때문에 6.6%     -> 10개 성공시 27.5%
        // - awardListEQ 에서 나올 확률 개당 0.1%     -> 총 개수가 24개이기 때문에 2.4%     -> 10개 성공시 12%
        // - awardListETC 에서 나올 확률 개당 0.4%    -> 총 개수가 미정
        // - 위에서 모두 실패시 코인이 나올 확률 80%  (20% 는 꽝)

        // ItemData l;

        // GameMng.I.rewardItem(
        //     l
        // );

    }
    void Fail()
    {
        MCamera.I.setTargetChange(GameMng.I.character.transform);
        GameMng.I.character._weapon.sprite = beforeWeapon;
        GameMng.I.character._anim.SetBool("Fishing", false);
        GameMng.I._keyMode = KEY_MODE.PLAYER_MODE;
        fishTargetObj.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
