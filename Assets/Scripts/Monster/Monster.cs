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

public class Monster : MonoBehaviour
{
    [Header("[  몬스터 기본 데이터  ]")]  // =================================================================================================================================
    protected bool isMoving = false;
    protected Transform _target = null;
    [SerializeField] protected Animator _anim;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Material[] materials = new Material[2];
    [SerializeField] private List<SpriteRenderer> render = new List<SpriteRenderer>();  // <- 보스와 달리 몬스터는 render 모두 넣어줘야함 (이유: 그림자, hp바 등 모두 자식으로 관리하기때문)
    Dictionary<BUFF, MonsterDebuff> buffDatas = new Dictionary<BUFF, MonsterDebuff>();


    [Space(20)][Header("[  몬스터 개인 데이터  ]")]  // =======================================================================================================================
    protected float _hp;
    protected float _fullHp;
    protected float _moveSpeed;
    protected int _damage;

    [Space(20)][Header("[  몬스터 UI]")]  // ===================================================================================================================================
    [SerializeField] GameObject hpbar;

    // 기본적인 반복 계산용
    int damageTemp;
    bool isBackAttack;

    protected virtual void Start()
    {
        endAct();
    }

    /**
     * @bref 맞을때 호출해주기
     */
    IEnumerator HitBlink()
    {
        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = materials[1];
        }

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = materials[0];
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // _target 한테 move
        }


        foreach (var bf in buffDatas)
        {
            if (bf.Value.isActive())
                bf.Value.countdown += Time.deltaTime;
        }
    }

    public void doSomething(int code)
    {
        isMoving = false;

        switch (code)
        {
            case 0:
                // 이동
                break;
            case 1:
                // 기본공격
                _anim.SetTrigger("Attack");
                break;
            case 2:
                // 패턴1
                _anim.SetTrigger("Skill_0");
                StartCoroutine(skill_0());
                break;
            default:
                break;
        }
    }


    protected virtual IEnumerator skill_0() { yield return null; }
    protected virtual IEnumerator skill_1() { yield return null; }
    protected virtual IEnumerator think() { yield return null; }
    void endAct()
    {
        //if (NetworkMng.I.roomOwner)
        StartCoroutine(think());    // TODO : 방장만 생각하게
    }

    public void setTarget(string uniqueNumber)
    {
        _target = NetworkMng.I.v_users[uniqueNumber].transform;
    }

    void searchTarget()
    {
        //NetworkMng.I.v_users
    }

    bool CheckCritical()
    {
        Debug.Log(buffDatas);
        float criticalrand = Random.Range(0.0f, 100.0f);
        float criticalPer = Character._stat.criticalPer;


        if (buffDatas.ContainsKey(BUFF.BUFF_GAL))
        {
            if (buffDatas[BUFF.BUFF_GAL].isActive())
                criticalPer += buffDatas[BUFF.BUFF_GAL].numerical;
        }

        return (criticalrand <= criticalPer);
    }

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
        // Weapon : 캐릭터에게 붙어있는 공격 콜리더
        // Weapon_disposable : 다른 사람의 일회용(맞으면 사라져야하는) 공격&스킬
        // Weapon_disposable_me : 나의 일회용 공격(일반 공격, 스킬 X)
        // Skill : 나의 스킬 공격 (분리된 것들)
        // Skill_disposable_me : 나의 일회용 스킬 공격 (분리된 것들)
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Weapon_disposable_me") || other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me"))
        {
            // 버프 존재하는 스킬에 맞은건지 확인 ======================================================================================================================
            if (Character.usingSkill && Character.usingSkill.getBuffData && Character.usingSkill.getBuffData.isBossDebuf)
            {
                BuffActive(Character.usingSkill.getBuffData);
            }

            // 크리티컬 계산 ==========================================================================================================================================
            bool isCritical = CheckCritical();

            // 타격 효과 ===============================================================================================================================================
            StartCoroutine(HitBlink());
            MCamera.I.shake(5f, .1f);

            if (Character.usingSkill && Character.usingSkill.isBackAttackSkill)
            {
                // 보스 우측 바라보는 상태에서  콜리더가 좌측에서 일어남 ===================================================================================================
                if (this.transform.localRotation.y == 180 && this.transform.position.x + 1 > other.transform.parent.transform.position.x)
                {
                    isBackAttack = true;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(-2f, 1f),
                        other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f),
                        transform.position.z
                    ));
                }
                // 보스 좌측 바라보는 상태에서  콜리더가 우측에서 일어남 ====================================================================================================
                else if (this.transform.localRotation.y == 0 && this.transform.position.x + 1 < other.transform.parent.transform.position.x)
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
            // 일반 공격 ================================================================================================================================================
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
            if (!Character.usingSkill && GameMng.I.userData.job.Equals((int)JOB.WARRIER))
            {
                NetworkMng.I.SendMsg(string.Format("FORCE:{0}:{1}", this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, -1.2f));
                GameMng.I.character.addForceImpulse(new Vector3(this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, 0, -1.2f));
            }

            // 데미지 표시 ===================================================================================================================================================
            damageTemp = GameMng.I.getCharacterDamage(
                other.gameObject.CompareTag("Weapon") ?
                    Character.usingSkill : (other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me")) ?
                    GameMng.I.character.skilldatas[int.Parse(other.gameObject.name)] :
                    null,       // "Weapon_disposable_me"
                isCritical,
                isBackAttack
            );

            GameMng.I.createDamage(
                other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0),
                damageTemp,
                isCritical
            );
            NetworkMng.I.SendMsg(string.Format("DAMAGE:{0}", damageTemp));
            // boss._nestingHp -= damageTemp;

            getDamage(damageTemp);
        }
    }

    public void getDamage(int dmg)
    {
        this._hp -= dmg;
        this.hpbar.transform.localScale = new Vector3(this._hp / this._fullHp, 1, 1);
        Debug.Log(_hp + " : " + this._fullHp + " : " + this._hp / this._fullHp);
    }
}
