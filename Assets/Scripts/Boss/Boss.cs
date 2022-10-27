using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public struct bossbaseUI
{
    public UnityEngine.UI.Image[] hpbar;      // <! 보스 체력바
    public TMPro.TextMeshProUGUI bosshpText;        // <! 보스 체력이 얼마나 남았는지
    public TMPro.TextMeshProUGUI bossnameText;      // <! 보스 이름
    public TMPro.TextMeshProUGUI timer;     // <! 레이드 시간
    public TMPro.TextMeshProUGUI nestingHp;        // <! 보스 체력 중첩
}

public class Boss : MonoBehaviour
{
    [Header("[  관리  ]")]  // 관리 ======================================================================================================
    public bool isLFlip = false, isRFlip = false;          // 방향
    [SerializeField] protected BossData bossdata;
    public BossData bossData { get { return bossdata; } }
    [SerializeField] protected Animator animator = null;
    [SerializeField] Transform bossO;      // 본체, (첫번째 자식)
    [SerializeField] public Rigidbody rigid;
    public float _targetDistance;
    [SerializeField] protected float _minDistance;      // 보스가 최대한 갈수 있는 거리
    [SerializeField] protected float _thinkDistance;     // 패턴 하는 거리
    protected StringBuilder messageElement = new StringBuilder();
    [SerializeField] protected List<string> targetList = new List<string>();      // 보스 타겟 설정하는거
    protected string getTarget => targetList[Random.Range(0, targetList.Count)];        // 타겟 렌덤
    [SerializeField] protected Transform _target;   // 나중에 4명 추가하는걸루
    public Transform target { get { return _target; } }

    // 공격 ======================================================================================================
    protected const int _annihilation = 99999;    // <! 전멸기
    [SerializeField] protected bool _isAnnihilation;
    public bool isAnnihilation { get { return _isAnnihilation; } }


    // 버프/디버프 =================================================================================================
    [SerializeField] protected bossbaseUI _baseUI;       // <! 보스의 기본 UI 담는 구조체
    public BossBuff[] bossDeBuffs;


    // 시간 관리 ===================================================================================================
    private float _radetime;        // 레이드 시간
    private float min, sec;         // 초
    [SerializeField] private bool _isBerserk;        // 광폭화


    // 체력 ======================================================================================================
    protected Vector3 _dir;                         // 보스와 타겟 방향
    public int _nestingHp;                          // 중첩 체력
    protected int _currentNesting;                  // 지금 체력바
    [SerializeField] protected int _currentHp;      // 헌제 체력
    private int merginDmg;                          // 여백, 데미지 차이
    public int getOnebarHp
    {
        get { return bossdata.getStartHp / bossdata.maxNesting; }
    }
    protected int _frontBarIndex;
    protected int _backBarIndex;
    protected int _StartBarindex
    {
        get
        {
            if (bossdata.maxNesting % (bossdata.barColor.Length) == 0)
                return 5;
            else
                return bossdata.maxNesting % (bossdata.barColor.Length) - 1;
        }
    }

    /**
     * @brief 보스 초기화
     */
    protected void BossInitialize()
    {
        this._targetDistance = 9999;
        this._radetime = bossdata.radetime;
        this._baseUI.bossnameText.text = bossdata.getName;
        this._currentHp = bossdata.getStartHp;

        _currentNesting = bossdata.maxNesting;

        _frontBarIndex = _StartBarindex;
        _backBarIndex = _frontBarIndex - 1;
    }

    /**
     * @brief 보스 체력이 0 이하일때 음수 표현 하지 않기 위한 함수
     */
    protected void SetZeroHp()
    {
        _baseUI.hpbar[1].enabled = false;

        StartCoroutine(ZeroHpbar());

        _baseUI.bosshpText.text = string.Format("{0} / {1}", 0, bossdata.getStartHp);
        _baseUI.nestingHp.text = string.Format("X {0}", 0);
    }

    /**
     * @brief 보스 체력바 텍스트 갱신
     */
    protected void ChangeHpText()
    {
        _currentHp = bossdata.getStartHp - (getOnebarHp * (bossdata.maxNesting - _currentNesting) - _nestingHp);
        _baseUI.bosshpText.text = string.Format("{0} / {1}", _currentHp, bossdata.getStartHp);
        _baseUI.nestingHp.text = string.Format("X {0}", _currentNesting + 1);
    }

