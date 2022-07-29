using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] protected Transform _target;        // <! 나중에 4명 추가하는걸루
    protected Vector3 _dir;      // <! 보스와 타겟 방향

    [SerializeField] protected int _currentHp;      // <! 헌제 체력

     public int _nestingHp;        // <! 중첩 체력

    protected const int _annihilation = 99999;    // <! 전멸기

    [SerializeField] protected bossbaseUI _baseUI;       // <! 보스의 기본 UI 담는 구조체

    protected int _currentNesting;     // <! 지금 체력바

    private float _radetime;       // <! 레이드 시간

    [SerializeField] private bool _isBerserk;        // <! 광폭화

    private float min;      // <! 분
    private float sec;      // <! 초

    private int merginDmg;

    bool isLFlip = false;
    bool isRFlip = false;

    [SerializeField] protected BossData bossdata;

    public int getOnebarHp
    {
        get { return bossdata.startHp / bossdata.maxNesting; }
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

    protected void BossInitialize()
    {
        this._radetime = bossdata.radetime;
        this._baseUI.bossnameText.text = bossdata.bossName;
        this._currentHp = bossdata.startHp;

        _currentNesting = bossdata.maxNesting;

        _frontBarIndex = _StartBarindex;
        _backBarIndex = _frontBarIndex - 1;
    }

    /**
     * @brief 보스 체력이 0 이하일때 음수 표현 하지 않기 위한 함수
     */
    protected void SetZeroHp()
    {
        StartCoroutine(ZeroHpbar());
        _baseUI.bosshpText.text = string.Format("{0} / {1}", 0, bossdata.startHp);
        _baseUI.nestingHp.text = string.Format("X {0}", 0);
    }

    /**
     * @brief 보스 체력바 텍스트 갱신
     */
    protected void ChangeHpText()
    {
        _currentHp = bossdata.startHp - (getOnebarHp * (bossdata.maxNesting - _currentNesting) - _nestingHp);
        _baseUI.bosshpText.text = string.Format("{0} / {1}", _currentHp, bossdata.startHp);
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
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
            this.transform.position += new Vector3(1.8f, 0, 0);
        }
        else if (_dir.x < -1f && !isLFlip)
        {
            isLFlip = true;
            isRFlip = false;
            this.transform.localRotation = Quaternion.identity;
            this.transform.position -= new Vector3(1.8f, 0, 0);
        }

        // if (Vector2.Distance(_target.localPosition, this.transform.localPosition) > 2f)
        // {
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(_target.localPosition.x, _target.localPosition.y - 1.0f, _target.localPosition.z), bossdata.moveSpeed * Time.deltaTime);
        // }
    }

    IEnumerator ZeroHpbar()
    {
        while (_baseUI.hpbar[0].fillAmount >= 0)
        {
            yield return new WaitForEndOfFrame();
            _baseUI.hpbar[0].fillAmount = Mathf.Lerp(_baseUI.hpbar[0].fillAmount, 0, 5 * Time.deltaTime);
        }
    }
}