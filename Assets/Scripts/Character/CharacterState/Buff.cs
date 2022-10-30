using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Buff : Tooltips, IPointerEnterHandler, IPointerExitHandler
{
    public BuffData buffData;
    [SerializeField] TextMeshProUGUI Mount;

    public UnityEngine.UI.Image BuffImg;

    public int duration;                     // 버프가 유지된 카운트
    public int count = 1;                           // 중첩 갯수

    public bool isApply;

    void OnEnable()
    {
        BuffImg.sprite = buffData.buffsprite;
        duration = buffData.duration;
        StartCoroutine(cool());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        base.tooltip.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z);
        base.tooltip.SetActive(true);
        base.toolTipSetting(buffData.buffsprite, buffData.buffname, buffData.buffcontent);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        base.tooltip.SetActive(false);
    }
    
    private void OnDisable()
    {
        base.tooltip.SetActive(false);
    }

    IEnumerator cool()
    {
        yield return new WaitForSeconds(1.0f);
        if (duration == 3)
        {
            duration--;
            StartCoroutine(cool());
            StartCoroutine(Blink(0.5f));
        }
        else if (duration >= 1)
        {
            duration--;
            StartCoroutine(cool());
        }
        else
        {
            // TODO : 이게 아마 디버프 해제 일텐데
            // if(!buffData.check_buff)
            //     StateMngSc.nPlayerDeBuffCount--;
            GameMng.I.stateMng.nPlayerBuffCount--;
            isApply = false;
            count = 1;
            gameObject.SetActive(false);
        }
        // TODO : 중첩 하기
        if (!buffData.isBuff && buffData.check_nesting)
            Mount.text = 'x' + count.ToString();

         if (NetworkMng.I.isRaidRoom())
            if (buffData.BuffKind.Equals(BUFF.DEBUFF_SMELL)) {
                GameMng.I.stateMng.takeDamage(Mathf.FloorToInt(1234 * (1 + 0.2f * count)));
            }
    }

    public IEnumerator Blink(float time)
    {
        Color color = BuffImg.color;
        while (color.a > 0.5f)
        {
            if (duration > 3)
            {
                color.a = 1;
                BuffImg.color = color;
                break;
            }
            color.a -= Time.deltaTime / time;
            BuffImg.color = color;
            yield return null;
        }
        while (color.a < 1f)
        {
            if (duration > 3)
            {
                color.a = 1;
                BuffImg.color = color;
                break;
            }
            color.a += Time.deltaTime / time;
            BuffImg.color = color;
            yield return null;
        }
        if (duration > 3)
            yield return null;
        else if (duration >= 0)
            StartCoroutine(Blink(0.5f));
    }
}
