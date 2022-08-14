using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstherManager : MonoBehaviour
{
    [SerializeField] Animator estherAnim;
    [SerializeField] UnityEngine.UI.Image gaugeImg;
    float gauge = 0f;
    
    [SerializeField] GameObject[] estherAppear;     // 에스더들 등장 일러스트
    [SerializeField] GameObject appearEffect;       // 에스더 소환되는 이펙트
    [SerializeField] GameObject[] estherAttack;     // 에스더 공격

    void Start()
    {
        GameMng.I.estherManager = this;
        setGauge(0);
    }

    void Update()
    {
        if (gauge < 1f) {
            addGauge(0.5f * Time.deltaTime);
            if (gauge >= 1f) {
                estherAnim.SetBool("isFull", true);
            }
        } else {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z)) {
                // 네트워크로 보내서 사용함. 네트워크 연결후엔 아래와 같이 X
                useEsther(0, GameMng.I.character.transform.position + new Vector3(3, -0.8f, 0), GameMng.I.character.transform.position + new Vector3(3, -0.8f, 0));
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.X)) {
                useEsther(1);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C)) {
                useEsther(2);
            }
        }
    }

    /*
    * @brief 에스더 사용
    * @param estherCode 사용 에스더 코드
    * @param effectPos 에스더 '소환' 이펙트 위치 (사용자 위치)
    * @param spawnPos 사용 에스더 코드
    */
    public void useEsther(int estherCode, Vector2 effectPos = new Vector2(), Vector2 spawnPos = new Vector2()) {
        setGauge(0);
        estherAnim.SetBool("isFull", false);

        // 소환 이펙트 등장
        appearEffect.transform.position = effectPos;
        appearEffect.SetActive(true);

        // 에스더 일러스트 등장        
        estherAppear[estherCode].SetActive(true);

        // 에스더 공격 이펙트 등장  
        // estherAttack[estherCode].transform.position = spawnPos;
        // estherAttack[estherCode].SetActive(true);
    }

    void setGauge(float mount) {
        gauge = mount;
        gaugeImg.fillAmount = gauge;
    }

    public void addGauge(float mount) {
        gauge += mount;
        gaugeImg.fillAmount = gauge;
    }
}
