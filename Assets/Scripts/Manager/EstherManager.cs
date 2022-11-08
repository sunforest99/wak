using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ISEDOL
{
    VIICHAN,
    INE,
    COTTON,
    LILPA,
    JINGBURER,
    GOSEGU
}
public enum ESTHER_BUFF
{
    NONE,
    JINGBURER_BUFF,
    INE_BUFF,
    COTTON_BUFF,
    GOSEGU_BUFF
}
public class EstherManager : MonoBehaviour
{
    [SerializeField] Animator estherAnim;
    [SerializeField] UnityEngine.UI.Image gaugeImg;
    float beforeGauge = 0f;
    float gauge = 0f;
    
    [SerializeField] GameObject appearEffect;       // 에스더 소환되는 이펙트
    [SerializeField] Animator appearLightAnim;      // 에스더 관련 라이트
    [SerializeField] GameObject[] estherAppear;     // 에스더들 등장 일러스트 (ISEDOL enum과 항상 동일)
    [SerializeField] GameObject[] estherSkill;     // 에스더 공격
                                                    //       < 귀상어두 >    |     < 계륵 >
                                                    //        0 : 비챤       |      3 : 릴파
                                                    //        1 : 아이네     |      4 : 징버거
                                                    //        2 : 주르르     |      
    [SerializeField] BuffData[] estherBuffDatas;    // 0: 주르르
                                                    // 1: 고세구

    // 에스더 소환 순서
    // 1. 에스더 소환되는 이펙트
    // 2. 에스더 관련 라이트로 변경
    // 3. 에스더 일러스트 작동
    // 3-2. 에스더 공격 (일러 애니메이션 도중)
    
    public ESTHER_BUFF _esther_buff_state;
    
    void Start()
    {
        GameMng.I.estherManager = this;

        // TODO : 클라마다 Start와 Update 시작이 다름. 방장만 게이지 관리
        setGauge(0);
       
        // NetworkMng.I.myRoom = ROOM_CODE.RAID_0; // TODO : 나중에는 무조건 변경된 이후에야 이곳이 열리기 때문에 지워도 됨
        // if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
    }

