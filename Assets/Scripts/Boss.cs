using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] protected Transform target;        // <! 나중에 4명 추가하는걸루
    [SerializeField] protected float moveSpeed;
    protected Vector3 dir;      // <! 보스와 타겟과으 방향

    protected string bossName;      // <! 보스이름

    protected float radetime;       // <! 레이드 시간

    protected int startHp;          // <! 보스 총 체력
    protected int currentHp;        // <! 헌제 체력

    [SerializeField] protected bool berserk;        // <! 광폭화

    protected const int annihilation = 99999;    // <! 전멸기

    [SerializeField] protected UnityEngine.UI.Image hpbar;
    [SerializeField] protected TMPro.TextMeshProUGUI bosshpText;
    [SerializeField] protected TMPro.TextMeshProUGUI bossnameText;
    [SerializeField] protected TMPro.TextMeshProUGUI timer;

    [SerializeField] MCamera _camera;

    [SerializeField] GameObject _eff;

    private float min;      // <! 분
    private float sec;      // <! 초

    protected void SetHpText() => bosshpText.text = string.Format("{0} / {1}", 0, startHp);

    protected void ChangeHpText()
    {
        bosshpText.text = string.Format("{0} / {1}", currentHp, startHp);
    }

    protected void ChangeHpbar()
    {
        hpbar.fillAmount = (float)currentHp / startHp;
    }

    protected void RaidTimer()
    {
        radetime -= Time.deltaTime;
        min = radetime / 60;
        sec = radetime % 60;
        timer.text = string.Format("{0} : {1}", Mathf.Floor(min), Mathf.Floor(sec));
    }

    /**
     * @brief 보스 이동
     */
    protected void BossMove()
    {
        currentHp -= 1000;
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
            _camera.Shake();
            Instantiate(_eff, other.ClosestPoint(transform.position) + new Vector2(0, 1f), Quaternion.identity);
        }
    }
}