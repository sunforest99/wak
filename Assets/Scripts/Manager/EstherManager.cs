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

        NetworkMng.I.myRoom = ROOM_CODE.RAID_0;
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
                // 귀상어두 - 비챤
                if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
                {
                    Vector3 point = getMouseHitPoint();

                    if (!float.IsNegativeInfinity(point.x))
                    {
                        NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 0, NetworkMng.I.uniqueNumber, point.x, point.z));
                        useEsther(0, GameMng.I.character.transform.parent.position, point, true);
                    }
                }
                // 계륵 - 릴파
                else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
                {
                    // ....
                }
            }
            else if (Input.GetKeyDown(KeyCode.X)) {
                // 귀상어두 - 아이네
                if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
                {
                    NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 1, NetworkMng.I.uniqueNumber, 0, 0));
                    useEsther(0, GameMng.I.character.transform.parent.position, Vector3.zero, true);
                }
                // 계륵 - 징버거
                else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
                {
                    Vector3 point = getMouseHitPoint();

                    if (!float.IsNegativeInfinity(point.x))
                    {
                        // ....
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                // 귀상어두 - 주르르
                if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
                {
                    NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 2, NetworkMng.I.uniqueNumber, 0, 0));
                    useEsther(0, GameMng.I.character.transform.parent.position, Vector3.zero, true);
                }
                // 계륵 - 고세구
                else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
                {
                    // ....
                }
            }
        }
    }

    /*
    * @brief 비챤 에스더 사용
    * @param effectPos 에스더 '소환' 이펙트 위치 (사용자 위치)
    * @param spawnPos 에스더 공격 위치
    * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
    */
    public IEnumerator useEstherViichan(Vector3 effectPos = new Vector3(), Vector3 spawnPos = new Vector3(), bool isMe = false)
    {
        // 1. 에스더 소환되는 이펙트
        effectPos.x += (effectPos.x < spawnPos.x) ? -3 : 3;
        effectPos.y = 0;
        appearEffect.transform.position = effectPos;
        appearEffect.SetActive(true);
        
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("0");
        
        // (1초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[0].SetActive(true);

        // (1초 뒤)
        yield return new WaitForSeconds(1);

        // 4. 에스더 공격 (일러 애니메이션 도중
        spawnPos.y = 0;
        estherAttack[0].transform.position = spawnPos;
        if (isMe)
            estherAttack[0].tag = "Esther_Attack_Skill";       // 시전자만 데미지 체크하기
        estherAttack[0].SetActive(true);
    }

    /*
    * @brief 에스더 사용
    * @param estherCode 사용 에스더 코드
    * @param effectPos 에스더 '소환' 이펙트 위치 (사용자 위치)
    * @param spawnPos 에스더 공격 위치
    */
    public void useEsther(int estherCode, Vector3 effectPos = new Vector3(), Vector3 spawnPos = new Vector3(), bool isMe = false)
    {
        setGauge(0);
        estherAnim.SetBool("isFull", false);

        switch (estherCode)
        {
            case 0: // 비챤
                StartCoroutine(useEstherViichan(effectPos, spawnPos, isMe));
                break;
            case 1: // 아이네
                break;
            case 2: // 주르르
                break;
            case 3: // 릴파
                break;
            case 4: // 징버거
                break;
            case 5: // 고세구
                break;
        }
    }

    void setGauge(float mount) {
        gauge = mount;
        gaugeImg.fillAmount = gauge;
    }

    public void addGauge(float mount) {
        gauge += mount;
        gaugeImg.fillAmount = gauge;
    }

    Vector3 getMouseHitPoint()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Map");

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 100f, layerMask)) {
            return hit.point;
        }
        return Vector3.negativeInfinity;
    }

}