    void Update()
    {
        if (gauge < 1f) {
            if (NetworkMng.I.roomOwner) {
                addGauge(0.5f * Time.deltaTime);
            }
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
                        useEsther(ISEDOL.VIICHAN, GameMng.I.character.transform.parent.position, point, true);
                    }
                }
                // 계륵 - 릴파
                else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
                {
                    NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 3, NetworkMng.I.uniqueNumber, 0, 0));
                    useEsther(ISEDOL.LILPA, GameMng.I.character.transform.parent.position, Vector3.zero, true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.X)) {
                // 귀상어두 - 아이네
                if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
                {
                    NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 1, NetworkMng.I.uniqueNumber, 0, 0));
                    useEsther(ISEDOL.INE, GameMng.I.character.transform.parent.position, Vector3.zero, true);
                }
                // 계륵 - 징버거
                else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
                {
                    Vector3 point = getMouseHitPoint();

                    if (!float.IsNegativeInfinity(point.x))
                    {
                        NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 4, NetworkMng.I.uniqueNumber, point.x, point.z));
                        useEsther(ISEDOL.JINGBURER, GameMng.I.character.transform.parent.position, point, true);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                // 귀상어두 - 주르르
                if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
                {
                    NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 2, NetworkMng.I.uniqueNumber, 0, 0));
                    useEsther(ISEDOL.COTTON, GameMng.I.character.transform.parent.position, Vector3.zero, true);
                }
                // 계륵 - 고세구
                else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
                {
                    NetworkMng.I.SendMsg(string.Format("ESTHER:{0}:{1}:{2}:{3}", 5, NetworkMng.I.uniqueNumber, 0, 0));
                    useEsther(ISEDOL.GOSEGU, GameMng.I.character.transform.parent.position, Vector3.zero, true);
                }
            }
        }
    }

    /*
    * @brief 에스더 사용
    * @param estherCode 사용 에스더 코드
    * @param effectPos 에스더 '소환' 이펙트 위치 (사용자 위치)
    * @param spawnPos 에스더 공격 위치
    */
    public void useEsther(ISEDOL estherCode, Vector3 effectPos = new Vector3(), Vector3 spawnPos = new Vector3(), bool isMe = false)
    {
        setGauge(0);
        estherAnim.SetBool("isFull", false);

        // 1. 에스더 소환되는 이펙트
        playAppearEffect(effectPos);

        switch (estherCode)
        {
            case ISEDOL.VIICHAN: // 비챤
                StartCoroutine(useEstherViichan(effectPos.x, spawnPos, isMe));
                break;
            case ISEDOL.INE: // 아이네
                StartCoroutine(useEstherIne(isMe));
                break;
            case ISEDOL.COTTON: // 주르르
                StartCoroutine(useEstherCotton(isMe));
                break;
            case ISEDOL.LILPA: // 릴파
                StartCoroutine(useEstherLilpa(isMe));
                break;
            case ISEDOL.JINGBURER: // 징버거
                StartCoroutine(useEstherJingburger(spawnPos, isMe));
                break;
            case ISEDOL.GOSEGU: // 고세구
                StartCoroutine(useEstherGosegu(isMe));
                break;
        }
    }

    void playAppearEffect(Vector3 effectPos)
    {
        // effectPos.x += (effectPos.x < spawnPos.x) ? -3 : 3;
        effectPos.y = 0;
        appearEffect.transform.position = effectPos;
        appearEffect.SetActive(true);
    }


    public void setGauge(float mount) {
        gauge = mount;
        beforeGauge = gauge;
        gaugeImg.fillAmount = gauge;
    }

    public void addGauge(float mount) {
        gauge += mount;

        // 잦은 에스더 충전 메세지를 방지하기 위해 일정 량 만큼만 킁가해야 메세지 보내도록함
        //     (0.02 면 에스더 한번 충전에 50번 보냄)
        if (gauge >= beforeGauge + 0.02f || gauge >= 1)
        {
            beforeGauge = gauge;
            gaugeImg.fillAmount = gauge;
            NetworkMng.I.SendMsg(string.Format("ESTHER_GAUGE:{0}", gauge));
        }
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


    /*
     * @brief 비챤 에스더 사용
     * @param effectPos 에스더 '소환' 이펙트 위치 (사용자 위치)
     * @param spawnPos 에스더 공격 위치
     * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
     */
    public IEnumerator useEstherViichan(float effectPosX, Vector3 spawnPos = new Vector3(), bool isMe = false)
    {
        // 1. 에스더 소환되는 이펙트
        // effectPos.x += (effectPos.x < spawnPos.x) ? -3 : 3;
        // effectPos.y = 0;
        // appearEffect.transform.position = effectPos;
        // appearEffect.SetActive(true);
        
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("0");
        
        // (0.5초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[(int)ISEDOL.VIICHAN].SetActive(true);

        // (1초 뒤)
        yield return new WaitForSeconds(1);

        // 4. 에스더 공격 (일러 애니메이션 도중
        spawnPos.y = 0;
        estherSkill[0].transform.position = spawnPos;
        if (isMe)
            estherSkill[0].tag = "Esther_Attack_Skill";       // 시전자만 데미지 체크하기
        if (effectPosX < spawnPos.x)
            estherSkill[0].transform.rotation = Quaternion.identity;
        else
            estherSkill[0].transform.rotation = Quaternion.Euler(0, 180, 0);
        estherSkill[0].SetActive(true);
    }

    /*
     * @brief 아이네 에스더 사용
     * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
     */
    public IEnumerator useEstherIne(bool isMe = false)
    {
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("1");

        // (0.5초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[(int)ISEDOL.INE].SetActive(true);

        // 4. 에스더 공격 (일러 애니메이션 도중
        foreach (var user in NetworkMng.I.v_users)
        {
            // 피가 0 이상이면, (살아있으면)
            // if 
            // 아이네 이펙트 생성하고 그 캐릭터한테 붙이기 (쉴드)
            if (user.Value.enabled) {
                GameObject ob = Instantiate(estherSkill[1], user.Value.transform);
                ob.transform.localPosition = Vector3.zero;
                // ob.transform.localScale = new Vector3(2, 2, 2);
            }
        }
        
        GameMng.I.stateMng.user_Shield_Numerical.Add(
            new ShieldBuff(8, Mathf.FloorToInt(GameMng.I.stateMng.user_HP_Numerical.fullHp * 0.9f))
        );
        GameMng.I.stateMng.removeRandomDebuff();
    }

    /*
     * @brief 주르르 에스더 사용
     * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
     */
    public IEnumerator useEstherCotton(bool isMe = false)
    {
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("2");

        // (0.5초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[(int)ISEDOL.COTTON].SetActive(true);

        // 4. 에스더 버프
        foreach (var user in NetworkMng.I.v_users)
        {
            // 피가 0 이상이면, (살아있으면)
            // 주르르 이펙트 생성하고 그 캐릭터한테 붙이기 (쿨감 & 뎀감)
            if (user.Value.enabled) {
                GameObject ob = Instantiate(estherSkill[2], user.Value.transform);
                ob.transform.localPosition = Vector3.zero;
                ob.transform.localScale = new Vector3(2, 2, 2);
            }
        }
        GameMng.I.stateMng.ActiveBuff(estherBuffDatas[0]);

        _esther_buff_state = ESTHER_BUFF.COTTON_BUFF;
        StartCoroutine(removeEstherBuff());
    }

    /*
     * @brief 릴파 에스더 사용
     * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
     */
    public IEnumerator useEstherLilpa(bool isMe = false)
    {
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("3");

        // (0.5초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[(int)ISEDOL.LILPA].SetActive(true);

        // 4. 에스더 공격 (일러 애니메이션 도중
        // Vector3 bossPos = GameMng.I.boss.transform.position;
        // estherSkill[0].transform.position = bossPos;
        // estherSkill[0].transform.parent = GameMng.I.boss.transform;
        estherSkill[3].SetActive(true);

        if (isMe)
            for (int i = 0; i < NetworkMng.I.v_users.Count; i++)
            {
                // NetworkMng.I.SendMsg(string.Format("ESTHER_DAMAGE:{0}:{1}:{2}:{3}", bossPos.x, bossPos.y, bossPos.z, 1000 /* TODO : 데미지 */));
                NetworkMng.I.SendMsg(string.Format("ESTHER_DAMAGE2:{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}", 
                                                    /* TODO : 데미지 */
                                                    1000,1000,1000,1000,1000,
                                                    1000,1000,1000,1000,1000));
            }
    }
    
    /*
     * @brief 징버거 에스더 사용
     * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
     */
    public IEnumerator useEstherJingburger(Vector3 spawnPos = new Vector3(), bool isMe = false)
    {
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("4");

        // (0.5초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[(int)ISEDOL.JINGBURER].SetActive(true);

        // 4. 에스더 공격 (일러 애니메이션 도중
        estherSkill[4].transform.position = spawnPos;
        estherSkill[4].SetActive(true);
        
        GameMng.I.stateMng.removeAllDebuff();
    }

    /*
     * @brief 고세구 에스더 사용
     * @param isMe 시전자인지 유무 (시전자만 데미지 계산 함)
     */
    public IEnumerator useEstherGosegu(bool isMe = false)
    {
        // 2. 에스더 관련 라이트로 변경
        appearLightAnim.SetTrigger("5");

        // (0.5초 뒤)
        yield return new WaitForSeconds(0.5f);

        // 3. 에스더 일러스트 작동
        estherAppear[(int)ISEDOL.GOSEGU].SetActive(true);

        // 4. 버프 적용
        GameMng.I.stateMng.ActiveBuff(estherBuffDatas[1]);

        if (GameMng.I.boss)
            if (GameMng.I.boss._raidtime > 0)
                GameMng.I.boss._raidtime += 30;
        
        _esther_buff_state = ESTHER_BUFF.GOSEGU_BUFF;
        StartCoroutine(removeEstherBuff());
    }
    
    /*
     * @brief 주르르, 고세구 버프 유지시관 관리 
     */
    IEnumerator removeEstherBuff()
    {
        // TODO : 유지시간 고치기
        yield return new WaitForSeconds(10);

        _esther_buff_state = ESTHER_BUFF.NONE;
    }
}