    /**
     * @breif 보스 체력바 줄어들기
     */
    protected void ChangeHpbar()
    {
        if (_nestingHp <= 0)
        {
            if (_currentNesting - 1 <= 0)
            {
                _baseUI.hpbar[0].color = bossdata.barColor[0];
                _baseUI.hpbar[1].enabled = false;
            }
            else
            {
                _baseUI.hpbar[0].color = bossdata.barColor[_frontBarIndex];
                _baseUI.hpbar[1].color = bossdata.barColor[_backBarIndex];
            }
            merginDmg = _nestingHp;
            _nestingHp = getOnebarHp + merginDmg;

            _frontBarIndex--;
            _backBarIndex--;

            if (_backBarIndex < 0)
            {
                _backBarIndex = bossdata.barColor.Length - 1;
            }
            else if (_frontBarIndex < 0)
            {
                _frontBarIndex = bossdata.barColor.Length - 1;
            }

            _currentNesting--;
        }

        _baseUI.hpbar[0].fillAmount = Mathf.Lerp(_baseUI.hpbar[0].fillAmount, (float)_nestingHp / getOnebarHp, 10f * Time.deltaTime);
    }

    /**
     * @breif 레이드 시간 계산
     */
    protected void RaidTimer()
    {
        _radetime -= Time.deltaTime;
        min = _radetime / 60;
        sec = _radetime % 60;
        _baseUI.timer.text = string.Format("{0:00} : {1:00}", Mathf.Floor(min), Mathf.Floor(sec));
        if (min <= 0 && sec <= 0 && !_isBerserk)
        {
            _isBerserk = true;
            Berserk();
        }
    }

    /**
     * @brief 광폭화
     */
    protected void Berserk()
    {
        // bossdata.moveSpeed = 2f;         // <! 이속
    }

    /**
     * @brief 보스 이동
     */
    protected void BossMove()
    {
        _dir = _target.transform.localPosition - this.transform.localPosition;

        if (_dir.x > 1f && !isRFlip)
        {
            isRFlip = true;
            isLFlip = false;
            this.bossO.localRotation = Quaternion.Euler(-20, 180, 0);
            // this.transform.position += new Vector3(1.8f, 0, 0);
        }
        else if (_dir.x < -1f && !isLFlip)
        {
            isLFlip = true;
            isRFlip = false;
            this.bossO.localRotation = Quaternion.Euler(new Vector3(20f, 0, 0));
            // this.transform.position -= new Vector3(1.8f, 0, 0);
        }

        // _targetDistance = Vector3.Distance(_target.position, this.transform.position);
        if (_targetDistance > _minDistance)
        {
            rigid.MovePosition(Vector3.MoveTowards(
                this.transform.position,
                _target.position,
                bossdata.getMoveSpeed * Time.deltaTime));
        }
    }

    IEnumerator ZeroHpbar()
    {
        _baseUI.hpbar[0].enabled = false;
        while (_baseUI.hpbar[0].fillAmount >= 0)
        {
            yield return new WaitForEndOfFrame();
            _baseUI.hpbar[0].fillAmount = Mathf.Lerp(_baseUI.hpbar[0].fillAmount, 0, 5 * Time.deltaTime);
        }
    }

    protected void SendBossPattern(int action, string msg = "")
    {
        // 뒤에 추가로 데이터가 필요로 하지 않은 패턴은 msg를 공백으로 보내서 split을 줄임
        // 만약 이거때문에 버그가 잦다면 붙여도 무관
        NetworkMng.I.SendMsg(string.Format("BOSS_PATTERN:{0}{1}", action, msg != "" ? ":" + msg : msg));
    }


    public void BuffActive(BuffData hitbuffData)
    {
        for (int i = 0; i < bossDeBuffs.Length; i++)
        {
            if (bossDeBuffs[i].gameObject.activeInHierarchy && bossDeBuffs[i].buffData.name == hitbuffData.name)
            {
                bossDeBuffs[i].duration = hitbuffData.duration;
                break;
            }
            else if (!bossDeBuffs[i].gameObject.activeInHierarchy)
            {
                bossDeBuffs[i].buffData = hitbuffData;
                bossDeBuffs[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public virtual void Raid_Start() { }
    public virtual void Action(string msg) { }
}