using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDebuff : MonoBehaviour
{
    // BuffData의 데이터
    public float numerical;                     // 수치
    public int duration;                        // 지속시간 (카운트를 위해 정수)

    // 관리용 데이터
    public float countdown;

    public MonsterDebuff(float numerical, int duration)
    {
        this.numerical = numerical;
        this.duration = duration;
        this.countdown = duration;
    }

    public void resetCountdown()
    {
        this.countdown = this.duration;
    }

    public bool isActive()
    {
        return this.countdown <= this.duration;
    }
}


/*

1. 공격 대상 선택
1-0. 시작은 무조건 랜덤
1-1. 가장 가까운 캐릭터     (40%)
1-2. 가장 먼 캐릭터         (20%)
1-3. 가장 피가 적은 캐릭터  (40%)

2. 패턴 선택
2-1. 공격 대상과 거리 계산해서 시전 가능한 패턴 중 랜덤
2-2. 사용 불가능할 정도로 거리가 멀다면 이동 혹은 이동 공격

*/

public class Monster : MonoBehaviour
{
    [Header("[  몬스터 기본 데이터  ]")]  // =================================================================================================================================
    protected bool isMoving = false;
    protected Transform _target = null;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected Transform _body;         // 스프라이트들 부모 (첫번째 자식, 본체)
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] private List<SpriteRenderer> render = new List<SpriteRenderer>();  // <- 보스와 달리 몬스터는 render 모두 넣어줘야함 (이유: 그림자, hp바 등 모두 자식으로 관리하기때문)
    Dictionary<BUFF, MonsterDebuff> buffDatas = new Dictionary<BUFF, MonsterDebuff>();


    [Space(20)][Header("[  몬스터 개인 데이터  ]")]  // =======================================================================================================================
    protected float _hp;
    protected float _fullHp;
    protected float _moveSpeed;
    protected int _damage;          // (가변) 데미지. 설정된 공격들의 데미지가 여기에 들어감
    protected float _nearness;      // 대상이랑 최소 얼마까지 가까워야 할지 (보통 기본 공격 사정거리 범위보다 조금 적게)
    protected int ATTACK_DAMAGE;    // 공격 데미지
    protected int SKILL_0_DAMAGE;   // 스킬 데미지

    [Space(20)][Header("[  몬스터 UI ]")]  // ===================================================================================================================================
    [SerializeField] GameObject hpbar;

    // 기본적인 반복 계산용
    int damageTemp;
    bool isBackAttack;

    protected virtual void Awake()
    {
        // TODO : 나중에 MONSTER_START 명령어 호출되면 그때
        _target = GameMng.I.character.transform.parent;

        endAct();
    }

    void Start()
    {
        // 퍼플라이트 맵에서 생성되었을때
        if (DungeonMng._dungeon_Type.Equals(DUNGEON_TYPE.MONSTER_PURPLER))
        {
            _hp *= 2;
            _fullHp *= 2;
            ATTACK_DAMAGE = Mathf.FloorToInt(ATTACK_DAMAGE * 1.5f);
            SKILL_0_DAMAGE = Mathf.FloorToInt(SKILL_0_DAMAGE * 1.5f);
        }
    }

    void Update()
    {
        // 몬스터가 가지고 있는 버프들 카운트하기
        foreach (var bf in buffDatas)
        {
            if (bf.Value.isActive())
                bf.Value.countdown += Time.deltaTime;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isMoving)
        {
            if (_target.position.x < transform.position.x)
                _body.transform.rotation = Quaternion.Euler(20, 0, 0);
            else
                _body.transform.rotation = Quaternion.Euler(-20, 180, 0);
                
            // _target 한테 move
            if (Vector3.Distance(_target.position, transform.position) > _nearness)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(
                    transform.position,
                    new Vector3(_target.position.x, transform.position.y, _target.position.z),
                    _moveSpeed * Time.deltaTime
                ));
            }
        }
    }

    /**
     * @brief 행동 지시
     * @param code 행동 번호
     * @param msg 행동 추가 데이터
     */
    public void doSomething(int code, string msg = "")
    {
        isMoving = false;

        string[] txt = msg.Split(':');
        // txt[0] "MONSTER_PATTERN"
        // txt[1] 몬스터 고유 이름
        // txt[2~] 데이터
        
        switch (code)
        {
            case 0:
                // 휴식
                _damage = 0;
                StartCoroutine(think());
                break;
            case 1:
                // 기본공격
                _anim.SetTrigger("Attack");
                attack(msg);
                break;
            case 2:
                // 패턴1
                _anim.SetTrigger("Skill_0");
                skill_0(msg);
                break;
            default:
                break;
        }
    }

    /**
     * @brief 행동 및 애니메이션 종료 (다음 행동 생각하기)
     */
    void endAct()
    {
        _damage = 0;
        isMoving = true;
        StartCoroutine(think());
    }

    /**
     * @brief 타겟과 거리 계산
     * @param uniqueNumber 타겟 고유 번호
     */
    public void setTarget(string uniqueNumber)
    {
        _target = NetworkMng.I.v_users[uniqueNumber].transform.parent;
    }

    /**
     * @brief 타겟과 거리 계산
     * @param target 타겟
     * @return 거리 정도
     */
    protected float getDistanceFromTarget(Vector3 target)
    {
        return Vector3.Distance(target, new Vector3(transform.position.x, target.y, transform.position.z));
    }

    /**
     * @brief 대상 검색
     */
    public void searchTarget()
    {
        int rand = Random.Range(0, 5);
        
        string chooseTarget = NetworkMng.I.uniqueNumber;
        float compareValue = 0;

        switch (rand)
        {
            // 가장 가까운 캐릭터
            case 0:
            case 1:
                compareValue = float.PositiveInfinity;
                foreach (var user in NetworkMng.I.v_users)
                {
                    if (getDistanceFromTarget(user.Value.transform.parent.position) < compareValue)
                    {
                        compareValue = getDistanceFromTarget(user.Value.transform.parent.position);
                        chooseTarget = user.Key;
                    }
                }
                break;
            // 가장 먼 캐릭터
            case 2:
                compareValue = float.NegativeInfinity;
                foreach (var user in NetworkMng.I.v_users)
                {
                    if (getDistanceFromTarget(user.Value.transform.parent.position) > compareValue)
                    {
                        compareValue = getDistanceFromTarget(user.Value.transform.parent.position);
                        chooseTarget = user.Key;
                    }
                }
                break;
            // 가장 피 적은 캐릭터
            case 3:
            case 4:
                Debug.LogError("여길 들어온다고?");
                // compareValue = GameMng.I.stateMng.user_HP_Numerical.Hp;
                // for (int i = 0; i < 4; i++)
                // {
                //     // 파티원들 중
                //     if (GameMng.I.stateMng.PartyHPImg[i].transform.parent.gameObject.activeSelf)
                //     {
                //         // 체력이 가장 적은
                //         if (GameMng.I.stateMng.Party_HP_Numerical[i].Hp < compareValue)
                //         {
                //             compareValue = GameMng.I.stateMng.Party_HP_Numerical[i].Hp;
                //             chooseTarget = GameMng.I.stateMng.PartyName[i].text;        // 닉네임 가져옴
                //         }
                //     }
                // }
                // // 찾은 유저 닉네임을 고유 번호로 변환하기 위해 검색
                // foreach (var user in NetworkMng.I.v_party)
                // {
                //     if (user.Value.nickName.Equals(chooseTarget))
                //     {
                //         chooseTarget = user.Key;
                //         break;
                //     }
                // }
                break;
        }
        NetworkMng.I.SendMsg(string.Format("MONSTER_PATTERN:{0}:{1}:{2}", name, 0, chooseTarget));
    }

    /**
     * @brief 크리티컬 확인
     */
    bool CheckCritical()
    {
        float criticalrand = Random.Range(0.0f, 100.0f);
        float criticalPer = Character._stat.criticalPer;

        if (buffDatas.ContainsKey(BUFF.BUFF_GAL))
        {
            if (buffDatas[BUFF.BUFF_GAL].isActive())
                criticalPer += buffDatas[BUFF.BUFF_GAL].numerical;
        }

        return (criticalrand <= criticalPer);
    }

    /**
     * @brief 버프 활성화
     * @param buffData 버프
     */
    void BuffActive(BuffData buffData)
    {
        if (buffDatas.ContainsKey(buffData.BuffKind))
        {
            // 버프 재 활성화
            buffDatas[buffData.BuffKind].resetCountdown();
        }
        else
        {
            // 처음 받는 버프라면 등록
            buffDatas.Add(buffData.BuffKind, new MonsterDebuff(buffData.numerical, buffData.duration));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Weapon : 캐릭터에게 붙어있는 공격 콜리더 (전사 기본공격 + 전사 스킬들) (힐러 겨울봄)
        // Weapon_disposable : 다른 사람의 일회용(맞으면 사라져야하는) 공격&스킬
        // Weapon_disposable_me : 나의 일회용 공격(일반 공격, 스킬 X) (법사, 힐러 기본공격)
        // Skill : 나의 스킬 공격 (분리된 것들)
        // Skill_disposable_me : 나의 일회용 스킬 공격 (분리된 것들)
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Weapon_disposable_me") || other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me"))
        {
            // 몬스터가 맞은게 공격인지 소환식 혹은 일반 스킬들인지 확인
            SkillData skillData = other.gameObject.CompareTag("Weapon") ?
                    Character.usingSkill : (other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me")) ?
                    GameMng.I.character.skilldatas[int.Parse(other.gameObject.name)] :
                    null /* "Weapon_disposable_me" */;
            
            // 버프 존재하는 스킬에 맞은건지 확인 ======================================================================================================================
            if (skillData && skillData.getBuffData && skillData.getBuffData.isBossDebuf)
            {
                BuffActive(skillData.getBuffData);
            }

            // 크리티컬 계산 ==========================================================================================================================================
            bool isCritical = CheckCritical();

            // 타격 효과 ===============================================================================================================================================
            StartCoroutine(HitBlink());
            if (skillData)
                MCamera.I.shake(skillData.getIntensity, skillData.getShakeTime);
            else if (GameMng.I.userData.job.Equals((int)JOB.WARRIER))
                MCamera.I.shake(5, .1f);
            else    // 법사 or 힐러 평타
                MCamera.I.shake(4, .1f);
            
            // 백어택 상관 있는 백어택 스킬들
            if (skillData && skillData.isBackAttackSkill)
            {
                // Debug.Log("몬스터 위치 : " + _body.position)
                // 보스 우측 바라보는 상태에서  콜리더가 좌측에서 일어남 ===================================================================================================
                if (_body.rotation.y.Equals(180) && this.transform.position.x + 1 > GameMng.I.character.transform.parent.position.x)
                {
                    isBackAttack = true;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(-2f, 1f),
                        other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f),
                        transform.position.z
                    ));
                }
                // 보스 좌측 바라보는 상태에서  콜리더가 우측에서 일어남 ====================================================================================================
                else if (_body.rotation.y.Equals(0) && this.transform.position.x + 1 < GameMng.I.character.transform.parent.position.x)
                {
                    isBackAttack = true;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(1.2f, 2.2f),
                        other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f),
                        transform.position.z
                    ));
                }
                // (백어택 가능 공격인데) 일반 공격 =======================================================================================================================
                else
                {
                    isBackAttack = false;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(-0.1f, 0.1f),
                        other.ClosestPoint(transform.position).y + Random.Range(1.2f, 1.4f),
                        transform.position.z
                    ));
                }
            }
            // 백어택 상관 없는 일반 공격 ======================================================================================================================================
            else
            {
                isBackAttack = false;
                GameMng.I.createEffect(isBackAttack, new Vector3(
                    transform.position.x + Random.Range(-0.1f, 0.1f),
                    other.ClosestPoint(transform.position).y + Random.Range(1.2f, 1.4f),
                    transform.position.z
                ));
            }

            // 평타면서 전사라면 공격 방향으로 밀려나는 효과 주기 ==========================================================================================================
            if (!skillData && GameMng.I.userData.job.Equals((int)JOB.WARRIER))
            {
                // NetworkMng.I.SendMsg(string.Format("FORCE:{0}:{1}", this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, -1.2f));
                GameMng.I.character.addForceImpulse(new Vector3(this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, 0, -1.2f));
            }

            // 데미지 표시 ===================================================================================================================================================
            damageTemp = GameMng.I.getCharacterDamage(
                skillData,
                isCritical,
                isBackAttack
            );

            GameMng.I.createDamage(
                other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0),
                damageTemp,
                isCritical
            );
            // NetworkMng.I.SendMsg(string.Format("DAMAGE:{0}:{1}", this.transform.name, damageTemp));

            getDamage(damageTemp);
        }
    }

    /**
     * @brief 맞을때 호출해주기
     */
    IEnumerator HitBlink()
    {
        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = GameMng.I.materials[1];
        }

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = GameMng.I.materials[0];
        }
    }

    /**
     * @brief 몬스터가 데미지 입었을 때
     */
    public void getDamage(int dmg)
    {
        this._hp -= dmg;
        this.hpbar.transform.localScale = new Vector3(this._hp / this._fullHp, 1, 1);

        if (this._hp <= 0)
        {
            Destroy(gameObject);

            if (Random.Range(0, 10) < 3)        // 30% 확률로 코인도 줌
            {
                GameObject coinObj = Instantiate(
                    GameMng.I.itemObj,
                    transform.position,
                    Quaternion.Euler(20, 0, 0)
                );
                coinObj.GetComponent<ItemObj>().saveItem = 
                    new Item(
                        Resources.Load<ItemData>("ItemData/COIN"),
                        1
                    );
                coinObj.SetActive(true);
            }

            int rand = Random.Range(0, 100);
            if (rand < 34)          // 34% 확률로 호감도 아이템
                rand = Random.Range((int)ITEM_INDEX._FAVORITE_ITEM_INDEX_ + 1, (int)ITEM_INDEX._FAVORITE_ITEM_INDEX_END_);
            else if (rand < 47)     // 13% 확률로 상의 아이템
                rand = Random.Range((int)ITEM_INDEX._SHIRTS_ITEM_INDEX + 1, (int)ITEM_INDEX._SHIRTS_ITEM_INDEX_END_);
            else if (rand < 60)     // 13% 확률로 하의 아이템
                rand = Random.Range((int)ITEM_INDEX._PANTS_ITEM_INDEX + 1, (int)ITEM_INDEX._PANTS_ITEM_INDEX_END_);
            else if (rand < 75)     // 15% 확률로 무기 아이템
                rand = Random.Range((int)ITEM_INDEX._WEAPON_ITEM_INDEX_ + 1, (int)ITEM_INDEX._WEAPON_ITEM_INDEX_END_);
            else                    // 25% 아무것도 안뜸
                return;
            
            GameObject obj = Instantiate(
                GameMng.I.itemObj,
                transform.position,
                Quaternion.Euler(20, 0, 0)
            );
            obj.GetComponent<ItemObj>().saveItem = 
                new Item(
                    Resources.Load<ItemData>(
                        $"ItemData/{((ITEM_INDEX)rand).ToString()}"
                    ),
                    1
                );
            obj.SetActive(true);
        }
    }

    protected virtual int decideAct() { return 0; }
    protected virtual void attack(string msg) {}
    protected virtual void skill_0(string msg) {}
    protected virtual void skill_1(string msg) {}
    protected virtual IEnumerator think() { yield return null; }

    protected virtual void OnDestroy()
    {
        // 던전에서만 몬스터 사망 인식하게끔.  (사망후 씬 이동할때 인식 잘못하는 문제때문)
        if (NetworkMng.I.myRoom < ROOM_CODE._WORLD_MAP_)
            DungeonMng.monsterDie();
    }
}
