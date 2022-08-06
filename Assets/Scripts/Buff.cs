using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buff : MonoBehaviour
{
    public BuffData buffData;
    public StateMng StateMngSc;
    [SerializeField] TextMeshProUGUI Mount;

    public UnityEngine.UI.Image BuffImg;

    public int kind;
    public int apply_count;                     // 버프가 유지된 카운트
    public int count;                           // 중첩 갯수

    public bool isApply;

    void OnEnable()
    {
        apply_count = buffData.duration;
        StartCoroutine(cool());
    }

    IEnumerator cool()
    {
        yield return new WaitForSeconds(1.0f);
        if (apply_count == 3)
        {
            apply_count--;
            StartCoroutine(cool());
            StartCoroutine(Blink(0.5f));
        }
        else if (apply_count >= 1)
        {
            apply_count--;
            StartCoroutine(cool());
        }
        else
        {   
            if(!buffData.check_buff)
                StateMngSc.nPlayerDeBuffCount--;
            StateMngSc.nPlayerBuffCount--;
            isApply = false;
            count = 0;
            gameObject.SetActive(false);
        }
        if (!buffData.check_buff && buffData.check_nesting)
            Mount.text = 'x' + count.ToString();
    }

    public IEnumerator Blink(float time)
    {
        Color color = BuffImg.color;
        while (color.a > 0.5f)
        {
            if (apply_count > 3)
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
            if (apply_count > 3)
            {
                color.a = 1;
                BuffImg.color = color;
                break;
            }
            color.a += Time.deltaTime / time;
            BuffImg.color = color;
            yield return null;
        }
        if (apply_count > 3)
            yield return null;
        else if (apply_count >= 0)
            StartCoroutine(Blink(0.5f));
    }
}
