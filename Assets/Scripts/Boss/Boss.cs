using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct bossbaseUI
{
    public UnityEngine.UI.Image[] hpbar;      // <! 보스 체력바
    public TMPro.TextMeshProUGUI bosshpText;        // <! 보스 체력이 얼마나 남았는지
    public TMPro.TextMeshProUGUI nestingHp;        // <! 보스 체력 중첩
    public TMPro.TextMeshProUGUI bossnameText;      // <! 보스 이름
    public TMPro.TextMeshProUGUI timer;     // <! 레이드 시간
}

public class Boss : MonoBehaviour
{
    [SerializeField] protected Transform target;        // <! 나중에 4명 추가하는걸루
    [SerializeField] protected float moveSpeed;
    protected Vector3 dir;      // <! 보스와 타겟 방향

    protected string bossName;      // <! 보스이름

    protected float radetime;       // <! 레이드 시간

    [SerializeField] protected int startHp;          // <! 보스 총 체력
    [SerializeField] protected int nestingHp;        // <! 중첩 체력
    protected int currentHp;      // <! 헌제 체력
    
    [SerializeField] protected bool isBerserk;        // <! 광폭화

    protected const int annihilation = 99999;    // <! 전멸기

    [SerializeField] protected bossbaseUI baseUI;       // <! 보스의 기본 UI 담는 구조체

    [SerializeField] MCamera _camera;

    [SerializeField] GameObject _eff;

    [SerializeField] protected int nesting;       // <! 체력바 중첩
    protected int nestingCount;
    private float min;      // <! 분
    private float sec;      // <! 초
    private bool change;    // <! 체력바 앞뒤 바꾸기

    private int merginDmg;

    public int getOnebarHp
    {
        get { return startHp / nesting; }
    }

    /**
     * @brief 보스 체력이 0 이하일때 음수 표현 하지 않기 위한 함수
     */
    protected void SetZeroHpText()
    {
        baseUI.bosshpText.text = string.Format("{0} / {1}", 0, startHp);
        baseUI.nestingHp.text = string.Format("X {0}", 0);
    }

    /**
     * @brief 보스 체력바 텍스트 갱신
     */
    protected void ChangeHpText()
    {
        currentHp = startHp - (getOnebarHp * (nesting - nestingCount) - nestingHp);
        baseUI.bosshpText.text = string.Format("{0} / {1}", currentHp, startHp);
        baseUI.nestingHp.text = string.Format("X {0}", nestingCount);
    }

    /**
     * @breif 보스 체력바 줄어들기
     */
    protected void ChangeHpbar()
    {
        if(nestingHp <= 0)
        {
            nestingCount--;
            merginDmg = nestingHp;
            nestingHp = getOnebarHp + merginDmg;
            change = change ? false : true;
        }

        void Changebar(int index)
        {
            baseUI.hpbar[index].rectTransform.SetSiblingIndex(0);
            baseUI.hpbar[index].fillAmount = 1;
        }

        if(!change)
        {
            Changebar(1);
            baseUI.hpbar[0].fillAmount = Mathf.Lerp(baseUI.hpbar[0].fillAmount, (float) nestingHp / getOnebarHp, 10f * Time.deltaTime);     // <! 속도조절 하기
        }
        else
        {
            Changebar(0);
            baseUI.hpbar[1].fillAmount =  Mathf.Lerp(baseUI.hpbar[1].fillAmount, (float) nestingHp / getOnebarHp,10f * Time.deltaTime);     // <! 속도조절 하기
        }
    }

    /**
     * @breif 레이드 시간 계산
     */
    protected void RaidTimer()
    {
        radetime -= Time.deltaTime;
        min = radetime / 60;
        sec = radetime % 60;
        baseUI.timer.text = string.Format("{0} : {1}", Mathf.Floor(min), Mathf.Floor(sec));

        if(min <= 0 && sec <= 0)
            isBerserk = true;
    }

    /**
     * @brief 광폭화
     */
    protected void Berserk()
    {

    }

    /**
     * @brief 보스 이동
     */
    protected void  BossMove()
    {
        dir = target.transform.localPosition - this.transform.localPosition;

        if (dir.x > 0)
        {
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.localRotation = Quaternion.identity;
        }

        if (Vector2.Distance(target.localPosition, this.transform.localPosition) > 2f)
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, target.localPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            _camera.shake();
            Instantiate(_eff, other.ClosestPoint(transform.position) + new Vector2(0, 1f), Quaternion.identity);
        }
    }
}