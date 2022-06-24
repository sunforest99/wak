using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] protected Transform target;        // <! 나중에 4명 추가하는걸루
    [SerializeField] protected float moveSpeed;
    protected Vector3 dir;

    [SerializeField] protected string bossName;

    [SerializeField] protected int startHp;
    protected int currentHp;        // <! 헌제 체력
    
    [SerializeField] protected bool berserk;        // <! 광폭화

    [SerializeField] protected bool isAnnihilation;     // <! 전멸기를 사용중인지 사용중이면 무적
    [SerializeField] protected const int annihilation = 99999;    // <! 전멸기

    [SerializeField] protected UnityEngine.UI.Text bossnameText;
    [SerializeField] protected UnityEngine.UI.Text bosshpText;
    [SerializeField] protected UnityEngine.UI.Image hpbar;

    protected void ChangeHpText()
    {
        bosshpText.text = string.Format("{0} / {1}", currentHp, startHp);
    }

    protected void ChangeHpbar()
    {
        hpbar.fillAmount = (float)currentHp / startHp;
    }

    /**
     * @brief 보스 이동
     */
    protected void BossMove()
    {
        currentHp -= 100;
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
}