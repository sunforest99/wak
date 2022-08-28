using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstherManager : MonoBehaviour
{
    [SerializeField] Animator estherAnim;
    [SerializeField] UnityEngine.UI.Image gaugeImg;
    float gauge = 0f;
    
    [SerializeField] GameObject appearEffect;       // 에스더 소환되는 이펙트
    [SerializeField] Animator appearLightAnim;      // 에스더 관련 라이트
    [SerializeField] GameObject[] estherAppear;     // 에스더들 등장 일러스트
    [SerializeField] GameObject[] estherAttack;     // 에스더 공격
    // 에스더 소환 순서
    // 1. 에스더 소환되는 이펙트
    // 2. 에스더 관련 라이트로 변경
    // 3. 에스더 일러스트 작동
    // 3-2. 에스더 공격 (일러 애니메이션 도중)

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
            if (Input.GetKeyDown(KeyCode.Z)) {
                // 네트워크로 보내서 사용함. 네트워크 연결후엔 아래와 같이 X
                StartCoroutine(useEsther(0, GameMng.I.character.transform.position + new Vector3(3, 0, 0), GameMng.I.character.transform.position + new Vector3(3, 0, 0)));
            }
            else if (Input.GetKeyDown(KeyCode.X)) {
                StartCoroutine(useEsther(0, GameMng.I.character.transform.position + new Vector3(3, 0, 0), GameMng.I.character.transform.position + new Vector3(3, 0, 0)));
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
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
    public IEnumerator useEsther(int estherCode, Vector3 effectPos = new Vector3(), Vector3 spawnPos = new Vector3()) {
        setGauge(0);
        estherAnim.SetBool("isFull", false);

        // 1. 에스더 소환되는 이펙트
        effectPos.y = -2.08f;
        appearEffect.transform.position = effectPos;
        appearEffect.SetActive(true);

        // 1초 뒤
        // yield return new WaitForSeconds(1);
        
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger(estherCode.ToString());
        
        // 1초 뒤
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[estherCode].SetActive(true);

        // 1.2초 뒤
        yield return new WaitForSeconds(1);

        // 3-2. 에스더 공격 (일러 애니메이션 도중
        estherAttack[estherCode].transform.position = spawnPos;
        estherAttack[estherCode].SetActive(true);
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
