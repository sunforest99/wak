using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossBuff : Tooltips, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UnityEngine.UI.Image buffImg;
    public BuffData buffData;
    public int duration;

    void OnEnable()
    {
        duration = buffData.duration;
        buffImg.sprite = buffData.buffsprite;
        StartCoroutine(cool());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        base.tooltip.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.0f, this.transform.position.z);
        base.tooltip.SetActive(true);
        base.toolTipSetting(buffData.buffsprite, buffData.buffname, buffData.buffcontent);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        base.tooltip.SetActive(false);
    }

    IEnumerator cool()
    {
        yield return new WaitForSeconds(1.0f);
        if (duration >= 1)
        {
            duration--;
            StartCoroutine(cool());
        }
        else
        {
            gameObject.SetActive(false);
            base.tooltip.SetActive(false);
        }
    }
}
